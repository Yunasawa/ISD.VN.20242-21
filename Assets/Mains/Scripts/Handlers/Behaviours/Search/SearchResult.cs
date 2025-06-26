using System;
using System.Collections.Generic;
using UnityEngine;

namespace YNL.JAMOS
{
    public partial class SearchResult : PageBehaviour
    {
        public Action<Product.Type, string> OnResultPageRefreshed;
        public Action<List<UID>> OnSearchResultDisplayed;

        [SerializeField] private SearchSort _sortPage;
        [SerializeField] private SearchFilter _filterPage;

        private View _view;
        private Controller _controller;

        protected override void Construct()
        {
            RegisterView(_view = ViewFactory.CreateView<View>(Root, this));
            RegisterController(_controller = ViewFactory.CreateController<Controller>(this));
        }

        protected override void Begin()
        {
            base.Begin();

            _view.SetPopupPage(_sortPage, _filterPage);
        }
    }
}