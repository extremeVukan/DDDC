using DDDC.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace DDDC.BLL
{
    public class AfterServices
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

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

                // 将退款申请插入数据库
                db.AfterSales.InsertOnSubmit(refundApplication);
                db.SubmitChanges();

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
            AfterSales af= (from c in db.AfterSales
                    where c.ordernumber == ord
                    select c).FirstOrDefault();
            if (af != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public AfterSales getAppByServicesID(int seid)
        {
            return (from c in db.AfterSales
                    where c.ServicesID == seid
                    select c).FirstOrDefault();
            

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
                UserID =Convert.ToInt32( r.UserID),
                Reason = r.Reason,
                ApplicationDate =Convert.ToDateTime( r.ApplicationDate),
                Status = r.Status
            }).ToList();
        }

        public bool ApproveRefund(int refundId)
        {
            var refund = db.AfterSales.SingleOrDefault(r => r.ServicesID == refundId);
            if (refund != null)
            {
                refund.Status = "同意";
                db.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool RejectRefund(int refundId)
        {
            var refund = db.AfterSales.SingleOrDefault(r => r.ServicesID == refundId);
            if (refund != null)
            {
                refund.Status = "拒绝";
                db.SubmitChanges();
                return true;
            }
            return false;
        }


        public void UpdateOrderStatus(string ordnum, string status)
        {
            // 查找指定订单的记录
            var order = db.OrderForm.SingleOrDefault(o => o.OrderNumber == ordnum);

            if (order != null)
            {
                // 更新订单状态
                order.Status = status;

                // 提交更改到数据库
                db.SubmitChanges();
            }
            else
            {
                // 如果找不到订单，你可以抛出一个异常或者进行错误处理
                throw new Exception("订单未找到");
            }
        }

        public void UpdateOrderTStatus1(string ordnum, string status ,decimal price)
        {
            // 查找指定订单的记录
            var order = db.orderT.SingleOrDefault(o => o.orderNumber == ordnum);

            if (order != null)
            {
                // 更新订单状态
                order.order_status = status;
                order.payment_status = status;
                order.total_price = price;

                // 提交更改到数据库
                db.SubmitChanges();
            }
            else
            {
                // 如果找不到订单，你可以抛出一个异常或者进行错误处理
                throw new Exception("订单未找到");
            }
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
