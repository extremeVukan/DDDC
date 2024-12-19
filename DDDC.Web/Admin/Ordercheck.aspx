<%@ Page Title="订单管理" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="Ordercheck.aspx.cs" Inherits="Admin_Ordercheck" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="order-management-container">
        <h1 class="page-title">订单管理</h1>

        <!-- 筛选功能 -->
        <div class="filter-bar">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-box" Placeholder="输入订单编号或状态"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="search-btn" />
        </div>

        <!-- 订单列表 -->
        <div class="order-table">
            <asp:GridView runat="server"
                DataSourceID="LinqDataSource2"
                ID="ctl02"
                AutoGenerateColumns="False"
                CssClass="styled-grid"
                GridLines="None"
                AllowPaging="True"
                PageSize="5"
                AllowSorting="True"
                OnPageIndexChanging="ctl02_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="OrderNumber" HeaderText="订单编号" ReadOnly="True" SortExpression="OrderNumber" />
                    <asp:BoundField DataField="ClientID" HeaderText="客户编号" ReadOnly="True" SortExpression="ClientID" />
                    <asp:BoundField DataField="ShipName" HeaderText="船只名称" ReadOnly="True" SortExpression="ShipName" />
                    <asp:BoundField DataField="OwnerName" HeaderText="船主姓名" ReadOnly="True" SortExpression="OwnerName" />
                    <asp:BoundField DataField="PrePosition" HeaderText="起始位置" ReadOnly="True" SortExpression="PrePosition" />
                    <asp:BoundField DataField="Destination" HeaderText="目的地" ReadOnly="True" SortExpression="Destination" />
                    <asp:BoundField DataField="Notes" HeaderText="备注" ReadOnly="True" SortExpression="Notes" />
                    <asp:BoundField DataField="Start_Time" HeaderText="开始时间" ReadOnly="True" SortExpression="Start_Time" />
                    <asp:BoundField DataField="End_Time" HeaderText="结束时间" ReadOnly="True" SortExpression="End_Time" />
                    <asp:TemplateField HeaderText="状态">
                        <ItemTemplate>
                            <span class="status-label <%# Eval("Status").ToString().ToLower() %>">
                                <%# Eval("Status") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Distance" HeaderText="距离 (km)" ReadOnly="True" SortExpression="Distance" />
                    <asp:BoundField DataField="Comment" HeaderText="评论" ReadOnly="True" SortExpression="Comment" />
                </Columns>
            </asp:GridView>
        </div>

        <!-- 数据源 -->
        <asp:LinqDataSource runat="server"
            ID="LinqDataSource2"
            ContextTypeName="DDDC.DAL.DataClasses1DataContext"
            TableName="OrderForm"
            Select="new (OrderNumber, ClientID, ShipName, OwnerName, PrePosition, Destination, Notes, Start_Time, End_Time, img, Status, ShipID, OrderID, OwnerID, Distance, Comment)"
            OrderBy="OrderID descending" />
    </div>

    <style>
        /* 容器样式 */
        .order-management-container {
            padding: 20px;
            max-width: 1200px;
            margin: auto;
        }

        .page-title {
            text-align: center;
            font-size: 28px;
            color: #2c3e50;
            margin-bottom: 30px;
        }

        /* 筛选条 */
        .filter-bar {
            display: flex;
            justify-content: center;
            margin-bottom: 20px;
        }

        .search-box {
            padding: 10px;
            width: 300px;
            border: 1px solid #ccc;
            border-radius: 5px;
            margin-right: 10px;
        }

        .search-btn {
            background-color: #3498db;
            color: white;
            padding: 10px 20px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }

        .search-btn:hover {
            background-color: #2980b9;
        }

        /* 表格样式 */
        .styled-grid {
            width: 100%;
            border-collapse: collapse;
        }

        .styled-grid th, .styled-grid td {
            text-align: center;
            padding: 10px;
            border-bottom: 1px solid #ddd;
        }

        .styled-grid th {
            background-color: #f4f6f9;
            font-weight: bold;
            color: #34495e;
        }

        .styled-grid tr:nth-child(even) {
            background-color: #f9f9f9;
        }

        .styled-grid tr:hover {
            background-color: #eaf2f8;
        }

        /* 状态标签 */
        .status-label {
            padding: 5px 10px;
            border-radius: 5px;
            font-size: 14px;
            font-weight: bold;
            color: white;
            text-transform: capitalize;
        }

        .status-label.pending {
            background-color: #f1c40f;
        }

        .status-label.completed {
            background-color: #2ecc71;
        }

        .status-label.cancelled {
            background-color: #e74c3c;
        }

        /* 分页样式 */
        .styled-grid .pager {
            text-align: center;
            padding: 10px;
            background-color: #f4f6f9;
        }

        .styled-grid .pager a {
            color: #3498db;
            padding: 5px 10px;
            text-decoration: none;
            border-radius: 5px;
            margin: 0 5px;
            border: 1px solid #3498db;
        }

        .styled-grid .pager a:hover {
            background-color: #3498db;
            color: white;
        }

        .styled-grid .pager span {
            padding: 5px 10px;
            color: #fff;
            background-color: #3498db;
            border-radius: 5px;
            border: 1px solid #3498db;
            margin: 0 5px;
        }
    </style>
</asp:Content>


