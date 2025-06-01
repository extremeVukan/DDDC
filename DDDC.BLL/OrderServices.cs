using DDDC.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DDDC.BLL
{
    public class OrderServices : IDisposable
    {
        // 数据上下文对象
        private DDDCModel1 db = new DDDCModel1();

        // 添加订单的方法
        public void AddOrder(string orderNumber, int clientId, string shipName, int shipId, int onwer_ID, string ownerName,
                              string prePosition, string destination, string notes, DateTime startTime,
                              DateTime endTime, string comment, string Shipimg, string meters, string status)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
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

                    // 将新订单对象添加到数据库中
                    db.OrderForm.Add(newOrder);

                    // 提交数据到数据库
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"添加订单失败: {ex.Message}", ex);
                }
            }
        }

        public void AcceptOrder(int orderId)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // 查找指定订单的记录
                    var order = db.OrderForm.FirstOrDefault(o => o.OrderID == orderId);

                    if (order != null)
                    {
                        // 更新订单状态为 "确认"
                        order.Status = "确认";

                        // 提交更改到数据库
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
                    throw new Exception($"接受订单失败: {ex.Message}", ex);
                }
            }
        }

        public void RejectOrder(int orderId)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // 查找指定订单的记录
                    var order = db.OrderForm.FirstOrDefault(o => o.OrderID == orderId);

                    if (order != null)
                    {
                        // 更新订单状态为 "已拒绝"
                        order.Status = "已拒绝";

                        // 提交更改到数据库
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
                    throw new Exception($"拒绝订单失败: {ex.Message}", ex);
                }
            }
        }

        public OrderForm GetOrderByorder_ID(int orderid)
        {
            return db.OrderForm.FirstOrDefault(c => c.OrderID == orderid);
        }

        public OrderForm GetOrderByOrdrNumber(string OrderNum)
        {
            return db.OrderForm.FirstOrDefault(c => c.OrderNumber == OrderNum);
        }

        public OrderForm GetOrdersByUserId(int Userid)
        {
            return db.OrderForm.FirstOrDefault(c => c.ClientID == Userid);
        }

        public class ShipSummaryDTO1
        {
            public string Ordernumber { get; set; }
            public string status { get; set; }
        }

        public List<ShipSummaryDTO1> GetOrder(int userid)
        {
            return db.OrderForm
                .Where(ord => ord.ClientID == userid && ord.Status != "已完成" && ord.Status != "已拒绝")
                .Select(ord1 => new ShipSummaryDTO1
                {
                    Ordernumber = ord1.OrderNumber,
                    status = ord1.Status,
                })
                .ToList();
        }

        public bool AddCommentToOrder(string orderNumber, string comment)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // 查询订单
                    var order = db.OrderForm.FirstOrDefault(o => o.OrderNumber == orderNumber);

                    if (order == null)
                    {
                        transaction.Rollback();
                        throw new Exception("未找到对应的订单！");
                    }

                    // 更新评论字段
                    order.Comment = comment;

                    // 保存更改到数据库
                    db.SaveChanges();
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // 记录错误日志
                    System.Diagnostics.Debug.WriteLine($"更新评论失败: {ex.Message}");
                    return false;
                }
            }
        }

        public void UpdateOrderStatus(string ordnum)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // 查找指定订单的记录
                    var order = db.OrderForm.FirstOrDefault(o => o.OrderNumber == ordnum);

                    if (order != null)
                    {
                        // 更新订单状态为 "已完成"
                        order.Status = "已完成";
                        order.End_Time = DateTime.Now;

                        // 提交更改到数据库
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

        public void UpdateOrderStatus1(string ordnum, string status)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // 查找指定订单的记录
                    var order = db.OrderForm.FirstOrDefault(o => o.OrderNumber == ordnum);

                    if (order != null)
                    {
                        // 更新订单状态
                        order.Status = status;

                        // 提交更改到数据库
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

        public bool IsUnfinishOrderExist(int userID)
        {
            return db.OrderForm.Any(c => c.OwnerID == userID && c.Status == "确认");
        }

        public void UpdateOrder(int orderID1, string ShipName1, int ShipID1, int OwnerID1, string OWName, string Position, string IMG)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // 查找指定订单的记录
                    var order = db.OrderForm.FirstOrDefault(o => o.OrderID == orderID1);

                    if (order != null)
                    {
                        // 更新订单信息
                        order.ShipName = ShipName1;
                        order.ShipID = ShipID1;
                        order.OwnerID = OwnerID1;
                        order.OwnerName = OWName;
                        order.PrePosition = Position;
                        order.img = IMG;

                        // 提交更改到数据库
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
                    throw new Exception($"更新订单信息失败: {ex.Message}", ex);
                }
            }
        }

        #region 新增的实用方法

        /// <summary>
        /// 获取指定用户的已完成订单
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>已完成订单列表</returns>
        public List<OrderForm> GetCompletedOrdersByUserId(int userId)
        {
            return db.OrderForm
                .Where(o => o.ClientID == userId && o.Status == "已完成")
                .OrderByDescending(o => o.End_Time)
                .ToList();
        }

        /// <summary>
        /// 获取指定船主的所有订单
        /// </summary>
        /// <param name="ownerId">船主ID</param>
        /// <returns>订单列表</returns>
        public List<OrderForm> GetOrdersByOwnerId(int ownerId)
        {
            return db.OrderForm
                .Where(o => o.OwnerID == ownerId)
                .OrderByDescending(o => o.Start_Time)
                .ToList();
        }

        /// <summary>
        /// 获取特定时间范围内的订单
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>订单列表</returns>
        public List<OrderForm> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            return db.OrderForm
                .Where(o => o.Start_Time >= startDate && o.Start_Time <= endDate)
                .OrderByDescending(o => o.Start_Time)
                .ToList();
        }

        /// <summary>
        /// 更新订单评分
        /// </summary>
        /// <param name="orderNumber">订单编号</param>
        /// <param name="rating">评分</param>
        /// <returns>是否更新成功</returns>
        
        

        #endregion

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
