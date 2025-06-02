<%@ Page Title="滴滴打船 - 首页" Language="C#" MasterPageFile="~/Index.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="map-container">
        <h2><i class="fas fa-map-marker-alt"></i> 实时位置</h2>
        <div id="allmap"></div>
    </div>

    <script type="text/javascript">
        function getLocation() {
            var map = new BMap.Map("allmap"); // 创建地图实例
            var geolocation = new BMap.Geolocation(); // 创建地理定位实例

            // 获取当前位置
            geolocation.getCurrentPosition(function (result) {
                if (this.getStatus() === BMAP_STATUS_SUCCESS) {
                    var point = new BMap.Point(result.point.lng, result.point.lat);  // 定位点
                    map.centerAndZoom(point, 15);  // 将地图中心移动到当前位置并设置缩放级别

                    // 添加带动画效果的标记
                    var marker = new BMap.Marker(point);
                    map.addOverlay(marker);
                    marker.setAnimation(BMAP_ANIMATION_BOUNCE); // 设置标记弹跳动画

                    // 添加自定义信息窗口
                    var infoWindow = new BMap.InfoWindow("您的当前位置", {
                        width: 200,
                        height: 60,
                        title: "位置信息",
                        enableMessage: false
                    });
                    marker.addEventListener("click", function () {
                        map.openInfoWindow(infoWindow, point);
                    });

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
                        
                        // 更新信息窗口内容
                        infoWindow.setContent("<div style='padding:5px;'>" + detailedAddress + "</div>");
                        map.openInfoWindow(infoWindow, point);
                    });
                    
                    // 添加控制组件
                    map.addControl(new BMap.NavigationControl());  // 添加平移缩放控件
                    map.addControl(new BMap.ScaleControl());       // 添加比例尺控件
                    map.addControl(new BMap.OverviewMapControl());  // 添加缩略地图控件
                    map.enableScrollWheelZoom(true);  // 允许滚轮缩放
                } else {
                    showToast("定位失败: " + this.getStatus(), "error");
                }
            }, { enableHighAccuracy: true }); 
        }

        // 显示提示信息
        function showToast(message, type) {
            var toast = document.createElement("div");
            toast.className = "toast " + (type || "info");
            toast.innerHTML = message;
            document.body.appendChild(toast);
            
            setTimeout(function() {
                toast.classList.add("show");
                setTimeout(function() {
                    toast.classList.remove("show");
                    setTimeout(function() {
                        document.body.removeChild(toast);
                    }, 300);
                }, 3000);
            }, 100);
        }

        // 页面加载时调用定位函数
        window.onload = getLocation;
    </script>
    <style>
        /* 地图容器样式 */
.map-container {
    height: 100%;
    display: flex;
    flex-direction: column;
    border-radius: 20px;
    overflow: hidden;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    background: rgba(255, 255, 255, 0.1);
}

.map-container h2 {
    background: rgba(255, 255, 255, 0.2);
    margin: 0;
    padding: 15px;
    color: white;
    font-size: 20px;
    display: flex;
    align-items: center;
    gap: 10px;
}

.map-container h2 i {
    color: #1abc9c;
}

#allmap {
    flex-grow: 1;
    border-radius: 0 0 20px 20px;
    overflow: hidden;
}

/* 位置信息容器样式 */
.location-info-container {
    padding: 15px;
    border-radius: 20px;
    background: rgba(255, 255, 255, 0.1);
    height: 100%;
    display: flex;
    flex-direction: column;
    box-sizing: border-box;
}

.section-title {
    margin: 0 0 20px 0;
    padding-bottom: 10px;
    color: white;
    font-size: 18px;
    border-bottom: 1px solid rgba(255, 255, 255, 0.3);
    display: flex;
    align-items: center;
    gap: 10px;
}

.info-group {
    display: flex;
    flex-direction: column;
    gap: 15px;
}

