using UnityEngine;
using UnityEngine.UIElements;

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
        }

        private void OnValueChanged_AccountInputField(ChangeEvent<string> evt)
        {

        }

        private void OnValueChanged_PasswordInputField(ChangeEvent<string> evt)
        {

        }

        private void OnValueChanged_ConfirmInputField(ChangeEvent<string> evt)
        {

        }

        private void SigningWithFacebook(PointerDownEvent evt)
        {

        }

        private void SignInWithGoogle(PointerDownEvent evt)
        {

        }

        private void SigningAccount()
        {

        }

        private void RecoveryAccount(PointerDownEvent evt)
        {

        }
    }
}
