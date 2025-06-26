using System;

namespace YNL.JAMOS
{
    public enum PaymentMethod : byte { CashOnDelivery, CreditCard, VISA, Paypal, VNPay }

    public partial class OrderPayment : PageBehaviour
    {
        public Action<DeliveryType, float, float, float, float, float> OnPaymentPageRefreshed;
        public Action OnOrderPaymentRequested;
        public Action<float> OnTotalPriceDisplayed;
        public Action<float> OnDeliveryPriceDisplayed;

        private View _view;
        private Controller _controller;

        protected override void Construct()
        {
            RegisterView(_view = ViewFactory.CreateView<View>(Root, this));
            RegisterController(_controller = ViewFactory.CreateController<Controller>(this));
        }
    }
}