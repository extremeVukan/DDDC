using DDDC.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_ShipController : System.Web.UI.Page
{
    Userservice userService = new Userservice();
    OrderServices orderService = new OrderServices();

    DriveService driveService = new DriveService();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindShipsData();
        }
    }

    private void BindShipsData(string searchQuery = "")
    {
        DriveService service = new DriveService();
        var ships = service.GetShips1(); // 获取所有船只数据
        
        if (!string.IsNullOrEmpty(searchQuery))
        {
            ships = ships.Where(s => s.ShipName.Contains(searchQuery) || s.ShipID.ToString().Contains(searchQuery)).ToList()  ;
        }

        RepeaterShips.DataSource = ships;
        RepeaterShips.DataBind();
    }


    protected void btnView_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int shipId = Convert.ToInt32(btn.CommandArgument);
        var getship =driveService.GetShipByID(shipId);
        if(getship .ship_status != "ban")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('该船只状态正常,操作失败！');", true);
        }
        else
        {
            driveService.UpdateShipStatusByShipID(shipId, "Available");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                "alert('操作成功！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/Admin/ShipController.aspx'; }, 100);", true);

        }
        
    }

    protected void btnDisable_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int shipId = Convert.ToInt32(btn.CommandArgument);
        var getship = driveService.GetShipByID(shipId);
        if (getship.ship_status == "ban")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('该船只已被禁用,操作失败！');", true);
        }
        else
        {
            driveService.UpdateShipStatusByShipID(shipId, "ban");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                "alert('操作成功！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/Admin/ShipController.aspx'; }, 100);", true);

        }

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string query = txtSearch.Text.Trim();
        BindShipsData(query);
    }
}