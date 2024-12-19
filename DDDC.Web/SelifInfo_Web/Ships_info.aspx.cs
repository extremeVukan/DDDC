using DDDC.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class SelifInfo_Web_Ships_info : System.Web.UI.Page
{
    Userservice userService = new Userservice();
    DriveService driveService = new DriveService();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // 从Session中获取用户ID
            int userID = Convert.ToInt32(Session["UserID"]);

            // 获取用户信息
            var user = userService.GetUserByID(userID);
            var ship = driveService.GetShipsByOwnerID2(userID);
            if (user != null)
            {

                lblemail.Text = user.email;
                lblName.Text = user.user_name;
                LabelNowStatus.Text = "当前船只状态:" + ship.ship_status;

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
    protected void btnRedirect_Click(object sender, EventArgs e)
    {
        try
        {
            // 从文本框获取用户输入的位置信息
            string province = txtprovince.Text.Trim();
            string city = txtcity.Text.Trim();
            string position = txtposition.Text.Trim();

            // 验证输入内容是否完整
            if (string.IsNullOrWhiteSpace(province) && string.IsNullOrWhiteSpace(city) && string.IsNullOrWhiteSpace(position))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('请输入完整的省、市和详细地址信息！');", true);
                return;
            }

            // 拼接完整地址
            string fullAddress = $"{province}{city}{position}";

            // 调用前端 JavaScript 函数定位地图
            ScriptManager.RegisterStartupScript(this, this.GetType(), "RedirectMap", $"redirectToLocation('{fullAddress}');", true);
        }
        catch (Exception ex)
        {
            // 处理异常并提示用户
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('定位失败：{ex.Message}');", true);
        }
    }



    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            // 从 Session 中获取当前用户 ID
            int userID = Convert.ToInt32(Session["UserID"]);

            // 获取用户输入的位置信息
            string province = txtprovince.Text.Trim();
            string city = txtcity.Text.Trim();
            string position = txtposition.Text.Trim();

            // 检查输入是否为空
            if (string.IsNullOrWhiteSpace(province) || string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(position))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('位置信息不能为空！');", true);
                return;
            }

            // 调用 DriveService 更新位置信息
            driveService.UpdateShipLocationByUserID(userID, province, city, position);

            // 成功提示
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('位置信息已成功提交！');", true);
        }
        catch (Exception ex)
        {
            // 捕获异常并提示
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('提交失败：{ex.Message}');", true);
        }
    }


    protected void btnUpdateStatus_Click(object sender, EventArgs e)
    {
        int userID = Convert.ToInt32(Session["UserID"]);
        var getship1 =driveService.GetShipsByOwnerID2(userID);
        if (getship1.ship_status == "ban")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('您的账号异常请前往线下处理！');", true);
        }else
        {
            try
            {
                // 从 Session 中获取当前用户 ID


                // 获取下拉框选择的状态
                string selectedStatus = ddlStatus.SelectedValue;
                if (ddlStatus.SelectedValue == "Available")
                {

                    string province = txtprovince.Text.Trim();
                    string city = txtcity.Text.Trim();
                    string position = txtposition.Text.Trim();
                    driveService.UpdateShipLocationByUserID(userID, province, city, position);


                }

                // 调用 DriveService 更新船只状态
                driveService.UpdateShipStatusByUserID(userID, selectedStatus);

                // 成功提示

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                "alert('状态更新成功！'); setTimeout(function(){ window.location.href = ''; }, 100);", true);
            }
            catch (Exception ex)
            {
                // 捕获异常并提示
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('更新失败：{ex.Message}');", true);
            }
        }
    }
        


    protected void btndirect_Click(object sender, EventArgs e)
    {
        try
        {
            // 调用前端的 getLocation 函数
            ScriptManager.RegisterStartupScript(this, this.GetType(), "GetLocation", "getLocation();", true);
        }
        catch (Exception ex)
        {
            // 处理异常并提示用户
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('定位失败：{ex.Message}');", true);
        }
    }

}