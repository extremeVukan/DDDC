using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Services;
using DDDC.DAL;
using DDDC.BLL;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

public partial class index : System.Web.UI.Page
{
    // 页面加载时触发
    DriveService driveService = new DriveService();
    Userservice userService = new Userservice();
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["OrderNumber"] = null;
        int userID = Convert.ToInt32(Session["UserID"]);

        // 获取用户信息
        var user = userService.GetUserByID(userID);
        var ship = driveService.GetShipsByOwnerID2(userID);

        this.MaintainScrollPositionOnPostBack = true;
        if (!IsPostBack)
        {
            if (user == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "alert('请登录！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/login.aspx'; }, 100);", true);
                
            }
            else if(driveService.IsShipExist(userID)&& ship.ship_status == "Available")
            {    
                 

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "alert('您处于接单状态！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/SelifInfo_Web/Ships_info.aspx'; }, 100);", true);
            }
            else if(user.UserSatus=="ban")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "alert('您的账号已被封禁！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/login.aspx'; }, 100);", true);
            }
                 else 
                    {
                        BindShipData();
                        // 在页面首次加载时，检查会话是否有存储的位置信息
                        if (Session["Province"] != null)
                        {
                            txtposition.Text = Session["Province"].ToString();  // 设置省份信息
                        }
                        if (Session["City"] != null)
                        {
                            txtcity.Text = Session["City"].ToString();  // 设置城市信息
                        }
                        if (Session["DetailedAddress"] != null)
                        {
                            txtposition.Text = Session["DetailedAddress"].ToString();  // 设置详细地址
                        }
                 }
                
           
        }
    }

    // 重新定位按钮点击事件



    // 确认按钮点击事件
    protected void btnOK_Click(object sender, EventArgs e)
    {
        // 这里可以处理用户确认后的逻辑，比如将位置信息保存到数据库等
        // 目前只是演示，不进行实际保存操作

    }

    // 定义一个Web方法，用于通过JavaScript更新省、市、详细地址到TextBox控件
    [WebMethod]
    public static void UpdateLocation(string province, string city, string detailedAddress)
    {
        // 使用Session来保存定位信息
        HttpContext.Current.Session["Province"] = province;
        HttpContext.Current.Session["City"] = city;
        HttpContext.Current.Session["DetailedAddress"] = detailedAddress;

        // 可以通过日志或调试来确认数据是否正确传递
        System.Diagnostics.Debug.WriteLine("省份: " + province + " 市: " + city + " 详细地址: " + detailedAddress);
        
    }
    private void BindShipData()
    {
        DriveService driveService = new DriveService();
        var shipsData = driveService.GetShips();  // 获取船只图片、名字和乘员数量的数据
        RepeaterShips.DataSource = shipsData;
        RepeaterShips.DataBind();
        

    }




    protected void btnRedirect_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "GetLocation", "getLocation();", true);
    }

    protected void RepeaterShips_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "SelectShip")
        {
            // 解析 CommandArgument 数据
            string[] shipData = e.CommandArgument.ToString().Split('|');
            if (shipData.Length == 4)
            {
                string shipName = shipData[0];
                string shipid = shipData[1];
                string capacity = shipData[2];
                string picture = shipData[3];


                // 设置到对应的 TextBox 和 Image 控件
                txtShipID.Text = shipid;
                txtShipName.Text = shipName;
                txtMaxCapacity.Text = capacity;
                txtAvailableSeats.Text = "可用";
                // 示例：计算剩余位置（假设逻辑是最大乘客量 - 5，实际逻辑可根据需求调整）
                //int remainingSeats = int.Parse(capacity) - 5; // 这里 5 是假设的已占位置数
                //txtAvailableSeats.Text = remainingSeats.ToString();

                // 更新图片控件
                shipImage.ImageUrl = ResolveUrl(picture);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('选择成功！');", true);

            }
        }
    }

    protected void btnOrder_Click(object sender, EventArgs e)
    {
        var self = userService.GetUserByID(Convert.ToInt32(Session["userID"]));
        if (self.photo == null)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "alert('请注册手机号！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/SelifInfo_Web/Self_Info.aspx'; }, 100);", true);
        }
        else
        {
            if (txtShipName.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('请选择要乘坐的船只！');", true);
            }
            else
            {
                if (TextBox1.Text == "" && TextBox2.Text == "" && txtcounty.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('请输入目的地！');", true);
                }
                else
                {
                    Session["CImgUrl"] = shipImage.ImageUrl;
                    Session["CShipName"] = txtShipName.Text;
                    Session["CShipID"] = txtShipID.Text;
                    Session["CMaxClient"] = txtMaxCapacity.Text;
                    Session["CPosition"] = TextBox1.Text + TextBox2.Text + txtcounty.Text;
                    Session["CHere"] = txtprovince.Text + txtcity.Text + txtposition.Text;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "alert('生成订单中！'); setTimeout(function(){ window.location.href = 'http://localhost:51058/OrderForm/Order.aspx'; }, 100);", true);
                }
            }
        }
    }
}

