using DDDC.BLL;
using DDDC.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ClientOrder_AfterSales : System.Web.UI.Page
{
    AfterServices afterServices = new AfterServices();
    OrderServices orderServices = new OrderServices();
    Userservice userService = new Userservice();
    DriveService driveService = new DriveService();
    MessageServices MsgStrv = new MessageServices();
    protected void Page_Load(object sender, EventArgs e)
    {
        int userID = Convert.ToInt32(Session["UserID"]);
        var user = userService.GetUserByID(userID);
        var ship = driveService.GetShipsByOwnerID2(userID);
        if (!IsPostBack)
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
                string orderNumber = Session["RefundOrderNumber"].ToString(); // 假设订单号通过查询字符串传递
                if (!string.IsNullOrEmpty(orderNumber))
                {
                    BindOrderInfo(orderNumber);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('未提供订单号！');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
            "alert('请登录！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/login.aspx'; }, 100);", true);
            }
        }
    }

    private void BindOrderInfo(string orderNumber)
    {
        var order = afterServices.GetOrderInfo(orderNumber);
        if (order != null)
        {
            lblOrderNumber.Text = order.OrderNumber;
            lblOrderAmount.Text = $"¥{order.TotalPrice}";
            lblOrderTime.Text = order.EndTime.HasValue ? order.EndTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "无";

        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('订单信息未找到！');", true);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (afterServices.getAppByOrdernumber(lblOrderNumber.Text))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('请勿重复退款!');", true);
        }
        else
        {
            string orderNumber = lblOrderNumber.Text;
            string reason = ddlReason.SelectedValue;
            string detailedReason = txtReason.Text;

            if (string.IsNullOrEmpty(orderNumber) || string.IsNullOrEmpty(reason))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('订单号和退款理由不能为空！');", true);
                return;
            }
            var getshipid = orderServices.GetOrderByOrdrNumber(orderNumber);
            int userId = Convert.ToInt32(getshipid.ClientID);
            int shipId = Convert.ToInt32(getshipid.ShipID);

            bool success = afterServices.SubmitRefundApplication(orderNumber, userId, shipId, reason + " - " + detailedReason);

            if (success)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('退款申请提交成功！');", true);
                string HeadText = "亲爱的用户，您的退款申请已提交";
                string Msg = "亲爱的用户您对订单" +lblOrderNumber.Text+"的退款申请已提交！" ;
                MsgStrv.addMsg(HeadText, Convert.ToInt32(getshipid.ClientID), 999, Msg, "订单", DateTime.Now, "未读");
                orderServices.UpdateOrderStatus1(orderNumber,"退款中");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('退款申请提交失败，请稍后重试。');", true);
            }
        }
    }

}