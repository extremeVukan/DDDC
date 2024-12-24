using DDDC.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OrderControl_CheckComment : System.Web.UI.Page
{
    OrderTServices orderTServices = new OrderTServices();
    Userservice userSrv = new Userservice();
    DriveService DriverSrv = new DriveService();
    protected void Page_Load(object sender, EventArgs e)
    {
        int userID = Convert.ToInt32(Session["UserID"]);
        if (!IsPostBack)
        {
            
            var user = userSrv.GetUserByID(userID);
            var ship1 = DriverSrv.GetShipsByOwnerID2(userID);

            if (user != null)
            {
                int shipid = ship1.ship_id;
                lblemail.Text = user.email;
                lblName.Text = user.user_name;
                lblShipName.Text = ship1.ship_name;
                lblShipStatus.Text = ship1.ship_status;
                int userid = Convert.ToInt32(Session["UserID"]);
                var s1 = DriverSrv.GetShipsByOwnerID2(userid);
                BindComments(s1.ship_id);
                if (!string.IsNullOrEmpty(user.photo))
                {
                    Image2.ImageUrl = user.photo;
                }
                else
                {
                    Image2.ImageUrl = "~/UserImg/暂无图片.gif";
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
            "alert('请登录！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/login.aspx'; }, 100);", true);
            }
        }
    }

    private void BindComments(int shipId)
    {
        var comments = orderTServices.GetCommentsByShipId(shipId);
        RepeaterComments.DataSource = comments;
        RepeaterComments.DataBind();
    }
    protected string GenerateStars(object estimate)
    {
        double stars = Convert.ToDouble(estimate);
        int fullStars = (int)Math.Floor(stars);
        bool hasHalfStar = stars - fullStars >= 0.5;

        string starHtml = "";

        for (int i = 0; i < fullStars; i++)
        {
            starHtml += "<i class='fas fa-star'></i>";
        }

        if (hasHalfStar)
        {
            starHtml += "<i class='fas fa-star-half-alt'></i>";
        }

        for (int i = fullStars + (hasHalfStar ? 1 : 0); i < 5; i++)
        {
            starHtml += "<i class='far fa-star'></i>";
        }

        return starHtml;
    }

}