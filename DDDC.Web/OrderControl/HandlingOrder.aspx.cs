using DDDC.BLL;
using DDDC.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class OrderControl_HandlingOrder : System.Web.UI.Page
{
    private Userservice userService = new Userservice();
    private OrderServices orderService = new OrderServices();
    private DriveService driveService = new DriveService();
    private OrderTServices orderTServices = new OrderTServices();
    private MessageServices MsgStrv = new MessageServices();

    // 用于排序的属性
    private string SortColumn
    {
        get
        {
            if (ViewState["SortColumn"] == null)
            {
                ViewState["SortColumn"] = "OrderID";
            }
            return ViewState["SortColumn"].ToString();
        }
        set
        {
            ViewState["SortColumn"] = value;
        }
    }

    private string SortDirection
    {
        get
        {
            if (ViewState["SortDirection"] == null)
            {
                ViewState["SortDirection"] = "DESC";
            }
            return ViewState["SortDirection"].ToString();
        }
        set
        {
            ViewState["SortDirection"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                "alert('请登录！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/login.aspx'; }, 100);", true);
            return;
        }

        int userID = Convert.ToInt32(Session["UserID"]);

        try
        {
            if (!IsPostBack)
            {
                // 获取用户信息
                var user = userService.GetUserByID(userID);
                var ship1 = driveService.GetShipsByOwnerID2(userID);

                if (user != null && ship1 != null)
                {
                    lblemail.Text = user.email;
                    lblName.Text = user.user_name;
                    lblShipName.Text = ship1.ship_name;
                    lblShipStatus.Text = ship1.ship_status;

                    // 显示头像，如果没有头像则显示默认头像
                    if (!string.IsNullOrEmpty(user.photo))
                    {
                        Image2.ImageUrl = user.photo;
                    }
                    else
                    {
                        Image2.ImageUrl = "~/UserImg/暂无图片.gif";
                    }

                    // 绑定订单数据
                    BindOrders();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                        "alert('用户或船只信息不存在！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/login.aspx'; }, 100);", true);
                }
            }
        }
        catch (Exception ex)
        {
            // 记录错误并显示友好的错误提示
            System.Diagnostics.Debug.WriteLine($"页面加载错误: {ex.Message}");
            ScriptManager.RegisterStartupScript(this, GetType(), "errorToast",
                $"showToast('加载数据时出错: {ex.Message}', 'error');", true);
        }
    }

    private void BindOrders()
    {
        try
        {
            int userID = Convert.ToInt32(Session["UserID"]);

            using (var db = new DDDCModel1())
            {
                // 查询当前用户可处理的待确认订单
                var query = db.OrderForm
                    .Where(o => o.OwnerID == 0 && o.Status == "待确认") // 未分配给任何船主的待确认订单
                    .AsQueryable();

                // 应用排序
                switch (SortColumn)
                {
                    case "OrderID":
                        query = SortDirection == "ASC" ? query.OrderBy(o => o.OrderID) : query.OrderByDescending(o => o.OrderID);
                        break;
                    case "OrderNumber":
                        query = SortDirection == "ASC" ? query.OrderBy(o => o.OrderNumber) : query.OrderByDescending(o => o.OrderNumber);
                        break;
                    case "Start_Time":
                        query = SortDirection == "ASC" ? query.OrderBy(o => o.Start_Time) : query.OrderByDescending(o => o.Start_Time);
                        break;
                    default:
                        query = query.OrderByDescending(o => o.OrderID);
                        break;
                }

                // 获取数据并绑定到GridView
                ctl02.DataSource = query.ToList();
                ctl02.DataBind();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"绑定订单数据错误: {ex.Message}");
            ScriptManager.RegisterStartupScript(this, GetType(), "errorToast",
                $"showToast('获取订单数据时出错: {ex.Message}', 'error');", true);
        }
    }

    protected void btnAcceptOrder_Click(object sender, EventArgs e)
    {
        int userID = Convert.ToInt32(Session["UserID"]);
        var ship = driveService.GetShipsByOwnerID2(userID);

        try
        {
            if (ship == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "alert('没有找到您的船只信息，请先注册船只！');", true);
                return;
            }

            string ShipL = (ship.province ?? "") + (ship.city ?? "") + (ship.Position ?? "");

            if (orderService.IsUnfinishOrderExist(userID))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                "alert('您还有订单尚未完成！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/OrderControl/ProducingOrder.aspx'; }, 100);", true);
            }
            else if (ship.ship_status == "Occupied" || ship.ship_status == "Offline")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                "alert('您的船只状态异常！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/SelifInfo_Web/Ships_info.aspx'; }, 100);", true);
            }
            else
            {
                // 获取按钮的 CommandArgument，表示被点击的订单的 OrderID
                Button btn = (Button)sender;
                int orderId = Convert.ToInt32(btn.CommandArgument);  // 从CommandArgument获取OrderID
                var ord = orderService.GetOrderByorder_ID(orderId);

                if (ord == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                        "alert('订单信息不存在！');", true);
                    return;
                }

                // 调用 AcceptOrder 方法来更新订单状态
                orderService.AcceptOrder(orderId);

                // 添加订单信息到orderT表
                orderTServices.AddOrderInfo(
                    ord.OrderID,
                    ord.OrderNumber,
                    Convert.ToInt32(ord.ClientID),
                    Convert.ToInt32(ship.ship_id),
                    "确认",
                    ord.PrePosition,
                    ord.Destination,
                    ShipL,
                    ord.Start_Time ?? DateTime.MinValue,
                    ord.End_Time ?? DateTime.MinValue,
                    0,
                    "未支付"
                );

                // 更新订单信息，绑定船只和船主信息
                orderService.UpdateOrder(
                    orderId,                         // 订单ID
                    ship.ship_name,                  // 船只名称
                    ship.ship_id,                    // 船只ID
                    userID,                          // 船主ID
                    Session["Username"]?.ToString() ?? lblName.Text,  // 船主名称
                    ShipL,                           // 船只位置
                    ship.Picture                     // 船只图片
                );

                // 发送消息给客户
                int cID = Convert.ToInt32(ord.ClientID);
                string HeadText = "亲爱的用户，您的订单" + ord.OrderNumber + "已接单，请注意查看！";
                string Msg = "亲爱的用户您由" + ord.PrePosition + "开往" + ord.Destination + "的订单已由司机确认,请确认订单信息无误后到达预定位置！";
                MsgStrv.addMsg(HeadText, cID, userID, Msg, "订单", DateTime.Now, "未读");

                // 更新船只状态为占用
                driveService.UpdateShipStatusByUserID(userID, "Occupied");

                // 提示用户操作成功
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                "alert('订单已成功接受并确认！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/OrderControl/ProducingOrder.aspx'; }, 100);", true);
            }
        }
        catch (Exception ex)
        {
            // 记录错误并显示友好的错误提示
            System.Diagnostics.Debug.WriteLine($"接单操作错误: {ex.Message}");
            ScriptManager.RegisterStartupScript(this, GetType(), "errorToast",
                $"showToast('接单时出错: {ex.Message}', 'error');", true);
        }
    }

    protected void ctl02_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ctl02.PageIndex = e.NewPageIndex;
        BindOrders(); // 重新绑定数据
    }

    protected void ctl02_Sorting(object sender, GridViewSortEventArgs e)
    {
        // 更新排序列和方向
        if (SortColumn == e.SortExpression)
        {
            SortDirection = (SortDirection == "ASC") ? "DESC" : "ASC";
        }
        else
        {
            SortColumn = e.SortExpression;
            SortDirection = "ASC";
        }

        // 重新绑定数据
        BindOrders();
    }
}
