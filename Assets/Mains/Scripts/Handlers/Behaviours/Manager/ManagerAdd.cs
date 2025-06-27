using System;

namespace YNL.JAMOS
{
    public partial class ManagerAdd : PageBehaviour
    {
        public Action<string, string> OnPropertyValueChanged;
        public Action<bool, ushort> OnGenreValueChanged;
        public Action OnAddButtonClicked;
        public Action<ProductWrapper> OnDataFieldReseted;

        private View _view;
        private Controller _controller;

        protected override void Construct()
        {
            RegisterView(_view = ViewFactory.CreateView<View>(Root, this));
            RegisterController(_controller = ViewFactory.CreateController<Controller>(this));
        }
    }
}