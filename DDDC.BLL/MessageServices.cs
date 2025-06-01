using DDDC.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DDDC.BLL
{
    public class MessageServices : IDisposable
    {
        private DDDCModel1 db = new DDDCModel1();

        public void addMsg(string head, int clientid, int driverid, string msg, string msgtype, DateTime sendtime, string status)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
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

                    // 使用EF的Add方法添加消息
                    db.ordernews.Add(Msg);

                    // 使用SaveChanges提交到数据库
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"添加消息失败: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// 获取未读消息数量
        /// </summary>
        /// <param name="userId">用户 ID</param>
        /// <returns>未读消息数量</returns>
        public int GetUnreadMessageCountByUser(int userId)
        {
            return db.ordernews
                     .Count(m => m.Client_id == userId && m.read_status == "未读");
        }

        public IQueryable<ordernews> GetMessagesByClientIdAndType(int clientId, string statusFilter)
        {
            var query = db.ordernews.Where(n => n.Client_id == clientId);

            if (statusFilter == "未读")
            {
                query = query.Where(n => n.read_status == "未读");
            }
            else if (statusFilter == "已读")
            {
                query = query.Where(n => n.read_status == "已读");
            }

            // 确保返回按发送时间降序排列的结果
            return query.OrderByDescending(n => n.send_time);
        }

        public void MarkMessageAsRead(int newsId)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var message = db.ordernews.FirstOrDefault(n => n.news_id == newsId);
                    if (message != null)
                    {
                        message.read_status = "已读";
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"标记消息已读失败: {ex.Message}", ex);
                }
            }
        }

        #region IDisposable实现

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
