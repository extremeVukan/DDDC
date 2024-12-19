using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using DDDC.BLL;
using DDDC.DAL;
using System.Activities.Expressions;

public partial class SelifInfo_Web_Self_Info : System.Web.UI.Page
{
    Userservice userService = new Userservice();
    OrderServices orderservices = new OrderServices();
    protected void Page_Load(object sender, EventArgs e)
    {
        int userID = Convert.ToInt32(Session["UserID"]);
        var getuser = userService.GetUserByID(userID);
        if (getuser.UserSatus == "ban")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                "alert('您的账号已被封禁！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/login.aspx'; }, 100);", true);
        }

        else if (!IsPostBack)
        {
            // 从Session中获取用户ID
            // 获取用户信息
            var user = userService.GetUserByID(userID);
            BindOrders();
            if (user != null)
            {
                // 加载个人信息
                txtName.Text = user.user_name;
                txtEmail.Text = user.email;
                txtPhone.Text = user.Phone;
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
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
            "alert('请登录！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/login.aspx'; }, 100);", true);
            }


        }
    }


    protected void BtnName_Click(object sender, EventArgs e)
    {
        int userID = Convert.ToInt32(Session["UserID"]);
        string newName = txtName.Text;
        var user = userService.GetUserByID(userID);
        if (user.user_name == txtName.Text)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('请勿重复修改！');", true);
        }
        else
        {
            userService.ChangeUserName(userID, newName);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('用户名修改成功！');", true);
        }
    }

    protected void BtnEmail_Click(object sender, EventArgs e)
    {
        int userID = Convert.ToInt32(Session["UserID"]);
        string newEmail = txtEmail.Text; ;
        var user = userService.GetUserByID(userID);
        if (user.email == txtEmail.Text)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('请勿重复修改！');", true);
        }
        else
        {
            userService.ChangeEmail(userID, newEmail);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('邮箱修改成功！');", true);
        }
    }

    protected void BtnPhone_Click(object sender, EventArgs e)
    {
        int userID = Convert.ToInt32(Session["UserID"]);
        string newPhone = txtPhone.Text;
        var user = userService.GetUserByID(userID);
        if (user.Phone == txtPhone.Text)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('请勿重复修改！');", true);
        }
        else
        {

            userService.ChangePhone(userID, newPhone);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('电话修改成功！');", true);
        }
    }
    protected void BtnPhoto_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            // 在保存文件前，注册事件验证
            ClientScript.RegisterForEventValidation(FileUpload1.UniqueID);

            // 获取文件扩展名
            string extension = Path.GetExtension(FileUpload1.FileName).ToLower();
            string[] validFileTypes = { ".jpg", ".jpeg", ".png", ".gif" };

            if (Array.Exists(validFileTypes, ext => ext == extension))
            {
                int userID = Convert.ToInt32(Session["UserID"]);

                // 设置上传路径
                string filePath = "~/UserImg/" + userID + "_" + DateTime.Now.Ticks + extension;
                string physicalPath = Server.MapPath(filePath);

                // 保存文件
                FileUpload1.SaveAs(physicalPath);

                // 更新数据库中的头像路径
                userService.UploadPhoto(userID, filePath);

                // 显示新的头像
                Image1.ImageUrl = filePath;
                Image2.ImageUrl = filePath;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('上传成功');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('请上传有效的图片');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('请选择要上传的图片');", true);
        }
    }
    private void BindOrders()
    {
        int userId = Convert.ToInt32(Session["UserID"]);
        var orders = orderservices.GetOrder(userId); // 假设有一个方法按用户ID获取订单

        RepeaterOrders.DataSource = orders;
        RepeaterOrders.DataBind();
    }

    

}
