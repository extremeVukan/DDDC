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
    Userservice userService = new Userservice();
    OrderServices orderService = new OrderServices();
    AfterServices  afterServices = new AfterServices();
    DriveService driveService = new DriveService();
    protected void Page_Load(object sender, EventArgs e)
    {
        int userID = Convert.ToInt32(Session["UserID"]);
        var user = userService.GetUserByID(userID);
        var ship = driveService.GetShipsByOwnerID2(userID);
        if (Session["UserID"] == null)
        {
            LinqDataSource1.Where = "1 == 0"; // 没有数据
        }
        if (!IsPostBack)
        {
            if (driveService.IsShipExist(userID) && ship.ship_status != "Offline")
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
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "alert('请登录！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/login.aspx'; }, 100);", true);
                    }


                }
            
            
        }
    }
    protected void btnCheckOrder_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int orderId = Convert.ToInt32(btn.CommandArgument);

        var orde = orderService.GetOrderByorder_ID(orderId);
        string statu = orde.Status;

        int s1 = Convert.ToInt32(orde.ShipID);
        var shipp = driveService.GetShipByID(s1);
        Session["CheckOrderNumber12"] = orde.OrderNumber;
        Session["CheckShipName12"] = orde.ShipName;
        Session["CheckHere"] = orde.PrePosition;
        Session["CheckDestination"] = orde.Destination;
        Session["CheckShipPosition"] = shipp.province + shipp.city + shipp.Position; ;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
              "alert('跳转中！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/FinishOrder/CheckFinishedOrder.aspx'; }, 100);", true);
    }

    protected void ctl02_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ctl02.PageIndex = e.NewPageIndex;

        // 重新绑定数据源
        ctl02.DataBind();
    }

    protected void btnRefund_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int orderId = Convert.ToInt32(btn.CommandArgument);

        // 数据上下文

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

            // 跳转到退款申请页面（可根据需求修改 URL）
            Response.Redirect("http://localhost:51058/ClientOrder/AfterSales.aspx");
        }
    }

