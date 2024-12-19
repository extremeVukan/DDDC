using DDDC.DAL;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace DDDC.BLL
{
    public class MessageServices
    {

        DataClasses1DataContext db = new DataClasses1DataContext();
        public void addMsg(string head,int clientid,int driverid,string msg,string msgtype,DateTime sendtime,string status)
        {
            ordernews Msg = new ordernews()
            {
                headText = head,               // 消息标题
                Client_id = clientid,          // 客户 ID
                Driver_id = driverid,          // 司机 ID
                message = msg,                 // 消息内容
                message_type = msgtype,        // 消息类型（订单/船只/客户等）
                send_time = sendtime,          // 发送时间
                read_status = status           // 消息读取状态（已读/未读）
            };
            db.ordernews.InsertOnSubmit(Msg);

            // 提交数据到数据库
            db.SubmitChanges();
        }

        /// <summary>
        /// 获取未读消息数量
        /// </summary>
        /// <param name="userId">用户 ID</param>
        /// <returns>未读消息数量</returns>
        public int GetUnreadMessageCountByUser(int userId)
        {
            return db.ordernews
                     .Where(m => m.Client_id == userId && m.read_status == "未读")
                     .Count();
        }

        public IQueryable<ordernews> GetMessagesByClientIdAndType(int clientId, string statusFilter)
        {
            var query = db.ordernews.Where(n => n.Client_id == clientId);

            if (statusFilter == "未读")
            {
                query = query.Where(n => n.read_status == "未读");
                query.OrderByDescending(n => n.send_time);
            }
            else if (statusFilter == "已读")
            {
                query = query.Where(n => n.read_status == "已读");
                query.OrderByDescending(n => n.send_time);
            }

            return query.OrderByDescending(n => n.send_time);
        }

        public void MarkMessageAsRead(int newsId)
        {
            var message = db.ordernews.FirstOrDefault(n => n.news_id == newsId);
            if (message != null)
            {
                message.read_status = "已读";
                db.SubmitChanges();
            }
        }


    }

}
