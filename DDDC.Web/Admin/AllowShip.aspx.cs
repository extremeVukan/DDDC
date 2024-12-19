using DDDC.BLL;
using DDDC.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
public partial class Admin_AllowShip : System.Web.UI.Page
{
    Userservice userService = new Userservice();
    OrderServices orderService = new OrderServices();
    MessageServices MsgStrv = new MessageServices();
    DriveService driveService = new DriveService();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["AdminName"] == null || Session["AdminId"]==null)
        {
            
            
        }
        else
        {
            int AdminID = Convert.ToInt32(Session["AdminId"]);
            string Admin_name = Session["AdminName"].ToString();

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
            driveService.AddShips(Si.ship_id,Convert.ToInt32( Si.owner_id),Si.ship_name, Si.ship_type, Convert.ToInt32(Si.capacity), "Offline",date, Si.Picture);
            userService.GetUserByID(Convert.ToInt32(Si.owner_id));
            userService.ChangeUserStatus(Convert.ToInt32( Si.owner_id));
            string headText = "恭喜！您的船只注册申请已通过！";
            string msg = "您的船只申请已通过！请注意遵守平台规则！";
            MsgStrv.addMsg(headText, Convert.ToInt32(Si.owner_id), Convert.ToInt32( Si.owner_id), msg, "平台消息", DateTime.Now, "未读");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('操作成功！');", true);
            
            

        }
        catch (Exception ex)
        {
           

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('操作失败:！'+ ex.Message);", true);
        }

        
        ctl02.DataBind();
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


        ctl02.DataBind();
    }

    protected void ctl02_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ctl02.PageIndex = e.NewPageIndex;

        // 重新绑定数据源
        ctl02.DataBind();
    }
}