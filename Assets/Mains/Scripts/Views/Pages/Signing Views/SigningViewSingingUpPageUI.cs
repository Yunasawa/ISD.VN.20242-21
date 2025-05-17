using PlasticGui;
using System.Text.RegularExpressions;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.Checkotel
{
    public class SigningViewSingingUpPageUI : MonoBehaviour, ICollectible
    {
        private VisualElement _root;

        private TextField _accountInputField;
        private Label _accountMessage;
        private TextField _passwordInputField;
        private Label _8charactersMessage;
        private Label _oneNumberMessage;
        private Label _oneSpecialCharacterMessage;
        private Label _passwordMessage;
        private TextField _confirmInputField;
        private Label _confirmMessage;

        private Button _signingButton;

        private VisualElement _signInWithFacebookButton;
        private VisualElement _signInWithGoogleButton;

        private Label _switchText;

        private bool _validEmailInput;
        private string _accountInput;
        private string _passwordInput;
        private bool _validAccountInput;
        private bool _validPasswordInput;
        private bool _validConfirmInput;

        private void Awake()
        {
            Marker.OnSystemStart += Collect;
        }

        private void OnDestroy()
        {
            Marker.OnSystemStart -= Collect;
        }

        public void Collect()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            var signingInputField = _root.Q("SigningInputField");

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

            _signInWithFacebookButton = _root.Q("SigningMethod").Q("FacebookSigning");
            _signInWithFacebookButton.RegisterCallback<PointerDownEvent>(SigningWithFacebook);

            _signInWithGoogleButton = _root.Q("SigningMethod").Q("GoogleSigning");
            _signInWithGoogleButton.RegisterCallback<PointerDownEvent>(SignInWithGoogle);

            _signingButton = signingInputField.Q("SigningButton") as Button;
            _signingButton.clicked += SigningAccount;

            _switchText = _root.Q("SwitchLabel").Q("SwitchText") as Label;
        }

        private void OnValueChanged_AccountInputField(ChangeEvent<string> evt)
        {
            _accountInput = evt.newValue;

            if (_accountInput == string.Empty) return;

            _validEmailInput = Extension.Validator.ValidateEmail(_accountInput);
            var validPhoneInput = Extension.Validator.ValidatePhoneNumber(_accountInput);

            _validAccountInput = false;

            if (!_validEmailInput && !validPhoneInput)
            {
                _accountMessage.SetText("Email or Phone number is not valid!");
                return;
            }

            _accountMessage.SetText(string.Empty);
            _validAccountInput = true;

            // Validate if account is existed.
        }

        private void OnValueChanged_PasswordInputField(ChangeEvent<string> evt)
        {
            _passwordInput = evt.newValue;

            if (_passwordInput == string.Empty) return;

            var valid8character = _passwordInput.Length >= 8;
            var valid1number = Regex.IsMatch(_passwordInput, @"\d");
            var valid1special = Regex.IsMatch(_passwordInput, @"[!@#$%^&*(),.?\:{ }|<>]");

            _8charactersMessage.SetColor(valid8character ? "#5FFF9F" : "#FF5F5F");
            _oneNumberMessage.SetColor(valid1number ? "#5FFF9F" : "#FF5F5F");
            _oneSpecialCharacterMessage.SetColor(valid1special ? "#5FFF9F" : "#FF5F5F");

            _validPasswordInput = false;

            if (!valid8character && !valid1number && !valid1special)
            {
                _passwordMessage.SetText("Password must meet all the requirements above.");
                return;
            }

            string[] accountParts = Regex.Split(_accountInput, @"[@.]+");
            foreach (string part in accountParts)
            {
                if (!string.IsNullOrWhiteSpace(part) && _passwordInput.Contains(part, StringComparison.OrdinalIgnoreCase))
                {
                    _passwordMessage.SetText("Your password cannot be the same as your username or email.");
                    _validPasswordInput = false;
                    return;
                }
            }

            _passwordMessage.SetText(string.Empty);
            _validPasswordInput = true;
        }

        private void OnValueChanged_ConfirmInputField(ChangeEvent<string> evt)
        {
            var input = evt.newValue;

            if (input == string.Empty) return;

            var isMatchWithPassword = input == _passwordInput;

            _validConfirmInput = false;

            if (!isMatchWithPassword)
            {
                _confirmMessage.SetText("Passwords do not match");
                return;
            }

            _confirmMessage.SetText(string.Empty);
            _validConfirmInput = true;
        }

        private void SigningWithFacebook(PointerDownEvent evt)
        {

        }

        private void SignInWithGoogle(PointerDownEvent evt)
        {

        }

        private void SigningAccount()
        {
            var account = new Account();
            if (_validEmailInput) account.Email = _accountInput;
            else account.PhoneNumber = _accountInput;
            account.Password = _passwordInput;

            //Main.Database.Accounts.Add(account);

            //Marker.OnViewPageSwitched?.Invoke(ViewType.MainView, ViewKey.MainViewHomePage, true);
        }

        private void RecoveryAccount(PointerDownEvent evt)
        {

        }
    }
}
