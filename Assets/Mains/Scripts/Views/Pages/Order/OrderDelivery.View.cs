using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class OrderDelivery
    {
        private class View : PageView
        {
            private OrderDelivery _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as OrderDelivery;
            }

            public override void Collect(VisualElement root)
            {

            }
        }
    }
}
