using DDDC.BLL;
using DDDC.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class OrderForm_Order : System.Web.UI.Page
{
    Userservice userService = new Userservice();
    DriveService driveService = new DriveService();
    MessageServices MsgStrv = new MessageServices();
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

                if (Session["OrderNumber"] == null || !(Session["OrderNumber"] is string))
                {
                    Session["OrderNumber"] = GenerateOrderNumber();
                    
                }

                txtOrderNumber.Text = Session["OrderNumber"].ToString();

                // 调试日志，查看最终设置的值
                System.Diagnostics.Debug.WriteLine($"Order Number in TextBox: {txtOrderNumber.Text}");
            }
        }
    }


    private void LoadOrderDetailsFromPreviousPage()
    {
            // 从 Session 获取数据并设置到相应的控件中
            if (Session["CShipName"] != null)
            {
                txtShipName.Text = Session["CShipName"].ToString();
            }
        else
        {
            txtShipName.Text = "待接单";
        }
            //1
            if (Session["CShipID"] != null)
        {
            txtShipid.Text = Session["CShipID"].ToString();
        }
        else
        {
            txtShipid.Text = "0";
        }
            //2
            if (Session["CMaxClient"] != null)
            {
                txtMaxCapacity.Text = Session["CMaxClient"].ToString();
            }
            else
            {
                txtMaxCapacity.Text = "待接单";
            }
            //3
             if (Session["CPosition"] != null)
            {
                txtDestination.Text = Session["CPosition"].ToString();
            }
            else
            {
            txtDestination.Text = "待接单";
            }
        //4
        if (Session["CImgUrl"] != null)
            {
                imgShipPhoto.ImageUrl = Session["CImgUrl"].ToString();  // 设置船只照片
            }
            else
            {
                imgShipPhoto.ImageUrl = "~/UserImg/暂无图片.gif";
            }
        //5



        if (txtShipid.Text != "0")
        {
            int shipid = Convert.ToInt32(txtShipid.Text);

            var GetShips = driveService.GetShipByID(shipid);
            // 示例：司机姓名可以通过额外查询填充
            var Getusers = userService.GetUserByID(Convert.ToInt32(GetShips.owner_id));
            txtownerName.Text = Getusers.user_name;
            Session["ShipPosition"] = GetShips.province + GetShips.city + GetShips.Position;
            Session["SOwnerID"] = GetShips.owner_id;
        }
        else
        {
            txtownerName.Text = "待接单"; // 如果没有船只信息，设置为待接单
            Session["ShipPosition"] = "待接单"; // 设置起始位置为待接单
            Session["SOwnerID"] = 0; // 假设没有司机时设置为0或其他默认值
        }
            }
    

    private string GenerateOrderNumber()
    {
        // 生成随机订单号 (8 位字母和数字)
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        char[] orderNumber = new char[8];
        for (int i = 0; i < orderNumber.Length; i++)
        {
            orderNumber[i] = chars[random.Next(chars.Length)];
        }

        // 转换为字符串
        string generatedOrderNumber = new string(orderNumber);

        // 输出到日志，确认生成内容
        System.Diagnostics.Debug.WriteLine($"Generated Order Number: {generatedOrderNumber}");

        // 保存订单号到 Session
        Session["OrderNumber"] = generatedOrderNumber;

        return generatedOrderNumber;
    }







    protected void btnSubmitOrder_Click(object sender, EventArgs e)
    {
        
        // 获取页面输入的订单信息
        string orderNumber = txtOrderNumber.Text; // 从文本框获取订单号
        int clientId = Convert.ToInt32(Session["UserID"]);// 假设ClientID存储在Session中
        string shipName = txtShipName.Text; // 船只名称
        int shipId = Convert.ToInt32(txtShipid.Text); // 船只编号
        int owner_id = Convert.ToInt32(Session["SOwnerID"]);
        string ownerName = txtownerName.Text; // 司机姓名
        string prePosition = Session["CHere"].ToString(); // 当前的起始位置（从Session中获取）
        string destination = txtDestination.Text; // 目的地
        string notes = txtRemarks.Text; // 备注信息
        string Simg =imgShipPhoto.ImageUrl;
        // 设置当前时间为开始时间，结束时间为空

        string meters = HiddenPrice.Value;
        DateTime startTime = DateTime.Now;
        DateTime endTime = DateTime.Today; // 如果不需要结束时间，可以设置为null

        // 订单状态为待确认
        string status = "待确认";

        // 订单服务实例
        OrderServices orderService = new OrderServices();

        // 调用AddOrder方法提交数据到数据库
        orderService.AddOrder(orderNumber, clientId, shipName, shipId,owner_id, ownerName, prePosition,
                              destination, notes, startTime, endTime, "无",Simg, meters,status);

        string HeadText = "新的订单";
        string Msg = "您有新的订单，请注意接收!";
        MsgStrv.addMsg(HeadText, owner_id, owner_id, Msg, "订单", DateTime.Now, "未读");
        // 提交成功后可以跳转或显示提示

        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
        "alert('订单提交成功！'); window.location.href = 'http://localhost:51058/ClientOrder/COrder.aspx';", true);
    }

}
