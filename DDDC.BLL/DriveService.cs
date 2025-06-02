using DDDC.DAL;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace DDDC.BLL
{
    public class DriveService : IDisposable
    {
        private DDDCModel1 db = new DDDCModel1();

        public void AddShip(int ownerID, string shipName, string shipType, int capacity, string shipStatus, DateTime shipRegTime, string picture, string isallow, string adminname)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    ShipHandle ship = new ShipHandle
                    {
                        owner_id = ownerID,
                        ship_name = shipName,
                        ship_type = shipType,
                        capacity = capacity,
                        ship_status = shipStatus,
                        ship_reg_time = shipRegTime,
                        Picture = picture,
                        IsAllowed = isallow,
                        Admin_name = adminname,
                    };
                    db.ShipHandle.Add(ship);
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"添加船只失败: {ex.Message}", ex);
                }
            }
        }

        public void AddShips(int shipid, int ownerID, string shipName, string shipType, int capacity, string shipStatus, DateTime shipRegTime, string picture)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    ships ship = new ships
                    {
                        ship_id = shipid,
                        owner_id = ownerID,
                        ship_name = shipName,
                        ship_type = shipType,
                        capacity = capacity,
                        ship_status = shipStatus,
                        ship_reg_time = shipRegTime,
                        Picture = picture,
                    };
                    db.ships.Add(ship);
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"添加船只失败: {ex.Message}", ex);
                }
            }
        }

        public void DeleteShip(int shipID)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    ships ship = db.ships.FirstOrDefault(s => s.ship_id == shipID);
                    if (ship != null)
                    {
                        db.ships.Remove(ship);
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                        throw new ArgumentException("未找到指定的船只ID，删除失败。");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"删除船只失败: {ex.Message}", ex);
                }
            }
        }

        public void UpdateShip(int shipID, string shipName, string shipType, int capacity, string shipStatus, DateTime shipRegTime, string picture)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    ships ship = db.ships.FirstOrDefault(s => s.ship_id == shipID);
                    if (ship != null)
                    {
                        ship.ship_name = shipName;
                        ship.ship_type = shipType;
                        ship.capacity = capacity;
                        ship.ship_status = shipStatus;
                        ship.ship_reg_time = shipRegTime;
                        ship.Picture = picture;

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                        throw new ArgumentException("未找到指定的船只ID，更新失败。");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"更新船只信息失败: {ex.Message}", ex);
                }
            }
        }

        public ships GetShipByID(int shipID)
        {
            return db.ships.FirstOrDefault(c => c.ship_id == shipID);
        }

        public ShipHandle GetHandleShipByShipID(int shipID)
        {
            return db.ShipHandle.FirstOrDefault(c => c.ship_id == shipID);
        }

        public ships GetShipsByOwnerID2(int ownerID)
        {
            return db.ships.FirstOrDefault(c => c.owner_id == ownerID);
        }

        public ShipHandle GetShipsByOwnerID3(int ownerID)
        {
            return db.ShipHandle.FirstOrDefault(c => c.owner_id == ownerID);
        }

        public IQueryable<ships> GetShipsByOwnerID(int ownerID)
        {
            return db.ships.Where(s => s.owner_id == ownerID);
        }

        public ships GetAllShips()
        {
            return db.ships.FirstOrDefault();
        }

        public class ShipSummaryDTO
        {
            public string Picture { get; set; }
            public string ShipName { get; set; }
            public string shiptype { get; set; }
            public int ShipID { get; set; }
            public string Capacity { get; set; }
            public string ship_status { get; set; }
        }

        public List<ShipSummaryDTO> GetShips()
        {
            return db.ships
                .Where(ship => ship.ship_status == "Available")
                .Select(ship => new ShipSummaryDTO
                {
                    ShipID = ship.ship_id,
                    Picture = ship.Picture,
                    ShipName = ship.ship_name,
                    Capacity = ship.capacity.ToString(),
                    shiptype = ship.ship_type,
                    ship_status = ship.ship_status,
                })
                .ToList();
        }

        public List<ShipSummaryDTO> GetShips1()
        {
            return db.ships
                .Select(ship => new ShipSummaryDTO
                {
                    ShipID = ship.ship_id,
                    Picture = ship.Picture,
                    ShipName = ship.ship_name,
                    Capacity = ship.capacity.ToString(),
                    shiptype = ship.ship_type,
                    ship_status = ship.ship_status,
                })
                .ToList();
        }

        public bool IsShipExist(int userID)
        {
            return db.ships.Any(c => c.owner_id == userID);
        }

        public bool IsShipExist1(int userID)
        {
            return db.ShipHandle.Any(c => c.owner_id == userID);
        }

        public void ApproveShip(int Shipid, string Adminname)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var order = db.ShipHandle.FirstOrDefault(o => o.ship_id == Shipid);

                    if (order != null)
                    {
                        order.IsAllowed = "同意";
                        order.Admin_name = Adminname;
                        order.ship_reg_time = DateTime.Now;

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                        throw new Exception("申请未找到");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"批准船只申请失败: {ex.Message}", ex);
                }
            }
        }

        public void RejectShip(int Shipid, string Adminname)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var order = db.ShipHandle.FirstOrDefault(o => o.ship_id == Shipid);

                    if (order != null)
                    {
                        order.IsAllowed = "拒绝";
                        order.Admin_name = Adminname;

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                        throw new Exception("申请未找到");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"拒绝船只申请失败: {ex.Message}", ex);
                }
            }
        }

        public void UpdateShipLocationByUserID(int userId, string province, string city, string position)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // 查询用户名下的船只（OwnerID 与 UserID 匹配）
                    var userShip = db.ships.FirstOrDefault(s => s.owner_id == userId);

                    if (userShip != null)
                    {
                        // 更新船只的位置信息
                        userShip.province = province;
                        userShip.city = city;
                        userShip.Position = position;

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                        throw new ArgumentException("未找到该用户名下的船只，无法更新位置信息。");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"更新船只位置信息失败: {ex.Message}", ex);
                }
            }
        }

        public void UpdateShipStatusByUserID(int userId, string status)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // 查询用户名下的第一艘船只
                    var userShip = db.ships.FirstOrDefault(s => s.owner_id == userId);

                    if (userShip != null)
                    {
                        // 更新船只状态
                        userShip.ship_status = status;

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                        throw new ArgumentException("未找到该用户名下的船只，无法更新状态。");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"更新船只状态失败: {ex.Message}", ex);
                }
            }
        }

        public void UpdateShipStatusByShipID(int shipid, string status)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // 查询特定ID的船只
                    var userShip = db.ships.FirstOrDefault(s => s.ship_id == shipid);

                    if (userShip != null)
                    {
                        // 更新船只状态
                        userShip.ship_status = status;

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                        throw new ArgumentException("未找到该用户名下的船只，无法更新状态。");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"更新船只状态失败: {ex.Message}", ex);
                }
            }
        }

        //Admin
        public class ShipLocation
        {
            public string ShipID { get; set; }
            public string ShipName { get; set; }
            public string Province { get; set; }
            public string City { get; set; }
            public string Position { get; set; }
        }

        public List<ShipLocation> GetAvailableShips()
        {
            // 从数据库中获取所有船只状态为 "Available" 的船只
            return db.ships
                .Where(s => s.ship_status == "Available")
                .Select(s => new ShipLocation
                {
                    ShipID = s.ship_id.ToString(),
                    ShipName = s.ship_name,
                    Province = s.province,
                    City = s.city,
                    Position = s.Position
                })
                .ToList();
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
