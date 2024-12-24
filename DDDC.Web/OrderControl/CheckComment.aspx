<%@ Page Title="查看评论" Language="C#" MasterPageFile="~/OrderControl/DriverMasterPage.master" AutoEventWireup="true" CodeFile="CheckComment.aspx.cs" Inherits="OrderControl_CheckComment" %>

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
    <div class="comments-container">
        <div class="header-bar">
            <h1 class="page-title">订单评论</h1>
            <div class="filter-section">
                <label for="ratingFilter">按评分筛选:</label>
                <select id="ratingFilter" onchange="filterComments()">
                    <option value="all">全部</option>
                    <option value="5">5 星</option>
                    <option value="4">4 星</option>
                    <option value="3">3 星</option>
                    <option value="2">2 星</option>
                    <option value="1">1 星</option>
                </select>
            </div>
        </div>

        <div class="comment-wrapper">
            <asp:Repeater ID="RepeaterComments" runat="server">
                <HeaderTemplate>
                    <div class="comment-list" id="commentList">
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="comment-item" data-rating="<%# Eval("Estimate") %>">
                        <div class="comment-header">
                            <span class="order-number">订单号: <%# Eval("OrderNumber") %></span>
                            <span class="order-date">日期: <%# Eval("OrderDate", "{0:yyyy-MM-dd}") %></span>
                        </div>
                        <div class="rating">
                            <%# GenerateStars(Eval("Estimate")) %>
                        </div>
                        <div class="comment-body">
                            <p class="comment-text"><strong>评论:</strong> <%# Eval("Comment") %></p>
                        </div>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                    <div class="no-comments" runat="server" visible='<%# RepeaterComments.Items.Count == 0 %>'>
                        暂无评论记录。
                    </div>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>

    <style>
        /* 父容器样式 */
        .comments-container {
            margin-top: 20px;
            margin-left:16%;
            max-width: 1400px;
            padding: 20px;
            background-color: #ffffff;
            border-radius: 10px;
            box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
        }

        .header-bar {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
        }

        .page-title {
            font-size: 24px;
            color: #34495e;
            margin: 0;
        }

        .filter-section {
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .filter-section label {
            font-size: 16px;
            color: #555;
        }

        #ratingFilter {
            padding: 5px;
            font-size: 16px;
            border: 1px solid #ddd;
            border-radius: 5px;
        }

        /* 滚动容器 */
        .comment-wrapper {
            max-height: 600px; /* 限制高度 */
            overflow-y: auto; /* 垂直滚动条 */
            padding: 10px;
        }

        /* 瀑布流布局 */
        .comment-list {
            display: grid;
            grid-template-columns: repeat(5, 1fr); /* 设置 5 列 */
            gap: 20px; /* 列间距 */
        }

        .comment-item {
            background-color: #f9f9f9;
            padding: 15px;
            border-radius: 10px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
            transition: transform 0.3s, opacity 0.3s;
            animation: fadeIn 0.5s ease;
        }

        .comment-item:hover {
            transform: scale(1.02);
        }

        .comment-header {
            display: flex;
            justify-content: space-between;
            font-size: 16px;
            color: #555;
            margin-bottom: 10px;
        }

        .rating {
            font-size: 20px;
            color: #f1c40f; /* 黄色星星 */
            margin-bottom: 10px;
        }

        .comment-body {
            font-size: 16px;
            color: #333;
        }

        .comment-body .comment-text {
            margin: 10px 0;
        }

        .no-comments {
            text-align: center;
            font-size: 16px;
            color: #888;
            margin-top: 20px;
        }

        /* 动画 */
        @keyframes fadeIn {
            from {
                opacity: 0;
                transform: translateY(20px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }
    </style>

    <script>
        // 筛选评论功能
        function filterComments() {
            const filterValue = document.getElementById("ratingFilter").value;
            const comments = document.querySelectorAll(".comment-item");

            comments.forEach(comment => {
                const rating = comment.getAttribute("data-rating");
                if (filterValue === "all" || rating === filterValue) {
                    comment.style.display = "block";
                } else {
                    comment.style.display = "none";
                }
            });
        }
    </script>
</asp:Content>



