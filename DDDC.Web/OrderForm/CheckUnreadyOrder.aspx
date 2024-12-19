<%@ Page Title="" Language="C#" MasterPageFile="~/OrderForm/OrderMasterPage.master" AutoEventWireup="true" CodeFile="CheckUnreadyOrder.aspx.cs" Inherits="OrderForm_CheckUnreadyOrder" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="main-container">
        <!-- 左侧订单信息 -->
        <section class="order-info">
            <h2>订单信息</h2>
            
            <!-- 船只信息 -->
            <div class="form-group">
                <label for="txtShipName">船只名称:</label>
                <asp:TextBox ID="txtShipName" runat="server" CssClass="form-input" ReadOnly="True"></asp:TextBox>
            </div>

            <div class="form-group">
                 <label for="txtShipid">船只编号:</label>
                 <asp:TextBox ID="txtShipid" runat="server" CssClass="form-input" ReadOnly="True"></asp:TextBox>
            </div>

            <div class="form-group">
                 <label for="txtownerName">司机姓名:</label>
                 <asp:TextBox ID="txtownerName" runat="server" CssClass="form-input" ReadOnly="True"></asp:TextBox>
            </div>
            <!-- 最大成员数 -->
            

            <!-- 目的地 -->
            <div class="form-group">
                <label for="txtDestination">目的地:</label>
                <asp:TextBox ID="txtDestination" runat="server" CssClass="form-input" Placeholder="请输入目的地"></asp:TextBox>
            </div>

            <!-- 备注信息 -->
            <div class="form-group">
                <label for="txtRemarks">备注信息:</label>
                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-input" TextMode="MultiLine" Rows="8" Placeholder="填写特殊要求或备注信息"></asp:TextBox>
            </div>

            <!-- 订单号 -->
            <div class="form-group">
                <label for="txtOrderNumber">订单号:</label>
                <asp:TextBox ID="txtOrderNumber" runat="server" CssClass="form-input" ReadOnly="True"></asp:TextBox>
            </div>

            <!-- 提交按钮 -->
            <div class="form-action">
                <asp:Button ID="btnSubmitOrder" runat="server" Text="订单已提交" CssClass="bn-button"  Enabled="False" />
                <asp:Button ID="btncancelOrder" runat="server" Text="取消订单" CssClass="submit-button"  OnClick="btncancelOrder_Click"/>
            </div>
        </section>

        <!-- 右侧地图展示 -->
        <section class="map-container">
            <h2>船只照片</h2>
            <!-- 船只照片 -->
            <asp:Image ID="imgShipPhoto" runat="server" CssClass="ship-photo" AlternateText="船只照片" />

            <h2>地图展示</h2>
            <div id="allmap"></div>
            <div class="form-group" style="margin-top: 20px;">
    <label for="txtPrice">距离(米):</label>
    <asp:TextBox ID="txtPrice" runat="server" CssClass="form-input" ReadOnly="True" Placeholder="路程"></asp:TextBox>
