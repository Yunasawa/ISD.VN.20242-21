using System;

namespace YNL.JAMOS
{
    public partial class SigningSigningIn : PageBehaviour
    {
        public Action<string> OnAccountFieldFilled;
        public Action<string> OnPasswordFieldFilled;
        public Action OnSignInRequested;
        public Action OnAccountMessageNotified;
        public Action OnPasswordMessageNotified;

        private View _view;
        private Controller _controller;

        protected override void Construct()
        {
            RegisterView(_view = ViewFactory.CreateView<View>(Root, this));
            RegisterController(_controller = ViewFactory.CreateController<Controller>(this));
        }
    }
}