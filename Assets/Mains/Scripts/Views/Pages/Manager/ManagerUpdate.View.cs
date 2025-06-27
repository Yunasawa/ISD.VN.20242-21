using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class ManagerUpdate
    {
        private class View : PageView
        {
            private ManagerUpdate _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerUpdate;
            }

            public override void Collect(VisualElement root)
            {

            }
        }
    }
}
