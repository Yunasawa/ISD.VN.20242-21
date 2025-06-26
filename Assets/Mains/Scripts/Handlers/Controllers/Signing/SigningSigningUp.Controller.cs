using Codice.Client.BaseCommands;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace YNL.JAMOS
{
    public partial class SigningSigningUp
    {
        private class Controller : PageController
        {
            private SigningSigningUp _b;

            private bool _validEmailInput;
            private string _accountInput;
            private string _passwordInput;
            private bool _validAccountInput;
            private bool _validPasswordInput;
            private bool _validConfirmInput;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as SigningSigningUp;

                _b.OnAccountInputChanged += OnAccountInputChanged;
                _b.OnPasswordInputChanged += OnPasswordInputChanged;
                _b.OnConfirmInputChanged += OnConfirmInputChanged;
                _b.OnSignUpRequested += OnSignUpRequested;
            }

            private void OnAccountInputChanged(string value)
            {
                _accountInput = value;

                if (_accountInput == string.Empty) return;

                _validEmailInput = Extension.Validator.ValidateEmail(_accountInput);
                var validPhoneInput = Extension.Validator.ValidatePhoneNumber(_accountInput);

                _validAccountInput = false;

                if (!_validEmailInput && !validPhoneInput)
                {
                    _b.OnAccountMessageNotified?.Invoke("Email or Phone number is not valid!");
                    return;
                }

                var account = Main.Database.Accounts.Values.FirstOrDefault(i => i.Email == _accountInput || i.PhoneNumber == _accountInput);

                if (account != null)
                {
                    _b.OnAccountMessageNotified?.Invoke("This email or phone number is registered by another account!");
                    return;
                }

                _b.OnAccountMessageNotified?.Invoke(string.Empty);
                _validAccountInput = true;
            }
        
            private void OnPasswordInputChanged(string value)
            {
                _passwordInput = value;

                if (_passwordInput == string.Empty) return;

                var valid8character = _passwordInput.Length >= 8;
                var valid1number = Regex.IsMatch(_passwordInput, @"\d");
                var valid1special = Regex.IsMatch(_passwordInput, @"[!@#$%^&*(),.?\:{ }|<>]");

                _validPasswordInput = false;

                if (!valid8character && !valid1number && !valid1special)
                {
                    _b.OnPasswordMessageNotified?.Invoke("Password must meet all the requirements above.");
                    return;
                }

                string[] accountParts = Regex.Split(_accountInput, @"[@.]+");
                foreach (string part in accountParts)
                {
                    if (!string.IsNullOrWhiteSpace(part) && _passwordInput.Contains(part, StringComparison.OrdinalIgnoreCase))
                    {
                        _b.OnPasswordMessageNotified?.Invoke("Your password cannot be the same as your username or email.");
                        _validPasswordInput = false;
                        return;
                    }
                }

                _b.OnPasswordMessageNotified?.Invoke(string.Empty);
                _validPasswordInput = true;
            }

            private void OnConfirmInputChanged(string value)
            {
                if (value == string.Empty) return;

                var isMatchWithPassword = value == _passwordInput;

                _validConfirmInput = false;

                if (!isMatchWithPassword)
                {
                    _b.OnConfirmMessageNotified?.Invoke("Passwords do not match");
                    return;
                }

                _b.OnPasswordMessageNotified?.Invoke(string.Empty);
                _validConfirmInput = true;
            }

            private void OnSignUpRequested()
            {
                var account = new Account();
                account.SetData();
                if (_validEmailInput) account.Email = _accountInput;
                else account.PhoneNumber = _accountInput;
                account.Password = _passwordInput;
                Main.Database.Accounts.Add(account.ID, account);
                Main.Runtime.Data.AccountID = account.ID;

                Marker.OnRuntimeSavingRequested?.Invoke();
                Marker.OnPageNavigated?.Invoke(ViewType.SigningViewPreferencePage, true, false);
                Marker.OnSignedInOrSignedUp?.Invoke();
            }
        }
    }
}