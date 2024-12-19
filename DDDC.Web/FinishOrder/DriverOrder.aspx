<%@ Page Title="" Language="C#" MasterPageFile="~/FinishOrder/MasterPage.master" AutoEventWireup="true" CodeFile="DriverOrder.aspx.cs" Inherits="FinishOrder_DriverOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="content-wrapper">
    <!-- 地图区域 -->
    <div class="map-container">
        <h2>订单全程</h2>
        <div id="allmap"></div>
    </div>

    <!-- 订单信息区域 -->
    <div class="order-info">
        <h2>订单详情</h2>
        <div class="info-group">
            <label for="orderNumber">订单号:</label>
            <asp:TextBox ID="txtOrderNumber" runat="server" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="info-group">
            <label for="shipName">船只名称:</label>
            <asp:TextBox ID="txtShipName" runat="server" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="info-group">
            <label for="preshipPosition">当前位置:</label>
            <asp:TextBox ID="txtShipPosition" runat="server" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="info-group">
            <label for="prePosition">客户位置:</label>
            <asp:TextBox ID="txtPrePosition" runat="server" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="info-group">
            <label for="Phone">客户电话:</label>
            <asp:TextBox ID="txtCPhone" runat="server" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="info-group">
            <label for="destination">目的地:</label>
            <asp:TextBox ID="txtDestination" runat="server" ReadOnly="True"></asp:TextBox>
        </div>

        <div class="progress-container">
            <label>订单进度:</label>
            <div class="progress-bar">
                <div class="progress" style="width: 50%;"></div>
            </div>
            <div class="progress-status">状态: 下单成功</div>
        </div>

        <asp:Button ID="btnCompleteOrder" runat="server" Text="完成订单" CssClass="complete-button" OnClick="btnCompleteOrder_Click"/>
    </div>
</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
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
    
<asp:Label ID="lblShipName" runat="server" Text="Label"></asp:Label>
</h2>

<h2 class="username" style="margin-top:10px; background:#1abc9c; border-radius:20px">
    
<asp:Label ID="lblShipStatus" runat="server" Text="Label"></asp:Label>
</h2>
    
</asp:Content>
