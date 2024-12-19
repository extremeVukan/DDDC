<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ChangePwd.aspx.cs" Inherits="ChangePwd" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
        <table class="tdleft" style="width: 93%; margin-top:25%; height: 177px; margin-left: 22px;border-radius:20px">
            <tr>
                <td colspan="2" style="height: 64px">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="White" Text="修改密码"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 245px; height: 39px;">原密码：</td>
                <td style="height: 39px">
                    <asp:TextBox ID="txtoldpwd" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtoldpwd" ErrorMessage="RequiredFieldValidator" ForeColor="Red">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="width: 245px; height: 39px;">新密码</td>
                <td style="height: 39px">
                    <asp:TextBox ID="txtpwd" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtpwd" ErrorMessage="RequiredFieldValidator" ForeColor="Red">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txtoldpwd" ControlToValidate="txtpwd" ErrorMessage="CompareValidator" ForeColor="Red" Operator="NotEqual">*</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td style="width: 245px; height: 39px">确认新密码</td>
                <td style="height: 39px">
                    <asp:TextBox ID="TextBox3" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextBox3" ErrorMessage="RequiredFieldValidator" ForeColor="Red">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtpwd" ControlToValidate="TextBox3" ErrorMessage="CompareValidator" ForeColor="Red">*</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td style="width: 245px; height: 39px">
                    <asp:Label ID="lblmsg" runat="server"></asp:Label>
                </td>
                <td style="height: 39px">
                    <asp:Button ID="btnOK" runat="server" Text="提交" Width="94px" OnClick="Button1_Click" />
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


