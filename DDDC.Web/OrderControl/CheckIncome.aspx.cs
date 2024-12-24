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

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserID"] != null)
        {
            if (!IsPostBack)
        {
            // 绑定订单收入
            
                int userid = Convert.ToInt32(Session["UserID"]);
                var getshipid = driveService.GetShipsByOwnerID2(userid);
                int shipId = getshipid.ship_id;
                var user = userservice.GetUserByID(userid);

                BindOrders(getshipid.ship_id);

                // 绑定收入统计
                lblTodayIncome.Text = orderTServices.GetTodayIncome(shipId).ToString("C");
                lblYesterdayIncome.Text = orderTServices.GetYesterdayIncome(shipId).ToString("C");
                lblMonthIncome.Text = orderTServices.GetMonthlyIncome(shipId).ToString("C");
                lblTotalIncome.Text = orderTServices.GetTotalIncome(shipId).ToString("C");
                lblemail.Text = user.email;
                lblName.Text = user.user_name;
                lblShipName.Text = getshipid.ship_name;
                lblShipStatus.Text = getshipid.ship_status;


                // 设置柱状图数据
                var chartData = GetChartData(getshipid.ship_id);
                ChartData = Newtonsoft.Json.JsonConvert.SerializeObject(chartData);
                if (!string.IsNullOrEmpty(user.photo))
                {
                    Image2.ImageUrl = user.photo;
                }
                else
                {
                    Image2.ImageUrl = "~/UserImg/暂无图片.gif";
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
        "alert('请登录！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/login.aspx'; }, 100);", true);
        }
    
    }

    public void BindOrders(int shipId)
    {
        var orders = orderTServices.GetOrdersByShipId(shipId);
        RepeaterOrders.DataSource = orders;
        RepeaterOrders.DataBind();
    }

    public object GetChartData(int shipId)
    {
        return new
        {
            labels = new[] { "今日", "昨日", "本月", "总计" },
            data = new[]
            {
                orderTServices.GetTodayIncome(shipId),
                orderTServices.GetYesterdayIncome(shipId),
                orderTServices.GetMonthlyIncome(shipId),
                orderTServices.GetTotalIncome(shipId)
            }
        };
    }

    public string ChartData { get; set; }
}