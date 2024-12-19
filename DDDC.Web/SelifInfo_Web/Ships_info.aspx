<%@ Page Title="" Language="C#" MasterPageFile="~/SelifInfo_Web/Self_info.master" AutoEventWireup="true" CodeFile="Ships_info.aspx.cs" Inherits="SelifInfo_Web_Ships_info" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    <asp:Image ID="Image2" runat="server" Height="96px" Width="136px" style="border-radius: 15px; border: 2px solid #ccc;" />
    <br />

    <h2 class="username" style="margin-top:10px;">
        <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label>
    </h2>
    <p class="email">
        <asp:Label ID="lblemail" runat="server" Text="Label"></asp:Label>
    </p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <div class="ships-info-container">
        <!-- 地图区域 -->
        <div class="map-section">
            <h2 class="section-title">船只定位</h2>
            <div id="allmap" class="map-container"></div>
        </div>

        <!-- 信息和操作区域 -->
        <div class="info-section">
            <h2 class="section-title">船只状态更新</h2>

            <!-- 定位信息输入区域 -->
            <div class="form-container">
                <table class="form-table">
                    <tr>
                        <td class="form-label">
                            <asp:Label ID="LabelProvince" runat="server" Text="省份:" CssClass="form-label-text"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtprovince" runat="server" CssClass="form-input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="form-label">
                            <asp:Label ID="LabelCity" runat="server" Text="市区:" CssClass="form-label-text"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcity" runat="server" CssClass="form-input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="form-label">
                            <asp:Label ID="LabelAddress" runat="server" Text="详细地址:" CssClass="form-label-text"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtposition" runat="server" CssClass="form-input"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>

            <!-- 状态更新选项 -->
            <div class="status-container">
                <asp:Label ID="LabelStatusUpdate" runat="server" Text="更新船只状态:" CssClass="status-label"></asp:Label><br />
                <asp:Label ID="LabelNowStatus" runat="server" Text="当前船只状态:" CssClass="status-label"></asp:Label>
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="status-dropdown">
                    <asp:ListItem Text="Available" Value="Available"></asp:ListItem>
                    <asp:ListItem Text="Occupied" Value="Occupied"></asp:ListItem>
                    <asp:ListItem Text="Offline" Value="Offline"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <!-- 按钮区域 -->
            <div class="button-container">
                <asp:Button ID="btndirect" runat="server" Text="定位" OnClick="btndirect_Click" CssClass="action-button" />
                <asp:Button ID="btnRedirect" runat="server" Text="重新定位" OnClick="btnRedirect_Click" CssClass="action-button" />
                <asp:Button ID="btnOK" runat="server" Text="确认定位" OnClick="btnOK_Click" CssClass="action-button" />
                <asp:Button ID="btnUpdateStatus" runat="server" Text="提交状态更新" OnClick="btnUpdateStatus_Click" CssClass="action-button" />
            </div>
        </div>
    </div>


    <!-- 样式 -->
    <style>
        
        body {
            font-family: Arial, sans-serif;
            background-color: #f5f5f5;
            margin: 0;
            padding: 0;
        }

        /* 容器布局 */
        .ships-info-container {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
            padding: 20px;
            gap: 20px;
        }

        /* 地图区域样式 */
        .map-section {
            flex: 1.5;
            background: #fff;
            padding: 20px;
            border-radius: 15px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

        .map-container {
            width: 100%;
            height: 500px;
            border-radius: 10px;
            border: 1px solid #ddd;
        }

        /* 信息和操作区域样式 */
        .info-section {
            flex: 1;
            background: #fff;
            padding: 20px;
            border-radius: 15px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

        .section-title {
            text-align: center;
            font-size: 24px;
            color: #34495e;
            margin-bottom: 20px;
        }

        .form-container, .status-container, .button-container {
            margin-bottom: 20px;
        }

        .form-table {
            width: 100%;
            border-spacing: 10px;
        }

        .form-label {
            text-align: right;
            padding: 8px;
            font-weight: bold;
            color: #34495e;
            width: 30%;
        }

        .form-label-text {
            font-size: 16px;
            color: #34495e;
        }

        .form-input {
            width: 100%;
            padding: 10px;
            font-size: 14px;
            border: 1px solid #ddd;
            border-radius: 5px;
            transition: border-color 0.3s;
        }

        .form-input:focus {
            border-color: #1abc9c;
            outline: none;
        }

        .status-label {
            font-size: 16px;
            font-weight: bold;
            color: #34495e;
        }

        .status-dropdown {
            width: 100%;
            padding: 10px;
            font-size: 14px;
            border-radius: 5px;
            border: 1px solid #ddd;
        }

        .action-button {
            background-color: #1abc9c;
            color: white;
            padding: 10px 20px;
            font-size: 14px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            transition: background-color 0.3s;
        }

        .action-button:hover {
            background-color: #16a085;
        }

        /* 响应式支持 */
        @media screen and (max-width: 768px) {
            .ships-info-container {
                flex-direction: column;
            }

            .map-section, .info-section {
                flex: none;
                width: 100%;
            }
        }
    </style>

    <!-- 百度地图定位脚本 -->
    <script type="text/javascript" src="https://api.map.baidu.com/api?v=3.0&ak=EAk6CzkY3oWmsIFib3VBNijhD7MGWCaC"></script>
    <script type="text/javascript">
        function getLocation() {
            var map = new BMap.Map("allmap"); // 创建地图实例
            var geolocation = new BMap.Geolocation(); // 创建地理定位实例

            // 获取当前位置
            geolocation.getCurrentPosition(function (result) {
                if (this.getStatus() === BMAP_STATUS_SUCCESS) {
                    var point = new BMap.Point(result.point.lng, result.point.lat); // 当前定位点
                    map.centerAndZoom(point, 15); // 设置地图中心和缩放级别
                    map.addOverlay(new BMap.Marker(point)); // 添加当前位置标记

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
                } else {
                    alert("定位失败: " + this.getStatus());
                }
            }, { enableHighAccuracy: true });
        }

        // 页面加载时调用定位函数
        window.onload = getLocation;
    </script>
    <script type="text/javascript">
        // 根据用户输入的地址进行定位
        function redirectToLocation(address) {
            var map = new BMap.Map("allmap"); // 创建地图实例

            // 地址解析器
            var myGeo = new BMap.Geocoder();

            // 对输入的地址进行解析
            myGeo.getPoint(address, function (point) {
                if (point) {
                    // 定位到解析的点
                    map.centerAndZoom(point, 15);

                    // 清除旧的标记
                    map.clearOverlays();

                    // 添加标记到地图
                    var marker = new BMap.Marker(point);
                    map.addOverlay(marker);

                    // 设置弹出信息框
                    var infoWindow = new BMap.InfoWindow(`目标位置：${address}`);
                    marker.addEventListener("click", function () {
                        map.openInfoWindow(infoWindow, point);
                    });

                    // 启用滚轮缩放功能
                    map.enableScrollWheelZoom(true);
                } else {
                    alert("无法解析地址，请检查输入内容是否正确！");
                }
            }, "中国");
        }
    </script>

</asp:Content>


