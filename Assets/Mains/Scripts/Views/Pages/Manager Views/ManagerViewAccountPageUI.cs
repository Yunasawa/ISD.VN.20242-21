using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public class ManagerViewAccountPageUI : ViewPageUI
    {
        private VisualElement _signOutField;

        protected override void Collect()
        {
            var contentContainer = Root.Q("ContentScroll").Q("unity-content-container");

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