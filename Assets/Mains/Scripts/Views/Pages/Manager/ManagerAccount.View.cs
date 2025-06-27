using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class ManagerAccount
    {
        private class View : PageView
        {
            private ManagerAccount _b;

            private VisualElement _signOutField;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerAccount;
            }

            public override void Collect(VisualElement root)
            {
                var contentContainer = root.Q("ContentScroll").Q("unity-content-container");

                var informationsField = contentContainer.Q("InformationsField");

                _signOutField = informationsField.Q("SignOutField");
                _signOutField?.RegisterCallback<PointerUpEvent>(OnClicked_SignOutField);
            }

            private void OnClicked_SignOutField(PointerUpEvent evt)
            {
                Main.Runtime.Data.AccountID = string.Empty;

                Marker.OnPageNavigated?.Invoke(ViewType.SigningViewSignInPage, true, true);
            }
        }
    }
}
