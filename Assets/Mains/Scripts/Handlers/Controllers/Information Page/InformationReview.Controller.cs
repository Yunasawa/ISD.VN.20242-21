using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class InformationReview
    {
        private class Controller : PageController
        {
            private InformationReview _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as InformationReview;

                _b.OnBackButtonClicked += OnBackButtonClicked;
            }

            private void OnBackButtonClicked()
            {
                Marker.OnPageBacked?.Invoke(true, false);
            }
        }
    }
}