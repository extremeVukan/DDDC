<%@ Page Title="" Language="C#" MasterPageFile="~/SelifInfo_Web/Self_info.master" AutoEventWireup="true" CodeFile="ShipReg.aspx.cs" Inherits="SelifInfo_Web_ShipReg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <br />
    <asp:Image ID="Image2" runat="server" Height="96px" Width="136px" style="border-radius: 15px; border: 2px solid #ccc;" />

    <br />
&nbsp;<h2 class="username">
        <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label>
    </h2>
            <p class="email">
                <asp:Label ID="lblemail" runat="server" Text="Label"></asp:Label>
    </p>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <div class="ship-registration-wrapper">
        <h2 class="section-title">船只注册</h2>
        <div class="form-container">
            <table class="form-table">
                <!-- 船只名称 -->
                <tr>
                    <td class="form-label-cell">
                        <asp:Label ID="lblShipName" runat="server" Text="船只名称：" AssociatedControlID="txtShipName" CssClass="form-label"></asp:Label>
                    </td>
                    <td class="form-input-cell">
                        <asp:TextBox ID="txtShipName" runat="server" CssClass="form-input" placeholder="请输入船只名称"></asp:TextBox>
                    </td>
                </tr>

                <!-- 船只编号 -->
                <tr>
                    <td class="form-label-cell">
                        <asp:Label ID="lblShipID" runat="server" Text="船只编号：" AssociatedControlID="txtShipID" CssClass="form-label"></asp:Label>
                    </td>
                    <td class="form-input-cell">
                        <asp:TextBox ID="txtShipID" runat="server" CssClass="form-input" placeholder="请输入船只编号"></asp:TextBox>
                    </td>
                </tr>

                <!-- 船只类型 -->
                <tr>
                    <td class="form-label-cell">
                        <asp:Label ID="lblShipType" runat="server" Text="船只类型：" AssociatedControlID="txtShipType" CssClass="form-label"></asp:Label>
                    </td>
                    <td class="form-input-cell">
                        <asp:TextBox ID="txtShipType" runat="server" CssClass="form-input" placeholder="请输入船只类型"></asp:TextBox>
                    </td>
                </tr>

                <!-- 船只容量 -->
                <tr>
                    <td class="form-label-cell">
                        <asp:Label ID="lblShipCapacity" runat="server" Text="船只容量：" AssociatedControlID="txtShipCapacity" CssClass="form-label"></asp:Label>
                    </td>
                    <td class="form-input-cell">
                        <asp:TextBox ID="txtShipCapacity" runat="server" CssClass="form-input" placeholder="请输入船只容量"></asp:TextBox>
                    </td>
                </tr>

                <!-- 船只照片 -->
                <tr>
                    <td class="form-label-cell">
                        <asp:Label ID="lblShipPhoto" runat="server" Text="船只照片：" AssociatedControlID="fuShipPhoto" CssClass="form-label"></asp:Label>
                    </td>
                    <td class="form-input-cell">
                        <asp:FileUpload ID="fuShipPhoto" runat="server" CssClass="file-upload" />
                        <asp:Button ID="btnimg" runat="server" Text="上传" CssClass="btn-upload" OnClick="Unnamed1_Click" />
                    </td>
                </tr>

                <!-- 照片预览 -->
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Image ID="Image3" runat="server" CssClass="photo-preview" />
                    </td>
                </tr>

                <!-- 提交按钮 -->
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Button ID="btnSubmit1" runat="server" Text="提交注册" CssClass="btn-submit" OnClick="btnSubmit_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <style>
        /* 全局样式 */
        body {
            font-family: 'Arial', sans-serif;
            background-color: #f5f5f5;
            margin: 0;
            padding: 0;
        }

        /* 头部用户信息 */
        .profile-header {
            text-align: center;
            margin-bottom: 30px;
            
        }

       
        

        .username {
            font-size: 24px;
            font-weight: bold;
            color:white;
        }

        .email {
            font-size: 14px;
            color: white;
            margin-top: 5px;
        }

        /* 船只注册外层容器 */
        .ship-registration-wrapper {
            max-width: 800px;
            margin: 0 auto;
            background-color: #fff;
            padding: 30px;
            border-radius: 15px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

        .section-title {
            text-align: center;
            font-size: 28px;
            margin-bottom: 20px;
            color: #34495e;
        }

        /* 表格样式 */
        .form-container {
            width: 100%;
        }

        .form-table {
            width: 100%;
            border-collapse: collapse;
        }

        .form-label-cell {
            text-align: right;
            padding: 10px 15px;
            font-weight: bold;
            color: #34495e;
            width: 30%;
        }

        .form-input-cell {
            text-align: left;
            padding: 10px 15px;
        }

        .form-input {
            width: 90%;
            padding: 10px;
            font-size: 14px;
            border: 1px solid #ddd;
            border-radius: 5px;
            transition: border-color 0.3s;
        }

        .form-input:focus {
            border-color: #3498db;
            outline: none;
        }

        .file-upload {
            margin-top: 10px;
        }

        .photo-preview {
            width: 200px;
            height: 100px;
            border-radius: 10px;
            border: 2px solid #ddd;
            object-fit: cover;
        }

        /* 按钮样式 */
        .btn-upload, .btn-submit {
            padding: 10px 20px;
            background-color: #3498db;
            color: white;
            border: none;
            border-radius: 5px;
            font-size: 14px;
            cursor: pointer;
            transition: background-color 0.3s;
        }

        .btn-upload:hover, .btn-submit:hover {
            background-color: #2980b9;
        }

        /* 响应式支持 */
        @media screen and (max-width: 768px) {
            .form-label-cell, .form-input-cell {
                display: block;
                width: 100%;
                text-align: center;
            }

            .form-input {
                width: 100%;
            }

            .photo-preview {
                width: 150px;
                height: 75px;
            }
        }
    </style>
</asp:Content>

