using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class InformationReview
    {
        private class View : PageView
        {
            private InformationReview _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as InformationReview;
            }

            public override void Collect(VisualElement root)
            {
            }
        }
    }
}