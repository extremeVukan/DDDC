using DDDC.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DDDC.BLL
{
    public class OrderTServices : IDisposable
    {
        private DDDCModel1 db = new DDDCModel1();

        /// <summary>
        /// 插入订单信息到 OrderT 表
        /// </summary>
        /// <param name="orderNumber">订单号</param>
        /// <param name="passengerId">乘客 ID</param>
        /// <param name="shipId">船只 ID</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="startLocation">起始位置</param>
        /// <param name="endLocation">终点位置</param>
        /// <param name="shipLocation">船只位置</param>
        /// <param name="startTime">订单开始时间</param>
        /// <param name="endTime">订单结束时间</param>
        /// <param name="totalPrice">总价格</param>
        /// <param name="paymentStatus">支付状态</param>
        public void AddOrderInfo(
            int orderId,
            string orderNumber,
            int passengerId,
            int shipId,
            string orderStatus,
            string startLocation,
            string endLocation,
            string shipLocation,
            DateTime startTime,
            DateTime endTime,
            decimal totalPrice,
            string paymentStatus)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // 创建一个新的订单对象
                    var newOrder = new orderT
                    {
                        order_id1 = orderId,
                        orderNumber = orderNumber,
                        passenger_id = passengerId,
                        ship_id = shipId,
                        order_status = orderStatus,
                        star_location = startLocation,
                        end_location = endLocation,
                        ship_locetion = shipLocation,
                        start_time = startTime,
                        end_time = endTime,
                        total_price = totalPrice,
                        estimate = "0",
                        payment_status = paymentStatus
                    };

                    // 将新订单添加到数据库中
                    db.orderT.Add(newOrder);

                    // 提交更改
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"插入订单失败：{ex.Message}");
                    throw; // 可以根据需求记录日志或抛出异常
                }
            }
        }

        public orderT GetPositionByOrdrNumber(String OrderNum)
        {
            return db.orderT.FirstOrDefault(c => c.orderNumber == OrderNum);
        }

        public void changeStatus(String OrderNum, decimal price)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var ss = db.orderT.FirstOrDefault(o => o.orderNumber == OrderNum);
                    if (ss != null)
                    {
                        ss.end_time = DateTime.Now;
                        ss.order_status = "已完成";
                        ss.payment_status = "已支付";
                        ss.total_price = price;
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                        throw new Exception("订单未找到");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"更新订单状态失败: {ex.Message}", ex);
                }
            }
        }

        public void SubmitEst(String OrderNum, string evaluate)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var ss = db.orderT.FirstOrDefault(o => o.orderNumber == OrderNum);
                    if (ss != null)
                    {
                        ss.estimate = evaluate;
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                        throw new Exception("订单未找到");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"提交评价失败: {ex.Message}", ex);
                }
            }
        }

        public decimal GetTodayIncome(int shipId)
        {
            // 计算时间范围，在查询外
            DateTime startDate = DateTime.Today;
            DateTime endDate = startDate.AddDays(1);

            // 使用本地变量在查询中
            var income = db.orderT
                .Where(o => o.ship_id == shipId
                            && o.end_time.HasValue
                            && o.end_time.Value >= startDate
                            && o.end_time.Value < endDate
                            && o.payment_status == "已支付")
                .Sum(o => (decimal?)o.total_price) ?? 0;

            return income;
        }

        public decimal GetYesterdayIncome(int shipId)
        {
            // 计算时间范围，在查询外
            DateTime startDate = DateTime.Today.AddDays(-1);
            DateTime endDate = DateTime.Today;

            var income = db.orderT
                .Where(o => o.ship_id == shipId
                            && o.end_time.HasValue
                            && o.end_time.Value >= startDate
                            && o.end_time.Value < endDate
                            && o.payment_status == "已支付")
                .Sum(o => (decimal?)o.total_price) ?? 0;

            return income;
        }

        public decimal GetMonthlyIncome(int shipId)
        {
            // 计算时间范围，在查询外
            DateTime today = DateTime.Today;
            DateTime startDate = new DateTime(today.Year, today.Month, 1);
            DateTime endDate = startDate.AddMonths(1);

            var income = db.orderT
                .Where(o => o.ship_id == shipId
                            && o.end_time.HasValue
                            && o.end_time.Value >= startDate
                            && o.end_time.Value < endDate
                            && o.payment_status == "已支付")
                .Sum(o => (decimal?)o.total_price) ?? 0;

            return income;
        }

        public decimal GetTotalIncome(int shipId)
        {
            // 这个方法不需要日期比较，保持不变
            var income = db.orderT
                .Where(o => o.ship_id == shipId
                            && o.payment_status == "已支付")
                .Sum(o => (decimal?)o.total_price) ?? 0;

            return income;
        }


        public orderT GetorderTByOrdN(string ordn)
        {
            return db.orderT.FirstOrDefault(c => c.orderNumber == ordn);
        }

        public class OrderCommentData
        {
            public string OrderNumber { get; set; }
            public string Estimate { get; set; }
            public string Comment { get; set; }
            public DateTime OrderDate { get; set; }
        }

        public List<OrderCommentData> GetCommentsByShipId(int shipId)
        {
            var query = from orderT in db.orderT
                        join orderForm in db.OrderForm on orderT.orderNumber equals orderForm.OrderNumber // 使用订单号进行关联
                        where orderT.ship_id == shipId
                        group new { orderT, orderForm } by orderT.orderNumber into groupedOrders // 按订单号分组
                        select new OrderCommentData
                        {
                            OrderNumber = groupedOrders.Key, // 订单号
                            Estimate = groupedOrders
                                .Select(o => o.orderT.estimate)
                                .FirstOrDefault(), // 取评分
                            Comment = groupedOrders
                                .Select(o => o.orderForm.Comment)
                                .FirstOrDefault(), // 取评论
                            OrderDate = groupedOrders
                                .Select(o => o.orderT.end_time ?? DateTime.Today)
                                .FirstOrDefault() // 取日期
                        };

            return query.ToList();
        }

        public class OrderPay
        {
            public string OrderNumber { get; set; }
            public decimal TotalPrice { get; set; }
        }

        public List<OrderPay> GetOrdersByShipId(int shipId)
        {
            // 先执行查询，获取基本数据
            var results = db.orderT
                            .Where(o => o.ship_id == shipId && o.payment_status == "已支付")
                            .Select(o => new { o.orderNumber, o.total_price })
                            .ToList();

            // 在内存中进行转换
            return results.Select(r => new OrderPay
            {
                OrderNumber = r.orderNumber,
                TotalPrice = r.total_price ?? 0m // 处理 null 值
            }).ToList();
        }




        #region IDisposable 实现

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
