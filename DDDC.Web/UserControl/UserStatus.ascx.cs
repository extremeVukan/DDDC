using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_UserStatus : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["AdminId"] != null || Session["UserID"] != null)  //用户已登录
        {
            if (Session["AdminId"] != null)  //管理员用户
            {
                lblWelcome.Text = "您好, " + Session["AdminName"].ToString();
                lnkbtnManage.Visible = true;
            }
            else if (Session["UserID"] != null)  //一般用户
            {
                lblWelcome.Text = "您好, " + Session["UserName"].ToString();
                lnkbtnPwd.Visible = true;
                lnkbtnOrder.Visible = true;
            }
            lnkbtnLogout.Visible = true;
        }
    }

    

    protected void lnkbtnLogout_Click1(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(
     this,
     this.GetType(),
     "confirmDialog",
     "if (confirm('是否退出登录？')) { setTimeout(function(){ window.location.href = 'http://localhost:51058/login.aspx'; }, 100); }",
     true
 );

    }

    protected void lnkbtnOrder_Click(object sender, EventArgs e)
    {
    
       
    }
}