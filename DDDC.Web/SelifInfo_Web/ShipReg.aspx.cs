using DDDC.BLL;
using DDDC.DAL;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class SelifInfo_Web_ShipReg : System.Web.UI.Page
{
    // 服务类实例
    Userservice userService = new Userservice();
    DriveService driveService = new DriveService(); // 用于操作船只信息的服务类

    protected void Page_Load(object sender, EventArgs e)
    {
        int userID = Convert.ToInt32(Session["UserID"]);

        // 获取用户信息
        var user = userService.GetUserByID(userID);
        var userShips = driveService.GetShipsByOwnerID(userID);
        //在ships表根据Userid查询是否名下有船只，如果有，跳转页面


        if (driveService.IsShipExist(userID))
        {
            Response.Redirect("http://localhost:51058/SelifInfo_Web/Ships_info.aspx");
        }
        else
        {
            if (driveService.IsShipExist1(userID))
            {
                Response.Redirect("http://localhost:51058/SelifInfo_Web/HavedSubmitShip.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    // 从 Session 中获取用户 ID


                    if (user != null)
                    {
                        // 加载个人信息
                        lblemail.Text = user.email;
                        lblName.Text = user.user_name;
                        var firstShip = userShips.FirstOrDefault();
                        if (firstShip != null && !string.IsNullOrEmpty(firstShip.Picture))
                        {
                            Image3.ImageUrl = firstShip.Picture;
                        }
                        else
                        {
                            Image3.ImageUrl = "~/UserImg/暂无图片.gif"; // 如果没有船或没有图片
                        }

                        // 显示头像，如果没有头像则显示默认头像
                        if (!string.IsNullOrEmpty(user.photo))
                        {
                            Image2.ImageUrl = user.photo;

                        }
                        else
                        {
                            Image2.ImageUrl = "~/UserImg/暂无图片.gif";
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                            "alert('请登录！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/login.aspx'; }, 100);", true);
                    }
                }
            }
        }

    }
    // 处理图片上传并显示在 img3 中
    protected void Unnamed1_Click(object sender, EventArgs e)
    {
        if (fuShipPhoto.HasFile)
        {
            try
            {
                // 获取上传文件的路径
                string fileName = fuShipPhoto.FileName;
                string filePath = "~/UserImg/" + fileName;

                // 将文件保存到服务器
                fuShipPhoto.SaveAs(Server.MapPath(filePath));

                // 将文件路径存储到 ViewState 中，以便在提交时使用
                ViewState["UploadedImagePath"] = filePath;

                // 显示上传的图片
                Image3.ImageUrl = filePath;
            }
            catch (Exception ex)
            {
                // 显示上传失败信息
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    $"alert('图片上传失败：{ex.Message}');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                "alert('请选择图片！');", true);
        }
    }

    // 提交船只注册信息到数据库
    // 提交船只注册信息到数据库
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int userID = Convert.ToInt32(Session["UserID"]);
        try
        {
            var self = userService.GetUserByID(Convert.ToInt32(Session["userID"]));
            if (self.Phone == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                        "alert('请注册手机号！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/SelifInfo_Web/Self_Info.aspx'; }, 100);", true);
            }



            // 验证用户是否已登录
            else if (Session["UserID"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "alert('请登录！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/login.aspx'; }, 100);", true);

            }
            else
            {
                int ownerID = Convert.ToInt32(Session["UserID"]);

                // 获取输入的船只信息
                string shipName = txtShipName.Text;
                string shipType = txtShipType.Text;
                int capacity = int.Parse(txtShipCapacity.Text);
                string shipStatus = "Available"; // 默认状态
                DateTime shipRegTime = DateTime.Now;

                // 获取上传图片路径
                string Picture1 = Image3.ImageUrl ?? "~/UserImg/暂无图片.gif";
                string ALLOW = "待审核";
                string admin = "无";
                // 调用DriveService中的AddShip方法，将船只信息插入数据库
                driveService.AddShip(ownerID, shipName, shipType, capacity, shipStatus, shipRegTime, Picture1, ALLOW, admin);

                // 提交成功提示
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                            "alert('船只信息提交成功！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/SelifInfo_Web/HavedSubmitShip.aspx'; }, 100);", true);
            }
        }

        // 从Session中获取当前用户的ID

        catch (Exception ex)
        {
            // 输出异常信息到日志或控制台，便于调试
            Console.WriteLine($"Error in btnSubmit_Click: {ex.Message}");

            // 提交失败提示
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                $"alert('提交失败：{ex.Message}');", true);
        }
    }
}
