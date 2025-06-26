using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class MainOrder
    {
        private class View : PageView
        {
            private MainOrder _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as MainOrder;
            }

            public override void Collect(VisualElement root)
            {

            }
        }
    }
}
