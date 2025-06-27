using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class ManagerProduct
    {
        private class View : PageView
        {
            private ManagerProduct _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerProduct;
            }

            public override void Collect(VisualElement root)
            {

            }
        }
    }
}
