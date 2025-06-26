using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class ManagerProduct
    {
        private class Controller : PageController
        {
            private ManagerProduct _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerProduct;
            }
        }
    }
}