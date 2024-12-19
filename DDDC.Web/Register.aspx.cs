using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;


using DDDC.BLL;
using DDDC.DAL;

public partial class Register : System.Web.UI.Page
{
    Userservice UserSrv =new Userservice();
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            //调用CustomerService类中的IsNameExist()方法判断用户名是否重名
            if (UserSrv.IsNameExist(txtName.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('用户名已存在！');", true);
            }
            else
            {
                //调用CustomerService类中的Insert()方法插入新用户记录
                UserSrv.Insert(txtName.Text.Trim(), txtpwd.Text.Trim(), txtemail.Text.Trim());
                
                Response.Redirect("Login.aspx?name=" + txtName.Text);
            }
        }
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        TextBox3.Text = "";
        txtemail.Text = "";
        txtName.Text = "";
        txtpwd.Text = "";
    }
}
