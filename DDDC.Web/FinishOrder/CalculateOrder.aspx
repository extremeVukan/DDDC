<%@ Page Title="订单评分" Language="C#" MasterPageFile="~/FinishOrder/CalculateMasterPage2.master" AutoEventWireup="true" CodeFile="CalculateOrder.aspx.cs" Inherits="FinishOrder_CalculateOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- 添加ScriptManager控件，必须放在UpdatePanel之前 -->
   
    <br />
    <asp:Image ID="Image2" runat="server" Height="96px" Width="136px" style="border-radius: 15px; border: 2px solid #ccc;" />
    <br />
    <h2 class="username" style="margin-top:10px;">
        <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label>
    </h2>
    <p class="email">
        <asp:Label ID="lblemail" runat="server" Text="Label"></asp:Label>
    </p>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <div class="checkout-wrapper">
        <div class="checkout-container">
            <!-- 订单详情卡片 -->
            <div class="order-details-card">
                <h2>订单详情</h2>
                <div class="order-details">
                    <div class="order-item">
                        <label>订单号</label>
                        <span><asp:Label ID="lblOrderNumber" runat="server"></asp:Label></span>
                    </div>
                    <div class="order-item">
                        <label>船只名称</label>
                        <span><asp:Label ID="lblShipName" runat="server"></asp:Label></span>
                    </div>
                    <div class="order-item">
                        <label>司机姓名</label>
                        <span><asp:Label ID="lblDriverName" runat="server"></asp:Label></span>
                    </div>
                    <div class="order-item">
                        <label>司机电话</label>
                        <span><asp:Label ID="lblDriverPhone" runat="server"></asp:Label></span>
                    </div>
                    <div class="order-item">
                        <label>起始位置</label>
                        <span><asp:Label ID="lblStartPosition" runat="server"></asp:Label></span>
                    </div>
                    <div class="order-item">
                        <label>目的地</label>
                        <span><asp:Label ID="lblDestination" runat="server"></asp:Label></span>
                    </div>
                    <div class="order-item">
                        <label>费用</label>
                        <span class="price"><asp:Label ID="lblCost" runat="server"></asp:Label></span>
                    </div>
                </div>

                <!-- 订单评分 -->
                <div class="order-rating">
                    <label style="font-weight:bold;color: #666;">评分</label>
                    <div class="rating-wrapper" style="margin-top:30px">
                        <button type="button" class="star-btn" data-value="1"></button>
                        <button type="button" class="star-btn" data-value="2"></button>
                        <button type="button" class="star-btn" data-value="3"></button>
                        <button type="button" class="star-btn" data-value="4"></button>
                        <button type="button" class="star-btn" data-value="5"></button>
                    </div>
                    <asp:HiddenField ID="hfRating" runat="server" />
                    <p class="rating-value">当前评分: <span id="ratingValue">0</span> 分</p>
                    
                    <!-- 显示评分提交状态 -->
                    <asp:Label ID="lblRatingStatus" runat="server" CssClass="status-message" Visible="false"></asp:Label>
                </div>
            </div>

            <!-- 船只照片和评论 -->
            <div class="photo-comment-container">
                <div class="ship-photo-card">
                    <h3>船只照片</h3>
                    <asp:Image ID="imgShipPhoto" runat="server" CssClass="ship-photo" AlternateText="船只照片" />
                </div>

                <!-- 评论区块，使用UpdatePanel实现无刷新提交 -->
                <asp:UpdatePanel ID="UpdatePanelComment" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="comment-card">
                            <h3>订单评论</h3>
                            <asp:TextBox ID="txtComment" runat="server" CssClass="comment-box" TextMode="MultiLine" Rows="4"
                                         Placeholder="请留下您对本次服务的评价..."></asp:TextBox>
                                         
                            <!-- 评价按钮 -->
                            <asp:Button ID="btnSubmitReview" runat="server" Text="提交评价与评分" CssClass="comment-button" 
                                        OnClick="btnSubmitComment_Click" />
                            
                            <!-- 评论提交状态信息 -->
                            <asp:Label ID="lblCommentStatus" runat="server" CssClass="status-message" Visible="false"></asp:Label>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSubmitReview" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>

            <!-- 支付按钮 -->
            <div class="payment-section">
                <asp:Button ID="btnPay" runat="server" Text="去付款" CssClass="pay-button" OnClientClick="showModal(); return false;" />
            </div>
        </div>

        <!-- 模态框 -->
        <div id="paymentModal" class="modal">
            <div class="modal-content">
                <span class="close" onclick="closeModal()">&times;</span>
                <h2>支付二维码</h2>
                <div class="qr-code">
                    <img id="qrCode" alt="支付二维码" />
                </div>
                <div class="modal-buttons">
                    <button onclick="closeModal()" class="return-button">返回</button>
                    <asp:Button ID="btnConfirmPayment" runat="server" Text="确认支付" CssClass="confirm-button" OnClick="btnConfirmPayment_Click" />
                </div>
            </div>
        </div>
    </div>

    <style>
        /* 全局布局 */
        body {
            font-family: Arial, sans-serif;
            background-color: #f5f5f5;
            margin: 0;
            padding: 0;
        }

        .checkout-wrapper {
            background: #ffffff;
            max-width: 1400px;
            margin: 20px 200px;
            padding: 20px;
            border-radius: 15px;
            box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
        }

        .checkout-container {
            display: grid;
            grid-template-columns: 2fr 1fr;
            gap: 20px;
        }

        /* 订单详情 */
        .order-details-card {
            background: white;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
            font-size: 18px;
        }

        .order-details-card h2 {
            margin-bottom: 20px;
            color: #34495e;
        }

        .order-details .order-item {
            display: flex;
            justify-content: space-between;
            margin-bottom: 30px;
            font-size: 18px;
        }

        .order-details .order-item label {
            font-weight: bold;
            color: #666;
        }

        .order-details .order-item span {
            color: #333;
        }

        .order-details .price {
            color: #e74c3c;
            font-weight: bold;
        }

        /* 船只照片与评论 */
        .photo-comment-container {
            display: grid;
            grid-template-rows: 1fr 1fr;
            gap: 20px;
        }

        .ship-photo-card, .comment-card {
            background: white;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
        }

        .ship-photo {
            width: 250px;
            height: 250px;
            max-height: 300px;
            border-radius: 5px;
            object-fit: cover;
        }

        .comment-box {
            width: 100%;
            padding: 15px;
            font-size: 16px;
            border-radius: 5px;
            border: 1px solid #ddd;
            margin-top: 10px;
        }

        .comment-button {
            display: block;
            width: 100%;
            margin-top: 10px;
            padding: 15px;
            background-color: #4CAF50;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 18px;
        }

        .comment-button:hover {
            background-color: #45a049;
        }
        
        .comment-button:disabled {
            background-color: #a0a0a0;
            cursor: not-allowed;
        }

        /* 支付部分 */
        .payment-section {
            grid-column: span 2;
            text-align: center;
            margin-top: 20px;
        }
        
        .pay-button {
            background-color: #1abc9c;
            color: white;
            border: none;
            padding: 20px 40px;
            border-radius: 10px;
            font-size: 20px;
            cursor: pointer;
        }

        .pay-button:hover {
            background-color: #16a085;
        }

        /* 模态框样式 */
        .modal {
            display: none;
            position: fixed;
            z-index: 1000;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.5);
        }

        .modal-content {
            background-color: #fff;
            margin: 10% auto;
            padding: 30px;
            border-radius: 15px;
            box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
            width: 90%;
            max-width: 500px;
            text-align: center;
        }

        .modal-content h2 {
            margin-bottom: 20px;
            font-size: 28px;
            color: #34495e;
        }

        .qr-code img {
            width: 200px;
            height: 200px;
            margin: 20px auto;
            display: block;
        }
       
        .modal-buttons {
            display: flex;
            justify-content: space-between;
            gap: 15px;
        }

        .modal-buttons .return-button,
        .modal-buttons .confirm-button {
            flex: 1;
            padding: 10px;
            font-size: 16px;
            font-weight: bold;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            transition: all 0.3s ease;
        }

        .modal-buttons .return-button {
            background-color: #e74c3c;
            color: white;
        }

        .modal-buttons .confirm-button {
            background-color: #1abc9c;
            color: white;
        }

        .modal-buttons .return-button:hover {
            background-color: #c0392b;
        }

        .modal-buttons .confirm-button:hover {
            background-color: #16a085;
        }

        .close {
            position: absolute;
            top: 10px;
            right: 20px;
            font-size: 28px;
            font-weight: bold;
            color: #999;
            cursor: pointer;
            transition: color 0.3s ease;
        }

        .close:hover {
            color: #333;
        }
        
        .rating-wrapper {
            display: flex;
            gap: 5px;
            justify-content: center;
            margin-top: 10px;
        }

        .star-btn {
            width: 40px;
            height: 40px;
            background-image: url("/UserImg/星星.png");
            background-size: contain;
            border: none;
            cursor: pointer;
            filter: grayscale(100%);
            transition: filter 0.3s ease;
        }

        .star-btn:hover,
        .star-btn.active {
            filter: none;
            background-color: #f1c40f;
        }

        .rating-value {
            text-align: center;
            margin-top: 10px;
            font-size: 16px;
            color: #333;
        }
        
        /* 状态消息样式 */
        .status-message {
            margin-top: 10px;
            padding: 10px;
            border-radius: 5px;
            text-align: center;
            font-weight: bold;
            animation: fadeOut 3s forwards;
            animation-delay: 3s;
        }
        
        @keyframes fadeOut {
            0% { opacity: 1; }
            100% { opacity: 0; }
        }
        
        .status-message.success {
            background-color: #d4edda;
            color: #155724;
            border: 1px solid #c3e6cb;
        }
        
        .status-message.error {
            background-color: #f8d7da;
            color: #721c24;
            border: 1px solid #f5c6cb;
        }
    </style>

    <!-- JavaScript处理逻辑 -->
    <script>
        // 模态框相关函数
        function showModal() {
            document.getElementById("paymentModal").style.display = "block";
            generateQRCode();
        }

        function closeModal() {
            document.getElementById("paymentModal").style.display = "none";
        }

        function generateQRCode() {
            const qrCodeImg = document.getElementById("qrCode");
            const randomData = Math.random().toString(36).substring(2, 10);
            qrCodeImg.src = `https://api.qrserver.com/v1/create-qr-code/?size=200x200&data=${randomData}`;
        }

        // 评分系统相关函数
        document.addEventListener("DOMContentLoaded", function () {
            const stars = document.querySelectorAll(".star-btn");
            const ratingValue = document.getElementById("ratingValue");
            const hiddenRating = document.getElementById("<%= hfRating.ClientID %>");

            // 初始化星级
            initializeStars();

            stars.forEach((star) => {
                star.addEventListener("click", function () {
                    const value = this.getAttribute("data-value");
                    hiddenRating.value = value; // 保存评分到隐藏字段
                    ratingValue.innerText = value;
                    updateStarColors(value);
                });
            });

            function initializeStars() {
                const initialRating = hiddenRating.value;
                if (initialRating) {
                    ratingValue.innerText = initialRating;
                    updateStarColors(initialRating);
                }
            }

            function updateStarColors(value) {
                stars.forEach((star) => {
                    if (parseInt(star.getAttribute("data-value")) <= parseInt(value)) {
                        star.classList.add("active");
                    } else {
                        star.classList.remove("active");
                    }
                });
            }
        });

        function updateRatingDisplay(rating) {
            if (!rating || rating === "0") return;

            // 更新评分显示
            document.getElementById("ratingValue").textContent = rating;

            // 更新隐藏字段
            document.getElementById("<%= hfRating.ClientID %>").value = rating;

            // 更新星星样式
            const stars = document.querySelectorAll(".star-btn");
            stars.forEach((star) => {
                const value = parseInt(star.getAttribute("data-value"));
                if (value <= parseInt(rating)) {
                    star.classList.add("active");
                } else {
                    star.classList.remove("active");
                }
            });
        }
        
        // 处理状态消息自动隐藏
        function hideStatusMessage(messageId) {
            setTimeout(function() {
                var message = document.getElementById(messageId);
                if (message) {
                    message.style.opacity = '0';
                    setTimeout(function() {
                        message.style.display = 'none';
                    }, 1000);
                }
            }, 3000);
        }
        
        // 提交评论后的回调函数
        function onCommentSubmitted(successful) {
            if (successful) {
                const statusMsg = document.getElementById('<%= lblCommentStatus.ClientID %>');
                if (statusMsg) {
                    // 3秒后自动隐藏消息
                    hideStatusMessage('<%= lblCommentStatus.ClientID %>');
                }
            }
        }
    </script>
</asp:Content>
