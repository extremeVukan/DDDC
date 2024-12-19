using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AllowShipMasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["AdminName"] == null)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
            "alert('请登录！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/login.aspx'; }, 100);", true);
        }
        else
        {
            Label1.Text = Session["AdminName"].ToString();
        }
        
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(
     this,
     this.GetType(),
     "confirmDialog",
     "if (confirm('是否退出登录？')) { setTimeout(function(){ window.location.href = 'http://localhost:51058/login.aspx'; }, 100); }",
     true
 );
    }
}
