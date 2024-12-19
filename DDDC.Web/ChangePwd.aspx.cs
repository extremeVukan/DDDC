using DDDC.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class ChangePwd : System.Web.UI.Page
{
    Userservice UserSrv = new Userservice();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)  //用户未登录
        {
            Response.Redirect("~/Login.aspx");
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            //调用CustomerService类中的CheckLogin()方法检查Session变量CustomerName关联的用户名和输入的原密码，返回值大于0表示输入的原密码正确
            if (UserSrv.CheckLogin(Session["Username"].ToString(), txtoldpwd.Text) > 0)
            {
                UserSrv.ChangePassword(Convert.ToInt32(Session["UserID"]), txtpwd.Text);
                lblmsg.Text = "密码修改成功！";
                Session.Clear();
                Response.Redirect("Login.aspx");
            }
            else  //输入的原密码不正确
            {
                lblmsg.Text = "原密码不正确！";
            }
        }
    }
}
