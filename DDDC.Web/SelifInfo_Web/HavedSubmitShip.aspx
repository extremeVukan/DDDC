<%@ Page Title="" Language="C#" MasterPageFile="~/SelifInfo_Web/Self_info.master" AutoEventWireup="true" CodeFile="HavedSubmitShip.aspx.cs" Inherits="SelifInfo_Web_HavedSubmitShip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    <asp:Image ID="Image2" runat="server" Height="96px" Width="136px" style="border-radius: 15px; border: 2px solid #ccc;" />
    <br />
    <h2 class="username" style="margin-top: 10px;">
        <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label>
    </h2>
    <p class="email">
        <asp:Label ID="lblemail" runat="server" Text="Label"></asp:Label>
    </p>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <div class="status-wrapper">
        <div class="circle-container">
            <div class="checkmark">
                &#10003;
            </div>
        </div>
        <div class="status-text">
            船只信息已提交,等待审核中
        </div>
    </div>

    <style>
        
        /* 外层容器居中对齐 */
        .status-wrapper {
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            height: 100%;
            margin-top: 50px;
            
        }

        /* 圈的样式 */
        .circle-container {
            width: 150px;
            height: 150px;
            border-radius: 50%;
            border: 5px solid #1abc9c;
            display: flex;
            justify-content: center;
            align-items: center;
            background-color: #f9f9f9;
            box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.2);
            
        }

        /* 勾号的样式 */
        .checkmark {
            font-size: 72px;
            color: #1abc9c;
            font-weight: bold;
        }

        /* 文本样式 */
        .status-text {
            margin-top: 20px;
            font-size: 20px;
            font-weight: bold;
            color: white;
            text-align: center;
           
        }
    </style>
</asp:Content>

