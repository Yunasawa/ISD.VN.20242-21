using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class ManagerRevuene
    {
        private class View : PageView
        {
            private ManagerRevuene _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerRevuene;
            }

            public override void Collect(VisualElement root)
            {

            }
        }
    }
}
