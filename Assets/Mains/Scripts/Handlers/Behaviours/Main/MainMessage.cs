using System;
using System.Collections.Generic;

namespace YNL.JAMOS
{
    public partial class MainMessage : PageBehaviour
    {
        public Action<List<MessageItem>> OnMessageListRefreshed;
        public Action<string> OnNewMessageSent;

        private View _view;
        private Controller _controller;

        protected override void Construct()
        {
            RegisterView(_view = ViewFactory.CreateView<View>(Root, this));
            RegisterController(_controller = ViewFactory.CreateController<Controller>(this));
        }
    }
}