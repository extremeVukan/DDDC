<%@ Page Title="船只信息管理" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="ShipController.aspx.cs" Inherits="Admin_ShipController" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="ship-management-container">
        <h2 class="page-title">船只信息管理</h2>

        <!-- 筛选功能 -->
        <div class="filter-bar">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-box" Placeholder="输入船只编号、名称或状态"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="search-btn" OnClick="btnSearch_Click" />
        </div>

        <!-- 船只列表 -->
        <div class="ship-list-container">
            <div class="ship-list">
                <asp:Repeater ID="RepeaterShips" runat="server">
                    <HeaderTemplate>
                        <div class="ship-header">
                            <div>图片</div>
                            <div>船只编号</div>
                            <div>船只名称</div>
                            <div>类型</div>
                            <div>容量</div>
                            <div>状态</div>
                            <div>操作</div>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="ship-item">
                            <div class="ship-image">
                                <img src='<%# ResolveUrl(Eval("Picture").ToString()) %>' alt="船只照片" />
                            </div>
                            <div><%# Eval("shipid") %></div>
                            <div><%# Eval("shipname") %></div>
                            <div><%# Eval("shiptype") %></div>
                            <div><%# Eval("capacity") %> 人</div>
                            <div class="status-label <%# Eval("ship_status") == "Available" ? "status-available" : "status-banned" %>">
                                <%# Eval("ship_status") %>
                            </div>
                            <div class="action-buttons">
                                <asp:Button ID="btnView" runat="server" Text="启用" CssClass="view-btn"
                                    CommandName="ViewShip" CommandArgument='<%# Eval("shipid") %>' OnClick="btnView_Click" />
                                <asp:Button ID="btnDisable" runat="server" Text="禁用" CssClass="disable-btn"
                                    CommandName="DisableShip" CommandArgument='<%# Eval("shipid") %>' OnClick="btnDisable_Click" />
                            </div>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div class="no-ship" runat="server" visible='<%# RepeaterShips.Items.Count == 0 %>'>
                            暂无船只信息。
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>

    <style>
        .ship-management-container {
            padding: 20px;
        }

        .page-title {
            text-align: center;
            font-size: 24px;
            color: #34495e;
            margin-bottom: 20px;
        }

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

        .ship-list-container {
            max-height: 400px;
            overflow-y: auto;
            border: 1px solid #ccc;
            border-radius: 5px;
            padding: 10px;
        }

        .ship-list {
            margin-top: 20px;
        }

        .ship-header,
        .ship-item {
            display: grid;
            grid-template-columns: 1.5fr 1fr 2fr 2fr 1fr 1fr 2fr; /* 根据内容调整列宽 */
            gap: 10px;
            padding: 10px;
            border-bottom: 1px solid #ddd;
        }

        .ship-header {
            background-color: #ecf0f1;
            font-weight: bold;
        }

        .ship-header div,
        .ship-item div {
            display: flex;
            justify-content: center;
            align-items: center;
            text-align: center;
            padding: 5px;
        }

        .ship-item img {
            width: 100%;
            height: 100px;
            object-fit: cover;
            border-radius: 5px;
        }

        .status-label {
            font-weight: bold;
            padding: 5px 10px;
            border-radius: 5px;
        }

        .status-available {
            background-color: #2ecc71;
            color: white;
        }

        .status-banned {
            background-color: #e74c3c;
            color: white;
        }

        .status-other {
            background-color: #f1c40f;
            color: white;
        }

        .action-buttons {
            display: flex;
            gap: 10px;
        }

        .view-btn,
        .disable-btn {
            padding: 5px 10px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }

        .view-btn {
            background-color: #3498db;
            color: white;
        }

        .view-btn:hover {
            background-color: #2980b9;
        }

        .disable-btn {
            background-color: #e67e22;
            color: white;
        }

        .disable-btn:hover {
            background-color: #d35400;
        }

        /* 滚动条样式（可选） */
        .ship-list-container::-webkit-scrollbar {
            width: 10px;
        }

        .ship-list-container::-webkit-scrollbar-thumb {
            background-color: #bdc3c7;
            border-radius: 5px;
        }

        .ship-list-container::-webkit-scrollbar-thumb:hover {
            background-color: #95a5a6;
        }
    </style>
</asp:Content>



