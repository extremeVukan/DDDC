<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FindPwd.aspx.cs" Inherits="FindPwd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="findpwd-wrapper">
        <div class="findpwd-header">
            <h2><asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="#1abc9c" Text="找回密码"></asp:Label></h2>
            <p class="findpwd-subtitle">请输入您的用户名和注册邮箱，我们会发送密码重置链接</p>
        </div>
        
        <div class="findpwd-form">
            <div class="form-row">
                <label for="txtName">用户名:</label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form-input" placeholder="请输入您的用户名"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txtName" ErrorMessage="*" 
                    ForeColor="#FF3300" CssClass="validator"></asp:RequiredFieldValidator>
            </div>
            
            <div class="form-row">
                <label for="txtEmail">邮箱:</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-input" placeholder="请输入您的注册邮箱"></asp:TextBox>
                <div class="validators">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ControlToValidate="txtEmail" ErrorMessage="*" 
                        ForeColor="#FF3300" CssClass="validator"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                        ControlToValidate="txtEmail" ErrorMessage="邮箱格式不正确" 
                        ForeColor="#FF3300" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                        CssClass="validator-text"></asp:RegularExpressionValidator>
                </div>
            </div>
            
            <div class="form-row message-row">
                <asp:Label ID="lblMsg" runat="server" CssClass="info-message">
                    <i class="fas fa-info-circle"></i> 找回密码需验证邮箱！
                </asp:Label>
            </div>
            
            <div class="button-row">
                <asp:Button ID="txtFindpwd" runat="server" Text="找回密码" 
                            CssClass="submit-btn" OnClick="Button1_Click" />
            </div>
            
            <div class="links-row">
                <a href="login.aspx" class="action-link">
                    <i class="fas fa-arrow-left"></i> 返回登录
                </a>
                <a href="Register.aspx" class="action-link">
                    注册新账户 <i class="fas fa-arrow-right"></i>
                </a>
            </div>
        </div>
    </div>
    
    <style>
        /* 找回密码页面样式 - 适配母版页风格 */
        .findpwd-wrapper {
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            padding: 20px;
            height: 100%;
            width: 100%;
            box-sizing: border-box;
        }
        
        .findpwd-header {
            text-align: center;
            margin-bottom: 30px;
        }
        
        .findpwd-subtitle {
            color: #ffffff;
            opacity: 0.8;
            margin-top: 10px;
            font-size: 16px;
        }
        
        .findpwd-form {
            width: 85%;
        }
        
        .form-row {
            margin-bottom: 20px;
            display: flex;
            align-items: center;
            flex-wrap: wrap;
        }
        
        .form-row label {
            width: 80px;
            color: white;
            font-size: 16px;
            text-align: right;
            padding-right: 15px;
        }
        
        .form-input {
            flex: 1;
            background-color: rgba(255, 255, 255, 0.2);
            border: 1px solid rgba(255, 255, 255, 0.4);
            color: white;
            padding: 10px 15px;
            border-radius: 25px;
            font-size: 16px;
            transition: all 0.3s;
        }
        
        .form-input:focus {
            background-color: rgba(255, 255, 255, 0.3);
            border-color: #1abc9c;
            outline: none;
            box-shadow: 0 0 0 2px rgba(26, 188, 156, 0.3);
        }
        
        .form-input::placeholder {
            color: rgba(255, 255, 255, 0.6);
        }
        
        .validator {
            margin-left: 5px;
            font-weight: bold;
        }
        
        .validators {
            display: flex;
            align-items: center;
        }
        
        .validator-text {
            margin-left: 5px;
            font-size: 14px;
        }
        
        .message-row {
            justify-content: center;
            min-height: 24px;
            margin: 15px 0;
        }
        
        .info-message {
            color: #f39c12;
            background-color: rgba(243, 156, 18, 0.1);
            padding: 10px 15px;
            border-radius: 5px;
            display: flex;
            align-items: center;
            font-size: 14px;
        }
        
        .info-message i {
            margin-right: 8px;
            font-size: 16px;
        }
        
        .button-row {
            text-align: center;
            margin: 20px 0;
        }
        
        .submit-btn {
            background-color: #1abc9c;
            color: white;
            border: none;
            padding: 12px 40px;
            border-radius: 25px;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
            transition: all 0.3s;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }
        
        .submit-btn:hover {
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
            display: flex;
            align-items: center;
        }
        
        .action-link i {
            margin: 0 5px;
        }
        
        .action-link:hover {
            background-color: rgba(26, 188, 156, 0.3);
            color: white;
        }
        
        /* 响应式调整 */
        @media (max-width: 480px) {
            .findpwd-form {
                width: 95%;
            }
            
            .form-row {
                flex-direction: column;
                align-items: flex-start;
            }
            
            .form-row label {
                width: 100%;
                text-align: left;
                margin-bottom: 5px;
                padding-right: 0;
            }
            
            .form-input {
                width: 100%;
            }
            
            .validators {
                margin-top: 5px;
            }
            
            .links-row {
                flex-direction: column;
                gap: 10px;
                align-items: center;
            }
        }
    </style>
    
    <!-- 添加Font Awesome图标库 -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder4" Runat="Server">
</asp:Content>
