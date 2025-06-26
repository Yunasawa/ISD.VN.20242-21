using System.Collections.Generic;

namespace YNL.JAMOS
{
    public partial class SearchSort
    {
        private class Controller : PageController
        {
            private SearchSort _b;

            private SortType _selectedSortType = SortType.ByTitleAToZ;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as SearchSort;

                _b.OnApplyButtonClicked += OnApplyButtonClicked;
                SortItem.OnSelected += OnSortItemSelected;
            }

            private void OnApplyButtonClicked()
            {
                Marker.OnSearchResultSorted?.Invoke(_selectedSortType);
            }

            private void OnSortItemSelected(SortType type)
            {
                _selectedSortType = type;
            }
        }
    }
}