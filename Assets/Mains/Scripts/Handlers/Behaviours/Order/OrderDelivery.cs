namespace YNL.JAMOS
{
    public enum DeliveryType : byte { Normal, Fast, Rush }

    public partial class OrderDelivery : PageBehaviour
    {
        private View _view;
        private Controller _controller;

        protected override void Construct()
        {
            RegisterView(_view = ViewFactory.CreateView<View>(Root, this));
            RegisterController(_controller = ViewFactory.CreateController<Controller>(this));
        }
    }
}