.info-item {
    display: flex;
    flex-direction: column;
    gap: 5px;
}

.info-item label {
    display: flex;
    align-items: center;
    gap: 8px;
    color: rgba(255, 255, 255, 0.9);
    font-size: 16px;
}

.info-item label i {
    color: #1abc9c;
}

.info-input {
    padding: 10px;
    border-radius: 8px;
    border: 1px solid rgba(255, 255, 255, 0.3);
    background: rgba(255, 255, 255, 0.1);
    color: white;
    width: 100%;
    box-sizing: border-box;
}

.action-buttons {
    display: flex;
    justify-content: center;
    margin-top: 20px;
}

.refresh-btn {
    background-color: #1abc9c;
    color: white;
    border: none;
    padding: 10px 20px;
    border-radius: 20px;
    display: flex;
    align-items: center;
    gap: 8px;
    cursor: pointer;
    transition: all 0.3s;
}

.refresh-btn:hover {
    background-color: #16a085;
    transform: scale(1.05);
}

/* 提示消息样式 */
.toast {
    position: fixed;
    top: 20px;
    right: 20px;
    padding: 15px 25px;
    background: rgba(0, 0, 0, 0.7);
    color: white;
    border-radius: 5px;
    z-index: 1000;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
    transition: transform 0.3s, opacity 0.3s;
    transform: translateX(110%);
    opacity: 0;
}

.toast.show {
    transform: translateX(0);
    opacity: 1;
}

.toast.info {
    border-left: 4px solid #3498db;
}

.toast.success {
    border-left: 4px solid #2ecc71;
}

