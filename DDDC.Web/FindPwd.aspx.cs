using DDDC.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FindPwd : System.Web.UI.Page
{
    Userservice UserSrv =   new Userservice();
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            //调用CustomerService类中的IsNameExist()方法判断输入的用户名是否存在
           // if (!UserSrv.IsNameExist(txtName.Text.Trim()))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('用户名不存在！');", true);

            }
           // else
            {
                //调用CustomerService类中的IsEmailExist()方法判断输入的用户名和邮箱是否存在
               // if (!UserSrv.IsEmailExist(txtName.Text.Trim(), txtEmail.Text.Trim()))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('邮箱不存在！');", true);

                }
               // else
                {
                    //调用CustomerService类中的ResetPassword()方法重置用户密码为用户名
                    UserSrv.ResetPassword(txtName.Text.Trim(), txtEmail.Text.Trim());
                    //新建自定义的EmailSender类实例emailSender对象
                    EmailSender emailSender = new EmailSender(txtEmail.Text.Trim(), txtName.Text.Trim());
                    //调用自定义的EmailSender类中的Send()方法发送邮件
                    emailSender.Send();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('密码已发送至邮箱！');", true);

                }
            }
        }
    }
}