using DDDC.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDC.BLL
{
    public class OrderServices
    {
        // 数据上下文对象
        DataClasses1DataContext db = new DataClasses1DataContext();

        // 添加订单的方法
        public void AddOrder(string orderNumber, int clientId, string shipName, int shipId,int onwer_ID, string ownerName,
                              string prePosition, string destination, string notes, DateTime startTime,
                              DateTime endTime, string comment, string Shipimg,string meters ,string status)
        {
            // 创建一个新的订单对象
            OrderForm newOrder = new OrderForm
            {
                OrderNumber = orderNumber,          // 订单号
                ClientID = clientId,                // 客户ID
                ShipName = shipName,                // 船只名称
                ShipID = shipId,                    // 船只ID
                OwnerID = onwer_ID,
                OwnerName = ownerName,              // 司机姓名
                PrePosition = prePosition,          // 起始位置
                Destination = destination,          // 目的地
                Notes = notes,                      // 备注
                Start_Time = startTime,             // 开始时间
                End_Time = endTime,                 // 结束时间
                Comment = comment,                  // 其他备注
                img = Shipimg,
                Distance = meters,
                Status = status                     // 状态
            };

            // 将新订单对象插入到数据库中
            db.OrderForm.InsertOnSubmit(newOrder);

            // 提交数据到数据库
            db.SubmitChanges();
        }


        public void AcceptOrder(int orderId)
        {
            // 查找指定订单的记录
            var order = db.OrderForm.SingleOrDefault(o => o.OrderID == orderId);

            if (order != null)
            {
                // 更新订单状态为 "确认"
                order.Status = "确认";

                // 提交更改到数据库
                db.SubmitChanges();
            }
            else
            {
                // 如果找不到订单，你可以抛出一个异常或者进行错误处理
                throw new Exception("订单未找到");
            }
        }
        public void RejectOrder(int orderId)
        {
            // 查找指定订单的记录
            var order = db.OrderForm.SingleOrDefault(o => o.OrderID == orderId);

            if (order != null)
            {
                // 更新订单状态为 "确认"
                order.Status = "已拒绝";

                // 提交更改到数据库
                db.SubmitChanges();
            }
            else
            {
                // 如果找不到订单，你可以抛出一个异常或者进行错误处理
                throw new Exception("订单未找到");
            }
        }
        public OrderForm GetOrderByorder_ID(int orderid)
        {
            return (from c in db.OrderForm
                    where c.OrderID == orderid
                    select c).FirstOrDefault();

        }
        

        public OrderForm GetOrderByOrdrNumber(String OrderNum)
        {
            return (from c in db.OrderForm
                    where c.OrderNumber == OrderNum
                    select c).FirstOrDefault();

        }

        public OrderForm GetOrdersByUserId(int Userid)
        {
            return (from c in db.OrderForm
                    where c.ClientID == Userid
                    select c).FirstOrDefault();

        }

        public class ShipSummaryDTO1
        {
            public string Ordernumber { get; set; }
            public string status { get; set; }

           
        }
        public List<ShipSummaryDTO1> GetOrder(int userid)
        {
            return db.OrderForm
                .Where(ord => ord.ClientID == userid&& ord.Status !="已完成" && ord.Status != "已拒绝")
                     .Select(ord1 => new ShipSummaryDTO1
                     {
                         Ordernumber = ord1.OrderNumber,
                         status = ord1.Status,
                         

                     })
                     .ToList();
        }





        public bool AddCommentToOrder(string orderNumber, string comment)
        {
            try
            {
                // 查询订单
                var order = db.OrderForm.SingleOrDefault(o => o.OrderNumber == orderNumber);

                if (order == null)
                {
                    throw new Exception("未找到对应的订单！");
                }

                // 更新评论字段
                order.Comment = comment;

                // 保存更改到数据库
                db.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                // 记录错误日志
                System.Diagnostics.Debug.WriteLine($"更新评论失败: {ex.Message}");
                return false;
            }
        }

        public void UpdateOrderStatus(string ordnum)
        {
            // 查找指定订单的记录
            var order = db.OrderForm.SingleOrDefault(o => o.OrderNumber == ordnum);

            if (order != null)
            {
                // 更新订单状态为 "确认"
                order.Status = "已完成";
                order.End_Time = DateTime.Now;
                // 提交更改到数据库
                db.SubmitChanges();
            }
            else
            {
                // 如果找不到订单，你可以抛出一个异常或者进行错误处理
                throw new Exception("订单未找到");
            }
        }
        public bool IsUnfinishOrderExist(int userID)
        {

            OrderForm ships1 = (from c in db.OrderForm
                            where c.OwnerID == userID && c.Status == "确认"
                            select c).FirstOrDefault();
            if (ships1 != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

