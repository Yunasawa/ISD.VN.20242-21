using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class ManagerNavigation
    {
        private class Controller : PageController
        {
            private ManagerNavigation _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerNavigation;
            }
        }
    }
}