using DDDC.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class SelifInfo_Web_News : System.Web.UI.Page
{
    Userservice userservice = new Userservice();
    MessageServices messageServices = new MessageServices();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int userID = Convert.ToInt32(Session["UserID"]);

            // 获取用户信息
            var user = userservice.GetUserByID(userID);
            BindNews("未读"); // 默认显示所有消息
            if (user != null)
            {
                // 加载个人信息
                
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

    private void BindNews(string messageType)
    {
        int userId = Convert.ToInt32(Session["UserID"]);
        var newsList = messageServices.GetMessagesByClientIdAndType(userId, messageType);

        RepeaterNews.DataSource = newsList;
        RepeaterNews.DataBind();
    }

    protected void FilterMessages(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string statusFilter = btn.CommandArgument;
        BindNews(statusFilter);
    }

    protected void MarkAsRead_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int newsId = Convert.ToInt32(btn.CommandArgument);
        messageServices.MarkMessageAsRead(newsId); // 标记为已读
        BindNews("全部"); // 重新加载消息
        Response.Redirect("http://localhost:51058/SelifInfo_Web/News.aspx");
    }
}