</div>
        </section>

    </div>

    <!-- 百度地图脚本 -->
    <script type="text/javascript" src="https://api.map.baidu.com/api?v=3.0&ak=EAk6CzkY3oWmsIFib3VBNijhD7MGWCaC"></script>
    <script type="text/javascript">
        // 初始化地图
        function initMap() {
            var map = new BMap.Map("allmap"); // 创建地图实例
            var point = new BMap.Point(116.404, 39.915); // 设置地图中心点（示例：北京）
            map.centerAndZoom(point, 15); // 设置中心和缩放级别
            map.enableScrollWheelZoom(); // 开启鼠标滚轮缩放
            return map;
        }

        // 计算并显示直线距离
        function calculateAndDisplayRoute() {
            var currentPosition = "<%= Session["CheckHere1"] %>"; // 获取Session中的当前位置
            var destination = "<%= Session["CheckDestination1"] %>" // 获取目的地输入框内容

        if (currentPosition && destination) {
            var map = initMap(); // 初始化地图
            var geocoder = new BMap.Geocoder(); // 创建地理编码器

            // 自定义图标
            var shipIcon = new BMap.Icon('<%= ResolveUrl("~/UserImg/客户.png") %>', new BMap.Size(32, 32));
            var destinationIcon = new BMap.Icon('<%= ResolveUrl("~/UserImg/目的地.png") %>', new BMap.Size(32, 32));

            // 获取当前位置经纬度
            geocoder.getPoint(currentPosition, function (startPoint) {
                if (startPoint) {
                    // 在地图上标记当前位置
                    var shipMarker = new BMap.Marker(startPoint, { icon: shipIcon });
                    map.addOverlay(shipMarker);

                  

                    // 获取目的地经纬度
                    geocoder.getPoint(destination, function (endPoint) {
                        if (endPoint) {
                            // 在地图上标记目的地
                            var destinationMarker = new BMap.Marker(endPoint, { icon: destinationIcon });
                            map.addOverlay(destinationMarker);

                           

                            // 绘制直线
                            var polyline = new BMap.Polyline([startPoint, endPoint], {
                                strokeColor: "Green", // 线颜色
                                strokeWeight: 4, // 线宽度
                                strokeOpacity: 0.8 // 透明度
                            });
                            map.addOverlay(polyline);

                            // 自动调整视野以适应两点
                            var bounds = new BMap.Bounds();
                            bounds.extend(startPoint);
                            bounds.extend(endPoint);
                            map.setViewport([startPoint, endPoint]);

                            // 计算并显示距离
                            var distance = map.getDistance(startPoint, endPoint); // 计算两点之间的距离
                            var distanceText = distance.toFixed(2); // 转换为字符串并保留两位小数
                            document.getElementById('<%= txtPrice.ClientID %>').value = distanceText;
                        } else {
                            alert("无法获取目的地的位置！");
                        }
                    });
                } else {
                    alert("无法获取当前位置！");
                }
            });
            } else {
                alert("当前位置或目的地不能为空！");
            }
        }

        // 页面加载时初始化地图和计算距离
        window.onload = function () {
            initMap();
            calculateAndDisplayRoute();
        };
    </script>



    <!-- 样式 -->
    <style>
        /* 主内容区域样式 */
        .main-container {
            display: flex;
            flex-wrap: wrap;
            justify-content: center;
            align-items: flex-start;
            padding: 20px;
            gap: 30px;
        }

        /* 订单信息样式 */
        .order-info,
        .map-container {
            flex: 1 1 45%; /* 保证两部分均匀分布 */
            background-color: #fff;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            padding: 20px;
            min-width: 300px; /* 设置最小宽度 */
        }

        .order-info h2,
        .map-container h2 {
            text-align: center;
            margin-bottom: 20px;
            font-size: 24px;
            color: #34495e;
        }

        .form-group {
            margin-bottom: 15px;
        }

        .form-group label {
            display: block;
            font-weight: bold;
            margin-bottom: 5px;
            color: #34495e;
        }

        .form-input {
            width: 100%;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 5px;
            font-size: 14px;
        }

        .submit-button {
            display: block;
            width: 100%;
            padding: 10px 20px;
            background-color: #4CAF50;
            color: white;
            border: none;
            border-radius: 5px;
            font-size: 16px;
            cursor: pointer;
            transition: background-color 0.3s;
            margin-top: 20px;
        }
        .bn-button {
    display: block;
    width: 100%;
    padding: 10px 20px;
    background-color: gray;
    color: white;
    border: none;
    border-radius: 5px;
    font-size: 16px;
    cursor: pointer;
    transition: background-color 0.3s;
    margin-top: 20px;
}
        .submit-button:hover {
            background-color: #45a049;
        }

        /* 船只照片样式 */
        .ship-photo {
            display: block;
            margin: 0 auto 20px;
            width: 300px;
            height: 150px;
            border-radius: 10px;
            object-fit: cover; /* 保持图片比例，填充父容器 */
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

        /* 地图容器样式 */
        #allmap {
            width: 100%;
            height: 400px;
            border-radius: 10px;
            border: 1px solid #ddd;
        }

        /* 响应式样式 */
        @media screen and (max-width: 768px) {
            .main-container {
                flex-direction: column;
                align-items: center;
            }

            .order-info,
            .map-container {
                flex: 1 1 100%;
                max-width: 100%;
            }
        }
    </style>
</asp:Content>


