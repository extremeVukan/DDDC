﻿<%@ Page Title="" Language="C#" MasterPageFile="~/ClientOrder/MasterPage.master" AutoEventWireup="true" CodeFile="CFhinshOrder.aspx.cs" Inherits="ClientOrder_CFhinshOrder" %>



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
    <div class="content-wrapper">
        <h1 class="page-title">历史订单</h1>
        <p class="page-description">查看订单详情</p>

       
        <div class="order-table">
            <asp:GridView runat="server" DataSourceID="LinqDataSource1" ID="ctl02" AutoGenerateColumns="False" CssClass="styled-grid" GridLines="None" AllowPaging="True" AllowSorting="True" PageSize="5" OnPageIndexChanging="ctl02_PageIndexChanging">
                 <Columns>
     <asp:BoundField DataField="OrderID" HeaderText="编号" ReadOnly="True" SortExpression="OrderID"></asp:BoundField>
         <asp:BoundField DataField="OrderNumber" HeaderText="订单号" SortExpression="OrderNumber"></asp:BoundField>
         
         <asp:BoundField DataField="ShipName" HeaderText="船名" SortExpression="ShipName"></asp:BoundField>
         
         
         <asp:BoundField DataField="OwnerName" HeaderText="船主" SortExpression="OwnerName"></asp:BoundField>
         <asp:BoundField DataField="PrePosition" HeaderText="预定位置" SortExpression="PrePosition"></asp:BoundField>
         <asp:BoundField DataField="Destination" HeaderText="目的地" SortExpression="Destination"></asp:BoundField>
         <asp:BoundField DataField="Notes" HeaderText="备注" SortExpression="Notes"></asp:BoundField>
         <asp:BoundField DataField="Start_Time" HeaderText="开始时间" SortExpression="Start_Time"></asp:BoundField>
         

         <asp:BoundField DataField="Status" HeaderText="状态" SortExpression="Status"></asp:BoundField>


         <asp:ImageField DataImageUrlField="img" NullImageUrl="~/UserImg/暂无图片.gif" HeaderText="图片" SortExpression="img"></asp:ImageField>

         <asp:TemplateField>
             <ItemTemplate>
                 <asp:Button ID="btnCheckOrder" runat="server" Text="查看订单" 
                     CommandName="CheckOrder" 
                     CommandArgument='<%# Eval("OrderID") %>'
                      OnClick="btnCheckOrder_Click"  CssClass="accept-btn" />
                                
                 <asp:Button ID="btnRefund" runat="server" Text="申请退款" 
                    CommandName="Refund" 
                    CommandArgument='<%# Eval("OrderID") %>'
                      OnClick="btnRefund_Click" CssClass="accept-btn" />
                 
              </ItemTemplate>

         </asp:TemplateField>
     </Columns>
                
    

            </asp:GridView>
        </div>
    </div>

    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSource1"
        ContextTypeName="DDDC.DAL.DataClasses1DataContext"
        TableName="OrderForm" Where="ClientID == @ClientID && Status == @Status || Status == @Status1 || Status == @Status2" Select="new (OrderNumber, ClientID, ShipName, OwnerName, PrePosition, Destination, Notes, Start_Time, End_Time, img, Status, ShipID, OrderID)" OrderBy="OrderID descending">
        <WhereParameters>
            <asp:SessionParameter SessionField="UserID" Name="ClientID" Type="Int32"></asp:SessionParameter>
            <asp:Parameter DefaultValue="已完成" Name="Status" Type="String"></asp:Parameter>
            <asp:Parameter DefaultValue="已退款" Name="Status1" Type="String"></asp:Parameter>
            <asp:Parameter DefaultValue="退款中" Name="Status2" Type="String"></asp:Parameter>
        </WhereParameters>
    </asp:LinqDataSource>

    
    <style>
        /* 页面内容容器 */
        .content-wrapper {
            max-width: 1600px;
            margin-top:30px;
            margin-left:240px;
            padding: 20px;
            background-color: #fff;
            border-radius: 10px;
            box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
        }

        /* 页面标题 */
        .page-title {
            font-size: 32px;
            color: #34495e;
            margin-bottom: 10px;
        }

        /* 页面描述 */
        .page-description {
            font-size: 18px;
            color: #7f8c8d;
            margin-bottom: 20px;
        }

        /* GridView 样式 */
        .styled-grid {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

        .styled-grid th,
        .styled-grid td {
            padding: 15px;
            text-align: center;
            border: 1px solid #ddd;
        }

        .styled-grid th {
            background-color: #f1f1f1;
            font-weight: bold;
            color: #34495e;
        }

        .styled-grid tr:nth-child(even) {
            background-color: #f9f9f9;
        }

        .styled-grid tr:hover {
            background-color: #e9e9e9;
            cursor: pointer;
        }

        .styled-grid img {
            max-width: 80px;
            height: auto;
            border-radius: 5px;
        }

        /* 接受订单按钮样式 */
        .accept-btn {
            background-color: #4CAF50;
            color: white;
            border: none;
            padding: 10px 20px;
            border-radius: 5px;
            cursor: pointer;
            transition: background-color 0.3s ease;
        }

        .accept-btn:hover {
            background-color: #45a049;
        }

        /* 响应式设计 */
        @media screen and (max-width: 768px) {
            .content-wrapper {
                padding: 15px;
            }

            .page-title {
                font-size: 28px;
            }

            .styled-grid th,
            .styled-grid td {
                padding: 10px;
                font-size: 14px;
            }

            .styled-grid img {
                max-width: 60px;
            }

            .accept-btn {
                padding: 8px 16px;
            }
        }
    </style>
</asp:Content>


