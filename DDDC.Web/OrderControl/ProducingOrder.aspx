<%@ Page Title="" Language="C#" MasterPageFile="~/OrderControl/DriverMasterPage.master" AutoEventWireup="true" CodeFile="ProducingOrder.aspx.cs" Inherits="OrderControl_FinishedOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    <asp:Image ID="Image2" runat="server" Height="96px" Width="136px" style="border-radius: 15px; border: 2px solid #ccc;" />
    <br />

    <h2 class="username" style="margin-top:10px;">
        <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label>
    </h2>
    <p class="email">
        <asp:Label ID="lblemail" runat="server" Text="Label"></asp:Label>
    </p>
    <h2 class="username" style="margin-top:10px; background:#1abc9c; border-radius:20px">
        <p>船只：</p>
        <asp:Label ID="lblShipName" runat="server" Text="Label"></asp:Label>
    </h2>

    <h2 class="username" style="margin-top:10px; background:#1abc9c; border-radius:20px">
        <p>状态：</p>
        <asp:Label ID="lblShipStatus" runat="server" Text="Label"></asp:Label>
    </h2>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <div class="content-wrapper">
        <h1 class="page-title">正在进行的订单</h1>
        <p class="page-description">可查看订单详情</p>

        <div class="order-table">
            <asp:GridView 
                runat="server" 
                ID="ctl02" 
                AutoGenerateColumns="False" 
                CssClass="styled-grid" 
                GridLines="None" 
                AllowPaging="True" 
                AllowSorting="True"
                PageSize="5"
                OnPageIndexChanging="ctl02_PageIndexChanging"
                OnSorting="ctl02_Sorting">
                
                <Columns>
                    <asp:BoundField DataField="OrderID" HeaderText="编号" ReadOnly="True" SortExpression="OrderID" />
                    <asp:BoundField DataField="OrderNumber" HeaderText="订单号" SortExpression="OrderNumber" />
                    <asp:BoundField DataField="PrePosition" HeaderText="预定位置" SortExpression="PrePosition" />
                    <asp:BoundField DataField="Destination" HeaderText="目的地" SortExpression="Destination" />
                    <asp:BoundField DataField="Notes" HeaderText="备注" SortExpression="Notes" />
                    <asp:BoundField DataField="Start_Time" HeaderText="开始时间" SortExpression="Start_Time" />
                    <asp:BoundField DataField="Status" HeaderText="状态" SortExpression="Status" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button 
                                ID="btnAcceptOrder" 
                                runat="server" 
                                Text="查看订单" 
                                CommandName="CheckOrder" 
                                CommandArgument='<%# Eval("OrderID") %>' 
                                OnClick="btnAcceptOrder_Click" 
                                CssClass="accept-btn" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <div class="empty-data">
                        <p>暂无正在进行的订单</p>
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
    
    <style>
        /* 页面内容容器 */
        .content-wrapper {
            max-width: 1600px;
            margin-top:30px;
            margin-left:240px;
            padding: 20px;
            background-color: #fff;
            border-radius: 10px;
            box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
        }

        /* 页面标题 */
        .page-title {
            font-size: 32px;
            color: #34495e;
            margin-bottom: 10px;
        }

        /* 页面描述 */
        .page-description {
            font-size: 18px;
            color: #7f8c8d;
            margin-bottom: 20px;
        }

        /* GridView 样式 */
        .styled-grid {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

        .styled-grid th,
        .styled-grid td {
            padding: 15px;
            text-align: center;
            border: 1px solid #ddd;
        }

        .styled-grid th {
            background-color: #f1f1f1;
            font-weight: bold;
            color: #34495e;
        }

        .styled-grid tr:nth-child(even) {
            background-color: #f9f9f9;
        }

        .styled-grid tr:hover {
            background-color: #e9e9e9;
            cursor: pointer;
        }

        .styled-grid img {
            max-width: 80px;
            height: auto;
            border-radius: 5px;
        }

        /* 接受订单按钮样式 */
        .accept-btn {
            background-color: #4CAF50;
            color: white;
            border: none;
            padding: 10px 20px;
            border-radius: 5px;
            cursor: pointer;
            transition: background-color 0.3s ease;
        }

        .accept-btn:hover {
            background-color: #45a049;
        }

        /* 空数据提示 */
        .empty-data {
            padding: 20px;
            text-align: center;
            background-color: #f9f9f9;
            border: 1px dashed #ddd;
            border-radius: 5px;
            margin: 10px 0;
        }
        
        .empty-data p {
            color: #7f8c8d;
            font-size: 16px;
            margin: 0;
        }

        /* 响应式设计 */
        @media screen and (max-width: 768px) {
            .content-wrapper {
                padding: 15px;
            }

            .page-title {
                font-size: 28px;
            }

            .styled-grid th,
            .styled-grid td {
                padding: 10px;
                font-size: 14px;
            }

            .styled-grid img {
                max-width: 60px;
            }

            .accept-btn {
                padding: 8px 16px;
            }
        }
        
        /* 提示消息样式 */
        .toast {
            position: fixed;
            top: 20px;
            right: 20px;
            padding: 12px 20px;
            background-color: rgba(0, 0, 0, 0.7);
            color: white;
            border-radius: 4px;
            z-index: 1000;
            visibility: hidden;
            opacity: 0;
            transition: visibility 0s, opacity 0.3s linear;
        }
        
        .toast.show {
            visibility: visible;
            opacity: 1;
        }
        
        .toast.success {
            background-color: rgba(46, 204, 113, 0.9);
        }
        
        .toast.error {
            background-color: rgba(231, 76, 60, 0.9);
        }
    </style>
    
    <script type="text/javascript">
        // 显示提示消息
        function showToast(message, type) {
            var toast = document.createElement('div');
            toast.className = 'toast ' + (type || '');
            toast.innerHTML = message;
            document.body.appendChild(toast);

            setTimeout(function () {
                toast.classList.add('show');
                setTimeout(function () {
                    toast.classList.remove('show');
                    setTimeout(function () {
                        document.body.removeChild(toast);
                    }, 300);
                }, 3000);
            }, 100);
        }
    </script>
</asp:Content>
