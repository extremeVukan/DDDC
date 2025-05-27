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
        if (!IsPostBack)
        {
            // 从Session中获取用户ID和订单信息
            int userID = Convert.ToInt32(Session["UserID"]);
            LoadUserAndOrderInfo(userID);
        }
        else
        {
            // 处理PostBack后的评分和评论显示
            UpdateRatingAndCommentDisplay();
        }
    }

    private void LoadUserAndOrderInfo(int userID)
    {
        var user = userService.GetUserByID(userID);

        if (user == null)
        {
            RedirectToLogin();
            return;
        }

        // 加载用户基本信息
        lblemail.Text = user.email;
        lblName.Text = user.user_name;

        // 设置用户头像
        Image2.ImageUrl = !string.IsNullOrEmpty(user.photo) ? user.photo : "~/UserImg/暂无图片.gif";

        // 加载订单信息
        if (Session["EON"] != null)
        {
            string orderNumber = Session["EON"].ToString();

            // 获取订单和评价信息
            var orderT = ordert.GetorderTByOrdN(orderNumber);
            var orderForm = orderService.GetOrderByOrdrNumber(orderNumber);

            if (orderT != null && orderForm != null)
            {
                // 设置订单基本信息
                lblOrderNumber.Text = orderNumber;
                lblDriverName.Text = Session["eDrivername"]?.ToString() ?? "";
                lblDriverPhone.Text = Session["Ephone"]?.ToString() ?? "";
                lblStartPosition.Text = Session["EPP"]?.ToString() ?? "";
                lblDestination.Text = Session["EDE"]?.ToString() ?? "";
                imgShipPhoto.ImageUrl = Session["EImg"]?.ToString() ?? "";
                lblShipName.Text = Session["EShipName"]?.ToString() ?? "";

                // 设置评论文本
                txtComment.Text = orderForm.Comment;

                // 计算并显示费用
                CalculateAndDisplayCost();

                // 设置评分显示
                string estimate = orderT.estimate ?? "0";
                hfRating.Value = estimate;

                // 注册脚本来初始化评分显示
                ScriptManager.RegisterStartupScript(this, this.GetType(), "initializeRating",
                    $"setTimeout(function() {{ updateRatingDisplay('{estimate}'); }}, 100);", true);
            }
        }
    }

    private void UpdateRatingAndCommentDisplay()
    {
        if (Session["EON"] != null)
        {
            string orderNumber = Session["EON"].ToString();
            var orderT = ordert.GetorderTByOrdN(orderNumber);
            var orderForm = orderService.GetOrderByOrdrNumber(orderNumber);

            if (orderT != null && orderForm != null)
            {
                // 使用AJAX更新评分显示，避免整页刷新
                string rating = orderT.estimate ?? "0";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "updateRating",
                    $"updateRatingDisplay('{rating}');", true);
            }
        }
    }

    private void RedirectToLogin()
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
            "alert('请登录！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/login.aspx'; }, 100);", true);
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
                    lblCost.Text = $"{totalCost:F2}";
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
        string orderNumber = lblOrderNumber.Text;
        string comment = txtComment.Text;
        string rating = hfRating.Value;

        try
        {
            // 默认评分为3星（如果用户未评分）
            if (string.IsNullOrEmpty(rating))
            {
                rating = "3";
            }

            // 保存评论和评分
            ordert.SubmitEst(orderNumber, rating);
            orderService.AddCommentToOrder(orderNumber, comment);

            // 更新UI状态，显示成功消息
            lblCommentStatus.Text = "评价提交成功！";
            lblCommentStatus.CssClass = "status-message success";
            lblCommentStatus.Visible = true;

            // 使用AJAX更新评分显示
            ScriptManager.RegisterStartupScript(this, this.GetType(), "updateRatingAfterSubmit",
                $"updateRatingDisplay('{rating}');", true);

            // 避免使用整页刷新或弹窗
        }
        catch (Exception ex)
        {
            // 显示错误消息
            lblCommentStatus.Text = "评价提交失败，请稍后重试！";
            lblCommentStatus.CssClass = "status-message error";
            lblCommentStatus.Visible = true;

            // 记录错误日志
            System.Diagnostics.Debug.WriteLine($"提交评价失败：{ex.Message}");
        }
    }

    protected void btnConfirmPayment_Click(object sender, EventArgs e)
    {
        string orderNumber = Session["EON"]?.ToString();

        if (string.IsNullOrEmpty(orderNumber))
        {
            return;
        }

        try
        {
            // 更新订单状态
            orderService.UpdateOrderStatus(orderNumber);

            // 获取订单信息
            var order = orderService.GetOrderByOrdrNumber(orderNumber);

            if (order != null)
            {
                // 更新船只状态
                driveService.UpdateShipStatusByShipID(Convert.ToInt32(order.ShipID), "Available");

                // 更新支付状态和金额
                decimal price = Convert.ToDecimal(lblCost.Text);
                ordert.changeStatus(orderNumber, price);

                // 发送消息通知
                string headText = $"您的订单{orderNumber}已完成,请注意查收";
                int driverId = Convert.ToInt32(order.OwnerID);
                MsgStrv.addMsg(headText, driverId, driverId, "订单已完成", "司机信息", DateTime.Now, "未读");

                // 重定向到订单页面
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect",
                    "setTimeout(function(){ window.location.href = 'http://localhost:51058/ClientOrder/COrder.aspx'; }, 100);", true);
            }
        }
        catch (Exception ex)
        {
            // 显示错误信息
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                $"alert('支付过程中出现错误：{ex.Message}');", true);

            // 记录错误日志
            System.Diagnostics.Debug.WriteLine($"支付处理失败：{ex.Message}");
        }
    }
}
