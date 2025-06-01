using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DDDC.DAL;

namespace DDDC.BLL
{
    public class Userservice : IDisposable
    {
        private DDDCModel1 db = new DDDCModel1();

        public int CheckLogin(string name, string password)
        {
            users user = db.users.FirstOrDefault(c => c.user_name == name && c.password == password);

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
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    users user = db.users.First(c => c.user_id == userID);
                    user.password = password;

                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"修改密码失败: {ex.Message}", ex);
                }
            }
        }

        public void ResetPassword(string name, string email)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    users user = db.users.First(c => c.user_name == name && c.email == email);
                    user.password = name;

                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"重置密码失败: {ex.Message}", ex);
                }
            }
        }

        public bool IsNameExist(string name)
        {
            return db.users.Any(c => c.user_name == name);
        }

        public bool IsEmailExist(string name, string email)
        {
            return db.users.Any(c => c.user_name == name && c.email == email);
        }

        public void Insert(string name, string Password, string Email)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    users user = new users
                    {
                        user_name = name,
                        password = Password,
                        email = Email,
                        Status = "customer",
                        UserSatus = "Normal"
                    };

                    db.users.Add(user);
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"插入用户失败: {ex.Message}", ex);
                }
            }
        }

        public void ChangeInfo(int userID, string userEmail)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    users user = db.users.First(c => c.user_id == userID);
                    user.email = userEmail;

                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"修改用户信息失败: {ex.Message}", ex);
                }
            }
        }

        public void ChangeUserName(int userID, string newName)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    users user = db.users.FirstOrDefault(c => c.user_id == userID);

                    if (user != null)
                    {
                        user.user_name = newName;
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
                    throw new Exception($"修改用户名失败: {ex.Message}", ex);
                }
            }
        }

        // 修改邮箱
        public void ChangeEmail(int userID, string newEmail)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    users user = db.users.FirstOrDefault(c => c.user_id == userID);

                    if (user != null)
                    {
                        user.email = newEmail;
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
                    throw new Exception($"修改邮箱失败: {ex.Message}", ex);
                }
            }
        }

        // 修改电话
        public void ChangePhone(int userID, string newPhone)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    users user = db.users.FirstOrDefault(c => c.user_id == userID);

                    if (user != null)
                    {
                        user.Phone = newPhone;
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
                    throw new Exception($"修改电话失败: {ex.Message}", ex);
                }
            }
        }

        // 上传头像图片，存储图片路径
        public void UploadPhoto(int userID, string photoPath)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    users user = db.users.FirstOrDefault(c => c.user_id == userID);

                    if (user != null)
                    {
                        user.photo = photoPath;
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
                    throw new Exception($"上传头像失败: {ex.Message}", ex);
                }
            }
        }

        // 获取用户信息
        public users GetUserByID(int userID)
        {
            return db.users.FirstOrDefault(c => c.user_id == userID);
        }

        public void ChangeUserStatus(int userID)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    users user = db.users.FirstOrDefault(c => c.user_id == userID);

                    if (user != null)
                    {
                        user.Status = "Driver";
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                        throw new Exception("用户不存在");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"修改用户状态失败: {ex.Message}", ex);
                }
            }
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
                        userstatu = user.UserSatus
                    })
                    .ToList();
        }

        public void AdminChangeUserStatus(int userID, string statu)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    users user = db.users.FirstOrDefault(c => c.user_id == userID);

                    if (user != null)
                    {
                        user.UserSatus = statu;
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                        throw new Exception("用户不存在");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"修改用户状态失败: {ex.Message}", ex);
                }
            }
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
