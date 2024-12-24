<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="123123.aspx.cs" Inherits="Admin_123123" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="file-read-container">
        <h1 class="page-title">读取文件内容</h1>

        <!-- 提示信息 -->
        <asp:Label ID="lblMessage" runat="server" CssClass="message-label"></asp:Label>

        <!-- 输入文件路径 -->
        <div class="file-path-section">
            <asp:TextBox ID="txtFilePath" runat="server" CssClass="file-path-box" Placeholder="请输入文件路径"></asp:TextBox>
            <asp:Button ID="btnReadFile" runat="server" Text="读取文件" CssClass="read-btn" OnClick="btnReadFile_Click" />
        </div>

        <!-- 数据展示 -->
        <div class="data-display">
            <asp:Repeater ID="RepeaterFileContent" runat="server">
                <HeaderTemplate>
                    <ul class="file-content-list">
                </HeaderTemplate>
                <ItemTemplate>
                    <li class="file-line"><%# Container.DataItem %></li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>

    <style>
        .file-read-container {
            margin: 20px auto;
            max-width: 800px;
            padding: 20px;
            background-color: #ffffff;
            border-radius: 10px;
            box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
        }

        .page-title {
            text-align: center;
            font-size: 24px;
            color: #34495e;
            margin-bottom: 20px;
        }

        .message-label {
            display: block;
            text-align: center;
            font-size: 16px;
            color: #2ecc71;
            margin-bottom: 10px;
        }

        .file-path-section {
            text-align: center;
            margin-bottom: 20px;
        }

        .file-path-box {
            width: 80%;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 5px;
            margin-right: 10px;
        }

        .read-btn {
            background-color: #3498db;
            color: white;
            padding: 10px 20px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }

        .read-btn:hover {
            background-color: #2980b9;
        }

        .data-display {
            margin-top: 20px;
        }

        .file-content-list {
            list-style: none;
            padding: 0;
        }

        .file-line {
            background-color: #f9f9f9;
            margin: 5px 0;
            padding: 10px;
            border-radius: 5px;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        }
    </style>
</asp:Content>

