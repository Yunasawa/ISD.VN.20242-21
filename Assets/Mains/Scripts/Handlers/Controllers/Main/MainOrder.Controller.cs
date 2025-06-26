namespace YNL.JAMOS
{
    public partial class MainOrder
    {
        private class Controller : PageController
        {
            private MainOrder _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as MainOrder;
            }
        }
    }
}