﻿using DDDC.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace DDDC.BLL
{
    public class OrderTServices
    {
        DataClasses1DataContext db = new DataClasses1DataContext();


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
                estimate  = "0",
                payment_status = paymentStatus
            };

            // 将新订单插入到数据库中
            db.orderT.InsertOnSubmit(newOrder);

            // 提交更改
            try
            {
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"插入订单失败：{ex.Message}");
                throw; // 可以根据需求记录日志或抛出异常
            }
        }
        public orderT GetPositionByOrdrNumber(String OrderNum)
        {
            return (from c in db.orderT
                    where c.orderNumber == OrderNum
                    select c).FirstOrDefault();

        }
        public void changeStatus(String OrderNum,decimal price)
        {
            var ss = db.orderT.SingleOrDefault(o => o.orderNumber == OrderNum);
            ss.end_time = DateTime.Now;
            ss.order_status = "已完成";
            ss.payment_status = "已支付";
            ss.total_price = price;
            db.SubmitChanges();
        }

        public void SubmitEst(String OrderNum, string evaluate)
        {
            var ss = db.orderT.SingleOrDefault(o => o.orderNumber == OrderNum);
            ss.estimate = evaluate;
            db.SubmitChanges();


        }

        public decimal GetTodayIncome(int shipId)
        {
            DateTime today = DateTime.Today;

            var income = db.orderT
                .Where(o => o.ship_id == shipId
                            && o.end_time.HasValue
                            && o.end_time.Value.Date == today.Date
                            && o.payment_status == "已支付")
                .Sum(o => (decimal?)o.total_price) ?? 0;

            return income;
        }

        public decimal GetYesterdayIncome(int shipId)
        {
            DateTime yesterday = DateTime.Today.AddDays(-1);

            var income = db.orderT
                .Where(o => o.ship_id == shipId
                            && o.end_time.HasValue
                            && o.end_time.Value.Date == yesterday.Date
                            && o.payment_status == "已支付")
                .Sum(o => (decimal?)o.total_price) ?? 0;

            return income;
        }

        public decimal GetMonthlyIncome(int shipId)
        {
            DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);

            var income = db.orderT
                .Where(o => o.ship_id == shipId
                            && o.end_time.HasValue
                            && o.end_time.Value >= firstDayOfMonth
                            && o.end_time.Value < firstDayOfNextMonth
                            && o.payment_status == "已支付")
                .Sum(o => (decimal?)o.total_price) ?? 0;

            return income;
        }


        public decimal GetTotalIncome(int shipId)
        {
            var income = db.orderT
                .Where(o => o.ship_id == shipId
                            && o.payment_status == "已支付")
                .Sum(o => (decimal?)o.total_price) ?? 0;

            return income;
        }


        public orderT GetorderTByOrdN(string ordn)
        {
            return (from c in db.orderT
                    where c.orderNumber == ordn
                    select c).FirstOrDefault();

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
                        join orderForm in db.OrderForm on orderT.ship_id equals orderForm.ShipID
                        where orderT.ship_id == shipId 
                        select new OrderCommentData
                        {
                            OrderNumber = orderT.orderNumber,
                            Estimate = orderT.estimate,
                            Comment = orderForm.Comment,
                            OrderDate = orderT.end_time.HasValue ? orderT.end_time.Value : DateTime.Today // 确保安全处理日期
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
            var query = from orderT in db.orderT
                        where orderT.ship_id == shipId && orderT.payment_status =="已支付"
                        select new OrderPay
                        {
                            OrderNumber = orderT.orderNumber,
                            TotalPrice =Convert.ToDecimal( orderT.total_price)
                            
                        };

            return query.ToList();
        }








    }

}



