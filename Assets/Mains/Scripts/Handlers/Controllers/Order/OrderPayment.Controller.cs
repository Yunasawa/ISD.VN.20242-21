namespace YNL.JAMOS
{
    public partial class OrderPayment
    {
        private class Controller : PageController
        {
            private OrderPayment _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as OrderPayment;
            }
        }
    }
}
