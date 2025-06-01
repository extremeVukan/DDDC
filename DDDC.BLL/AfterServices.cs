using DDDC.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DDDC.BLL
{
    public class AfterServices
    {
        private DDDCModel1 db = new DDDCModel1();

        public OrderInfoDTO GetOrderInfo(string orderNumber)
        {
            return (from o in db.orderT
                    where o.orderNumber == orderNumber
                    select new OrderInfoDTO
                    {
                        OrderNumber = o.orderNumber,
                        StartTime = o.start_time.HasValue ? o.start_time.Value : (DateTime?)null,
                        EndTime = o.end_time.HasValue ? o.end_time.Value : (DateTime?)null,
                        TotalPrice = o.total_price ?? 0,
                        OrderStatus = o.order_status,
                        PaymentStatus = o.payment_status
                    }).FirstOrDefault();
        }

        public bool SubmitRefundApplication(string orderNumber, int userId, int shipId, string reason)
        {
            try
            {
                // 创建退款申请对象
                var refundApplication = new AfterSales
                {
                    ordernumber = orderNumber,
                    UserID = userId,
                    Shipid = shipId,
                    Reason = reason,
                    ApplicationDate = DateTime.Now,
                    Status = "待处理"
                };

                // 将退款申请添加到数据库 - EF 版本
                db.AfterSales.Add(refundApplication);
                db.SaveChanges();

                return true; // 插入成功
            }
            catch (Exception ex)
            {
                // 记录错误日志
                Console.WriteLine($"提交退款申请时出错: {ex.Message}");
                return false; // 插入失败
            }
        }

        public bool getAppByOrdernumber(string ord)
        {
            AfterSales af = db.AfterSales.FirstOrDefault(c => c.ordernumber == ord);
            return af != null;
        }

        public AfterSales getAppByServicesID(int seid)
        {
            return db.AfterSales.FirstOrDefault(c => c.ServicesID == seid);
        }

        ///////////////////Admin
        public class RefundDTO
        {
            public int RefundID { get; set; }
            public string OrderNumber { get; set; }
            public int UserID { get; set; }
            public string Reason { get; set; }
            public DateTime ApplicationDate { get; set; }
            public string Status { get; set; }
        }

        public List<RefundDTO> GetRefundList()
        {
            return db.AfterSales.Select(r => new RefundDTO
            {
                RefundID = r.ServicesID,
                OrderNumber = r.ordernumber,
                UserID = Convert.ToInt32(r.UserID),
                Reason = r.Reason,
                ApplicationDate = Convert.ToDateTime(r.ApplicationDate),
                Status = r.Status
            }).ToList();
        }

        public bool ApproveRefund(int refundId)
        {
            var refund = db.AfterSales.FirstOrDefault(r => r.ServicesID == refundId);
            if (refund != null)
            {
                refund.Status = "同意";
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool RejectRefund(int refundId)
        {
            var refund = db.AfterSales.FirstOrDefault(r => r.ServicesID == refundId);
            if (refund != null)
            {
                refund.Status = "拒绝";
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public void UpdateOrderStatus(string ordnum, string status)
        {
            // 查找指定订单的记录
            var order = db.OrderForm.FirstOrDefault(o => o.OrderNumber == ordnum);

            if (order != null)
            {
                // 更新订单状态
                order.Status = status;

                // 提交更改到数据库
                db.SaveChanges();
            }
            else
            {
                // 如果找不到订单，抛出异常或进行错误处理
                throw new Exception("订单未找到");
            }
        }

        public void UpdateOrderTStatus1(string ordnum, string status, decimal price)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // 查找指定订单的记录
                    var order = db.orderT.FirstOrDefault(o => o.orderNumber == ordnum);

                    if (order != null)
                    {
                        // 更新订单状态
                        order.order_status = status;
                        order.payment_status = status;
                        order.total_price = price;

                        // 提交更改到数据库
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        // 如果找不到订单，抛出异常
                        transaction.Rollback();
                        throw new Exception("订单未找到");
                    }
                }
                catch (Exception ex)
                {
                    // 出现任何错误时回滚事务
                    transaction.Rollback();
                    throw new Exception($"更新订单状态时出错: {ex.Message}", ex);
                }
            }
        }

        // 使用 IDisposable 模式释放资源
        private bool disposed = false;

        // 保护的 Dispose 方法
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

        // 公开的 Dispose 方法
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public class OrderInfoDTO
    {
        public string OrderNumber { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
    }
}
