using System.Linq;
using System.Text.RegularExpressions;
using System;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class SigningSigningUp
    {
        private class View : PageView
        {
            private SigningSigningUp _b;

            private TextField _accountInputField;
            private Label _accountMessage;
            private TextField _passwordInputField;
            private Label _8charactersMessage;
            private Label _oneNumberMessage;
            private Label _oneSpecialCharacterMessage;
            private Label _passwordMessage;
            private TextField _confirmInputField;
            private Label _confirmMessage;

            private VisualElement _switchLabel;

            private Button _signingButton;

            private VisualElement _signInWithFacebookButton;
            private VisualElement _signInWithGoogleButton;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as SigningSigningUp;

                _b.OnAccountMessageNotified += OnAccountMessageNotified;
                _b.OnPasswordMessageNotified += OnPasswordMessageNotified;
                _b.OnConfirmMessageNotified += OnConfirmMessageNotified;
            }

            public override void Collect(VisualElement root)
            {
                var signingInputField = root.Q("SigningInputField");

                _accountInputField = signingInputField.Q("AccountField").Q("TextField") as TextField;
                _accountInputField.RegisterValueChangedCallback(OnValueChanged_AccountInputField);

                _accountMessage = signingInputField.Q("AccountField").Q("Message") as Label;

                _passwordInputField = signingInputField.Q("PasswordField").Q("TextField") as TextField;
                _passwordInputField.RegisterValueChangedCallback(OnValueChanged_PasswordInputField);

                _8charactersMessage = signingInputField.Q("PasswordField").Q("AtLest8Characters") as Label;
                _oneNumberMessage = signingInputField.Q("PasswordField").Q("AtLeastOneNumber") as Label;
                _oneSpecialCharacterMessage = signingInputField.Q("PasswordField").Q("AtLeastOneSpecialCharacter") as Label;
                _passwordMessage = signingInputField.Q("PasswordField").Q("Message") as Label;

                _confirmInputField = signingInputField.Q("ConfirmField").Q("TextField") as TextField;
                _confirmInputField.RegisterValueChangedCallback(OnValueChanged_ConfirmInputField);

                _confirmMessage = signingInputField.Q("ConfirmField").Q("Message") as Label;

                _signInWithFacebookButton = root.Q("SigningMethod").Q("FacebookSigning");
                _signInWithFacebookButton.RegisterCallback<PointerUpEvent>(SigningWithFacebook);

                _signInWithGoogleButton = root.Q("SigningMethod").Q("GoogleSigning");
                _signInWithGoogleButton.RegisterCallback<PointerUpEvent>(SignInWithGoogle);

                _signingButton = signingInputField.Q("SigningButton").Q("Button") as Button;
                _signingButton.clicked += SigningAccount;

                _switchLabel = root.Q("SwitchLabel");
                _switchLabel.RegisterCallback<PointerUpEvent>(OnClicked_SwitchLabel);
            }

            public override void Refresh()
            {
                _accountInputField.value = string.Empty;
                _passwordInputField.value = string.Empty;
                _confirmInputField.value = string.Empty;

                _accountMessage.SetText(string.Empty);
                _passwordMessage.SetText(string.Empty);
                _confirmMessage.SetText(string.Empty);
            }

            private void OnValueChanged_AccountInputField(ChangeEvent<string> evt)
            {
                _b.OnAccountInputChanged?.Invoke(evt.newValue);
            }

            private void OnValueChanged_PasswordInputField(ChangeEvent<string> evt)
            {
                _b.OnPasswordInputChanged?.Invoke(evt.newValue);

                var _passwordInput = evt.newValue;

                var valid8character = _passwordInput.Length >= 8;
                var valid1number = Regex.IsMatch(_passwordInput, @"\d");
                var valid1special = Regex.IsMatch(_passwordInput, @"[!@#$%^&*(),.?\:{ }|<>]");

                _8charactersMessage.SetColor(valid8character ? "#5FFF9F" : "#FF5F5F");
                _oneNumberMessage.SetColor(valid1number ? "#5FFF9F" : "#FF5F5F");
                _oneSpecialCharacterMessage.SetColor(valid1special ? "#5FFF9F" : "#FF5F5F");
            }

            private void OnValueChanged_ConfirmInputField(ChangeEvent<string> evt)
            {
                _b.OnConfirmInputChanged?.Invoke(evt.newValue);
            }

            private void OnClicked_SwitchLabel(PointerUpEvent evt)
            {
                Marker.OnPageNavigated?.Invoke(ViewType.SigningViewSignInPage, true, false);
            }

            private void SigningWithFacebook(PointerUpEvent evt) { }
            private void SignInWithGoogle(PointerUpEvent evt) { }

            private void SigningAccount()
            {
                _b.OnSignUpRequested?.Invoke();
            }

            private void RecoveryAccount(PointerUpEvent evt) { }

            private void OnAccountMessageNotified(string message)
            {
                _accountMessage.SetText(message);
            }

            private void OnPasswordMessageNotified(string message)
            {
                _passwordMessage.SetText(message);
            }

            private void OnConfirmMessageNotified(string message)
            {
                _confirmMessage.SetText(message);
            }
        }
    }
}
