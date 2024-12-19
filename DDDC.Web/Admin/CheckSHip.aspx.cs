using DDDC.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using static DDDC.BLL.DriveService;

public partial class Admin_CheckSHip : System.Web.UI.Page
{
    
        DriveService driverServices = new DriveService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["AdminName"] == null || Session["AdminId"] == null)
        {


        }
        else
        {
            int AdminID = Convert.ToInt32(Session["AdminId"]);
            string Admin_name = Session["AdminName"].ToString();
            
        }
        


    }


    /// <summary>
    /// 将船只数据转换为JSON格式，供前端使用
    /// </summary>
    /// <returns>JSON字符串</returns>
    public string GetShipsJson()
    {
        List<ShipLocation> ships = driverServices.GetAvailableShips();
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(ships);
        


    }


}
