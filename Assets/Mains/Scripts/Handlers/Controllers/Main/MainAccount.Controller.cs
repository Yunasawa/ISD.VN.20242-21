namespace YNL.JAMOS
{
    public partial class MainAccount
    {
        private class Controller : PageController
        {
            private MainAccount _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as MainAccount;
            }
        }
    }
}