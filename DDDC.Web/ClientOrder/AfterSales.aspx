<%@ Page Title="申请退款" Language="C#" MasterPageFile="~/ClientOrder/MasterPage.master" AutoEventWireup="true" CodeFile="AfterSales.aspx.cs" Inherits="ClientOrder_AfterSales" %>

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
    <div class="refund-container">
        <h1 class="page-title">申请退款</h1>
        <p class="page-description">请确认订单信息并填写退款理由</p>

        <div class="refund-form">
            <!-- 显示订单信息 -->
            <div class="form-group">
                <label for="orderInfo">订单信息</label>
                <div id="orderInfo" class="order-info fade-in">
                    <p><strong>订单号：</strong><span><asp:Label ID="lblOrderNumber" runat="server" /></span></p>
                    <p><strong>订单金额：</strong><span><asp:Label ID="lblOrderAmount" runat="server" /></span></p>
                    <p><strong>订单时间：</strong><span><asp:Label ID="lblOrderTime" runat="server" /></span></p>
                </div>
            </div>

            <!-- 选择退款理由 -->
            <div class="form-group slide-in">
                <label for="reason">退款理由</label>
                <asp:DropDownList ID="ddlReason" runat="server" CssClass="form-control">
                    <asp:ListItem Text="请选择退款理由" Value="" />
                    <asp:ListItem Text="服务不满意" Value="服务不满意" />
                    <asp:ListItem Text="误操作下单" Value="误操作下单" />
                    <asp:ListItem Text="其他问题" Value="其他问题" />
                </asp:DropDownList>
                <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="4" Placeholder="请输入详细说明（选填）"></asp:TextBox>
            </div>

            <!-- 提交按钮 -->
            <div class="form-actions zoom-in">
                <asp:Button ID="btnSubmit" runat="server" Text="提交申请" CssClass="submit-btn"  OnClick="btnSubmit_Click" />
            </div>
        </div>
    </div>

    <style>
        /* 全局样式 */
        body {
            font-family: 'Arial', sans-serif;
            background-color: #f5f7fa;
            margin: 0;
            padding: 0;
        }

        .refund-container {
            max-width: 900px;
            margin: 40px auto;
            padding: 40px;
            background-color: #ffffff;
            border-radius: 15px;
            box-shadow: 0px 8px 15px rgba(0, 0, 0, 0.2);
            animation: fade-in 0.8s ease-out;
        }

        .page-title {
            font-size: 32px;
            color: #2c3e50;
            text-align: center;
            margin-bottom: 10px;
        }

        .page-description {
            font-size: 18px;
            color: #7f8c8d;
            text-align: center;
            margin-bottom: 30px;
        }

        .form-group {
            margin-bottom: 30px;
            opacity: 0;
            transform: translateY(30px);
            animation: slide-in 0.8s ease-out forwards;
        }

        .form-group:nth-child(2) {
            animation-delay: 0.3s;
        }

        .form-group:nth-child(3) {
            animation-delay: 0.6s;
        }

        .form-control {
            width: 100%;
            padding: 12px;
            border: 1px solid #ddd;
            border-radius: 5px;
            font-size: 16px;
            transition: border-color 0.3s ease-in-out, box-shadow 0.3s ease-in-out;
        }

        .form-control:focus {
            border-color: #1abc9c;
            box-shadow: 0 0 10px rgba(26, 188, 156, 0.2);
            outline: none;
        }

        .form-actions {
            text-align: center;
            margin-top: 20px;
            animation: zoom-in 0.8s ease-out forwards;
        }

        .submit-btn {
            padding: 12px 25px;
            background-color: #1abc9c;
            color: white;
            border: none;
            border-radius: 5px;
            font-size: 18px;
            cursor: pointer;
            transition: background-color 0.3s ease-in-out, transform 0.2s ease-in-out;
        }

        .submit-btn:hover {
            background-color: #16a085;
            transform: scale(1.05);
        }

        .order-info {
            background-color: #f9f9f9;
            padding: 15px;
            border: 1px solid #ddd;
            border-radius: 5px;
            animation: fade-in 0.8s ease-out;
        }

        .order-info p {
            margin: 10px 0;
            font-size: 16px;
        }

        .order-info span {
            color: #2c3e50;
            font-weight: bold;
        }

        /* 动画 */
        @keyframes fade-in {
            from {
                opacity: 0;
                transform: translateY(20px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        @keyframes slide-in {
            from {
                opacity: 0;
                transform: translateX(-30px);
            }
            to {
                opacity: 1;
                transform: translateX(0);
            }
        }

        @keyframes zoom-in {
            from {
                opacity: 0;
                transform: scale(0.9);
            }
            to {
                opacity: 1;
                transform: scale(1);
            }
        }
    </style>
</asp:Content>



