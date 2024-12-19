using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDDC.DAL;



namespace DDDC.BLL
{
    public class Userservice
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        public int CheckLogin(string name, string password)
        {
            users user = (from c in db.users
                              where c.user_name == name && c.password == password
                                 select c).FirstOrDefault();

            if (user != null) 
            {
                return user.user_id;
            }
            else
            {
                return 0;
            }
        }

     
        public void ChangePassword(int userID, string password)
        {
            users user = (from c in db.users
                                 where c.user_id == userID
                                 select c).First();
            user.password = password;

            db.SubmitChanges();
        }

         
        public void ResetPassword(string name, string email)
        {
            users user = (from c in db.users
                                 where c.user_name == name && c.email == email
                                 select c).First();
            user.password = name;
            db.SubmitChanges();
        }

       
        public bool IsNameExist(string name)
        {
            
            users user = (from c in db.users
                                 where c.user_name == name
                                 select c).FirstOrDefault();
            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

       
        public bool IsEmailExist(string name, string email)
        {
            users user = (from c in db.users
                                 where c.user_name == name && c.email == email
                                 select c).FirstOrDefault();
            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

      
        public void Insert(string name, string Password, string Email)
        {
            users user = new users
            {
                user_name = name,
                password = Password,
                email = Email,
                Status = "customer",
                UserSatus = "Normal"
            };
            db.users.InsertOnSubmit(user);
            db.SubmitChanges();
        }


        public void ChangeInfo(int userID, string userEmail)
        {
            users user = (from c in db.users
                          where c.user_id == userID
                          select c).First();
            user.email = userEmail;

            db.SubmitChanges();
        }
        public void ChangeUserName(int userID, string newName)
        {
            users user = (from c in db.users
                          where c.user_id == userID
                          select c).FirstOrDefault();

            if (user != null)
            {
                user.user_name = newName;
                db.SubmitChanges();
            }
        }

        // 修改邮箱
        public void ChangeEmail(int userID, string newEmail)
        {
            users user = (from c in db.users
                          where c.user_id == userID
                          select c).FirstOrDefault();

            if (user != null)
            {
                user.email = newEmail;
                db.SubmitChanges();
            }
        }

        // 修改电话
        public void ChangePhone(int userID, string newPhone)
        {
            users user = (from c in db.users
                          where c.user_id == userID
                          select c).FirstOrDefault();

            if (user != null)
            {
                user.Phone = newPhone;
                db.SubmitChanges();
            }
        }

        // 上传头像图片，存储图片路径
        public void UploadPhoto(int userID, string photoPath)
        {
            users user = (from c in db.users
                          where c.user_id == userID
                          select c).FirstOrDefault();

            if (user != null)
            {
                user.photo = photoPath;
                db.SubmitChanges();
            }
        }

        // 获取用户信息
        public users GetUserByID(int userID)
        {
            return (from c in db.users
                    where c.user_id == userID
                    select c).FirstOrDefault();
        }



        public void ChangeUserStatus(int userID)
        {
            users user = (from c in db.users
                          where c.user_id == userID
                          select c).SingleOrDefault();
            user.Status = "Driver";

            db.SubmitChanges();
        }
        public class ShipSummaryDTO
        {
            public int Userid { get; set; }
            public string username { get; set; }

            public string email { get; set; }
            public string phone { get; set; }
            public string phote { get; set; }
            public string status { get; set; }
            public string userstatu { get; set; }
        }
        public List<ShipSummaryDTO> GetUserlist()
        {
            return db.users

                     .Select(user => new ShipSummaryDTO
                     {
                         Userid = user.user_id,
                         username = user.user_name,
                         email = user.email,
                         phone = user.Phone,
                         phote = user.photo,
                         status = user.Status,
                         userstatu =user.UserSatus
                     })
                     .ToList();
        }
        public void AdminChangeUserStatus(int userID,string statu)
        {
            users user = (from c in db.users
                          where c.user_id == userID
                          select c).SingleOrDefault();
            user.UserSatus = statu;

            db.SubmitChanges();
        }
    }
}

