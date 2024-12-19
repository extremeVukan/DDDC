using DDDC.DAL;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace DDDC.BLL
{
    public class DriveService
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        public void AddShip(int ownerID, string shipName, string shipType, int capacity, string shipStatus, DateTime shipRegTime, string picture,string isallow,string adminname)
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
            db.ShipHandle.InsertOnSubmit(ship);
            db.SubmitChanges();
        }
        public void AddShips(int shipid, int ownerID, string shipName, string shipType, int capacity, string shipStatus, DateTime shipRegTime, string picture)
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
            db.ships.InsertOnSubmit(ship);
            db.SubmitChanges();
        }

        public void DeleteShip(int shipID)
        {
            ships ship = db.ships.SingleOrDefault(s => s.ship_id == shipID);
            if (ship != null)
            {
                db.ships.DeleteOnSubmit(ship);
                db.SubmitChanges();
            }
            else
            {
                throw new ArgumentException("未找到指定的船只ID，删除失败。");
            }
        }

        public void UpdateShip(int shipID, string shipName, string shipType, int capacity, string shipStatus, DateTime shipRegTime, string picture)
        {
            ships ship = db.ships.SingleOrDefault(s => s.ship_id == shipID);
            if (ship != null)
            {
                
                ship.ship_name = shipName;
                ship.ship_type = shipType;
                ship.capacity = capacity;
                ship.ship_status = shipStatus;
                ship.ship_reg_time = shipRegTime;
                ship.Picture = picture;

                db.SubmitChanges();
            }
            else
            {
                throw new ArgumentException("未找到指定的船只ID，更新失败。");
            }
        }

        public ships GetShipByID(int shipID)
        {
            return (from c in db.ships
                    where c.ship_id == shipID
                    select c).FirstOrDefault();

        }

        public ShipHandle GetHandleShipByShipID(int shipID)
        {
            return (from c in db.ShipHandle
                    where c.ship_id == shipID
                    select c).FirstOrDefault();

        }

        public ships GetShipsByOwnerID2(int ownerID)
        {
            return (from c in db.ships
                    where c.owner_id == ownerID
                    select c).FirstOrDefault();
        }
        public ShipHandle GetShipsByOwnerID3(int ownerID)
        {
            return (from c in db.ShipHandle
                    where c.owner_id == ownerID
                    select c).FirstOrDefault();
        }
        public IQueryable<ships> GetShipsByOwnerID(int ownerID)
        {
            return db.ships.Where(s => s.owner_id == ownerID);
        }
        public ships GetAllShips()
        {
            return (from ship in db.ships
                    select ship).FirstOrDefault();
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

            ships ships1 = (from c in db.ships
                          where c.owner_id == userID
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
        public bool IsShipExist1(int userID)
        {

            ShipHandle ships1 = (from c in db.ShipHandle
                                 where c.owner_id == userID
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

        public void ApproveShip(int Shipid ,string Adminname)
        {
            
            var order = db.ShipHandle.SingleOrDefault(o => o.ship_id == Shipid);

            if (order != null)
            {
                
                order.IsAllowed = "同意";
                order.Admin_name = Adminname;
                order.ship_reg_time = DateTime.Now;
                // 提交更改到数据库
                db.SubmitChanges();
            }
            else
            {
                // 如果找不到订单，你可以抛出一个异常或者进行错误处理
                throw new Exception("申请未找到");
            }
        }


        public void RejectShip(int Shipid, string Adminname)
        {

            var order = db.ShipHandle.SingleOrDefault(o => o.ship_id == Shipid);

            if (order != null)
            {

                order.IsAllowed = "拒绝";
                order.Admin_name = Adminname;
                // 提交更改到数据库
                db.SubmitChanges();
            }
            else
            {
                // 如果找不到订单，你可以抛出一个异常或者进行错误处理
                throw new Exception("申请未找到");
            }
        }







        public void UpdateShipLocationByUserID(int userId, string province, string city, string position)
        {
            // 查询用户名下的船只（OwnerID 与 UserID 匹配）
            var userShip = db.ships.FirstOrDefault(s => s.owner_id == userId);

            if (userShip != null)
            {
                // 更新船只的位置信息
                userShip.province = province;
                userShip.city = city;
                userShip.Position = position;

                // 提交更改到数据库
                db.SubmitChanges();
            }
            else
            {
                throw new ArgumentException("未找到该用户名下的船只，无法更新位置信息。");
            }
        }

        public void UpdateShipStatusByUserID(int userId, string status)
        {
            // 查询用户名下的第一艘船只
            var userShip = db.ships.FirstOrDefault(s => s.owner_id == userId);

            if (userShip != null)
            {
                // 更新船只状态
                userShip.ship_status = status;

                // 提交更改到数据库
                db.SubmitChanges();
            }
            else
            {
                throw new ArgumentException("未找到该用户名下的船只，无法更新状态。");
            }
        }
        public void UpdateShipStatusByShipID(int shipid, string status)
        {
            // 查询用户名下的第一艘船只
            var userShip = db.ships.FirstOrDefault(s => s.ship_id == shipid);

            if (userShip != null)
            {
                // 更新船只状态
                userShip.ship_status = status;

                // 提交更改到数据库
                db.SubmitChanges();
            }
            else
            {
                throw new ArgumentException("未找到该用户名下的船只，无法更新状态。");
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
            var ships = (from s in db.ships
                         where s.ship_status == "Available"
                         select new ShipLocation
                         {
                             ShipID = s.ship_id.ToString(),
                             ShipName = s.ship_name,
                             Province = s.province,
                             City = s.city,
                             Position = s.Position
                         }).ToList();

            return ships;
        }

    }
}
