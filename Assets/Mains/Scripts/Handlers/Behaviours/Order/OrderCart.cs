using System;
using System.Collections.Generic;

namespace YNL.JAMOS
{
    public partial class OrderCart : PageBehaviour
    {
        public Action<List<UID>> OnCartListDisplayed;
        public Action<float> OnCartItemAmountAdjusted;

        private View _view;
        private Controller _controller;

        protected override void Construct()
        {
            RegisterView(_view = ViewFactory.CreateView<View>(Root, this));
            RegisterController(_controller = ViewFactory.CreateController<Controller>(this));
        }
    }
}