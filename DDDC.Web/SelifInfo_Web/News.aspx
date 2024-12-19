<%@ Page Title="消息中心" Language="C#" MasterPageFile="~/SelifInfo_Web/Self_info.master" AutoEventWireup="true" CodeFile="News.aspx.cs" Inherits="SelifInfo_Web_News" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <br />
<asp:Image ID="Image2" runat="server" Height="96px" Width="136px" style="border-radius: 15px; border: 2px solid #ccc;" />
<br />
<h2 class="username" style="margin-top: 10px;">
    <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label>
</h2>
<p class="email">
    <asp:Label ID="lblemail" runat="server" Text="Label"></asp:Label>
</p>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <div class="news-container">
        <h2 class="news-title">消息中心</h2>
        
        <!-- 主内容区 -->
        <div class="content-wrapper">
            <!-- 消息列表 -->
            <div class="news-list">
                <asp:Repeater ID="RepeaterNews" runat="server">
                    <ItemTemplate>
                        <div class='<%# Eval("read_status").ToString() == "未读" ? "news-item unread" : "news-item" %>'>
                            <!-- 消息头部 -->
                            <div class="news-header">
                                <h3><%# Eval("headText") %></h3>
                                <span class="news-status"><%# Eval("read_status") %></span>
                            </div>
                            
                            <!-- 隐藏的消息详情 -->
                            <div class="news-details">
                                <p><%# Eval("message") %></p>
                                <p><%# Eval("send_time") %></p>
                            </div>
                            
                            <!-- 按钮区域 -->
                            <div class="news-actions">
                                <button type="button" class="toggle-btn" onclick="toggleMessage(this)">展开</button>
                                <asp:Button ID="btnMarkAsRead" runat="server" Text="标记为已读" CssClass="action-btn" 
                                    CommandName="MarkAsRead" CommandArgument='<%# Eval("news_id") %>' OnClick="MarkAsRead_Click" />
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <!-- 筛选按钮 -->
            <div class="filter-container">
                <asp:Button ID="btnAll" runat="server" Text="全部" CssClass="filter-btn" OnClick="FilterMessages" CommandArgument="全部" />
                <asp:Button ID="btnUnread" runat="server" Text="未读" CssClass="filter-btn" OnClick="FilterMessages" CommandArgument="未读" />
                <asp:Button ID="btnRead" runat="server" Text="已读" CssClass="filter-btn" OnClick="FilterMessages" CommandArgument="已读" />
            </div>
        </div>
    </div>
    <style>
        /* 容器样式 */
.news-container {
    width: 80%;
    margin: 0 auto;
    padding: 20px;
    background: #f9f9f9;
    border-radius: 15px;
    box-shadow: 0px 8px 16px rgba(0, 0, 0, 0.2);
}

/* 标题 */
.news-title {
    font-size: 24px;
    text-align: center;
    color: #34495e;
    margin-bottom: 20px;
}

/* 内容区布局 */
.content-wrapper {
    display: flex;
    gap: 20px;
}

/* 消息列表 */
.news-list {
    flex: 3;
    max-height: 600px;
    overflow-y: auto; /* 启用滚动条 */
}

.news-list::-webkit-scrollbar {
    width: 8px;
}

.news-list::-webkit-scrollbar-thumb {
    background-color: #1abc9c;
    border-radius: 4px;
}

/* 筛选按钮 */
.filter-container {
    flex: 1;
    display: flex;
    flex-direction: column;
    gap: 10px;
}

.filter-btn {
    background-color: #1abc9c;
    color: white;
    padding: 10px 20px;
    border: none;
    border-radius: 5px;
    font-size: 14px;
    cursor: pointer;
    transition: background-color 0.3s;
}

.filter-btn:hover {
    background-color: #16a085;
}

/* 单条消息 */
.news-item {
    background: #ffffff;
    padding: 12px;
    margin-bottom: 7px;
    border: 1px solid #ddd;
    border-radius: 10px;
    box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
    transition: box-shadow 0.3s;
}

.news-item:hover {
    box-shadow: 0px 8px 16px rgba(0, 0, 0, 0.2);
}

.news-item.unread {
    border-left: 5px solid #e74c3c;
}

/* 消息头部 */
.news-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 10px;
}

.news-header h3 {
    font-size: 18px;
    color: #34495e;
    margin: 0;
}

.news-status {
    font-size: 14px;
    color: #888;
}

/* 隐藏的消息详情 */
.news-details {
    max-height: 0;
    overflow: hidden;
    transition: max-height 0.3s ease;
    padding-top: 10px;
    color: #555;
}

/* 按钮区域 */
.news-actions {
    display: flex;
    gap: 10px;
    align-items: center;
    margin-top: 6px;
}

.action-btn, .toggle-btn {
    background-color: #4CAF50;
    color: white;
    padding: 8px 16px;
    border: none;
    border-radius: 5px;
    font-size: 14px;
    cursor: pointer;
    transition: background-color 0.3s;
}

.action-btn:hover, .toggle-btn:hover {
    background-color: #45a049;
}

    </style>
    <!-- 展开/收起动画 JS -->
    <script>
        function toggleMessage(button) {
            const details = button.closest('.news-item').querySelector('.news-details');
            const isExpanded = details.style.maxHeight;

            if (!isExpanded || isExpanded === "0px") {
                details.style.maxHeight = details.scrollHeight + "px";
                button.innerText = "收起";
            } else {
                details.style.maxHeight = "0";
                button.innerText = "展开";
            }
        }
    </script>
</asp:Content>



