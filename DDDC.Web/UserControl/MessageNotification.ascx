<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MessageNotification.ascx.cs" Inherits="UserControl_MessageNotification" %>
<span id="notificationBadge" runat="server" class="notification-badge" style="display: none;">0</span>

<style>
    .notification-badge {
        position: absolute;
        top: -5px;
        right: -10px;
        background-color: #e74c3c;
        color: white;
        font-size: 12px;
        font-weight: bold;
        width: 20px;
        height: 20px;
        text-align: center;
        line-height: 20px;
        border-radius: 50%;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
    }
</style>
