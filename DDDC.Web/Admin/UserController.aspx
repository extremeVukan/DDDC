<%@ Page Title="用户管理" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="UserController.aspx.cs" Inherits="Admin_UserController" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="user-management-container">
        <h2 class="page-title">用户管理</h2>

        <!-- 筛选功能 -->
        <div class="filter-bar">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-box" Placeholder="输入用户编号、昵称或状态"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="search-btn" OnClick="btnSearch_Click" />
        </div>

        <!-- 用户列表 -->
        <div class="user-list-container">
            <div class="user-list">
                <asp:Repeater ID="RepeaterUsers" runat="server">
                    <HeaderTemplate>
                        <div class="user-header">
                            <div>用户编号</div>
                            <div>用户昵称</div>
                            <div>邮箱</div>
                            <div>电话</div>
                            <div>身份</div>
                            <div>状态</div>
                            <div>操作</div>
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="user-item">
                            <div><%# Eval("userid") %></div>
                            <div><%# Eval("username") %></div>
                            <div><%# Eval("email") %></div>
                            <div><%# Eval("phone") %></div>
                            <div><%# Eval("status") %></div>
                            <div class="status-label <%# Eval("userstatu") == "Normal" ? "status-available" : "status-banned" %>">
                                <%# Eval("userstatu") %>
                            </div>
                            <div class="action-buttons">
                                <asp:Button ID="btnEnable" runat="server" Text="启用" CssClass="enable-btn"
                                    CommandName="EnableUser" CommandArgument='<%# Eval("userid") %>' OnClick="btnView_Click" />
                                <asp:Button ID="btnDisable" runat="server" Text="禁用" CssClass="disable-btn"
                                    CommandName="DisableUser" CommandArgument='<%# Eval("userid") %>' OnClick="btnDisable_Click" />
                            </div>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div class="no-user" runat="server" visible='<%# RepeaterUsers.Items.Count == 0 %>'>
                            暂无用户信息。
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>

    <style>
        .user-management-container {
            padding: 20px;
        }

        .page-title {
            text-align: center;
            font-size: 24px;
            color: #34495e;
        }

        .filter-bar {
            display: flex;
            justify-content: center;
            margin: 20px 0;
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

        .user-list-container {
            max-height: 400px; /* 限制列表最大高度 */
            overflow-y: auto; /* 超出时显示滚动条 */
            border: 1px solid #ccc;
            border-radius: 5px;
            padding: 10px;
        }

        .user-list {
            margin-top: 20px;
        }

        .user-header,
        .user-item {
            display: grid;
            grid-template-columns: 1fr 2fr 3fr 2fr 1fr 1fr 2fr; /* 调整列宽比例 */
            gap: 10px;
            padding: 10px;
            border-bottom: 1px solid #ddd;
        }

        /* 表头样式 */
        .user-header {
            background-color: #ecf0f1;
            font-weight: bold;
        }

        /* 单元格样式 */
        .user-header div,
        .user-item div {
            display: flex;
            justify-content: center;
            align-items: center;
            text-align: center;
            padding: 5px;
        }

        /* 状态标签样式 */
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

        .action-buttons {
            display: flex;
            gap: 10px;
        }

        .enable-btn,
        .disable-btn {
            padding: 5px 10px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }

        .enable-btn {
            background-color: #3498db;
            color: white;
        }

        .enable-btn:hover {
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
        .user-list-container::-webkit-scrollbar {
            width: 10px;
        }

        .user-list-container::-webkit-scrollbar-thumb {
            background-color: #bdc3c7;
            border-radius: 5px;
        }

        .user-list-container::-webkit-scrollbar-thumb:hover {
            background-color: #95a5a6;
        }
    </style>
</asp:Content>

