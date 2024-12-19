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
}
