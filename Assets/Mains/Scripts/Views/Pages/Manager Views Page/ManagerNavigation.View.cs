using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class ManagerNavigation
    {
        private class View : PageView
        {
            private ManagerNavigation _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerNavigation;
            }

            public override void Collect(VisualElement root)
            {

            }
        }
    }
}
