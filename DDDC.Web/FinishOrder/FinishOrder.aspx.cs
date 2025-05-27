using DDDC.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FinishOrder_Order : System.Web.UI.Page
{
    Userservice userService = new Userservice();
    OrderServices orderService = new OrderServices();
    DriveService driveService = new DriveService();
    MessageServices MsgStrv = new MessageServices(); // 添加消息服务

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // 从Session中获取用户ID
            int userID = Convert.ToInt32(Session["UserID"]);

            // 获取用户信息
            var user = userService.GetUserByID(userID);

            if (user != null)
            {
                lblemail.Text = user.email;
                lblName.Text = user.user_name;
                txtDestination.Text = Session["CheckDestination"].ToString();
                txtOrderNumber.Text = Session["CheckOrderNumber"].ToString();
                txtPrePosition.Text = Session["CheckHere"].ToString();
                txtShipName.Text = Session["CheckShipName"].ToString();
                var checkPhone = orderService.GetOrderByOrdrNumber(txtOrderNumber.Text);
                var userPhone = userService.GetUserByID(Convert.ToInt32(checkPhone.OwnerID));
                txtDriverPhone.Text = userPhone.Phone;

                // 获取并显示订单状态
                string orderStatus = checkPhone.Status;


                // 如果订单状态已经是"进行中"或"已完成"，禁用"确认上船"按钮
                if (orderStatus == "进行中" || orderStatus == "已完成" || orderStatus=="已搭乘")
                {
                    btnConfirmBoarding.Enabled = false;
                    btnConfirmBoarding.CssClass = "boarding-button disabled";
                }

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

    // 添加"确认上船"按钮的点击事件处理
    protected void btnConfirmBoarding_Click(object sender, EventArgs e)
    {
        int userID = Convert.ToInt32(Session["UserID"]);
        string orderNumber = txtOrderNumber.Text;

        // 获取订单信息
        var order = orderService.GetOrderByOrdrNumber(orderNumber);
        int driverId = Convert.ToInt32(order.OwnerID);

        // 更新订单状态为"进行中"
        orderService.UpdateOrderStatus1(orderNumber, "进行中");

        // 发送消息通知司机
        string HeadText = "乘客已上船，行程开始";
        string Msg = "乘客已确认上船，行程已开始。订单号：" + orderNumber + "，目的地：" + txtDestination.Text;
        MsgStrv.addMsg(HeadText, driverId, userID, Msg, "订单", DateTime.Now, "未读");

        // 更新页面上的状态显示
        var ChekcOrderN = orderService.GetOrderByOrdrNumber(txtOrderNumber.Text);
        orderService.UpdateOrderStatus1(txtOrderNumber.Text, "已搭乘");
        // 禁用"确认上船"按钮
        btnConfirmBoarding.Enabled = false;
        btnConfirmBoarding.CssClass = "boarding-button disabled";

        // 提示操作成功
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
            "alert('已确认上船，行程开始！');", true);
    }

    protected void btnCompleteOrder_Click(object sender, EventArgs e)
    {
        Session["EON"] = txtOrderNumber.Text;
        Session["ESN"] = txtShipName.Text;
        Session["EPP"] = txtPrePosition.Text;
        Session["EDE"] = txtDestination.Text;

        var s1 = orderService.GetOrderByOrdrNumber(txtOrderNumber.Text);
        Session["comment"] = s1.Comment.ToString();
        Session["EShipName"] = s1.ShipName.ToString();
        Session["EDistance"] = s1.Distance.ToString();
        Session["EImg"] = s1.img.ToString();
        var Driver = userService.GetUserByID(Convert.ToInt32(s1.OwnerID));
        Session["eDrivername"] = Driver.user_name.ToString();
        Session["Ephone"] = Driver.Phone.ToString();

        ScriptManager.RegisterStartupScript(
            this,
            this.GetType(),
            "confirmDialog",
            "if (confirm('是否前往结算页面？')) { setTimeout(function(){ window.location.href = 'http://localhost:51058/FinishOrder/CalculateOrder.aspx'; }, 100); }",
            true
        );
    }
}