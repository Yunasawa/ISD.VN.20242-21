using System.Linq;
using UnityEngine.UIElements;
using YNL.Utilities.Extensions;

namespace YNL.JAMOS
{
    public partial class SigningSigningIn
    {
        private class Controller : PageController
        {
            private SigningSigningIn _b;

            private string _accountInput;
            private string _passwordInput;
            private bool _validAccountInput;
            private bool _validPasswordInput;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as SigningSigningIn;

                _b.OnAccountFieldFilled += OnAccountFieldFilled;
                _b.OnPasswordFieldFilled += OnPasswordFieldFilled;
                _b.OnSignInRequested += OnSignInRequested;
            }

            public override void Begin()
            {
                if (string.IsNullOrEmpty(Main.Runtime.Data.AccountID) == false)
                {
                    var account = Main.Database.Accounts[Main.Runtime.Data.AccountID];

                    if (account.Type == AccountType.Customer)
                    {
                        Marker.OnPageNavigated?.Invoke(ViewType.MainViewHomePage, true, true);
                    }
                    else
                    {
                        Marker.OnPageNavigated?.Invoke(ViewType.ManagerViewProductPage, true, true);
                    }

                    Marker.OnSignedInOrSignedUp?.Invoke();
                }

                Marker.OnClosingStartingPageRequested?.Invoke();
            }

            private void OnAccountFieldFilled(string value)
            {
                _accountInput = value;
            }

            private void OnPasswordFieldFilled(string value)
            {
                _passwordInput = value;
            }

            private void OnSignInRequested()
            {
                var existedEmailAccount = Main.Database.Accounts.Any(i => i.Value.Email == _accountInput);
                var existedPhoneAccount = Main.Database.Accounts.Any(i => i.Value.PhoneNumber == _accountInput);

                if (!existedEmailAccount && !existedPhoneAccount)
                {
                    _b.OnAccountMessageNotified?.Invoke();
                    return;
                }

                var account = Main.Database.Accounts.Values.FirstOrDefault(i => i.Email == _accountInput || i.PhoneNumber == _accountInput);
                var id = Main.Database.Accounts.GetKeyByValue(account);

                if (_passwordInput != account.Password)
                {
                    _b.OnPasswordMessageNotified?.Invoke();
                    return;
                }

                Main.Runtime.Data.AccountID = id;
                Marker.OnRuntimeSavingRequested?.Invoke();

                if (account.Type == AccountType.Customer)
                {
                    Marker.OnPageNavigated?.Invoke(ViewType.MainViewHomePage, true, true);
                }
                else
                {
                    Marker.OnPageNavigated?.Invoke(ViewType.ManagerViewProductPage, true, true);
                }

                Marker.OnSignedInOrSignedUp?.Invoke();
            }
        }
    }
}