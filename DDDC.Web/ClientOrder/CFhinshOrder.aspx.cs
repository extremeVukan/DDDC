using DDDC.BLL;
using DDDC.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClientOrder_CFhinshOrder : System.Web.UI.Page
{
    private Userservice userService = new Userservice();
    private OrderServices orderService = new OrderServices();
    private AfterServices afterServices = new AfterServices();
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

        int userID = Convert.ToInt32(Session["UserID"]);

        try
        {
            var user = userService.GetUserByID(userID);
            var ship = driveService.GetShipsByOwnerID2(userID);

            if (!IsPostBack)
            {
                if (ship != null && driveService.IsShipExist(userID) && ship.ship_status != "Offline")
                {
                    Response.Redirect("http://localhost:51058/OrderControl/HandlingOrder.aspx");
                }
                else
                {
                    if (user != null)
                    {
                        lblemail.Text = user.email;
                        lblName.Text = user.user_name;

                        // 显示头像，如果没有头像则显示默认头像
                        if (!string.IsNullOrEmpty(user.photo))
                        {
                            Image2.ImageUrl = user.photo;
                        }
                        else
                        {
                            Image2.ImageUrl = "~/UserImg/暂无图片.gif";
                        }

                        // 绑定 GridView 数据
                        BindGridView();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                            "alert('请登录！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/login.aspx'; }, 100);", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"加载页面错误: {ex.Message}");
            ScriptManager.RegisterStartupScript(this, GetType(), "toastScript",
                "showToast('加载数据时出错，请稍后再试', 'error');", true);
        }
    }

    protected void btnCheckOrder_Click(object sender, EventArgs e)
    {
        try
        {
            Button btn = (Button)sender;
            int orderId = Convert.ToInt32(btn.CommandArgument);

            var orde = orderService.GetOrderByorder_ID(orderId);
            if (orde == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "toastScript",
                    "showToast('找不到订单信息', 'error');", true);
                return;
            }

            string statu = orde.Status;

            int s1 = Convert.ToInt32(orde.ShipID);
            var shipp = driveService.GetShipByID(s1);

            Session["CheckOrderNumber12"] = orde.OrderNumber;
            Session["CheckShipName12"] = orde.ShipName;
            Session["CheckHere"] = orde.PrePosition;
            Session["CheckDestination"] = orde.Destination;

            if (shipp != null)
            {
                Session["CheckShipPosition"] =
                    (shipp.province ?? string.Empty) +
                    (shipp.city ?? string.Empty) +
                    (shipp.Position ?? string.Empty);
            }
            else
            {
                Session["CheckShipPosition"] = "未知位置";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                "alert('跳转中！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/FinishOrder/CheckFinishedOrder.aspx'; }, 100);", true);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"查看订单错误: {ex.Message}");
            ScriptManager.RegisterStartupScript(this, GetType(), "toastScript",
                "showToast('处理订单时出错，请稍后再试', 'error');", true);
        }
    }

    protected void ctl02_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ctl02.PageIndex = e.NewPageIndex;
        BindGridView(); // 重新绑定数据
    }

    protected void ctl02_Sorting(object sender, GridViewSortEventArgs e)
    {
        // 切换排序方向
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
        BindGridView();
    }

    protected void btnRefund_Click(object sender, EventArgs e)
    {
        try
        {
            Button btn = (Button)sender;
            int orderId = Convert.ToInt32(btn.CommandArgument);

            // 查询订单信息
            var order = orderService.GetOrderByorder_ID(orderId);

            if (order == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('订单不存在！');", true);
                return;
            }

            // 判断是否超过三天
            if (order.End_Time.HasValue && (DateTime.Now - order.End_Time.Value).TotalDays > 3)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('订单已超过三天，不可退款！');", true);
                return;
            }

            // 判断订单是否已经退款
            if (order.Status == "已退款")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('订单已退款，不可重复申请！');", true);
                return;
            }

            // 将订单号保存到 Session 中
            Session["RefundOrderNumber"] = order.OrderNumber;

            // 跳转到退款申请页面
            Response.Redirect("http://localhost:51058/ClientOrder/AfterSales.aspx");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"退款申请错误: {ex.Message}");
            ScriptManager.RegisterStartupScript(this, GetType(), "toastScript",
                "showToast('处理退款申请时出错，请稍后再试', 'error');", true);
        }
    }

    private void BindGridView()
    {
        try
        {
            if (Session["UserID"] == null)
                return;

            int userID = Convert.ToInt32(Session["UserID"]);

            using (var db = new DDDCModel1())
            {
                // 查询满足条件的历史订单（已完成、已退款、退款中）
                var query = db.OrderForm
                    .Where(o => o.ClientID == userID &&
                           (o.Status == "已完成" || o.Status == "已退款" || o.Status == "退款中"));

                // 应用排序
                if (SortDirection == "ASC")
                {
                    switch (SortColumn)
                    {
                        case "OrderID":
                            query = query.OrderBy(o => o.OrderID);
                            break;
                        case "OrderNumber":
                            query = query.OrderBy(o => o.OrderNumber);
                            break;
                        case "Status":
                            query = query.OrderBy(o => o.Status);
                            break;
                        case "Start_Time":
                            query = query.OrderBy(o => o.Start_Time);
                            break;
                        default:
                            query = query.OrderBy(o => o.OrderID);
                            break;
                    }
                }
                else
                {
                    switch (SortColumn)
                    {
                        case "OrderID":
                            query = query.OrderByDescending(o => o.OrderID);
                            break;
                        case "OrderNumber":
                            query = query.OrderByDescending(o => o.OrderNumber);
                            break;
                        case "Status":
                            query = query.OrderByDescending(o => o.Status);
                            break;
                        case "Start_Time":
                            query = query.OrderByDescending(o => o.Start_Time);
                            break;
                        default:
                            query = query.OrderByDescending(o => o.OrderID);
                            break;
                    }
                }

                // 执行查询并绑定到 GridView
                ctl02.DataSource = query.ToList();
                ctl02.DataBind();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"绑定 GridView 错误: {ex.Message}");
            ScriptManager.RegisterStartupScript(this, GetType(), "toastScript",
                "showToast('加载订单数据时出错，请稍后再试', 'error');", true);
        }
    }
}
