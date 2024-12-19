<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FindPwd.aspx.cs" Inherits="FindPwd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
        <table style="width: 98%; margin-top:27%; height: 146px; margin-left: 24px;">
            <tr>
                <td colspan="2" class="tdleft">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Large" Text="找回密码"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 245px; height: 37px;" class="tdleft">用户名:</td>
                <td class="tdleft" style="height: 37px">
                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName" ErrorMessage="RequiredFieldValidator" ForeColor="Red">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="width: 245px" class="tdleft">邮箱:</td>
                <td class="tdleft">
                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEmail" ErrorMessage="RequiredFieldValidator" ForeColor="Red">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail" ErrorMessage="RegularExpressionValidator" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td style="width: 245px">
                    <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Text="找回密码需验证邮箱！"></asp:Label>
                </td>
                <td>
                    <asp:Button ID="txtFindpwd" runat="server" Text="找回密码" OnClick="Button1_Click"/>
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

