using System;
using System.Collections.Generic;

namespace YNL.JAMOS
{
    public partial class InformationReview : PageBehaviour
    {
        public Action<UID, Product.Data, List<UID>> OnDataRefreshed;

        private View _view;
        private Controller _controller;

        protected override void Construct()
        {
            RegisterView(_view = ViewFactory.CreateView<View>(Root, this));
            RegisterController(_controller = ViewFactory.CreateController<Controller>(this));
        }
    }
}