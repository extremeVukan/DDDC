<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<%@ Register Src="~/UserControl/UserStatus.ascx" TagPrefix="uc1" TagName="UserStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="login-wrapper">
        <div class="login-header">
            <h2><asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="#1abc9c" Text="欢迎登录"></asp:Label></h2>
            <p class="login-subtitle">登录您的账户以继续使用滴滴打船服务</p>
        </div>
        
        <div class="login-form">
            <div class="form-row">
                <label for="txtname">用户名:</label>
                <asp:TextBox ID="txtname" runat="server" CssClass="login-input" placeholder="请输入用户名"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txtname" ErrorMessage="*" 
                    ForeColor="#FF3300" CssClass="validator"></asp:RequiredFieldValidator>
            </div>
            
            <div class="form-row">
                <label for="txtpwd">密&#12288;码:</label>
                <asp:TextBox ID="txtpwd" runat="server" TextMode="Password" CssClass="login-input" placeholder="请输入密码"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="txtpwd" ErrorMessage="*" 
                    ForeColor="#FF3300" CssClass="validator"></asp:RequiredFieldValidator>
            </div>
            
            <div class="form-row message-row">
                <asp:Label ID="lblmsg" runat="server" CssClass="message-text"></asp:Label>
            </div>
            
            <div class="button-row">
                <asp:Button ID="btnOK" runat="server" Text="登录" CssClass="login-btn" OnClick="Button1_Click" />
            </div>
            
            <div class="links-row">
                <a href="Register.aspx" class="action-link">注册新账户</a>
                <a href="FindPwd.aspx" class="action-link">忘记密码?</a>
            </div>
        </div>
    </div>
    
    <style>
        /* 登录页面样式 - 适配母版页风格 */
        .login-wrapper {
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            padding: 20px;
            height: 100%;
            width: 100%;
            box-sizing: border-box;
        }
        
        .login-header {
            text-align: center;
            margin-bottom: 30px;
        }
        
        .login-subtitle {
            color: #ffffff;
            opacity: 0.8;
            margin-top: 10px;
            font-size: 16px;
        }
        
        .login-form {
            width: 80%;
        }
        
        .form-row {
            margin-bottom: 20px;
            display: flex;
            align-items: center;
        }
        
        .form-row label {
            width: 80px;
            color: white;
            font-size: 16px;
            text-align: right;
            padding-right: 15px;
        }
        
        .login-input {
            flex: 1;
            background-color: rgba(255, 255, 255, 0.2);
            border: 1px solid rgba(255, 255, 255, 0.4);
            color: white;
            padding: 10px 15px;
            border-radius: 25px;
            font-size: 16px;
            transition: all 0.3s;
        }
        
        .login-input:focus {
            background-color: rgba(255, 255, 255, 0.3);
            border-color: #1abc9c;
            outline: none;
            box-shadow: 0 0 0 2px rgba(26, 188, 156, 0.3);
        }
        
        .login-input::placeholder {
            color: rgba(255, 255, 255, 0.6);
        }
        
        .validator {
            margin-left: 5px;
            font-weight: bold;
        }
        
        .message-row {
            justify-content: center;
            min-height: 24px;
        }
        
        .message-text {
            color: #1abc9c;
            font-weight: 500;
        }
        
        .button-row {
            text-align: center;
            margin: 10px 0 20px;
        }
        
        .login-btn {
            background-color: #1abc9c;
            color: white;
            border: none;
            padding: 10px 40px;
            border-radius: 25px;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
            transition: background-color 0.3s;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }
        
        .login-btn:hover {
            background-color: #16a085;
        }
        
        .links-row {
            display: flex;
            justify-content: space-between;
            margin-top: 20px;
        }
        
        .action-link {
            color: #3498db;
            text-decoration: none;
            padding: 8px 15px;
            background-color: rgba(255, 255, 255, 0.1);
            border-radius: 20px;
            font-size: 14px;
            transition: all 0.3s;
        }
        
        .action-link:hover {
            background-color: rgba(26, 188, 156, 0.3);
            color: white;
        }
        
        /* 响应式调整 */
        @media (max-width: 480px) {
            .login-form {
                width: 90%;
            }
            
            .form-row {
                flex-direction: column;
                align-items: flex-start;
            }
            
            .form-row label {
                width: 100%;
                text-align: left;
                margin-bottom: 5px;
            }
            
            .login-input {
                width: 100%;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder4" Runat="Server">
</asp:Content>
