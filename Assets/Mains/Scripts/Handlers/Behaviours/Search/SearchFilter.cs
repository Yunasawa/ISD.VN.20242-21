using System;
using System.Collections.Generic;
using UnityEngine;

namespace YNL.JAMOS
{
    public partial class SearchFilter : PageBehaviour
    {
        public Action<bool> OnViewOpenRequested;
        public Action OnApplyButtonClicked;
        public Action<string[], List<string>> OnGenreListDisplayed;
        public Action<Vector2> OnPriceRangeChanged;
        public Action<int, int> OnPriceRangeDisplayed;

        private View _view;
        private Controller _controller;

        protected override void Construct()
        {
            RegisterView(_view = ViewFactory.CreateView<View>(Root, this));
            RegisterController(_controller = ViewFactory.CreateController<Controller>(this));
        }

        public override void OnPageOpened(bool isOpen, bool needRefresh = true)
        {
            OnViewOpenRequested?.Invoke(isOpen);

            if (isOpen && needRefresh) Refresh();
        } 
    }
}