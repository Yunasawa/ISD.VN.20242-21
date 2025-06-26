using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class SigningSigningIn
    {
        private class Controller : PageController
        {
            private SigningSigningIn _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as SigningSigningIn;

                _b.OnBackButtonClicked += OnBackButtonClicked;
            }

            private void OnBackButtonClicked()
            {
                Marker.OnPageBacked?.Invoke(true, false);
            }
        }
    }
}