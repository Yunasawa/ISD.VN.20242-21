namespace YNL.JAMOS
{
    public partial class ManagerOrder
    {
        private class Controller : PageController
        {
            private ManagerOrder _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerOrder;
            }
        }
    }
}