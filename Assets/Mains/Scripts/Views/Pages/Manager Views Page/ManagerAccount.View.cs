using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class ManagerAccount
    {
        private class View : PageView
        {
            private ManagerAccount _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerAccount;
            }

            public override void Collect(VisualElement root)
            {

            }
        }
    }
}
