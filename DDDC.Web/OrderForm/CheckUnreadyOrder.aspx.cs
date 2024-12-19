using DDDC.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OrderForm_CheckUnreadyOrder : System.Web.UI.Page
{
         OrderServices orderService = new OrderServices();
         Userservice userService = new Userservice();
         DriveService driveService = new DriveService();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserID"] == null)
                {
                Response.Redirect("http://localhost:51058/login.aspx");
            }
                else
                {
                    LoadOrderDetailsFromPreviousPage();
                }
            }
        }


        private void LoadOrderDetailsFromPreviousPage()
        {
            // 从 Session 获取数据并设置到相应的控件中
            if (Session["CheckShipName1"] != null)
            {
                txtShipName.Text = Session["CheckShipName1"].ToString();
            }
            if (Session["CheckOrderNumber1"] != null)
            {
                txtOrderNumber.Text = Session["CheckOrderNumber1"].ToString();
            }
            if (Session["CheckShipID1"] != null)
            {
                txtShipid.Text = Session["CheckShipID1"].ToString();
            }

            if (Session["CheckDestination1"] != null)
            {
                txtDestination.Text = Session["CheckDestination1"].ToString();
            }

            if (Session["CheckShipImg1"] != null)
            {
                imgShipPhoto.ImageUrl = Session["CheckShipImg1"].ToString();  // 设置船只照片
            }
        
            if (Session["CheckdriverName1"] != null)
            {
                 txtownerName.Text = Session["CheckdriverName1"].ToString();  // 设置船只照片
            }
        
            if (Session["CheckComment1"] != null)
            {
            txtRemarks.Text = Session["CheckComment1"].ToString();  // 设置船只照片
            }
    }

    protected void btncancelOrder_Click(object sender, EventArgs e)
    {
        int ordId = Convert.ToInt32(Session["Checkordeid1"]);
        orderService.RejectOrder(ordId);
        
        ScriptManager.RegisterStartupScript(
     this,
     this.GetType(),
     "confirmDialog",
     "if (confirm('是否取消订单？')) { setTimeout(function(){ window.location.href = 'http://localhost:51058/ClientOrder/COrder.aspx'; }, 100); }",
     true
 );
    }
}