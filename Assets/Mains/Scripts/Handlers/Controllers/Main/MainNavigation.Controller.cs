namespace YNL.JAMOS
{
    public partial class MainNavigation
    {
        private class Controller : PageController
        {
            private MainNavigation _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as MainNavigation;
            }
        }
    }
}