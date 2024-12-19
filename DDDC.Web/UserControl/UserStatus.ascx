<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserStatus.ascx.cs" Inherits="UserControl_UserStatus" %>


<asp:Label ID="lblWelcome" runat="server" Text="您还未登录！" Font-Size="Large" Font-Bold="true" ></asp:Label>   &nbsp  &nbsp 
<asp:LinkButton ID="lnkbtnPwd" runat="server" ForeColor="White" Visible="False" PostBackUrl="~/ChangePwd.aspx" Font-Bold="true">密码修改</asp:LinkButton> &nbsp &nbsp 
<!--<asp:LinkButton ID="lnkbtnManage" runat="server" ForeColor="White" Visible="False" PostBackUrl="~/Admin/Default.aspx">系统管理</asp:LinkButton>&nbsp -->
<asp:LinkButton ID="lnkbtnOrder" runat="server" ForeColor="White" Visible="False" PostBackUrl="http://localhost:51058/ClientOrder/COrder.aspx" OnClick="lnkbtnOrder_Click" Font-Bold="true">订单记录</asp:LinkButton>&nbsp  &nbsp 
<asp:LinkButton ID="lnkbtnLogout" runat="server" ForeColor="White" Visible="False" OnClick="lnkbtnLogout_Click1" Font-Bold="true">退出登录</asp:LinkButton>&nbsp &nbsp 

