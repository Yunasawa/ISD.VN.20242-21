using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class OrderResult
    {
        private class View : PageView
        {
            private InformationMain _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as InformationMain;
            }

            public override void Collect(VisualElement root)
            {

            }
        }
    }
}
