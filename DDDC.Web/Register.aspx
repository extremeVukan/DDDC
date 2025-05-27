<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="register-wrapper">
        <div class="register-header">
            <h2><asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="#1abc9c" Text="欢迎注册"></asp:Label></h2>
            <p class="register-subtitle">创建您的滴滴打船账户，体验便捷服务</p>
        </div>
        
        <div class="register-form">
            <div class="form-row">
                <label for="txtName">用户名:</label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form-input" placeholder="请输入用户名"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txtName" ErrorMessage="*" 
                    ForeColor="#FF3300" CssClass="validator"></asp:RequiredFieldValidator>
            </div>
            
            <div class="form-row">
                <label for="txtpwd">密码:</label>
                <asp:TextBox ID="txtpwd" runat="server" TextMode="Password" CssClass="form-input" placeholder="请输入密码"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="txtpwd" ErrorMessage="*" 
                    ForeColor="#FF3300" CssClass="validator"></asp:RequiredFieldValidator>
            </div>
            
            <div class="form-row">
                <label for="TextBox3">确认密码:</label>
                <asp:TextBox ID="TextBox3" runat="server" TextMode="Password" CssClass="form-input" placeholder="请再次输入密码"></asp:TextBox>
                <div class="validators">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                        ControlToValidate="TextBox3" ErrorMessage="*" 
                        ForeColor="#FF3300" CssClass="validator"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" 
                        ControlToCompare="txtpwd" ControlToValidate="TextBox3" 
                        ErrorMessage="两次密码不一致" ForeColor="#FF3300" CssClass="validator-text"></asp:CompareValidator>
                </div>
            </div>
            
            <div class="form-row">
                <label for="txtemail">邮箱:</label>
                <asp:TextBox ID="txtemail" runat="server" CssClass="form-input" placeholder="请输入有效的电子邮箱"></asp:TextBox>
                <div class="validators">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                        ControlToValidate="txtemail" ErrorMessage="*" 
                        ForeColor="#FF3300" CssClass="validator"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                        ControlToValidate="txtemail" ErrorMessage="邮箱格式不正确" 
                        ForeColor="#FF3300" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                        CssClass="validator-text"></asp:RegularExpressionValidator>
                </div>
            </div>
            
            <div class="form-row message-row">
                <asp:Label ID="lblmsg" runat="server" CssClass="message-text"></asp:Label>
            </div>
            
            <div class="button-row">
                <asp:Button ID="btnOK" runat="server" Text="注册" CssClass="action-btn submit-btn" OnClick="btnOK_Click" />
                <asp:Button ID="btncancel" runat="server" Text="重置" CssClass="action-btn reset-btn" OnClick="btncancel_Click" CausesValidation="false" />
            </div>
            
            <div class="links-row">
                <a href="login.aspx" class="action-link">已有账户？立即登录</a>
            </div>
        </div>
    </div>
    
    <style>
        /* 注册页面样式 - 适配母版页风格 */
        .register-wrapper {
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            padding: 20px;
            height: 100%;
            width: 100%;
            box-sizing: border-box;
        }
        
        .register-header {
            text-align: center;
            margin-bottom: 20px;
        }
        
        .register-subtitle {
            color: #ffffff;
            opacity: 0.8;
            margin-top: 10px;
            font-size: 16px;
        }
        
        .register-form {
            width: 90%;
        }
        
        .form-row {
            margin-bottom: 15px;
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
        }
        
        .message-text {
            color: #1abc9c;
            font-weight: 500;
        }
        
        .button-row {
            display: flex;
            justify-content: space-between;
            margin: 10px 0 15px;
        }
        
        .action-btn {
            padding: 10px 30px;
            border-radius: 25px;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
            transition: all 0.3s;
            border: none;
            flex: 0 0 calc(50% - 10px);
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }
        
        .submit-btn {
            background-color: #1abc9c;
            color: white;
        }
        
        .submit-btn:hover {
            background-color: #16a085;
        }
        
        .reset-btn {
            background-color: rgba(255, 255, 255, 0.2);
            color: white;
        }
        
        .reset-btn:hover {
            background-color: rgba(255, 255, 255, 0.3);
        }
        
        .links-row {
            display: flex;
            justify-content: center;
            margin-top: 15px;
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
            .register-form {
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
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder4" Runat="Server">
</asp:Content>
