using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class ManagerInformation
    {
        private class View : PageView
        {
            private ManagerInformation _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerInformation;
            }

            public override void Collect(VisualElement root)
            {

            }
        }
    }
}
