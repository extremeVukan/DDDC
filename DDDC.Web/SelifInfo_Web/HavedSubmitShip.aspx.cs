using DDDC.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SelifInfo_Web_HavedSubmitShip : System.Web.UI.Page
{
    Userservice userService = new Userservice();
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