.toast.error {
    border-left: 4px solid #e74c3c;
}

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <div class="location-info-container">
        <h2 class="section-title"><i class="fas fa-location-arrow"></i> 当前位置信息</h2>
        
        <div class="info-group">
            <div class="info-item">
                <label for="txtprovince"><i class="fas fa-map"></i> 省份：</label>
                <asp:TextBox ID="txtprovince" runat="server" CssClass="info-input" ReadOnly="false"></asp:TextBox>
            </div>
            
            <div class="info-item">
                <label for="txtcity"><i class="fas fa-city"></i> 市区：</label>
                <asp:TextBox ID="txtcity" runat="server" CssClass="info-input" ReadOnly="false"></asp:TextBox>
            </div>
            
            <div class="info-item">
                <label for="txtposition"><i class="fas fa-map-pin"></i> 详细位置：</label>
                <asp:TextBox ID="txtposition" runat="server" CssClass="info-input" ReadOnly="false"></asp:TextBox>
            </div>
            
            <div class="action-buttons">
                <button type="button" onclick="getLocation()" class="refresh-btn">
                    <i class="fas fa-sync-alt"></i> 刷新位置
                </button>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder4" Runat="Server">
    <div class="order-panel">
        <div class="panel-header">
            <h2><i class="fas fa-ticket-alt"></i> 订单信息</h2>
        </div>
        
        <!-- 船只信息显示 -->
        <div class="ship-card">
            <div class="ship-image-container">
                <asp:Image ID="shipImage" runat="server" ImageUrl="/UserImg/OIP (2).jpg" AlternateText="船只图片" CssClass="ship-image" />
                <div class="ship-status available">可用</div>
            </div>
            
            <div class="ship-details">
                <div class="detail-item">
                    <span class="detail-label"><i class="fas fa-ship"></i> 船只名称</span>
                    <asp:TextBox ID="txtShipName" runat="server" CssClass="detail-value" ReadOnly="True" />
                </div>
                
                <div class="detail-item">
                    <span class="detail-label"><i class="fas fa-fingerprint"></i> 船只编号</span>
                    <asp:TextBox ID="txtShipID" runat="server" CssClass="detail-value" ReadOnly="True" />
                </div>
                
                <div class="detail-item">
                    <span class="detail-label"><i class="fas fa-users"></i> 乘客量</span>
                    <asp:TextBox ID="txtMaxCapacity" runat="server" CssClass="detail-value" ReadOnly="True" />
                </div>
                
                <div class="detail-item">
                    <span class="detail-label"><i class="fas fa-info-circle"></i> 状态</span>
                    <asp:TextBox ID="txtAvailableSeats" runat="server" CssClass="detail-value" ReadOnly="True" />
                </div>
            </div>
        </div>

        <!-- 位置信息输入 -->
        <div class="destination-form">
            <h3><i class="fas fa-map-signs"></i> 目的地信息</h3>
            
            <div class="form-group">
                <label for="TextBox1"><i class="fas fa-map"></i> 省份</label>
                <asp:TextBox ID="TextBox1" runat="server" CssClass="form-input" placeholder="输入目的地省份"></asp:TextBox>
            </div>
            
            <div class="form-group">
                <label for="TextBox2"><i class="fas fa-city"></i> 市区</label>
                <asp:TextBox ID="TextBox2" runat="server" CssClass="form-input" placeholder="输入目的地市区"></asp:TextBox>
            </div>
            
            <div class="form-group">
                <label for="txtcounty"><i class="fas fa-map-marker-alt"></i> 区县</label>
                <asp:TextBox ID="txtcounty" runat="server" CssClass="form-input" placeholder="输入目的地区县"></asp:TextBox>
            </div>
        </div>

        <!-- 下单按钮 -->
        <div class="order-action">
            <asp:Button ID="btnOrder" runat="server" Text="确认下单" CssClass="order-button" OnClick="btnOrder_Click" />
        </div>
    </div>
    
    <style>
        /* 订单面板样式 */
        .order-panel {
            display: flex;
            flex-direction: column;
            background-color: rgba(255, 255, 255, 0.2);
            border-radius: 20px;
            padding: 20px;
            height: 100%;
            box-sizing: border-box;
        }
        
        .panel-header {
            text-align: center;
            margin-bottom: 20px;
        }
        
        .panel-header h2 {
            color: #333;
            font-size: 22px;
            margin: 0;
            padding: 10px;
            border-bottom: 2px solid rgba(26, 188, 156, 0.5);
            display: flex;
            align-items: center;
            justify-content: center;
        }
        
        .panel-header h2 i {
            margin-right: 10px;
            color: #1abc9c;
        }
        
        /* 船只卡片样式 */
        .ship-card {
            background-color: rgba(255, 255, 255, 0.3);
            border-radius: 15px;
            padding: 15px;
            margin-bottom: 20px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }
        
        .ship-image-container {
            position: relative;
            text-align: center;
            margin-bottom: 10px;
        }
        
        .ship-image {
            width: 100%;
            height: 150px;
            border-radius: 10px;
            object-fit: cover;
        }
        
        .ship-status {
            position: absolute;
            top: 10px;
            right: 10px;
            padding: 5px 10px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: bold;
        }
        
        .ship-status.available {
            background-color: #2ecc71;
            color: white;
        }
        
        .ship-status.unavailable {
            background-color: #e74c3c;
            color: white;
        }
        
        .ship-details {
            display: flex;
            flex-direction: column;
            gap: 10px;
        }
        
        .detail-item {
            display: flex;
            align-items: center;
            gap: 10px;
        }
        
        .detail-label {
            flex: 1;
            font-weight: bold;
            color: #333;
            display: flex;
            align-items: center;
            gap: 5px;
        }
        
        .detail-label i {
            color: #1abc9c;
        }
        
        .detail-value {
            flex: 2;
            background: transparent;
            border: none;
            border-bottom: 1px solid rgba(26, 188, 156, 0.3);
            padding: 5px;
            color: #333;
        }
        
        /* 目的地表单样式 */
        .destination-form {
            background-color: rgba(255, 255, 255, 0.3);
            border-radius: 15px;
            padding: 15px;
            margin-bottom: 10px;
        }
        
        .destination-form h3 {
            color: #333;
            font-size: 18px;
            margin-top: 0;
            margin-bottom: 10px;
            display: flex;
            align-items: center;
            gap: 8px;
        }
        
        .destination-form h3 i {
            color: #1abc9c;
        }
        
        .form-group {
            margin-bottom: 15px;
        }
        
        .form-group label {
            display: block;
            margin-bottom: 5px;
            font-weight: bold;
            color: #333;
            display: flex;
            align-items: center;
            gap: 5px;
        }
        
        .form-group label i {
            color: #1abc9c;
        }
        
        .form-input {
            width: 100%;
            padding: 1px;
            border: 1px solid rgba(26, 188, 156, 0.3);
            border-radius: 8px;
            background-color: rgba(255, 255, 255, 0.5);
            color: #333;
            transition: all 0.3s;
        }
        
        .form-input:focus {
            border-color: #1abc9c;
            box-shadow: 0 0 0 2px rgba(26, 188, 156, 0.2);
            outline: none;
        }
        
        .form-input::placeholder {
            color: rgba(51, 51, 51, 0.5);
        }
        
        /* 下单按钮样式 */
        .order-action {
            text-align: center;
            margin-top: auto;
        }
        
        .order-button {
            background: linear-gradient(135deg, #1abc9c, #16a085);
            color: white;
            border: none;
            padding: 10px 30px;
            border-radius: 25px;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
            transition: all 0.3s;
            box-shadow: 0 4px 6px rgba(22, 160, 133, 0.3);
        }
        
        .order-button:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 8px rgba(22, 160, 133, 0.4);
        }
        
        .order-button:active {
            transform: translateY(0);
            box-shadow: 0 2px 4px rgba(22, 160, 133, 0.4);
        }
    </style>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder5" Runat="Server">
    <div class="ships-catalog">
        <div class="catalog-header">
            <h2><i class="fas fa-ship"></i> 可用船只</h2>
            <div class="search-filter">
                <input type="text" id="shipSearch" placeholder="搜索船只..." onkeyup="filterShips()" />
                <button type="button" class="filter-btn">
                    <i class="fas fa-filter"></i>
                </button>
            </div>
        </div>
        
        <div class="ships-grid" id="shipsGrid">
            <asp:Repeater ID="RepeaterShips" runat="server" OnItemCommand="RepeaterShips_ItemCommand">
                <ItemTemplate>
                    <div class="ship-item" data-name='<%# Eval("ShipName") %>'>
                        <div class="ship-image-wrapper">
                            <img src='<%# ResolveUrl(Eval("Picture").ToString()) %>' alt='<%# Eval("ShipName") %>' />
                            <div class="ship-overlay">
                                <asp:Button ID="btnSelectShip" runat="server" Text="选择此船" CssClass="select-btn"
                                    CommandName="SelectShip"
                                    CommandArgument='<%# Eval("ShipName") + "|" + Eval("ShipID") + "|" + Eval("Capacity") + "|" + Eval("Picture") %>' />
                            </div>
                        </div>
                        <div class="ship-info">
                            <h3><%# Eval("ShipName") %></h3>
                            <p><i class="fas fa-fingerprint"></i> 船只编号: <span class="info-value"><%# Eval("ShipID") %></span></p>
                            <p><i class="fas fa-users"></i> 乘客容量: <span class="info-value"><%# Eval("Capacity") %> 人</span></p>
                            <div class="ship-rating">
                                <i class="fas fa-star"></i>
                                <i class="fas fa-star"></i>
                                <i class="fas fa-star"></i>
                                <i class="fas fa-star"></i>
                                <i class="fas fa-star-half-alt"></i>
                                <span>4.5</span>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>

    <script type="text/javascript">
        function filterShips() {
            var input, filter, grid, items, name, i;
            input = document.getElementById("shipSearch");
            filter = input.value.toUpperCase();
            grid = document.getElementById("shipsGrid");
            items = grid.getElementsByClassName("ship-item");

            for (i = 0; i < items.length; i++) {
                name = items[i].getAttribute("data-name");
                if (name.toUpperCase().indexOf(filter) > -1) {
                    items[i].style.display = "";
                } else {
                    items[i].style.display = "none";
                }
            }
        }
    </script>

    <style>
        /* 船只目录样式 */
        .ships-catalog {
            height: 100%;
            display: flex;
            flex-direction: column;
        }
        
        .catalog-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 15px;
            background-color: rgba(255, 255, 255, 0.2);
            border-radius: 10px;
            margin-bottom: 15px;
        }
        
        .catalog-header h2 {
            margin: 0;
            color: #333;
            font-size: 22px;
            display: flex;
            align-items: center;
            gap: 10px;
        }
        
        .catalog-header h2 i {
            color: #1abc9c;
        }
        
        .search-filter {
            display: flex;
            gap: 10px;
        }
        
        .search-filter input {
            padding: 8px 15px;
            border: 1px solid rgba(26, 188, 156, 0.3);
            border-radius: 20px;
            background-color: rgba(255, 255, 255, 0.5);
        }
        
        .search-filter input:focus {
            outline: none;
            border-color: #1abc9c;
            box-shadow: 0 0 0 2px rgba(26, 188, 156, 0.2);
        }
        
        .filter-btn {
            background-color: #1abc9c;
            color: white;
            border: none;
            border-radius: 50%;
            width: 36px;
            height: 36px;
            display: flex;
            align-items: center;
            justify-content: center;
            cursor: pointer;
        }
        
        /* 船只网格样式 */
        .ships-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
            gap: 20px;
            padding: 15px;
            overflow-y: auto;
            flex-grow: 1;
        }
        
        .ship-item {
            background-color: rgba(255, 255, 255, 0.3);
            border-radius: 15px;
            overflow: hidden;
            transition: transform 0.3s, box-shadow 0.3s;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }
        
        .ship-item:hover {
            transform: translateY(-5px);
            box-shadow: 0 8px 15px rgba(0, 0, 0, 0.1);
        }
        
        .ship-image-wrapper {
            position: relative;
            height: 180px;
            overflow: hidden;
        }
        
        .ship-image-wrapper img {
            width: 100%;
            height: 100%;
            object-fit: cover;
            transition: transform 0.3s;
        }
        
        .ship-item:hover .ship-image-wrapper img {
            transform: scale(1.05);
        }
        
        .ship-overlay {
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: rgba(0, 0, 0, 0.3);
            display: flex;
            align-items: center;
            justify-content: center;
            opacity: 0;
            transition: opacity 0.3s;
        }
        
        .ship-item:hover .ship-overlay {
            opacity: 1;
        }
        
        .select-btn {
            background-color: #1abc9c;
            color: white;
            border: none;
            padding: 10px 20px;
            border-radius: 20px;
            font-weight: bold;
            cursor: pointer;
            transition: all 0.3s;
        }
        
        .select-btn:hover {
            background-color: #16a085;
            transform: scale(1.05);
        }
        
        .ship-info {
            padding: 15px;
        }
        
        .ship-info h3 {
            margin: 0 0 10px 0;
            color: #333;
            font-size: 18px;
        }
        
        .ship-info p {
            margin: 5px 0;
            color: #333;
            display: flex;
            align-items: center;
            gap: 8px;
        }
        
        .ship-info i {
            color: #1abc9c;
        }
        
        .info-value {
            font-weight: bold;
        }
        
        .ship-rating {
            margin-top: 10px;
            color: #f39c12;
            display: flex;
            align-items: center;
            gap: 3px;
        }
        
        .ship-rating span {
            margin-left: 5px;
            color: #333;
        }
    </style>
</asp:Content>

<asp:Content ID="Content6" runat="server" contentplaceholderid="ContentPlaceHolder6">
</asp:Content>
