<%@ Page Title="" Language="C#" MasterPageFile="~/SelifInfo_Web/Self_info.master" AutoEventWireup="true" CodeFile="Self_Info.aspx.cs" Inherits="SelifInfo_Web_Self_Info" EnableEventValidation="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <div class="account-settings-wrapper">
        <h2 class="section-title">账户设置</h2>
        <div class="account-settings-card">
            <form>
                <table class="account-settings-table">
                    <!-- 昵称 -->
                    <tr>
                        <td class="label-cell">
                            <asp:Label ID="Label1" runat="server" Text="昵称：" CssClass="form-label"></asp:Label>
                        </td>
                        <td class="input-cell">
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-input"></asp:TextBox>
                            <asp:Button ID="BtnName" runat="server" Text="更改" CssClass="btn-update" OnClick="BtnName_Click" />
                        </td>
                    </tr>

                    <!-- 邮箱 -->
                    <tr>
                        <td class="label-cell">
                            <asp:Label ID="Label2" runat="server" Text="邮箱：" CssClass="form-label"></asp:Label>
                        </td>
                        <td class="input-cell">
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-input"></asp:TextBox>
                            <asp:Button ID="BtnEmail" runat="server" Text="更改" CssClass="btn-update" OnClick="BtnEmail_Click" />
                        </td>
                    </tr>

                    <!-- 电话 -->
                    <tr>
                        <td class="label-cell">
                            <asp:Label ID="Label3" runat="server" Text="电话：" CssClass="form-label"></asp:Label>
                        </td>
                        <td class="input-cell">
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-input"></asp:TextBox>
                            <asp:Button ID="BtnPhone" runat="server" Text="更改" CssClass="btn-update" OnClick="BtnPhone_Click" />
                        </td>
                    </tr>

                    <!-- 头像 -->
                    <tr>
                        <td class="label-cell">
                            <asp:Label ID="Label4" runat="server" Text="头像：" CssClass="form-label"></asp:Label>
                        </td>
                        <td class="input-cell">
                            <asp:Image ID="Image1" runat="server" CssClass="profile-image" />
                            <asp:FileUpload ID="FileUpload1" runat="server" CssClass="file-upload" />
                            <asp:Button ID="BtnPhoto" runat="server" Text="上传" CssClass="btn-upload" OnClick="BtnPhoto_Click" />
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lblMessage" runat="server" CssClass="message-label"></asp:Label>
            </form>
        </div>
    </div>
    <div class="wrapper2">
         <h2 class="section-title">最近订单</h2>
        <asp:Repeater ID="RepeaterOrders" runat="server">
            <ItemTemplate>
                <div class="order-card">
                    <h3>订单号: <%# Eval("OrderNumber") %></h3>
                    <p>状态: <%# Eval("Status") %></p>
                    
                </div>
            </ItemTemplate>
        </asp:Repeater>
        </div>
    <style>
        /* 外层容器 */
        .account-settings-wrapper {
            max-width: 800px;
            margin-top: 50px;
            margin-left:130px;
            background-color: #fff;
            padding: 30px;
            border-radius: 15px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            float:left;
        }
        .wrapper2{
            width: 300px;
            height:auto;
            margin: 50px 10px;
            background-color: #fff;
            padding: 30px;
            border-radius: 15px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            float:left;
            overflow:clip;
        }
          .order-card {
            background: linear-gradient(135deg, #74b9ff, #0984e3);
            border-radius: 10px;
            padding: 15px;
            margin-bottom: 15px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            text-align: left;
        }

        .order-card h3 {
            margin-bottom: 10px;
            color: black;
            font-size: 16px;
        }

        .order-card p {
            margin: 5px 0;
            color: lawngreen;
            font-size: 14px;
            font-weight:bold;
        }
        /* 标题样式 */
        .section-title {
            text-align: center;
            font-size: 24px;
            margin-bottom: 20px;
            color: #34495e;
        }

        /* 卡片布局 */
        .account-settings-card {
            padding: 20px;
            background-color: #f9f9f9;
            border-radius: 10px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }

        /* 表格样式 */
        .account-settings-table {
            width: 100%;
            border-collapse: collapse;
        }

        .label-cell {
            text-align: right;
            padding: 10px 15px;
            vertical-align: middle;
            font-weight: bold;
            color: #333;
            width: 20%;
        }

        .input-cell {
            text-align: left;
            padding: 10px 15px;
            vertical-align: middle;
        }

        /* 表单控件样式 */
        .form-label {
            font-size: 16px;
            color: #34495e;
        }

        .form-input {
            width: 80%;
            padding: 8px;
            font-size: 14px;
            border: 1px solid #ddd;
            border-radius: 5px;
            transition: border-color 0.3s;
        }

        .form-input:focus {
            border-color: #1abc9c;
            outline: none;
        }

        .file-upload {
            margin-top: 10px;
        }

        .profile-image {
            width: 100px;
            height: 100px;
            border-radius: 50%;
            margin-right: 10px;
            border: 2px solid #ddd;
            object-fit: cover;
        }

        /* 按钮样式 */
        .btn-update, .btn-upload {
            padding: 10px 20px;
            background-color: #1abc9c;
            color: white;
            border: none;
            border-radius: 5px;
            font-size: 14px;
            cursor: pointer;
            margin-left: 10px;
            transition: background-color 0.3s, transform 0.3s;
        }

        .btn-update:hover, .btn-upload:hover {
            background-color: #16a085;
            transform: translateY(-2px);
        }

        /* 消息提示 */
        .message-label {
            display: block;
            margin-top: 10px;
            font-size: 14px;
            color: red;
        }

        /* 响应式 */
        @media screen and (max-width: 768px) {
            .form-input {
                width: 100%;
            }

            .btn-update, .btn-upload {
                margin-left: 0;
                margin-top: 10px;
            }

            .profile-image {
                width: 80px;
                height: 80px;
            }
        }
    </style>
</asp:Content>



<asp:Content ID="Content1" runat="server" contentplaceholderid="ContentPlaceHolder1">
    <br />
    <asp:Image ID="Image2" runat="server" Height="96px" Width="136px" style="border-radius: 15px; border: 2px solid #ccc;" />

    <br />
&nbsp;<h2 class="username">
        <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label>
    </h2>
            <p class="email">
                <asp:Label ID="lblemail" runat="server" Text="Label"></asp:Label>
    </p>
</asp:Content>



