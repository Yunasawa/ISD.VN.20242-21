using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class ManagerUpdate
    {
        private class Controller : PageController
        {
            private InformationMain _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as InformationMain;

                _b.OnBackButtonClicked += OnBackButtonClicked;
            }

            private void OnBackButtonClicked()
            {
                Marker.OnPageBacked?.Invoke(true, false);
            }
        }
    }
}