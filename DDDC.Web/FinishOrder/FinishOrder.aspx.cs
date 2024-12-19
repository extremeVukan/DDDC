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
        Session["EON"]=txtOrderNumber.Text;
        Session["ESN"]=txtShipName.Text;
        Session["EPP"]=txtPrePosition.Text;
        Session["EDE"] = txtDestination.Text;

        var s1 = orderService.GetOrderByOrdrNumber(txtOrderNumber.Text);
        Session["comment"] =s1.Comment.ToString();
        Session["EShipName"] =s1.ShipName.ToString();
        Session["EDistance"] =s1.Distance.ToString();
        Session["EImg"]=s1.img.ToString();
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