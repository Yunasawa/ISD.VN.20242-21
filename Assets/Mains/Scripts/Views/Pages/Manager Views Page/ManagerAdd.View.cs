using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class ManagerAdd
    {
        private class View : PageView
        {
            private ManagerAdd _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerAdd;
            }

            public override void Collect(VisualElement root)
            {

            }
        }
    }
}
