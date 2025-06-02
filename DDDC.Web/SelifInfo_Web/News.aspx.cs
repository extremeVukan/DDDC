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
            if (Session["UserID"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "alert('请登录！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/login.aspx'; }, 100);", true);
                return;
            }

            int userID = Convert.ToInt32(Session["UserID"]);

            // 获取用户信息
            var user = userservice.GetUserByID(userID);

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

                // 默认显示未读消息
                BindNews("未读");
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
        try
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            // 获取消息并转换为List，避免直接绑定IQueryable
            var newsList = messageServices.GetMessagesByClientIdAndType(userId, messageType).ToList();

            RepeaterNews.DataSource = newsList;
            RepeaterNews.DataBind();
        }
        catch (Exception ex)
        {
            // 记录错误并显示友好的错误提示
            System.Diagnostics.Debug.WriteLine($"绑定消息数据时出错: {ex.Message}");
            ScriptManager.RegisterStartupScript(this, GetType(), "toastScript",
                $"alert('加载消息时出错: {ex.Message}');", true);
        }
    }

    protected void FilterMessages(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string statusFilter = btn.CommandArgument;
        BindNews(statusFilter);
    }

    protected void MarkAsRead_Click(object sender, EventArgs e)
    {
        try
        {
            Button btn = (Button)sender;
            int newsId = Convert.ToInt32(btn.CommandArgument);
            messageServices.MarkMessageAsRead(newsId); // 标记为已读

            // 重新加载当前筛选类型的消息
            string currentFilter = ViewState["CurrentFilter"] as string ?? "全部";
            BindNews(currentFilter);

            // 刷新当前页面更新未读消息计数
            Response.Redirect(Request.RawUrl);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"标记消息已读时出错: {ex.Message}");
            ScriptManager.RegisterStartupScript(this, GetType(), "errorAlert",
                $"alert('处理消息时出错: {ex.Message}');", true);
        }
    }
}
