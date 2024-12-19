<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <table style="width: 100%;margin-top:25%; height: 228px; margin-left: 34px;">
        <tr>
            <td colspan="2" style="height: 28px" class="tdleft">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Italic="False" Font-Size="X-Large" ForeColor="#FF3300" Text="欢迎新用户注册！"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="tdleft" style="height: 20px; width: 248px;">用户名:</td>
            <td class="tdleft" style="height: 19px; width: 297px;">
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName" ErrorMessage="RequiredFieldValidator" ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="tdleft" style="width: 248px">密码:</td>
            <td class="tdleft" style="width: 297px">
                <asp:TextBox ID="txtpwd" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtpwd" ErrorMessage="RequiredFieldValidator" ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="tdleft" style="height: 25px; width: 248px;">确认密码:</td>
            <td class="tdleft" style="height: 25px; width: 297px;">
                <asp:TextBox ID="TextBox3" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextBox3" ErrorMessage="RequiredFieldValidator" ForeColor="Red">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtpwd" ControlToValidate="TextBox3" ErrorMessage="CompareValidator" ForeColor="Red">*</asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td class="tdleft" style="width: 248px">邮箱:</td>
            <td class="tdleft" style="width: 297px">
                <asp:TextBox ID="txtemail" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtemail" ErrorMessage="RequiredFieldValidator" ForeColor="Red">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtemail" ErrorMessage="RegularExpressionValidator" ForeColor="#FF3300" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="tdleft" style="width: 248px">
                <asp:Button ID="btnOK" runat="server" Text="提交" OnClick="btnOK_Click" />
            </td>
            <td class="tdleft" style="width: 297px">
                <asp:Button ID="btncancel" runat="server" Text="重置" OnClick="btncancel_Click" />
            </td>
        </tr>
        <tr>
            <td class="tdleft" colspan="2">
                <button><a href="login.aspx" style="color:black">我要登录</a></button>
                <asp:Label ID="lblmsg" runat="server"></asp:Label>
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

