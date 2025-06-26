using System;

namespace YNL.JAMOS
{
    public enum SortType : byte
    {
        ByTitleAToZ,
        ByTitleZToA,
        NewestReleaseDate,
        OldestReleaseDate,
        MostPopular,
        LeastPopular,
        HighestRating,
        LowestRating,
        HighestPrice,
        LowestPrice,
        LongestDuration,
        ShortestDuration
    }

    public partial class SearchSort : PageBehaviour
    {
        public Action<bool> OnViewOpenRequested;
        public Action OnApplyButtonClicked;

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