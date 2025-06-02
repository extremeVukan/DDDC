using DDDC.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using DDDC.DAL;
public partial class OrderControl_CheckIncome : System.Web.UI.Page
{
    OrderTServices orderTServices = new OrderTServices();
    Userservice userservice = new Userservice();
    DriveService driveService = new DriveService();

    // 初始化 ChartData 属性以避免为 null
    public string ChartData { get; set; } = "{ \"labels\": [], \"data\": [] }";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                "alert('请登录！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/login.aspx'; }, 100);", true);
            return;
        }

        try
        {
            if (!IsPostBack)
            {
                int userid = Convert.ToInt32(Session["UserID"]);
                var getshipid = driveService.GetShipsByOwnerID2(userid);

                if (getshipid == null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                        "alert('未找到您的船只信息！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/SelifInfo_Web/Ships_info.aspx'; }, 100);", true);
                    return;
                }

                int shipId = getshipid.ship_id;
                var user = userservice.GetUserByID(userid);

                // 绑定用户信息
                lblemail.Text = user.email;
                lblName.Text = user.user_name;
                lblShipName.Text = getshipid.ship_name;
                lblShipStatus.Text = getshipid.ship_status;

                if (!string.IsNullOrEmpty(user.photo))
                {
                    Image2.ImageUrl = user.photo;
                }
                else
                {
                    Image2.ImageUrl = "~/UserImg/暂无图片.gif";
                }

                // 绑定订单收入
                BindOrders(shipId);

                // 绑定收入统计
                decimal todayIncome = orderTServices.GetTodayIncome(shipId);
                decimal yesterdayIncome = orderTServices.GetYesterdayIncome(shipId);
                decimal monthIncome = orderTServices.GetMonthlyIncome(shipId);
                decimal totalIncome = orderTServices.GetTotalIncome(shipId);

                lblTodayIncome.Text = todayIncome.ToString("C");
                lblYesterdayIncome.Text = yesterdayIncome.ToString("C");
                lblMonthIncome.Text = monthIncome.ToString("C");
                lblTotalIncome.Text = totalIncome.ToString("C");

                // 设置柱状图数据
                ChartData = GetChartDataJson(shipId);
            }
        }
        catch (Exception ex)
        {
            // 记录错误并显示友好的错误信息
            System.Diagnostics.Debug.WriteLine($"加载收入数据出错: {ex.Message}");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                $"alert('加载收入数据出错: {ex.Message}');", true);
        }
    }

    public void BindOrders(int shipId)
    {
        try
        {
            var orders = orderTServices.GetOrdersByShipId(shipId);
            RepeaterOrders.DataSource = orders;
            RepeaterOrders.DataBind();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"绑定订单数据出错: {ex.Message}");
        }
    }

    public string GetChartDataJson(int shipId)
    {
        try
        {
            decimal todayIncome = orderTServices.GetTodayIncome(shipId);
            decimal yesterdayIncome = orderTServices.GetYesterdayIncome(shipId);
            decimal monthIncome = orderTServices.GetMonthlyIncome(shipId);
            decimal totalIncome = orderTServices.GetTotalIncome(shipId);

            // 将小数转换为可以正确序列化的格式
            var chartData = new
            {
                labels = new[] { "今日", "昨日", "本月", "总计" },
                data = new[]
                {
                    Convert.ToDouble(todayIncome),
                    Convert.ToDouble(yesterdayIncome),
                    Convert.ToDouble(monthIncome),
                    Convert.ToDouble(totalIncome)
                }
            };

            return JsonConvert.SerializeObject(chartData);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"生成图表数据出错: {ex.Message}");
            // 返回一个空的有效 JSON 对象
            return "{ \"labels\": [\"今日\", \"昨日\", \"本月\", \"总计\"], \"data\": [0, 0, 0, 0] }";
        }
    }
}
