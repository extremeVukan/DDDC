using DDDC.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_MessageNotification : System.Web.UI.UserControl
{                    
     
    protected void Page_Load(object sender, EventArgs e)
    {
            if (!IsPostBack)
            {
                UpdateNotificationBadge();
            }
        }

        /// <summary>
        /// 更新红点显示
        /// </summary>
        private void UpdateNotificationBadge()
        {
            try
            {
               MessageServices messageServices = new MessageServices();
                int userId = Convert.ToInt32(Session["UserID"]);
                

                // 查询未读消息数量
                int unreadCount = messageServices.GetUnreadMessageCountByUser(userId);

                // 更新前端控件
                if (unreadCount > 0)
                {
                    notificationBadge.InnerText = unreadCount.ToString();
                    notificationBadge.Style["display"] = "inline-block";
                }
                else
                {
                    notificationBadge.Style["display"] = "none";
                }
            }
            catch
            {
                // 如果 Session["UserID"] 不存在或查询失败，则隐藏红点
                notificationBadge.Style["display"] = "none";
            }
        }
    }
