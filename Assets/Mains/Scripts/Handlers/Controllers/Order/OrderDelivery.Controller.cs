namespace YNL.JAMOS
{
    public partial class OrderDelivery
    {
        private class Controller : PageController
        {
            private OrderDelivery _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as OrderDelivery;
            }
        }
    }
}