using DDDC.BLL;
using DDDC.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class OrderControl_HandlingOrder : System.Web.UI.Page
{
    Userservice userService = new Userservice();
    OrderServices orderService = new OrderServices();
    DriveService driveService = new DriveService();
    OrderTServices orderTServices = new OrderTServices();
    MessageServices MsgStrv = new MessageServices();
    protected void Page_Load(object sender, EventArgs e)
    {
        int userID = Convert.ToInt32(Session["UserID"]);

        if (Session["UserID"] == null)
        {
            LinqDataSource1.Where = "1 == 0"; // 没有数据
        }
        if (!IsPostBack)
        {
            // 从Session中获取用户ID


            // 获取用户信息
            var user = userService.GetUserByID(userID);
            var ship1 = driveService.GetShipsByOwnerID2(userID);
            if (user != null)
            {
                lblemail.Text = user.email;
                lblName.Text = user.user_name;
                lblShipName.Text = ship1.ship_name;
                lblShipStatus.Text =ship1.ship_status;
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

    protected void btnAcceptOrder_Click(object sender, EventArgs e)
    {
        int userID = Convert.ToInt32(Session["UserID"]);
        var ship = driveService.GetShipsByOwnerID2(userID);
        string ShipL = ship.province + ship.city + ship.Position;
        if (orderService.IsUnfinishOrderExist(userID))
        {
            
            
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
        "alert('您还有订单尚未完成！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/OrderControl/ProducingOrder.aspx'; }, 100);", true);


        }
        else {

            if (ship.ship_status == "Occupied" || ship.ship_status == "Offline")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
        "alert('您的船只状态异常！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/SelifInfo_Web/Ships_info.aspx'; }, 100);", true);
            }
            else
            {
                // 获取按钮的 CommandArgument，表示被点击的订单的 OrderID
                Button btn = (Button)sender;
                int orderId = Convert.ToInt32(btn.CommandArgument);  // 从CommandArgument获取OrderID
                var ord = orderService.GetOrderByorder_ID(orderId);
                // 创建 OrderServices 实例

                try
                {
                    // 调用 AcceptOrder 方法来更新订单状态
                    orderService.AcceptOrder(orderId);
                    orderTServices.AddOrderInfo(ord.OrderID, ord.OrderNumber, Convert.ToInt32(ord.ClientID), Convert.ToInt32(ord.ShipID), "确认", ord.PrePosition, ord.Destination, ShipL, ord.Start_Time ?? DateTime.MinValue, ord.End_Time ?? DateTime.MinValue, 0, "未支付");

                    
                    int cID = Convert.ToInt32(ord.ClientID);
                    string HeadText = "亲爱的用户，您的订单" + ord.OrderNumber + "已接单，请注意查看！";
                    string Msg = "亲爱的用户您由" + ord.PrePosition + "开往" + ord.Destination + "的订单已由司机确认,请确认订单信息无误后到达预定位置！";
                    MsgStrv.addMsg(HeadText, cID, userID, Msg, "订单", DateTime.Now, "未读");



                    // 提示用户操作成功
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                "alert('订单已成功接受并确认！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/OrderControl/ProducingOrder.aspx'; }, 100);", true);

                    driveService.UpdateShipStatusByUserID(userID, "Occupied");



                }
                catch (Exception ex)
                {
                    // 如果发生异常，显示错误信息

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('操作失败:！'+ ex.Message);", true);
                }

                // 重新绑定 GridView 数据，确保显示的数据更新
                ctl02.DataBind();

            }
        }
        
       

    }


    protected void btnrejectOrder_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int orderId = Convert.ToInt32(btn.CommandArgument);  // 从CommandArgument获取OrderID
        var ord = orderService.GetOrderByorder_ID(orderId);
        // 创建 OrderServices 实例
        int userID = Convert.ToInt32(Session["UseriD"]);
        try
        {
            // 调用 AcceptOrder 方法来更新订单状态
            orderService.RejectOrder(orderId);

            // 提示用户操作成功
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('订单已取消！');", true);
            int cID = Convert.ToInt32(ord.ClientID);
            string HeadText = "亲爱的用户，您的订单" + ord.OrderNumber + "已被取消，请注意查看！";
            string Msg = "亲爱的用户您由" + ord.PrePosition + "开往" + ord.Destination + "的订单已被取消,请重新下单！";
            MsgStrv.addMsg(HeadText, cID, userID, Msg, "订单", DateTime.Now, "未读");


        }
        catch (Exception ex)
        {
            // 如果发生异常，显示错误信息

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('操作失败:！'+ ex.Message);", true);
        }

        // 重新绑定 GridView 数据，确保显示的数据更新
        ctl02.DataBind();
    }

    protected void ctl02_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ctl02.PageIndex = e.NewPageIndex;

        // 重新绑定数据源
        ctl02.DataBind();
    }
}