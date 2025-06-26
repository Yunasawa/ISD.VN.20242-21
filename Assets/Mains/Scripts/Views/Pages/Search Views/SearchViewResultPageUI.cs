using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class SearchViewResultPageUI : PageBehaviour
    {
        [SerializeField] private SearchViewSortPageUI _sortPage;
        [SerializeField] private SearchFilter _filterPage;

        private SerializableDictionary<UID, Product.Data> _products => Main.Database.Products;

        private Label _searchText;
        private VisualElement _typeIcon;
        private ListView _resultList;

        private string _searchingInput;
        private Product.Type _searchingProductType;
        private List<UID> _originalResults = new();
        private List<UID> _filteredResults = new();

        protected override void VirtualAwake()
        {
            Marker.OnGenreSearchRequested += OnGenreSearchRequested;
            Marker.OnSearchingInputEntered += OnSearchingInputEntered;
            Marker.OnSearchResultSorted +=   OnSearchResultSorted;
            Marker.OnSearchResultFiltered += OnSearchResultFiltered;
        }

        private void OnDestroy()
        {
            Marker.OnGenreSearchRequested -= OnGenreSearchRequested;
            Marker.OnSearchingInputEntered -= OnSearchingInputEntered;
            Marker.OnSearchResultSorted -= OnSearchResultSorted;
            Marker.OnSearchResultFiltered -= OnSearchResultFiltered;
        }

        protected override void Construct()
        {
            var resultPage = Root.Q("SearchingResultPage");

            var searchBar = resultPage.Q("SearchBar");
            searchBar.RegisterCallback<PointerUpEvent>(OnClicked_SearchBar);

            _searchText = searchBar.Q("SearchField").Q("SearchText") as Label;

            _typeIcon = searchBar.Q("SearchField").Q("TypeIcon");

            var toolBar = resultPage.Q("ToolBar");

            var sortButton = toolBar.Q("SortingButton");
            sortButton.RegisterCallback<PointerUpEvent>(OnClicked_SortButton);

            var filterButton = toolBar.Q("FilteringButton");
            filterButton.RegisterCallback<PointerUpEvent>(OnClicked_FilterButton);

            var resultScroll = resultPage.Q("ResultScroll");
            resultPage.Remove(resultScroll);

            _resultList = resultPage.Q("ResultList") as ListView;
            _resultList.Q("unity-content-container").SetFlexGrow(1);
            _resultList.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
            _resultList.itemsSource = _filteredResults;
            _resultList.makeItem = () => new SearchingResultItemUI();
            _resultList.bindItem = (element, index) =>
            {
                var item = element as SearchingResultItemUI;
                if (item != null)
                {
                    item.Apply(_filteredResults[index]);
                }
            };
        }

        protected override void Refresh()
        {
            _typeIcon.SetBackgroundImage(Main.Resources.Icons[_searchingProductType.ToString()]);

            var searchInput = string.IsNullOrEmpty(_searchingInput) ? "Anything" : _searchingInput;
            _searchText.SetText(searchInput);

            _originalResults.Clear();
            _filteredResults.Clear();

            foreach (var product in _products)
            {
                if (_searchingProductType != Product.Type.None && _searchingProductType != product.Value.Type)
                {
                    continue;
                }

                if ((product.Value.Title.FuzzyContains(_searchingInput) ||
                    product.Value.Creators.Any(i => i.FuzzyContains(_searchingInput))))
                {
                    _originalResults.Add(product.Key);
                    _filteredResults.Add(product.Key);
                }
            }

            _resultList.RebuildListView(_filteredResults);
        }

        private void OnClicked_SearchBar(PointerUpEvent evt)
        {
            Marker.OnPageNavigated?.Invoke(ViewType.SearchViewMainPage, true, false);
        }

        private void OnClicked_SortButton(PointerUpEvent evt)
        {
            _sortPage.OnPageOpened(true, false);
        }

        private void OnClicked_FilterButton(PointerUpEvent evt)
        {
            _filterPage.OnPageOpened(true, false);
        }

        private void OnGenreSearchRequested(string genre)
        {
            _typeIcon.SetBackgroundImage(Main.Resources.Icons["None"]);
            _searchText.SetText($"<color=#a0a0a0>Genre:</color> <b>{genre.AddSpaces()}</b>");

            _originalResults = _filteredResults = _products.Where(i => i.Value.Genres.Contains(genre)).Select(i => i.Key).ToList();
            _resultList.RebuildListView(_filteredResults);
        }
    
        private void OnSearchingInputEntered(string input, Product.Type type)
        {
            _searchingInput = input;
            _searchingProductType = type;
        }

        private void OnSearchResultSorted(SortType type)
        {
            _filteredResults = _originalResults.GetSortedItemList(type);
            _resultList.RebuildListView(_filteredResults);
        }

        private void OnSearchResultFiltered(MRange priceRange, Product.Type productType, RatingScoreType ratingScore, List<string> genres)
        {
            _filteredResults.Clear();

            foreach (var id in _originalResults)
            {
                var product = Main.Database.Products[id];
                var rating = product.Review.AverageTotalRating;

                if (product.LastPrice < priceRange.Min || product.LastPrice > priceRange.Max)
                {
                    continue;
                }
                if (productType != Product.Type.None && product.Type != productType)
                {
                    continue;
                }
                if (ratingScore switch
                {
                    RatingScoreType.Any => false,
                    RatingScoreType.GE45 => rating < 4.5f,
                    RatingScoreType.GE40 => rating < 4.0f,
                    RatingScoreType.GE35 => rating < 3.5f,
                    _ => true
                })
                {
                    continue;
                }
                if (genres.Count != 0 && genres.Any(i => product.Genres.Contains(i)) == false)
                {
                    continue;
                }

                _filteredResults.Add(id);
            }

            _resultList.RebuildListView(_filteredResults);
        }
    }
}   