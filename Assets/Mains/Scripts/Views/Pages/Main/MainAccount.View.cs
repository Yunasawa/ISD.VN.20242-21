using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class MainAccount
    {
        private class View : PageView
        {
            private MainAccount _b;

            private VisualElement _profilePicture;
            private Label _nameText;
            private VisualElement _changeNameButton;
            private Label _phoneField;
            private Label _emailField;
            private VisualElement _languageField;
            private Label _languageText;
            private VisualElement _locationField;
            private Label _locationText;
            private VisualElement _qaFieldField;
            private VisualElement _policyField;
            private VisualElement _versionField;
            private Label _versionText;
            private VisualElement _contactField;
            private VisualElement _signOutField;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as MainAccount;

                Marker.OnSignedInOrSignedUp += OnSignedInOrSignedUp;
            }

            ~View()
            {
                Marker.OnSignedInOrSignedUp -= OnSignedInOrSignedUp;
            }

            public override void Collect(VisualElement root)
            {
                var contentContainer = root.Q("ContentScroll").Q("unity-content-container");

                var accountField = contentContainer.Q("AccountField");

                _profilePicture = accountField.Q("ProfilePicture");

                var infoField = accountField.Q("InfoField");

                _nameText = infoField.Q("NameField").Q("NameText") as Label;
                _phoneField = infoField.Q("PhoneField") as Label;
                _emailField = infoField.Q("EmailField") as Label;

                _changeNameButton = infoField.Q("ChangeButton");
                _changeNameButton.RegisterCallback<PointerUpEvent>(OnClicked_ChangeNameButton);

                var settingsField = contentContainer.Q("SettingsField");

                _languageField = settingsField.Q("LanguageField");
                _languageText = _languageField.Q("Text") as Label;
                _languageField?.RegisterCallback<PointerUpEvent>(OnClicked_LanguageField);

                _locationField = settingsField.Q("LocationField");
                _locationText = _locationField.Q("Text") as Label;
                _locationField?.RegisterCallback<PointerUpEvent>(OnClicked_LocationField);

                var informationsField = contentContainer.Q("InformationsField");

                _qaFieldField = informationsField.Q("QAField");
                _qaFieldField?.RegisterCallback<PointerUpEvent>(OnClicked_QandAField);

                _policyField = informationsField.Q("PolicyField");
                _policyField?.RegisterCallback<PointerUpEvent>(OnClicked_PolicyField);

                _versionField = informationsField.Q("VersionField");
                _versionField?.RegisterCallback<PointerUpEvent>(OnClicked_VersionField);

                _contactField = informationsField.Q("ContactField");
                _contactField?.RegisterCallback<PointerUpEvent>(OnClicked_ContactField);

                _signOutField = informationsField.Q("SignOutField");
                _signOutField?.RegisterCallback<PointerUpEvent>(OnClicked_SignOutField);
            }

            private void OnClicked_ChangeNameButton(PointerUpEvent evt)
            {

            }

            private void OnClicked_LanguageField(PointerUpEvent evt)
            {

            }

            private void OnClicked_LocationField(PointerUpEvent evt)
            {

            }

            private void OnClicked_QandAField(PointerUpEvent evt)
            {

            }

            private void OnClicked_PolicyField(PointerUpEvent evt)
            {

            }

            private void OnClicked_VersionField(PointerUpEvent evt)
            {

            }

            private void OnClicked_ContactField(PointerUpEvent evt)
            {

            }

            private void OnClicked_SignOutField(PointerUpEvent evt)
            {
                Main.Runtime.Data.AccountID = string.Empty;

                Marker.OnPageNavigated?.Invoke(ViewType.SigningViewSignInPage, true, true);
            }

            private void OnSignedInOrSignedUp()
            {
                var account = Main.Database.Accounts[Main.Runtime.Data.AccountID];

                _nameText.SetText(account.Name);
                _phoneField.SetText(account.PhoneNumber);
                _emailField.SetText(account.Email);
            }
        }
    }
}
