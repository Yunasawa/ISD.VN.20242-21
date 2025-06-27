using System;
using System.Collections.Generic;

namespace YNL.JAMOS
{
    public partial class ManagerChat : PageBehaviour
    {
        public Action<string> OnNewMessageRequested;
        public Action<List<MessageItem>> OnMessageListDisplayed;

        private View _view;
        private Controller _controller;

        protected override void Construct()
        {
            RegisterView(_view = ViewFactory.CreateView<View>(Root, this));
            RegisterController(_controller = ViewFactory.CreateController<Controller>(this));
        }
    }
}