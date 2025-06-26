using System.Collections.Generic;
using System.Linq;
using UnityEngine.Analytics;
using YNL.Utilities.Addons;

namespace YNL.JAMOS
{
    public partial class SearchResult
    {
        private class Controller : PageController
        {
            private SerializableDictionary<UID, Product.Data> _products => Main.Database.Products;

            private SearchResult _b;

            private string _searchingInput;
            private Product.Type _searchingProductType;
            private List<UID> _originalResults = new();
            private List<UID> _filteredResults = new();

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as SearchResult;

                Marker.OnGenreSearchRequested += OnGenreSearchRequested;
                Marker.OnSearchingInputEntered += OnSearchingInputEntered;
                Marker.OnSearchResultSorted += OnSearchResultSorted;
                Marker.OnSearchResultFiltered += OnSearchResultFiltered;
            }

            ~Controller()
            {
                Marker.OnGenreSearchRequested -= OnGenreSearchRequested;
                Marker.OnSearchingInputEntered -= OnSearchingInputEntered;
                Marker.OnSearchResultSorted -= OnSearchResultSorted;
                Marker.OnSearchResultFiltered -= OnSearchResultFiltered;
            }

            public override void Refresh()
            {
                var searchInput = string.IsNullOrEmpty(_searchingInput) ? "Anything" : _searchingInput;

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

                _b.OnResultPageRefreshed?.Invoke(_searchingProductType, searchInput);

                _b.OnSearchResultDisplayed.Invoke(_filteredResults);
            }

            private void OnGenreSearchRequested(string genre)
            {
                _originalResults = _filteredResults = _products.Where(i => i.Value.Genres.Contains(genre)).Select(i => i.Key).ToList();

                _b.OnSearchResultDisplayed.Invoke(_filteredResults);
            }

            private void OnSearchingInputEntered(string input, Product.Type type)
            {
                _searchingInput = input;
                _searchingProductType = type;
            }

            private void OnSearchResultSorted(SortType type)
            {
                _filteredResults = _originalResults.GetSortedItemList(type);

                _b.OnSearchResultDisplayed.Invoke(_filteredResults);
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

                _b.OnSearchResultDisplayed.Invoke(_filteredResults);
            }
        }
    }
}