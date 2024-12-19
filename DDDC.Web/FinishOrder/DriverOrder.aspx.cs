using DDDC.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FinishOrder_DriverOrder : System.Web.UI.Page
{
    Userservice userService = new Userservice();
    OrderServices orderService = new OrderServices();

    DriveService driveService = new DriveService();
    MessageServices MsgStrv = new MessageServices();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            // 从Session中获取用户ID

            int userID = Convert.ToInt32(Session["UserID"]);
            // 获取用户信息
            var user = userService.GetUserByID(userID);
            var ship1 = driveService.GetShipsByOwnerID2(userID);
            if (user != null)
            {
                lblShipName.Text = ship1.ship_name;
                lblShipStatus.Text = ship1.ship_status;
                lblemail.Text = user.email;
                lblName.Text = user.user_name;
                txtDestination.Text = Session["CheckDestination"].ToString();
                txtOrderNumber.Text = Session["CheckOrderNumber"].ToString();
                txtShipPosition.Text = Session["CheckShipPosition"].ToString();
                txtPrePosition.Text = Session["CheckHere"].ToString();
                txtShipName.Text = Session["CheckShipName"].ToString();
                var checkPhone = orderService.GetOrderByOrdrNumber(txtOrderNumber.Text);
                var userPhone = userService.GetUserByID(Convert.ToInt32(checkPhone.ClientID));
                txtCPhone.Text = userPhone.Phone;
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

    protected void btnCompleteOrder_Click(object sender, EventArgs e)
    {
        int userID = Convert.ToInt32(Session["UserID"]);
        string status = "Available";
        driveService.UpdateShipStatusByUserID(userID, status);
        var ChekcOrderN =orderService.GetOrderByOrdrNumber(txtOrderNumber.Text);
        int cID =Convert.ToInt32( ChekcOrderN.ClientID);
        string HeadText = "亲爱的用户，您的订单" + txtOrderNumber.Text + "已完成，请及时付款！";
        string Msg = "亲爱的用户您由" + txtPrePosition.Text + "开往" + txtDestination.Text + "的订单已由司机确认完成,请在确认订单信息后及时付款！";
        MsgStrv.addMsg(HeadText, cID, userID, Msg, "订单", DateTime.Now, "未读");

        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
            "alert('已提醒乘客付款！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/OrderControl/HandlingOrder.aspx'; }, 100);", true);
    }
}