<%@ Page Title="收入查看" Language="C#" MasterPageFile="~/OrderControl/DriverMasterPage.master" AutoEventWireup="true" CodeFile="CheckIncome.aspx.cs" Inherits="OrderControl_CheckIncome" %>

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
    <h2 class="username" style="margin-top:10px; background:#1abc9c; border-radius:20px">
    <p>船只：</p>
<asp:Label ID="lblShipName" runat="server" Text="Label"></asp:Label>
</h2>

<h2 class="username" style="margin-top:10px; background:#1abc9c; border-radius:20px">
    <p>状态：</p>
<asp:Label ID="lblShipStatus" runat="server" Text="Label"></asp:Label>
</h2>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <div class="income-container">
        <h1 class="page-title">收入查看</h1>

        <!-- 订单收入展示 -->
        <div class="income-orders">
            <h2 class="section-title">订单收入</h2>
            <asp:Repeater ID="RepeaterOrders" runat="server">
                <HeaderTemplate>
                    <div class="order-list">
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="order-item">
                        <p class="order-number">订单号: <%# Eval("OrderNumber") %></p>
                        <p class="order-price">收入: ¥<%# Eval("TotalPrice", "{0:F2}") %></p>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
        </div>

        <!-- 收入统计 -->
        <div class="income-summary">
            <h2 class="section-title">收入统计</h2>
            <div class="income-info">
                <div class="income-card">
                    <p>今日收入：</p>
                    <span><asp:Label ID="lblTodayIncome" runat="server" Text="¥0.00"></asp:Label></span>
                </div>
                <div class="income-card">
                    <p>昨日收入：</p>
                    <span><asp:Label ID="lblYesterdayIncome" runat="server" Text="¥0.00"></asp:Label></span>
                </div>
                <div class="income-card">
                    <p>本月收入：</p>
                    <span><asp:Label ID="lblMonthIncome" runat="server" Text="¥0.00"></asp:Label></span>
                </div>
                <div class="income-card">
                    <p>总收入：</p>
                    <span><asp:Label ID="lblTotalIncome" runat="server" Text="¥0.00"></asp:Label></span>
                </div>
            </div>
        </div>

        <!-- 柱状图 -->
        <div class="income-chart">
            <h2 class="section-title">收入柱状图</h2>
            <canvas id="incomeBarChart" width="400" height="200"></canvas>
        </div>
    </div>

    <style>
        .income-container {
            margin: 20px auto;
            max-width: 1200px;
            padding: 20px;
            background-color: #ffffff;
            border-radius: 10px;
            box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
        }

        .page-title {
            font-size: 24px;
            color: #34495e;
            text-align: center;
            margin-bottom: 30px;
        }

        .section-title {
            font-size: 20px;
            color: #2c3e50;
            margin-bottom: 20px;
            border-bottom: 2px solid #ddd;
            padding-bottom: 5px;
        }

        .income-orders {
            margin-bottom: 30px;
        }

        .order-list {
            display: flex;
            flex-wrap: wrap;
            gap: 20px;
        }

        .order-item {
            flex: 1 1 calc(33.33% - 20px);
            background-color: #f9f9f9;
            padding: 15px;
            border-radius: 10px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            text-align: center;
            transition: transform 0.3s ease;
        }

        .order-item:hover {
            transform: scale(1.05);
        }

        .order-number {
            font-size: 16px;
            color: #555;
            margin-bottom: 10px;
        }

        .order-price {
            font-size: 18px;
            color: #e74c3c;
            font-weight: bold;
        }

        .income-info {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
            gap: 20px;
            margin-bottom: 30px;
        }

        .income-card {
            background-color: #f9f9f9;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            text-align: center;
        }

        .income-card p {
            font-size: 16px;
            color: #666;
            margin-bottom: 10px;
        }

        .income-card span {
            font-size: 20px;
            color: #34495e;
            font-weight: bold;
        }

        .income-chart {
            text-align: center;
        }

        canvas {
            max-width: 100%;
            margin: 0 auto;
        }
    </style>

    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            // 使用 Chart.js 创建柱状图
            const ctx = document.getElementById('incomeBarChart').getContext('2d');
            const chartData = JSON.parse('<%= ChartData %>'); // 后端注入的 JSON 数据

            new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: chartData.labels,
                    datasets: [{
                        label: '收入 (¥)',
                        data: chartData.data,
                        backgroundColor: 'rgba(46, 204, 113, 0.7)',
                        borderColor: 'rgba(39, 174, 96, 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });
        });
    </script>
</asp:Content>
