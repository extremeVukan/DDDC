<%@ Page Title="审核船只注册" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="AllowShip.aspx.cs" Inherits="Admin_AllowShip"  ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page-container">
        <h1 class="page-title">船只注册审核</h1>
        <p class="page-description">请审核以下船只注册申请，并选择同意或拒绝。</p>
        
        <div class="grid-container">
            <asp:GridView runat="server" DataSourceID="LinqDataSource1" ID="ctl02" AutoGenerateColumns="False" CssClass="styled-grid" GridLines="None" AllowPaging="True" PageSize="10" OnPageIndexChanging="ctl02_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="ship_id" HeaderText="船只编号" ReadOnly="True" SortExpression="ship_id" />
                    <asp:BoundField DataField="owner_id" HeaderText="所有者编号" ReadOnly="True" SortExpression="owner_id" />
                    <asp:BoundField DataField="ship_name" HeaderText="船只名称" ReadOnly="True" SortExpression="ship_name" />
                    <asp:BoundField DataField="ship_type" HeaderText="船只类型" ReadOnly="True" SortExpression="ship_type" />
                    <asp:BoundField DataField="capacity" HeaderText="荷载" ReadOnly="True" SortExpression="capacity" />
                    <asp:BoundField DataField="ship_status" HeaderText="状态" ReadOnly="True" SortExpression="ship_status" />
                    <asp:ImageField DataImageUrlField="Picture" HeaderText="图片" ControlStyle-Width="120px" ControlStyle-Height="80px" />
                    <asp:BoundField DataField="IsAllowed" HeaderText="审核状态" ReadOnly="True" SortExpression="IsAllowed" />
                    <asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <asp:Button ID="btnApprove" runat="server" Text="同意" CssClass="btn-approve"
                                        CommandName="Approve" OnClick="btnApprove_Click" CommandArgument='<%# Eval("ship_id") %>' />
                            <asp:Button ID="btnReject" runat="server" Text="拒绝" CssClass="btn-reject"
                                        CommandName="Reject" OnClick="btnReject_Click" CommandArgument='<%# Eval("ship_id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <asp:LinqDataSource runat="server" EntityTypeName="" ID="LinqDataSource1"
        ContextTypeName="DDDC.DAL.DataClasses1DataContext"
        TableName="ShipHandle" Where="IsAllowed == @IsAllowed"
        Select="new (ship_id, owner_id, ship_name, ship_type, capacity, ship_status, Picture, IsAllowed, Admin_name)">
        <WhereParameters>
            <asp:Parameter DefaultValue="待审核" Name="IsAllowed" Type="String" />
        </WhereParameters>
    </asp:LinqDataSource>

    <style>
        .page-container {
            width: 100%;
            margin: 0 auto;
            padding: 10px;
            background: #ffffff;
            border-radius: 10px;
            box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
        }

        .page-title {
            font-size: 28px;
            color: #34495e;
            text-align: center;
            margin-bottom: 10px;
        }

        .page-description {
            text-align: center;
            color: #7f8c8d;
            margin-bottom: 20px;
        }

        .grid-container {
            margin-top: 20px;
        }

        .styled-grid {
            width: 100%;
            border-collapse: collapse;
            margin-top: 10px;
        }

        .styled-grid th {
            background-color: #34495e;
            color: white;
            font-weight: bold;
            text-align: center;
            padding: 10px;
        }

        .styled-grid td {
            padding: 10px;
            text-align: center;
            border: 1px solid #ddd;
        }

        .styled-grid tr:nth-child(even) {
            background-color: #f9f9f9;
        }

        .styled-grid tr:hover {
            background-color: #f1f1f1;
            cursor: pointer;
        }

        .btn-approve {
            background-color: #2ecc71;
            color: white;
            border: none;
            padding: 5px 10px;
            border-radius: 5px;
            cursor: pointer;
            transition: background-color 0.3s;
        }

        .btn-approve:hover {
            background-color: #27ae60;
        }

        .btn-reject {
            background-color: #e74c3c;
            color: white;
            border: none;
            padding: 5px 10px;
            border-radius: 5px;
            cursor: pointer;
            transition: background-color 0.3s;
        }

        .btn-reject:hover {
            background-color: #c0392b;
        }
    </style>
</asp:Content>
