using DDDC.BLL;
using DDDC.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_UserController : System.Web.UI.Page
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
        var users1 = userService .GetUserlist(); // 获取所有船只数据

        // 假设 users1 已正确初始化
        if (!string.IsNullOrEmpty(searchQuery))
        {
            users1 = users1.Where(s =>
                s.Userid.ToString().Contains(searchQuery) || // 查询用户ID
                (s.username != null && s.username.Contains(searchQuery)) || // 查询用户名，确保不为空
                (s.status != null && s.status.Contains(searchQuery)) // 查询状态，确保不为空
            ).ToList();
        }


        RepeaterUsers.DataSource = users1;
        RepeaterUsers.DataBind();
    }


    protected void btnView_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int userid = Convert.ToInt32(btn.CommandArgument);
        var getuser = userService.GetUserByID(userid);
        if (getuser.UserSatus!= "ban")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('该用户状态正常,操作失败！');", true);
        }
        else
        {
            
            userService.AdminChangeUserStatus(userid,"Normal");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                "alert('操作成功！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/Admin/UserController.aspx'; }, 100);", true);

        }

    }

    protected void btnDisable_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int userid = Convert.ToInt32(btn.CommandArgument);
        var getuser = userService.GetUserByID(userid);
        if (getuser.Status == "ban")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('该用户已被封禁,操作失败！');", true);
        }
        else
        {

            userService.AdminChangeUserStatus(userid, "ban"); ;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                "alert('操作成功！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/Admin/UserController.aspx'; }, 100);", true);

        }

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string query = txtSearch.Text.Trim();
        BindShipsData(query);
    }
}