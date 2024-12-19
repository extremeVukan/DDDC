<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<%@ Register Src="~/UserControl/UserStatus.ascx" TagPrefix="uc1" TagName="UserStatus" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
        <table style="width: 83%; margin-top:25%;text-align:center; height: 239px; margin-left: 48px;">
            <tr>
                <td colspan="2" class="tdleft" style="height: 68px">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="#FF3300" Text="欢迎登录！"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tdleft" style="height: 40px">用户名:</td>
                <td class="tdleft" style="height: 40px">
                    <asp:TextBox ID="txtname" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtname" ErrorMessage="RequiredFieldValidator" ForeColor="#FF3300">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="tdleft" style="height: 51px">密码:</td>
                <td class="tdleft" style="height: 51px">
                    <asp:TextBox ID="txtpwd" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtpwd" ErrorMessage="RequiredFieldValidator" ForeColor="#FF3300">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="tdleft">
                    <asp:Button ID="btnOK" runat="server" Text="登录" Width="82px" OnClick="Button1_Click" />
                </td>
                <td>
                    <asp:Label ID="lblmsg" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 29px" class="tdleft">
                   <button><a href="Register.aspx" style="color:black" >我要注册!</a></button>
                </td>
                <td style="height: 29px" class="tdleft">
                    <button><a href="FindPwd.aspx" style="color:black">忘记密码?</a></button>

                </td>
            </tr>
        </table>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder4" Runat="Server">
</asp:Content>

