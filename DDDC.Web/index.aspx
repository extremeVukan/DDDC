<%@ Page Title="" Language="C#" MasterPageFile="~/Index.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div id="allmap" style="width: 500px; height: 100%;border-radius:50px"></div>


<script type="text/javascript">
    function getLocation() {
        var map = new BMap.Map("allmap"); // 创建地图实例
        var geolocation = new BMap.Geolocation(); // 创建地理定位实例

        // 获取当前位置
        geolocation.getCurrentPosition(function (result) {
            if (this.getStatus() === BMAP_STATUS_SUCCESS) {
                var point = new BMap.Point(result.point.lng, result.point.lat);  // 定位点
                map.centerAndZoom(point, 15);  // 将地图中心移动到当前位置并设置缩放级别
                map.addOverlay(new BMap.Marker(point));  // 添加标记到地图上

                // 获取详细地址
                var geocoder = new BMap.Geocoder();
                geocoder.getLocation(point, function (res) {
                    var province = res.addressComponents.province;
                    var city = res.addressComponents.city;
                    var detailedAddress = res.address;

                    // 更新文本框值
                    document.getElementById('<%= txtprovince.ClientID %>').value = province;
                    document.getElementById('<%= txtcity.ClientID %>').value = city;
                    document.getElementById('<%= txtposition.ClientID %>').value = detailedAddress;

                    // 通过 AJAX 调用后端 WebMethod 更新 Session
                    PageMethods.UpdateLocation(province, city, detailedAddress);
                });
                map.enableScrollWheelZoom(true);
           
            } else {
                alert("定位失败: " + this.getStatus());
            }
        }, { enableHighAccuracy: true }); 
    }

    // 页面加载时调用定位函数
    
    window.onload = getLocation;
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <table style="width: 80%; height: 396px;margin-left:100px">
        <tr>
            <td colspan="2" style="height: 61px;">
                <div style="margin-left:-100px">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="微软雅黑" Font-Size="X-Large" ForeColor="White" Text="当前位置信息"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td class="tdleft" style="width: 127px">
                <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="微软雅黑" ForeColor="White" Text="省份："></asp:Label>
            </td>
            <td class="tdleft">
                <asp:TextBox ID="txtprovince" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdleft" style="width: 127px">
                <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="微软雅黑" ForeColor="White" Text="市区："></asp:Label>
            </td>
            <td class="tdleft">
                <asp:TextBox ID="txtcity" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdleft" style="width: 127px">
                <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="微软雅黑" ForeColor="White" Text="详细位置："></asp:Label>
            </td>
            <td class="tdleft">
                <asp:TextBox ID="txtposition" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tdleft" style="width: 127px">
                
            </td>
            <td class="tdleft">
                
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        // 使用百度地图 Geolocation API 获取用户的当前位置
        function getLocation() {
            var geolocation = new BMap.Geolocation();  // 创建地理定位实例
            geolocation.getCurrentPosition(function (result) {
                if (this.getStatus() == BMAP_STATUS_SUCCESS) {
                    console.log("定位成功: ", result);
                    // 获取省、市和详细地址
                    var geocoder = new BMap.Geocoder();
                    geocoder.getLocation(result.point, function (res) {
                        var province = res.addressComponents.province;
                        var city = res.addressComponents.city;
                        var detailedAddress = res.address;
                        console.log("省份: " + province + " 市区: " + city + " 详细地址: " + detailedAddress);
                        // 更新页面上的TextBox值
                        document.getElementById('<%= txtprovince.ClientID %>').value = province;
                        document.getElementById('<%= txtcity.ClientID %>').value = city;
                        document.getElementById('<%= txtposition.ClientID %>').value = detailedAddress;
                        // 通过AJAX调用后台方法更新服务器端Session0
                        updateLocation(province, city, detailedAddress);
                    });
                } else {
                    console.error('定位失败，状态码：' + this.getStatus());
                }
            }, { enableHighAccuracy: true });
        }

        // 通过AJAX更新ASP.NET控件的值
        function updateLocation(province, city, detailedAddress) {
            // 调用后台的Web方法
            PageMethods.UpdateLocation(province, city, detailedAddress, function (response) {
                console.log("位置更新成功");
            }, function (error) {
                console.error("更新位置失败：" + error.get_message());
            });
        }
    </script>
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder4" Runat="Server">
    <div class="details-container">
        
        <!-- 船只信息显示 -->
        <div class="ship-details">
            <asp:Image ID="shipImage" runat="server" ImageUrl="/UserImg/OIP (2).jpg" AlternateText="船只图片" CssClass="ship-image" />
            <div class="ship-info">
                <h3 class="ship-info-title" style="color:black">船只名称: <asp:TextBox ID="txtShipName" runat="server" Text="" ReadOnly="True" CssClass="ship-info-text" /></h3>
                <p><strong style="color:black">船只编号:</strong> <asp:TextBox ID="txtShipID" runat="server" Text="" ReadOnly="True" CssClass="ship-info-text" /></p>
                <p><strong style="color:black">最大乘客量:</strong> <asp:TextBox ID="txtMaxCapacity" runat="server" Text="" ReadOnly="True" CssClass="ship-info-text" /></p>
                <p><strong style="color:black">状态:</strong> <asp:TextBox ID="txtAvailableSeats" runat="server" Text="" ReadOnly="True" CssClass="ship-info-text" /></p>
            </div>
        </div>

        <!-- 位置信息输入 -->
        <div class="location-details">
            <table style="width: 90%; margin-top: 20px;">
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Label ID="LabelLocation" runat="server" Text="请输入目的地信息：" Font-Bold="True" Font-Names="微软雅黑" Font-Size="Large" ForeColor="Black"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 30%; text-align: right;">
                        <asp:Label ID="LabelProvince" runat="server" Text="省份：" Font-Bold="True" Font-Names="微软雅黑" ForeColor="Black"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox1" runat="server" CssClass="location-input"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="LabelCity" runat="server" Text="市区：" Font-Bold="True" Font-Names="微软雅黑" ForeColor="Black"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox2" runat="server" CssClass="location-input"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="LabelCounty" runat="server" Text="县/区：" Font-Bold="True" Font-Names="微软雅黑" ForeColor="Black"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtcounty" runat="server" CssClass="location-input"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>

        <!-- 下单按钮 -->
        <div class="order-section">
            <asp:Button ID="btnOrder" runat="server" Text="确定下单" Font-Bold="True" Font-Names="微软雅黑" CssClass="order-button" OnClick="btnOrder_Click" />
        </div>
    </div>

    <!-- 样式设置 -->
    <style>
        /* 主容器样式 */
        .details-container {
            display: flex;
            flex-direction: column;
            align-items: center;
            
            background-color: rgba(225, 225, 225, 0.5);
            border-radius: 50px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            margin:5px auto;
            width: 380px;
            height: 750px;
            padding:5px
        }

        /* 船只信息样式 */
        .ship-details {
            text-align: center;
            margin-bottom: 10px;
        }

        .ship-image {
            width: 250px;
            height: 150px;
            
            border-radius: 20px;
            margin-bottom: 15px;
        }

        .ship-info {
            background-color: #fff;
            padding: 15px;
            border-radius: 20px;
            width: 90%;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            text-align: center;
        }

        .ship-info-text {
            border: none;
            background-color: transparent;
            text-align: center;
            width: 100%;
            font-size: 16px;
            font-weight: normal;
            color: #333;
        }

        /* 位置信息输入框样式 */
        .location-details {
            margin-top: 10px;
            text-align: center;
            width: 90%;
        }

        .location-input {
            width: 80%;
            padding: 8px;
            border-radius: 5px;
            border: 1px solid #ccc;
        }

        /* 下单按钮样式 */
        .order-section {
            text-align: center;
            width: 100%;
            margin-top: 10px;
        }

        .order-button {
            background-color: #4CAF50;
            color: white;
            padding: 10px 20px;
            font-size: 16px;
            border: none;
            border-radius: 20px;
            cursor: pointer;
            transition: background-color 0.3s;
        }

        .order-button:hover {
            background-color: #45a049;
        }
    </style>

    
