using DDDC.BLL;
using DDDC.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FinishOrder_EndOrder : System.Web.UI.Page
{
    Userservice userService = new Userservice();
    OrderServices orderService = new OrderServices();
    DriveService driveService = new DriveService();
    OrderTServices ordert = new OrderTServices();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // 从Session中获取用户ID
            var ordt = ordert.GetorderTByOrdN(Session["EON123"].ToString());

            ScriptManager.RegisterStartupScript(this, this.GetType(), "initializeRating",
                    $"setTimeout(function() {{ updateRatingDisplay({ordt.estimate}); }}, 100);", true);
            int userID = Convert.ToInt32(Session["UserID"]);

            // 获取用户信息
            var user = userService.GetUserByID(userID);

            if (user != null)
            {
                lblemail.Text = user.email;
                lblName.Text = user.user_name;
                lblOrderNumber.Text = Session["EON123"].ToString();
                lblDriverName.Text = Session["eDrivername123"].ToString();
                lblDriverPhone.Text = Session["Ephone123"].ToString();
                lblStartPosition.Text = Session["EPP123"].ToString();
                lblDestination.Text = Session["EDE123"].ToString();
                imgShipPhoto.ImageUrl = Session["EImg123"].ToString();
                lblShipName.Text = Session["EShipName123"].ToString();
                txtComment.Text = Session["comment123"].ToString();
                CalculateAndDisplayCost();
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



    private void CalculateAndDisplayCost()
    {
        try
        {
            // 从 Session 中获取并解析路程值
            if (Session["EDistance123"] != null)
            {
                double distanceInMeters;

                // 支持直接解析为 double 或从字符串转换
                if (double.TryParse(Session["EDistance123"].ToString(), out distanceInMeters))
                {
                    // 转换为公里
                    double distanceInKilometers = distanceInMeters / 1000.0;

                    // 每公里费用
                    double costPerKilometer = 2.5;

                    // 起步价定义（如 5 元）
                    double startingPrice = 5.0;

                    // 计算总费用
                    double totalCost;

                    if (distanceInKilometers < 2)
                    {
                        // 路程不足 2 公里时使用起步价
                        totalCost = startingPrice;
                    }
                    else
                    {
                        // 否则按实际路程计算费用
                        totalCost = distanceInKilometers * costPerKilometer;
                    }

                    // 显示费用，保留两位小数
                    lblCost.Text = $"¥ {totalCost:F2}";
                }
                else
                {
                    lblCost.Text = "路程数据格式无效！";
                }
            }
            else
            {
                lblCost.Text = "路程数据不可用！";
            }
        }
        catch (Exception ex)
        {
            // 处理异常并显示友好提示
            lblCost.Text = "费用计算失败，请稍后重试！";
            // 可选：记录日志
            System.Diagnostics.Debug.WriteLine($"费用计算错误：{ex.Message}");
        }
    }
}