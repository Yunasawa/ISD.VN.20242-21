using System;

namespace YNL.JAMOS
{
    public partial class SigningSigningUp : PageBehaviour
    {
        public Action<string> OnAccountInputChanged;
        public Action<string> OnAccountMessageNotified;
        public Action<string> OnPasswordInputChanged;
        public Action<string> OnPasswordMessageNotified;
        public Action<string> OnConfirmInputChanged;
        public Action<string> OnConfirmMessageNotified;
        public Action OnSignUpRequested;

        private View _view;
        private Controller _controller;

        protected override void Construct()
        {
            RegisterView(_view = ViewFactory.CreateView<View>(Root, this));
            RegisterController(_controller = ViewFactory.CreateController<Controller>(this));
        }
    }
}