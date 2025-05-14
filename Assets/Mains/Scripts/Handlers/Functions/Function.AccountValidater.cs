using UnityEngine;

namespace YNL.Checkotel
{
    public partial class Function
    {
        public class AccountValidater
        {
            private static Account? GetAccount(AccountVerificationType type, string input)
            {
                return type == AccountVerificationType.Email
                    ? DataContainer.Accounts?.Find(i => i.Email == input)
                    : DataContainer.Accounts?.Find(i => i.PhoneNumber == input);
            }
            private static Account? GetAccount(UID id)
            {
                return DataContainer.Accounts?.Find(i => i.ID == id);
            }

            public static void SignAccount(AccountSignType signType, AccountVerificationType verificationType, string input, string password, string? confirmedPassword = null)
            {
                Account? account = GetAccount(verificationType, input);

                if (verificationType == AccountVerificationType.Email)
                {
                    if (!Extension.Validator.ValidateEmail(input))
                    {
                        Marker.OnAccountVerificated?.Invoke(signType == AccountSignType.SignUp
                            ? AccountVerificationResult.SignUpEmailNotValid
                            : AccountVerificationResult.SignInEmailNotValid);

                        return;
                    }
                }
                else if (verificationType == AccountVerificationType.PhoneNumber)
                {
                    if (!Extension.Validator.ValidatePhoneNumber(input))
                    {
                        Marker.OnAccountVerificated?.Invoke(signType == AccountSignType.SignUp
                            ? AccountVerificationResult.SignUpPhoneNumberNotValid
                            : AccountVerificationResult.SignInPhoneNumberNotValid);

                        return;
                    }
                }

                if (signType == AccountSignType.SignUp)
                {
                    if (account != null)
                    {
                        Marker.OnAccountVerificated?.Invoke(verificationType == AccountVerificationType.Email
                            ? AccountVerificationResult.SignUpEmailHasExisted
                            : AccountVerificationResult.SignUpPhoneNumberHasExisted);

                        return;
                    }
                    else if (password != confirmedPassword)
                    {
                        Marker.OnAccountVerificated?.Invoke(AccountVerificationResult.SignUpPasswordNotMatch);

                        return;
                    }

                    Debug.Log("Signing up may pass!");
                }
                else if (signType == AccountSignType.SignIn)
                {
                    if (account == null)
                    {
                        Marker.OnAccountVerificated?.Invoke(AccountVerificationResult.SignInAccountNotFound);

                        return;
                    }
                    else if (account.Password != password)
                    {
                        Marker.OnAccountVerificated?.Invoke(AccountVerificationResult.SignInPasswordNotCorrect);

                        return;
                    }

                    Debug.Log($"Account ID: {account.ID}");
                }
            }

            public static void DeleteAccount(UID id)
            {
                Account? account = GetAccount(id);

                if (account == null)
                {
                    Marker.OnAccountDeleted?.Invoke(AccountDeletionResult.AccountNotFound);

                    return;
                }

                DataContainer.Accounts?.Remove(account);
                Marker.OnAccountDeleted?.Invoke(AccountDeletionResult.AccountDeletionSuccess);

                Debug.Log($"Account with ID {id} has been deleted.");
            }
        }
    }
}
