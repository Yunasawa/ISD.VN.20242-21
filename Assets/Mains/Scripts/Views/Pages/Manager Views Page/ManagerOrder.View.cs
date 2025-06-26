using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class ManagerOrder
    {
        private class View : PageView
        {
            private ManagerOrder _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerOrder;
            }

            public override void Collect(VisualElement root)
            {

            }
        }
    }
}
