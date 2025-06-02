using DDDC.BLL;
using DDDC.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClientOrder_COrder : System.Web.UI.Page
{
    private Userservice userService = new Userservice();
    private OrderServices orderService = new OrderServices();
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
        var user = userService.GetUserByID(userID);

        // 在ships表根据Userid查询是否名下有船只，如果有，跳转页面
        try
        {
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
                        Session["UserID"] = userID;

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
            // 记录错误
            System.Diagnostics.Debug.WriteLine($"加载页面时出错: {ex.Message}");
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

            if (statu == "待确认")
            {
                Session["CheckOrderNumber1"] = orde.OrderNumber;
                Session["CheckShipName1"] = orde.ShipName;
                Session["CheckHere1"] = orde.PrePosition;
                Session["CheckDestination1"] = orde.Destination;
                Session["CheckShipID1"] = orde.ShipID;
                Session["CheckdriverName1"] = orde.OwnerName;
                Session["CheckShipImg1"] = orde.img;
                Session["CheckComment1"] = orde.Notes;
                Session["Checkordeid1"] = orderId.ToString();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "alert('跳转中！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/OrderForm/CheckUnreadyOrder.aspx'; }, 100);", true);
            }
            else
            {
                Session["CheckOrderNumber"] = orde.OrderNumber;
                Session["CheckShipName"] = orde.ShipName;
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
                    "alert('跳转中！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/FinishOrder/FinishOrder.aspx'; }, 100);", true);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"订单查看错误: {ex.Message}");
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

    private void BindGridView()
    {
        try
        {
            if (Session["UserID"] == null)
                return;

            int userID = Convert.ToInt32(Session["UserID"]);

            using (var db = new DDDCModel1())
            {
                // 查询满足条件的订单
                var query = db.OrderForm
                    .Where(o => o.ClientID == userID
                           && o.Status != "已拒绝"
                           && o.Status != "已完成"
                           && o.Status != "退款中"
                           && o.Status != "已退款");

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
            System.Diagnostics.Debug.WriteLine($"绑定GridView时出错: {ex.Message}");
            ScriptManager.RegisterStartupScript(this, GetType(), "toastScript",
                "showToast('加载订单数据时出错，请稍后再试', 'error');", true);
        }
    }
}
