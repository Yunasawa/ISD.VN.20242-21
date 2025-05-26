using UnityEngine;
using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public class SigningViewSingingInPageUI : ViewPageUI
    {
        private TextField _accountInputField;
        private Label _accountMessage;
        private TextField _passwordInputField;
        private Label _passwordMessage;

        private Button _signingButton;
        private VisualElement _recoveryButton;

        private VisualElement _signInWithFacebookButton;
        private VisualElement _signInWithGoogleButton;

        protected override void Collect()
        {
            var signingInputField = Root.Q("SigningInputField");

            _accountInputField = signingInputField.Q("AccountField").Q("TextField") as TextField;
            _accountInputField.RegisterValueChangedCallback(OnValueChanged_AccountInputField);

            _accountMessage = signingInputField.Q("AccountField").Q("Message") as Label;

            _passwordInputField = signingInputField.Q("PasswordField").Q("TextField") as TextField;
            _passwordInputField.RegisterValueChangedCallback(OnValueChanged_PasswordInputField);

            _passwordMessage = signingInputField.Q("PasswordField").Q("Message") as Label;

            _signInWithFacebookButton = Root.Q("SigningMethod").Q("FacebookSigning");
            _signInWithFacebookButton.RegisterCallback<PointerUpEvent>(SigningWithFacebook);

            _signInWithGoogleButton = Root.Q("SigningMethod").Q("GoogleSigning");
            _signInWithGoogleButton.RegisterCallback<PointerUpEvent>(SignInWithGoogle);

            _signingButton = signingInputField.Q("SigningButton").Q("Button") as Button;
            _signingButton.clicked += SigningAccount;

            _recoveryButton = signingInputField.Q("RecoveryButton");
            _recoveryButton.RegisterCallback<PointerUpEvent>(RecoveryAccount);
        }

        private void OnValueChanged_AccountInputField(ChangeEvent<string> evt)
        {

        }

        private void OnValueChanged_PasswordInputField(ChangeEvent<string> evt)
        {

        }

        private void SigningWithFacebook(PointerUpEvent evt)
        {

        }

        private void SignInWithGoogle(PointerUpEvent evt)
        {

        }

        private void SigningAccount()
        {

        }

        private void RecoveryAccount(PointerUpEvent evt)
        {

        }
    }
}
