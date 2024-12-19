<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="CheckSHip.aspx.cs" Inherits="Admin_CheckSHip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- 地图容器 -->
    <div class="map-container">
        <h2>船只实时位置</h2>

        <!-- 搜索船只功能 -->
        <div class="search-container">
            
            <input type="text" id="txtShipID" placeholder="输入船只ID进行搜索..." />
            <button type="button" id="btnSearch" onclick="searchShip()">搜索</button>
        </div>

        <!-- 百度地图 -->
        <div id="allmap"></div>
    </div>

    <!-- 百度地图API -->
    <script type="text/javascript" src="https://api.map.baidu.com/api?v=3.0&ak=EAk6CzkY3oWmsIFib3VBNijhD7MGWCaC"></script>
    <script type="text/javascript">
        // 获取船只数据 (传递 JSON 数据)
        var shipData = <%= GetShipsJson() %>;

        // 初始化百度地图
        var map = new BMap.Map("allmap");
        var centerPoint = new BMap.Point(116.404, 39.915); // 初始中心点
        map.centerAndZoom(centerPoint, 6);
        map.enableScrollWheelZoom();

        // 自定义图标
        var shipIcon = new BMap.Icon('<%= ResolveUrl("~/UserImg/船只1.png") %>', new BMap.Size(32, 32));

        var markers = []; // 存储所有标记

        // 遍历船只数据并添加标记
        shipData.forEach(function (ship) {
            var fullAddress = ship.Province + ship.City + ship.Position;

            var geocoder = new BMap.Geocoder();
            geocoder.getPoint(fullAddress, function (point) {
                if (point) {
                    var marker = new BMap.Marker(point, { icon: shipIcon });
                    marker.shipID = ship.ShipID; // 将船只ID绑定到标记上
                    map.addOverlay(marker);
                    markers.push({ id: ship.ShipID, marker: marker, point: point });

                    // 添加信息窗口
                    var infoWindow = new BMap.InfoWindow(
                        `<div>
                            <h4>船只名称: ${ship.ShipName}</h4>
                            <p>船只编号: ${ship.ShipID}</p>
                            <p>位置: ${fullAddress}</p>
                        </div>`,
                        { width: 200, height: 100 }
                    );

                    marker.addEventListener("click", function () {
                        map.openInfoWindow(infoWindow, point);
                    });
                } else {
                    console.log(`无法定位: ${fullAddress}`);
                }
            });
        });

        // 搜索船只ID并聚焦
        function searchShip(event) {
            if (event) event.preventDefault(); // 阻止默认行为

            var searchID = document.getElementById('txtShipID').value.trim();
            if (!searchID) {
                alert("请输入船只ID进行搜索！");
                return;
            }

            var found = false;
            for (var i = 0; i < markers.length; i++) {
                if (markers[i].id === searchID) {
                    map.panTo(markers[i].point); // 地图聚焦到该船只的位置
                    map.openInfoWindow(new BMap.InfoWindow(`<div>船只编号: ${searchID}</div>`), markers[i].point);
                    found = true;
                    break;
                }
            }

            if (!found) {
                alert("未找到对应的船只ID，请检查输入！");
            }
        }
        

    </script>

    <!-- 样式 -->
    <style>
        .map-container {
            text-align: center;
            margin-top: -20px;
        }

        .map-container h2 {
            font-size: 24px;
            color: #333;
            margin-bottom: 20px;
        }

        /* 地图样式 */
        #allmap {
            width: 100%;
            height: 600px;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        }

        /* 搜索栏样式 */
        .search-container {
            display: flex;
            justify-content: center;
            margin-bottom: 20px;
        }

        .search-container input {
            width: 300px;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 5px;
            font-size: 16px;
            margin-right: 10px;
            outline: none;
        }

        .search-container button {
            padding: 10px 20px;
            background-color: #1abc9c;
            color: white;
            border: none;
            border-radius: 5px;
            font-size: 16px;
            cursor: pointer;
            transition: background-color 0.3s ease;
        }

        .search-container button:hover {
            background-color: #16a085;
        }
    </style>
</asp:Content>