</asp:Content>




<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder5" Runat="Server">
    <div class="waterfall-container">
    <!-- 物资项目将动态绑定到这里 -->
    <asp:Repeater ID="RepeaterShips" runat="server" OnItemCommand="RepeaterShips_ItemCommand">
            <ItemTemplate>
                <div class="waterfall-item">
                    <!-- 图片显示 -->
                    <img src='<%# ResolveUrl(Eval("Picture").ToString()) %>' alt='<%# Eval("ShipName") %>' />
                    <!-- 船只名称 -->
                    <h3><%# Eval("ShipName") %></h3>
                    <!-- 船只编号 -->
                    <p>船只编号: <%# Eval("ShipID") %></p>
                    <!-- 成员数量显示 -->
                    <p>人员荷载: <%# Eval("Capacity") %></p>
                    
                    <!-- 选择按钮 -->
                    <asp:Button ID="btnSelectShip" runat="server" Text="选择船只" CssClass="btn-select"
                                CommandName="SelectShip"
                                CommandArgument='<%# Eval("ShipName") + "|" + Eval("ShipID") + "|" + Eval("Capacity") + "|" + Eval("Picture") %>' />
                </div>
            </ItemTemplate>
        </asp:Repeater>
</div>



   <style>
    /* 容器设置：设置排列方式和间距 */
    .waterfall-container {
    width: 100%; /* 占满父容器宽度 */
    height: 100%; /* 占满父容器高度 */
    display: flex;
    flex-wrap: wrap;
    justify-content: space-around;
    padding: 20px;
    gap: 20px;
    overflow-y: auto; /* 允许垂直滚动 */
    border-radius: 50px;
    background-color: #f9f9f9;
}

    /* 每个物资项的样式 */
    .waterfall-item {
    flex-basis: calc(25% - 20px); /* 每行显示 4 个项目，间距为 20px */
    flex-grow: 1;
    max-width: 250px; /* 限制单项的最大宽度 */
    border: 1px solid #ddd;
    border-radius: 8px;
    overflow: hidden;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    text-align: center;
    background-color: #fff;
}
   
    /* 图片样式：确保图片充满容器，且保持比例 */
    .waterfall-item img {
        width: 100%;
        height: 200px; /* 可以根据实际情况调整 */
        object-fit: cover; /* 保持图片比例，裁剪多余部分 */
    }

    /* 标题样式 */
    .waterfall-item h3 {
        font-size: 18px;
        color: #333;
        margin: 15px 0 5px;
    }

    /* 文字描述样式 */
    .waterfall-item p {
        font-size: 14px;
        color: #666;
        margin: 5px 0;
    }

    /* 按钮样式 */
    .waterfall-item button {
        margin: 15px 0;
        padding: 8px 16px;
        background-color: #4CAF50; /* 绿色按钮 */
        color: white;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        font-size: 14px;
        transition: background-color 0.3s;
    }

    /* 按钮悬停效果 */
    .waterfall-item button:hover {
        background-color: #45a049;
    }
    .btn-select {
            background-color: #1abc9c;
            color: white;
            padding: 8px 16px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 14px;
            margin: 15px 0;
        }
        .btn-select:hover {
            background-color: #16a085;
        }
</style>

</asp:Content>



<asp:Content ID="Content6" runat="server" contentplaceholderid="ContentPlaceHolder6">
</asp:Content>


