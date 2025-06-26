using System;
using System.Collections.Generic;
namespace YNL.JAMOS
{
    public enum SearchingSuggestionType : byte { Creator, Book, CD, DVD, LP }

    public partial class SearchMain : PageBehaviour
    {
        public Action<string> OnSearchInputChanged;
        public Action<string> OnSearchInputApplied;
        public Action OnSearchButtonClicked;
        public Action<List<(SearchingSuggestionType type, string value)>> OnSuggestionDisplayed;

        public Action OnEmptySearchingInputNotified;
        public Action OnNoSearchingMatchNotified;
        public Action<bool> OnEmptySuggestionDisplayed;

        private View _view;
        private Controller _controller;

        protected override void Construct()
        {
            RegisterView(_view = ViewFactory.CreateView<View>(Root, this));
            RegisterController(_controller = ViewFactory.CreateController<Controller>(this));
        }
    }
}