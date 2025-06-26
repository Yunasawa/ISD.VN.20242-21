using System.Linq;
using UnityEngine.UIElements;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class SigningSigningIn
    {
        private class View : PageView
        {
            private SigningSigningIn _b;

            private TextField _accountInputField;
            private Label _accountMessage;
            private TextField _passwordInputField;
            private Label _passwordMessage;

            private Button _signingButton;
            private VisualElement _recoveryButton;

            private VisualElement _signInWithFacebookButton;
            private VisualElement _signInWithGoogleButton;

            private VisualElement _switchLabel;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as SigningSigningIn;

                _b.OnAccountMessageNotified += OnAccountMessageNotified;
                _b.OnPasswordMessageNotified += OnPasswordMessageNotified;
            }

            public override void Collect(VisualElement root)
            {
                var signingInputField = root.Q("SigningInputField");

                _accountInputField = signingInputField.Q("AccountField").Q("TextField") as TextField;
                _accountInputField.RegisterValueChangedCallback(OnValueChanged_AccountInputField);

                _accountMessage = signingInputField.Q("AccountField").Q("Message") as Label;

                _passwordInputField = signingInputField.Q("PasswordField").Q("TextField") as TextField;
                _passwordInputField.RegisterValueChangedCallback(OnValueChanged_PasswordInputField);

                _passwordMessage = signingInputField.Q("PasswordField").Q("Message") as Label;

                _signInWithFacebookButton = root.Q("SigningMethod").Q("FacebookSigning");
                _signInWithFacebookButton.RegisterCallback<PointerUpEvent>(SigningWithFacebook);

                _signInWithGoogleButton = root.Q("SigningMethod").Q("GoogleSigning");
                _signInWithGoogleButton.RegisterCallback<PointerUpEvent>(SignInWithGoogle);

                _signingButton = signingInputField.Q("SigningButton").Q("Button") as Button;
                _signingButton.clicked += SigningAccount;

                _recoveryButton = signingInputField.Q("RecoveryButton");
                _recoveryButton.RegisterCallback<PointerUpEvent>(RecoveryAccount);

                _switchLabel = root.Q("SwitchLabel");
                _switchLabel.RegisterCallback<PointerUpEvent>(OnClicked_SwitchLabel);
            }

            public override void Refresh()
            {
                _accountInputField.value = string.Empty;
                _passwordInputField.value = string.Empty;

                _accountMessage.SetText(string.Empty);
                _passwordMessage.SetText(string.Empty);
            }

            private void OnValueChanged_AccountInputField(ChangeEvent<string> evt)
            {
                _accountMessage.SetText(string.Empty);

                _b.OnAccountFieldFilled?.Invoke(evt.newValue);
            }

            private void OnValueChanged_PasswordInputField(ChangeEvent<string> evt)
            {
                _passwordMessage.SetText(string.Empty);

                _b.OnPasswordFieldFilled?.Invoke(evt.newValue);
            }

            private void OnClicked_SwitchLabel(PointerUpEvent evt)
            {
                Marker.OnPageNavigated?.Invoke(ViewType.SigningViewSignUpPage, true, false);
            }

            private void SigningWithFacebook(PointerUpEvent evt)
            {

            }

            private void SignInWithGoogle(PointerUpEvent evt)
            {

            }

            private void SigningAccount()
            {
                _b.OnSignInRequested?.Invoke();
            }

            private void RecoveryAccount(PointerUpEvent evt)
            {

            }

            private void OnAccountMessageNotified()
            {
                _accountMessage.SetText("This email or phone number is not registered yet");
            }

            private void OnPasswordMessageNotified()
            {
                _passwordMessage.SetText("Incorrect password! Please check again.");
            }
        }
    }
}
