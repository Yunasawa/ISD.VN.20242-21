using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class ManagerMessage
    {
        private class View : PageView
        {
            private ManagerMessage _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerMessage;
            }

            public override void Collect(VisualElement root)
            {

            }
        }
    }
}
