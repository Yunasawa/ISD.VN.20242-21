using System.Collections.Generic;
using UnityEngine;
using YNL.Utilities.Addons;
using YNL.Utilities.Extensions;

namespace YNL.JAMOS
{
    public partial class SearchFilter
    {
        private class Controller : PageController
        {
            private readonly (int Min, int Max) _priceRange = (0, 100);

            private SearchFilter _b;

            private Product.Type _selectedProductType;

            private List<string> _filteredGenres = new();
            private MRange _filteredPriceRange = new(0, 1000);
            private RatingScoreType _filteredRatingScore = RatingScoreType.GE45;
            private Product.Type _filteredProductType = Product.Type.None;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as SearchFilter;

                _b.OnApplyButtonClicked += OnApplyButtonClicked;
                _b.OnPriceRangeChanged += OnPriceRangeChanged;
                ProductTypeItem.OnSelected += OnProductTypeItemSelected;
                RatingScoreItem.OnSelected += OnRatingScoreItemSelected;
            }

            ~Controller()
            {
                ProductTypeItem.OnSelected -= OnProductTypeItemSelected;
                RatingScoreItem.OnSelected -= OnRatingScoreItemSelected;
            }

            public override void Begin()
            {
                OnGenreListRequested();
            }

            private void OnApplyButtonClicked()
            {
                Marker.OnSearchResultFiltered?.Invoke(_filteredPriceRange, _filteredProductType, _filteredRatingScore, _filteredGenres);
            }

            private void OnGenreListRequested()
            {
                var genreString = _selectedProductType.GetAllProductGenreString();

                _b.OnGenreListDisplayed?.Invoke(genreString, _filteredGenres);
            }

            private void OnProductTypeItemSelected(Product.Type type)
            {
                _filteredProductType = _selectedProductType = type;

                OnGenreListRequested();
            }

            private void OnRatingScoreItemSelected(RatingScoreType type)
            {
                _filteredRatingScore = type;
            }

            private void OnPriceRangeChanged(Vector2 value)
            {
                (float min, float max) ratio = (value.x, value.y);

                int minPrice = Mathf.RoundToInt(ratio.min.Remap(new(0, 1), new(_priceRange.Min, _priceRange.Max)));
                int maxPrice = Mathf.RoundToInt(ratio.max.Remap(new(0, 1), new(_priceRange.Min, _priceRange.Max)));

                _filteredPriceRange.Min = minPrice;
                _filteredPriceRange.Max = maxPrice;

                _b.OnPriceRangeDisplayed?.Invoke(minPrice, maxPrice);
            }
        }
    }
}