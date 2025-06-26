using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class OrderPayment
    {
        private class View : PageView
        {
            private OrderPayment _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as OrderPayment;
            }

            public override void Collect(VisualElement root)
            {

            }
        }
    }
}
