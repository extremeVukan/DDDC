<%@ Page Title="退款管理" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="RefundController.aspx.cs" Inherits="Admin_RefundController" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="refund-management-container">
        <h2 class="page-title">退款管理</h2>

        <!-- 筛选功能 -->
        <div class="filter-bar">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-box" Placeholder="输入订单号、用户昵称或状态"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="search-btn" OnClick="btnSearch_Click" />
        </div>

        <!-- 退款列表 -->
        <div class="refund-list-container">
            <asp:Repeater ID="RepeaterRefunds" runat="server" OnItemDataBound="RepeaterRefunds_ItemDataBound">
                <HeaderTemplate>
                    <div class="refund-header">
                        <div>订单号</div>
                        <div>用户ID</div>
                        <div>退款理由</div>
                        <div>申请时间</div>
                        <div>状态</div>
                        <div>操作</div>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="refund-item">
                        <div><%# Eval("OrderNumber") %></div>
                        <div><%# Eval("UserID") %></div>
                        <div><%# Eval("Reason") %></div>
                        <div><%# Eval("ApplicationDate", "{0:yyyy-MM-dd HH:mm}") %></div>
                        <div class="status-label <%# Eval("Status").ToString() == "待处理" ? "status-pending" : "status-completed" %>">
                            <%# Eval("Status") %>
                        </div>
                        <div class="action-buttons">
                            <asp:Button ID="btnApprove" runat="server" Text="同意" CssClass="approve-btn"
                                CommandName="Approve" CommandArgument='<%# Eval("RefundID") %>' OnClick="btnApprove_Click" 
                                Visible='<%# Eval("Status").ToString() == "待处理" %>' />
                            <asp:Button ID="btnReject" runat="server" Text="拒绝" CssClass="reject-btn"
                                CommandName="Reject" CommandArgument='<%# Eval("RefundID") %>' OnClick="btnReject_Click" 
                                Visible='<%# Eval("Status").ToString() == "待处理" %>' />
                            <asp:Button ID="btnContact" runat="server" Text="联系用户" CssClass="contact-btn"
                                CommandName="Contact" CommandArgument='<%# Eval("UserID") %>' OnClick="btnContact_Click" />
                        </div>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
            
            <!-- 无数据提示 -->
            <asp:Panel ID="NoDataPanel" runat="server" CssClass="no-refunds" Visible="false">
                暂无退款申请记录。
            </asp:Panel>
        </div>
    </div>

    <style>
        .refund-management-container {
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

        .refund-list-container {
            max-height: 500px; /* 限制列表最大高度 */
            overflow-y: auto; /* 超出时显示滚动条 */
            border: 1px solid #ccc;
            border-radius: 5px;
            padding: 10px;
        }

        .refund-header,
        .refund-item {
            display: grid;
            grid-template-columns: 2fr 2fr 3fr 2fr 1fr 3fr; /* 调整列宽比例 */
            gap: 10px;
            padding: 10px;
            border-bottom: 1px solid #ddd;
        }

        .refund-header {
            background-color: #ecf0f1;
            font-weight: bold;
        }

        .refund-item div {
            display: flex;
            justify-content: center;
            align-items: center;
            text-align: center;
            padding: 5px;
        }

        .status-label {
            font-weight: bold;
            padding: 5px 10px;
            border-radius: 5px;
        }

        .status-pending {
            background-color: #f39c12;
            color: white;
        }

        .status-completed {
            background-color: #2ecc71;
            color: white;
        }

        .action-buttons {
            display: flex;
            gap: 10px;
        }

        .approve-btn {
            background-color: #2ecc71;
            color: white;
            border: none;
            padding: 5px 10px;
            border-radius: 5px;
            cursor: pointer;
        }

        .approve-btn:hover {
            background-color: #27ae60;
        }

        .reject-btn {
            background-color: #e74c3c;
            color: white;
            border: none;
            padding: 5px 10px;
            border-radius: 5px;
            cursor: pointer;
        }

        .reject-btn:hover {
            background-color: #c0392b;
        }

        .contact-btn {
            background-color: #3498db;
            color: white;
            border: none;
            padding: 5px 10px;
            border-radius: 5px;
            cursor: pointer;
        }

        .contact-btn:hover {
            background-color: #2980b9;
        }
        
        .no-refunds {
            padding: 20px;
            text-align: center;
            color: #7f8c8d;
            font-size: 16px;
        }
    </style>
</asp:Content>
