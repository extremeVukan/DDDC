using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using DDDC.BLL;
public partial class login : System.Web.UI.Page
{
    Userservice UserSrv = new Userservice();
    DriveService DriverSrv = new DriveService();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            //Login.aspx页面传递过来的查询字符串变量name值非空
            if (Request.QueryString["name"] != null)
            {
                txtname.Text = Request.QueryString["name"];
                lblmsg.Text = "注册成功，请登录!";
            }
            int userid = Convert.ToInt32(Session["UserID"]);
            var user =UserSrv.GetUserByID(userid);
            var getship = DriverSrv.GetShipByID(userid);
            if (Session["UserID"] == null)
            {
                Session.Clear();
            }
            else
            {
                if (user.Status == "Driver" )
                {
                    if (getship.ship_status == "ban")
                    {
                        DriverSrv.UpdateShipStatusByUserID(userid, "ban");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('您已退出登录');", true);
                    }
                    if(getship.ship_status !="ban")
                        DriverSrv.UpdateShipStatusByUserID(userid, "Offline");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('您已退出登录');", true);
                        
                    }
                }
                
            }

            Session.Clear();
        }
        
    


    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            //调用CustomerService类中的CheckLogin()方法检查输入的用户名和密码是否正确
            int userID = UserSrv.CheckLogin(txtname.Text.Trim(), txtpwd.Text.Trim());
            
            if (userID > 0)   //用户名和密码正确
            {
                Session.Clear();   //清理Session中保存的内容        
                if (txtname.Text.Trim() == "admin")  //管理员登录
                {
                    Session["AdminId"] = userID;
                    Session["AdminName"] = txtname.Text;
                    Response.Redirect("http://localhost:51058/Admin/AllowShip.aspx");
                }
                else  //一般用户登录
                {
                    Session["UserID"] = userID;
                    Session["UserName"] = txtname.Text;
                    Response.Redirect("~/index.aspx");
                }
            }
            else  //用户名或密码错误
            {
                // 显示成功提示，不跳转页面
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('用户名或密码错误！');", true);

            }
        }
    }
}