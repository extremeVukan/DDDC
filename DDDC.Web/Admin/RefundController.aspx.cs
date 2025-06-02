using DDDC.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_RefundController : System.Web.UI.Page
{
    Userservice userservice = new Userservice();
    AfterServices afterServices = new AfterServices();
    MessageServices MsgStrv = new MessageServices();
    OrderServices orderServices = new OrderServices();

    protected void Page_Load(object sender, EventArgs e)
    {
        // 验证管理员登录状态
        if (Session["AdminName"] == null || Session["AdminId"] == null)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                "alert('请先登录管理员账号！'); setTimeout(function(){ window.location.href = '/Admin/Login.aspx'; }, 100);", true);
            return;
        }

        if (!IsPostBack)
        {
            BindRefundsData();
        }
    }

    private void BindRefundsData(string searchQuery = "")
    {
        try
        {
            var refunds = afterServices.GetRefundList(); // 获取退款列表

            if (!string.IsNullOrEmpty(searchQuery))
            {
                refunds = refunds.Where(r =>
                    (r.OrderNumber != null && r.OrderNumber.Contains(searchQuery)) || // 查询订单号
                    r.UserID.ToString().Contains(searchQuery) || // 查询用户ID
                    (r.Status != null && r.Status.Contains(searchQuery)) // 查询状态
                ).ToList();
            }

            RepeaterRefunds.DataSource = refunds;
            RepeaterRefunds.DataBind();

            // 设置"暂无数据"面板的可见性
            NoDataPanel.Visible = refunds == null || refunds.Count == 0;
        }
        catch (Exception ex)
        {
            // 记录错误
            System.Diagnostics.Debug.WriteLine($"绑定退款数据出错: {ex.Message}");

            // 显示错误消息
            ScriptManager.RegisterStartupScript(this, GetType(), "errorAlert",
                $"alert('加载退款数据失败: {ex.Message.Replace("'", "\\'")}');", true);

            // 确保在出错时也正确设置"无数据"面板
            NoDataPanel.Visible = true;
        }
    }

    protected void RepeaterRefunds_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            // 可以在这里进行额外的UI逻辑处理
            // 比如根据状态动态调整按钮可见性等
        }
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int refundId = Convert.ToInt32(btn.CommandArgument);

        try
        {
            var refundApp = afterServices.getAppByServicesID(refundId);
            if (refundApp == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    "alert('找不到对应的退款申请记录！');", true);
                return;
            }

            string orderNumber = refundApp.ordernumber;
            var orderInfo = orderServices.GetOrderByOrdrNumber(orderNumber);

            if (orderInfo == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    "alert('找不到对应的订单记录！');", true);
                return;
            }

            if (afterServices.ApproveRefund(refundId))
            {
                afterServices.UpdateOrderStatus(orderNumber, "已退款");
                afterServices.UpdateOrderTStatus1(orderNumber, "已退款", 0);

                // 发送消息给客户
                string headText = "退款申请已通过！";
                string msg = "亲爱的用户您对订单" + orderNumber + "的退款申请已经通过！";
                MsgStrv.addMsg(headText, Convert.ToInt32(orderInfo.ClientID), 999, msg, "订单", DateTime.Now, "未读");

                // 发送消息给船主
                if (orderInfo.OwnerID.HasValue)
                {
                    string headText1 = "退款申请！";
                    string msg1 = "客户对" + orderNumber + "的退款申请已经通过！";
                    MsgStrv.addMsg(headText1, Convert.ToInt32(orderInfo.OwnerID), 999, msg1, "订单", DateTime.Now, "未读");
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('退款申请已批准！');", true);
                BindRefundsData(); // 刷新数据
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('操作失败，请稍后重试。');", true);
            }
        }
        catch (Exception ex)
        {
            // 记录错误并显示
            System.Diagnostics.Debug.WriteLine($"批准退款时出错: {ex.Message}");
            ScriptManager.RegisterStartupScript(this, GetType(), "errorAlert",
                $"alert('处理退款申请时出错: {ex.Message.Replace("'", "\\'")}');", true);
        }
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int refundId = Convert.ToInt32(btn.CommandArgument);

        try
        {
            var refundApp = afterServices.getAppByServicesID(refundId);
            if (refundApp == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    "alert('找不到对应的退款申请记录！');", true);
                return;
            }

            string orderNumber = refundApp.ordernumber;
            var orderInfo = orderServices.GetOrderByOrdrNumber(orderNumber);

            if (orderInfo == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    "alert('找不到对应的订单记录！');", true);
                return;
            }

            if (afterServices.RejectRefund(refundId))
            {
                // 发送消息给客户
                string headText = "退款申请未通过！";
                string msg = "亲爱的用户您对订单" + orderNumber + "的退款申请未被通过！";
                MsgStrv.addMsg(headText, Convert.ToInt32(orderInfo.ClientID), 999, msg, "订单", DateTime.Now, "未读");

                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('退款申请已拒绝！');", true);
                BindRefundsData(); // 刷新数据
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('操作失败，请稍后重试。');", true);
            }
        }
        catch (Exception ex)
        {
            // 记录错误并显示
            System.Diagnostics.Debug.WriteLine($"拒绝退款时出错: {ex.Message}");
            ScriptManager.RegisterStartupScript(this, GetType(), "errorAlert",
                $"alert('处理退款申请时出错: {ex.Message.Replace("'", "\\'")}');", true);
        }
    }

    protected void btnContact_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int userId = Convert.ToInt32(btn.CommandArgument);

        try
        {
            var user = userservice.GetUserByID(userId);
            if (user != null)
            {
                string phoneInfo = string.IsNullOrEmpty(user.Phone) ? "未设置电话号码" : user.Phone;
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    $"alert('联系用户：{user.user_name}，电话：{phoneInfo}');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('用户信息未找到！');", true);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"获取用户联系信息时出错: {ex.Message}");
            ScriptManager.RegisterStartupScript(this, GetType(), "errorAlert",
                $"alert('获取用户联系信息时出错: {ex.Message.Replace("'", "\\'")}');", true);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string query = txtSearch.Text.Trim();
        BindRefundsData(query);
    }
}
