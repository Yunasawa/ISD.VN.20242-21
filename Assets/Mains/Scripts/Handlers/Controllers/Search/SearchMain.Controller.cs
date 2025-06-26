using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.Extensions;

namespace YNL.JAMOS
{
    public partial class SearchMain
    {
        private class Controller : PageController
        {
            private SerializableDictionary<UID, Product.Data> _products => Main.Database.Products;

            private SearchMain _b;

            private List<(SearchingSuggestionType type, string value)> _suggestionValues = new();
            private string _searchingInput;
            private Product.Type _searchingProductType;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as SearchMain;

                _b.OnSearchButtonClicked += OnSearchButtonClicked;
                _b.OnSearchInputChanged += OnSearchInputChanged;
                _b.OnSearchInputApplied += OnSearchInputApplied;
                ProductTypeButtonUI.OnSelected += OnProductTypeSelected;
            }

            private void OnSearchInputApplied(string value)
            {
                _searchingInput = value;
            }

            private void OnSearchInputChanged(string value)
            {
                _searchingInput = value;

                _suggestionValues.Clear();

                if (value == string.Empty)
                {
                    _suggestionValues = Main.Runtime.Data.SearchingHistory;

                    _b.OnEmptySearchingInputNotified?.Invoke();
                }
                else
                {
                    _b.OnNoSearchingMatchNotified?.Invoke();

                    foreach (var product in _products.Values)
                    {
                        if (_searchingProductType != Product.Type.None && _searchingProductType != product.Type)
                        {
                            continue;
                        }

                        if (product.Title.FuzzyContains(value))
                        {
                            _suggestionValues.Add((product.Type.ToProductType(), product.Title));
                        }

                        foreach (var creator in product.Creators.Where(c => c.FuzzyContains(value)))
                        {
                            _suggestionValues.Add((SearchingSuggestionType.Creator, creator));
                        }
                    }
                }

                bool emptyResult = _suggestionValues.IsEmpty();

                _b.OnEmptySuggestionDisplayed?.Invoke(emptyResult);

                if (!emptyResult)
                {
                    _suggestionValues.Sort();
                    _b.OnSuggestionDisplayed?.Invoke(_suggestionValues);
                }
            }

            private void OnSearchButtonClicked()
            {
                Marker.OnSearchingInputEntered?.Invoke(_searchingInput, _searchingProductType);
                Marker.OnPageNavigated?.Invoke(ViewType.SearchViewResultPage, true, true);
            }

            private void OnProductTypeSelected(Product.Type type)
            {
                _searchingProductType = type;
            }
        }
    }
}