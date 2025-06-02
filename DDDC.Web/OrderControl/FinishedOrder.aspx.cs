using DDDC.BLL;
using DDDC.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OrderControl_FinishedOrder : System.Web.UI.Page
{
    private Userservice userService = new Userservice();
    private OrderServices orderService = new OrderServices();
    private OrderTServices orderTServices = new OrderTServices();
    private DriveService driveService = new DriveService();

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

        try
        {
            int userID = Convert.ToInt32(Session["UserID"]);

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
            System.Diagnostics.Debug.WriteLine($"页面加载错误: {ex.Message}");
            ScriptManager.RegisterStartupScript(this, GetType(), "errorToast",
                $"showToast('加载数据时出错: {ex.Message}', 'error');", true);
        }
    }

    protected void btnAcceptOrder_Click(object sender, EventArgs e)
    {
        try
        {
            Button btn = (Button)sender;
            int orderId = Convert.ToInt32(btn.CommandArgument);

            var orde = orderService.GetOrderByorder_ID(orderId);
            if (orde == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "errorToast",
                    "showToast('找不到订单信息', 'error');", true);
                return;
            }

            string statu = orde.Status;

            if (orde.ShipID == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "errorToast",
                    "showToast('订单中没有船只信息', 'error');", true);
                return;
            }

            int s1 = Convert.ToInt32(orde.ShipID);
            var shipp = driveService.GetShipByID(s1);

            if (shipp == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "errorToast",
                    "showToast('找不到相关船只信息', 'error');", true);
                return;
            }

            Session["CheckOrderNumber12"] = orde.OrderNumber;
            Session["CheckShipName12"] = orde.ShipName;
            Session["CheckHere"] = orde.PrePosition;
            Session["CheckDestination"] = orde.Destination;

            // 组合船只位置信息，并处理可能的null值
            string shipPosition =
                (shipp.province ?? "") +
                (shipp.city ?? "") +
                (shipp.Position ?? "");

            Session["CheckShipPosition"] = shipPosition;

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "confirmDialog",
                "if (confirm('是否跳转到订单页面？')) { setTimeout(function(){ window.location.href = 'http://localhost:51058/FinishOrder/DriverFinishOrder.aspx'; }, 100); }",
                true
            );
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"查看订单错误: {ex.Message}");
            ScriptManager.RegisterStartupScript(this, GetType(), "errorToast",
                $"showToast('处理订单时出错: {ex.Message}', 'error');", true);
        }
    }

    protected void ctl02_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ctl02.PageIndex = e.NewPageIndex;
        BindOrders(); // 使用后台代码重新绑定数据
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

    private void BindOrders()
    {
        try
        {
            if (Session["UserID"] == null)
                return;

            int userID = Convert.ToInt32(Session["UserID"]);

            using (var db = new DDDCModel1())
            {
                // 查询已完成的订单
                var query = db.OrderForm
                    .Where(o => o.OwnerID == userID &&
                           (o.Status == "已完成" || o.Status == "已退款"))
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
                    case "Status":
                        query = SortDirection == "ASC" ? query.OrderBy(o => o.Status) : query.OrderByDescending(o => o.Status);
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
            System.Diagnostics.Debug.WriteLine($"绑定数据错误: {ex.Message}");
            ScriptManager.RegisterStartupScript(this, GetType(), "errorToast",
                $"showToast('获取订单数据时出错: {ex.Message}', 'error');", true);
        }
    }
}

