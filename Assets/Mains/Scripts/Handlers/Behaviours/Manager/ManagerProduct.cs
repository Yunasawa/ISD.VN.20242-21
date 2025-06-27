using System;

namespace YNL.JAMOS
{
    public partial class ManagerProduct : PageBehaviour
    {
        public Action<UID[]> OnProductListDisplayed;

        private View _view;
        private Controller _controller;

        protected override void Construct()
        {
            RegisterView(_view = ViewFactory.CreateView<View>(Root, this));
            RegisterController(_controller = ViewFactory.CreateController<Controller>(this));
        }
    }
}