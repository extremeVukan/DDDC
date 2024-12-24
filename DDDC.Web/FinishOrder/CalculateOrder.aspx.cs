using DDDC.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FinishOrder_CalculateOrder : System.Web.UI.Page
{
    Userservice userService = new Userservice();
    OrderServices orderService = new OrderServices();
    DriveService driveService = new DriveService();
    OrderTServices ordert = new OrderTServices();
    MessageServices MsgStrv = new MessageServices();
    protected void Page_Load(object sender, EventArgs e)
    {
        var ordt = ordert.GetorderTByOrdN(Session["EON"].ToString());
        var comm = orderService.GetOrderByOrdrNumber(Session["EON"].ToString());
        ScriptManager.RegisterStartupScript(this, this.GetType(), "initializeRating",
                $"setTimeout(function() {{ updateRatingDisplay({ordt.estimate}); }}, 100);", true);
        
        txtComment.Text = comm.Comment;
        if (!IsPostBack)
        {
            
            // 从Session中获取用户ID
            int userID = Convert.ToInt32(Session["UserID"]);

            // 获取用户信息
            var user = userService.GetUserByID(userID);

            if (user != null)
            {
                lblemail.Text = user.email;
                lblName.Text = user.user_name;
                lblOrderNumber.Text = Session["EON"].ToString();
                lblDriverName.Text = Session["eDrivername"].ToString();
                lblDriverPhone.Text = Session["Ephone"].ToString();
                lblStartPosition.Text = Session["EPP"].ToString();
                lblDestination.Text = Session["EDE"].ToString();
                imgShipPhoto.ImageUrl = Session["EImg"].ToString();
                lblShipName.Text = Session["EShipName"].ToString();
                
                

                

                


                CalculateAndDisplayCost();

                // 显示头像
                if (!string.IsNullOrEmpty(user.photo))
                {
                    Image2.ImageUrl = user.photo;
                }
                else
                {
                    Image2.ImageUrl = "~/UserImg/暂无图片.gif";
                }

                // 注入脚本，设置前端评分显示
                ScriptManager.RegisterStartupScript(this, this.GetType(), "initializeRating",
                    $"setTimeout(function() {{ updateRatingDisplay({ordt.estimate}); }}, 100);", true);
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
            if (Session["EDistance"] != null)
            {
                double distanceInMeters;

                // 支持直接解析为 double 或从字符串转换
                if (double.TryParse(Session["EDistance"].ToString(), out distanceInMeters))
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
                    lblCost.Text = $" {totalCost:F2}";
                }
                else
                {
                    lblCost.Text = Session["EDistance"].ToString();
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


    protected void btnSubmitComment_Click(object sender, EventArgs e)
    {
        var ord = orderService.GetOrderByOrdrNumber(lblOrderNumber.Text);
        string Comment = txtComment.Text;
        string eva = hfRating.Value.ToString();
        if (eva=="")
        {
            ordert.SubmitEst(lblOrderNumber.Text,"3");
            orderService.AddCommentToOrder(lblOrderNumber.Text, Comment);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
            "alert('评论成功！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/FinishOrder/CalculateOrder.aspx'; }, 100);", true);

        }
        else
        {
            ordert.SubmitEst(lblOrderNumber.Text, eva);
            orderService.AddCommentToOrder(lblOrderNumber.Text, Comment);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
            "alert('评论成功！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/FinishOrder/CalculateOrder.aspx'; }, 100);", true);

        }
        
    }
    protected void btnEva_Click(object sender, EventArgs e)
    {
        var ord = orderService.GetOrderByOrdrNumber(lblOrderNumber.Text);
        string Comment = txtComment.Text;
        string eva = hfRating.Value.ToString();
        if (eva == "")
        {
      
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('您未输入评分！');", true);
        }
        else
        {
            ordert.SubmitEst(lblOrderNumber.Text, eva);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
            "alert('评分成功！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/FinishOrder/CalculateOrder.aspx'; }, 100);", true);

        }
        

        
    }
        protected void btnConfirmPayment_Click(object sender, EventArgs e)
    {
        // 支付成功的后端逻辑，例如记录支付状态
        string orderNumber = Session["EON"]?.ToString(); // 示例：获取订单号
        if (!string.IsNullOrEmpty(orderNumber))
        {
            
            orderService.UpdateOrderStatus(orderNumber); // 更新订单状态为已完成
           var msg = orderService.GetOrderByOrdrNumber(orderNumber);

            driveService.UpdateShipStatusByShipID(Convert.ToInt32(msg.ShipID), "Available");// 更新船只状态
            decimal price = Convert.ToDecimal(lblCost.Text);
            ordert.changeStatus(orderNumber,price);


            string HeadText = "您的订单" + orderNumber + "已完成,请注意查收";
           int Cid = Convert.ToInt32(msg.OwnerID);
            MsgStrv.addMsg(HeadText, Cid, Cid, "订单已完成", "司机信息", DateTime.Now, "未读");
        }

        // 可选：添加更多支付逻辑，如生成支付记录、更新账户余额等
        // ...

        // 页面反馈或重定向
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
            "alert('支付成功！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/ClientOrder/COrder.aspx'; }, 100);", true);
    }

}
