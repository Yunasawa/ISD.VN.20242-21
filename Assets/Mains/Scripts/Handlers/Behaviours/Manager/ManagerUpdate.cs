using System;
using UnityEngine;

namespace YNL.JAMOS
{
    public partial class ManagerUpdate : PageBehaviour
    {
        public Action<string, string> OnPropertyValueChanged;
        public Action<bool, ushort> OnGenreValueChanged;
        public Action<ProductWrapper> OnDataFieldReseted;
        public Action OnProductUpdateApplied;

        private View _view;
        private Controller _controller;

        protected override void Construct()
        {
            RegisterView(_view = ViewFactory.CreateView<View>(Root, this));
            RegisterController(_controller = ViewFactory.CreateController<Controller>(this));
        }
    }
}