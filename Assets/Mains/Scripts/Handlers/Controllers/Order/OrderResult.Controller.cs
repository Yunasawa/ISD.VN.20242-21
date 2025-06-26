namespace YNL.JAMOS
{
    public partial class OrderResult
    {
        private class Controller : PageController
        {
            private OrderResult _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as OrderResult;
            }
        }
    }
}