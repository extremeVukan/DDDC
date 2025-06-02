using DDDC.BLL;
using DDDC.DAL;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AllowShip : System.Web.UI.Page
{
    Userservice userService = new Userservice();
    OrderServices orderService = new OrderServices();
    MessageServices MsgStrv = new MessageServices();
    DriveService driveService = new DriveService();

    private string SortColumn
    {
        get { return ViewState["SortColumn"] as string ?? "ship_id"; }
        set { ViewState["SortColumn"] = value; }
    }
    private string SortDirection
    {
        get { return ViewState["SortDirection"] as string ?? "ASC"; }
        set { ViewState["SortDirection"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["AdminName"] == null || Session["AdminId"] == null)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                "alert('请先登录管理员账号！'); setTimeout(function(){ window.location.href = '/login.aspx'; }, 100);", true);
            return;
        }

        if (!IsPostBack)
        {
            BindGrid();
        }
    }

    private void BindGrid()
    {
        using (var db = new DDDCModel1())
        {
            var query = db.ShipHandle
                .Where(s => s.IsAllowed == "待审核");

            // 排序
            switch (SortColumn)
            {
                case "ship_id":
                    query = SortDirection == "ASC" ? query.OrderBy(s => s.ship_id) : query.OrderByDescending(s => s.ship_id);
                    break;
                case "owner_id":
                    query = SortDirection == "ASC" ? query.OrderBy(s => s.owner_id) : query.OrderByDescending(s => s.owner_id);
                    break;
                case "ship_name":
                    query = SortDirection == "ASC" ? query.OrderBy(s => s.ship_name) : query.OrderByDescending(s => s.ship_name);
                    break;
                case "ship_type":
                    query = SortDirection == "ASC" ? query.OrderBy(s => s.ship_type) : query.OrderByDescending(s => s.ship_type);
                    break;
                case "capacity":
                    query = SortDirection == "ASC" ? query.OrderBy(s => s.capacity) : query.OrderByDescending(s => s.capacity);
                    break;
                case "ship_status":
                    query = SortDirection == "ASC" ? query.OrderBy(s => s.ship_status) : query.OrderByDescending(s => s.ship_status);
                    break;
                case "IsAllowed":
                    query = SortDirection == "ASC" ? query.OrderBy(s => s.IsAllowed) : query.OrderByDescending(s => s.IsAllowed);
                    break;
                default:
                    query = query.OrderBy(s => s.ship_id);
                    break;
            }

            ctl02.DataSource = query.ToList();
            ctl02.DataBind();
        }
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        string Admin_name = Session["AdminName"].ToString();
        Button btn = (Button)sender;
        int ShipID = Convert.ToInt32(btn.CommandArgument);
        try
        {
            driveService.ApproveShip(ShipID, Admin_name);
            var Si = driveService.GetHandleShipByShipID(ShipID);
            DateTime date = DateTime.Now;
            driveService.AddShips(Si.ship_id, Convert.ToInt32(Si.owner_id), Si.ship_name, Si.ship_type, Convert.ToInt32(Si.capacity), "Offline", date, Si.Picture);
            userService.GetUserByID(Convert.ToInt32(Si.owner_id));
            userService.ChangeUserStatus(Convert.ToInt32(Si.owner_id));
            string headText = "恭喜！您的船只注册申请已通过！";
            string msg = "您的船只申请已通过！请注意遵守平台规则！";
            MsgStrv.addMsg(headText, Convert.ToInt32(Si.owner_id), Convert.ToInt32(Si.owner_id), msg, "平台消息", DateTime.Now, "未读");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('操作成功！');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('操作失败:！'+ ex.Message);", true);
        }
        BindGrid();
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        string Admin_name = Session["AdminName"].ToString();
        Button btn = (Button)sender;
        int ShipID = Convert.ToInt32(btn.CommandArgument);
        try
        {
            driveService.RejectShip(ShipID, Admin_name);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('已拒绝申请！');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('操作失败:！'+ ex.Message);", true);
        }
        BindGrid();
    }

    protected void ctl02_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ctl02.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void ctl02_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (SortColumn == e.SortExpression)
        {
            SortDirection = (SortDirection == "ASC") ? "DESC" : "ASC";
        }
        else
        {
            SortColumn = e.SortExpression;
            SortDirection = "ASC";
        }
        BindGrid();
    }
}
