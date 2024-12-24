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
        if (!IsPostBack)
        {
            BindRefundsData();
        }
    }

    private void BindRefundsData(string searchQuery = "")
    {
        var refunds = afterServices.GetRefundList(); // 获取退款列表

        if (!string.IsNullOrEmpty(searchQuery))
        {
            refunds = refunds.Where(r =>
                r.OrderNumber.Contains(searchQuery) || // 查询订单号
                r.UserID.ToString().Contains(searchQuery) || // 查询用户名
                r.Status.Contains(searchQuery) // 查询状态
            ).ToList();
        }

        RepeaterRefunds.DataSource = refunds;
        RepeaterRefunds.DataBind();
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int refundId = Convert.ToInt32(btn.CommandArgument);
        var get = afterServices.getAppByServicesID(refundId);
        string ordn = get.ordernumber;
        var id = orderServices.GetOrderByOrdrNumber(ordn);

        if (afterServices.ApproveRefund(refundId))
        {
            afterServices.UpdateOrderStatus(ordn, "已退款");
            afterServices.UpdateOrderTStatus1(ordn, "已退款", 0);
            string HeadText = "退款申请已通过！";
            string Msg = "亲爱的用户您对订单" + ordn + "的退款申请已经通过！";
            string HeadText1 = "退款申请！";
            string Msg1 = "客户对" + ordn + "的退款申请已经通过！";
            MsgStrv.addMsg(HeadText, Convert.ToInt32(id.ClientID), 999, Msg, "订单", DateTime.Now, "未读");
            MsgStrv.addMsg(HeadText1, Convert.ToInt32(id.OwnerID), 999, Msg1, "订单", DateTime.Now, "未读");
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('退款申请已批准！');", true);
            BindRefundsData();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('操作失败，请稍后重试。');", true);
        }
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int refundId = Convert.ToInt32(btn.CommandArgument);
        var get = afterServices.getAppByServicesID(refundId);
        string ordn = get.ordernumber;
        var id = orderServices.GetOrderByOrdrNumber(ordn);
        if (afterServices.RejectRefund(refundId))
        {
            string HeadText = "退款申请未通过！";
            string Msg = "亲爱的用户您对订单" + ordn + "的退款申请未被通过！";
            MsgStrv.addMsg(HeadText, Convert.ToInt32(id.ClientID), 999, Msg, "订单", DateTime.Now, "未读");
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('退款申请已拒绝！');", true);
            BindRefundsData();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('操作失败，请稍后重试。');", true);
        }
    }

    protected void btnContact_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int userId = Convert.ToInt32(btn.CommandArgument);

        var user = userservice.GetUserByID(userId);
        if (user != null)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                $"alert('联系用户：{user.user_name}，电话：{user.Phone}');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('用户信息未找到！');", true);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string query = txtSearch.Text.Trim();
        BindRefundsData(query);
    }
}