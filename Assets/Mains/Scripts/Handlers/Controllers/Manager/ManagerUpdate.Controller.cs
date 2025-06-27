using System.Globalization;
using System;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using System.Linq;

namespace YNL.JAMOS
{
    public partial class ManagerUpdate
    {
        private class Controller : PageController
        {
            private SerializableDictionary<UID, Product.Data> _products => Main.Database.Products;

            private ManagerUpdate _b;

            private ProductWrapper _productData = new();
            private UID _productID;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerUpdate;

                _b.OnPropertyValueChanged += OnPropertyValueChanged;
                _b.OnGenreValueChanged += OnGenreValueChanged;
                _b.OnProductUpdateApplied += OnProductUpdateApplied;
                Marker.OnProductUpdatingRequested += OnProductUpdatingRequested;
            }

            ~Controller()
            {
                Marker.OnProductUpdatingRequested -= OnProductUpdatingRequested;
            }

            public override void Refresh()
            {
                if (string.IsNullOrEmpty(_productID)) return;
                if (_products.TryGetValue(_productID, out var product) == false) return;

                _productData.ProductName = product.Title;
                _productData.ProductCreators = string.Join(", ", product.Creators);
                _productData.ProductType = product.Type;
                _productData.ProductGenres = product.Type.ToGenreBitmask(product.Genres);
                _productData.PublicationDate = product.PublicationDate.Value;
                _productData.ProductDescription = product.Description;
                _productData.ProductPrice = product.Price;
                _productData.ProductStock = product.Quantity;

                _productData.PropertyInputs.Clear();
                foreach (var kvp in product.Properties)
                {
                    _productData.PropertyInputs[kvp.Key] = kvp.Value;
                }

                _b.OnDataFieldReseted?.Invoke(_productData);
            }

            private void OnProductUpdatingRequested(UID productID)
            {
                _productID = productID;
            }

            private void OnGenreValueChanged(bool isSelected, ushort genre)
            {
                if (isSelected)
                {
                    _productData.ProductGenres |= genre;
                }
                else
                {
                    _productData.ProductGenres &= (ushort)~genre;
                }
            }

            public void OnPropertyValueChanged(string type, string value)
            {
                switch (type)
                {
                    case "Name": _productData.ProductName = value; break;
                    case "Creators": _productData.ProductCreators = value; break;
                    case "Type": _productData.ProductType = (Product.Type)Enum.Parse(typeof(Product.Type), value); break;
                    case "Description": _productData.ProductDescription = value; break;
                    case "Price": _productData.ProductPrice = float.Parse(value); break;
                    case "Stock": _productData.ProductStock = int.Parse(value); break;
                    case "PublicationDate": _productData.PublicationDate = value; break;
                    default:
                        if (Enum.TryParse(type, out Product.Property property))
                        {
                            _productData.PropertyInputs[property] = value;
                        }
                        else
                        {
                            throw new ArgumentException($"Unknown property type: {type}");
                        }
                        break;
                }
            }

            private void OnProductUpdateApplied()
            {
                var productData = _products[_productID];
                productData.Type = _productData.ProductType;
                productData.Title = _productData.ProductName;
                productData.Genres = _productData.ProductType.GetProductGenresString(_productData.ProductGenres);
                productData.Creators = _productData.ProductCreators.Split(",").Select(s => s.Trim()).ToArray();
                productData.PublicationDate = new(DateTime.ParseExact(_productData.PublicationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                productData.Description = _productData.ProductDescription.Replace("\r\n", "#");
                productData.Price = _productData.ProductPrice;
                productData.Quantity = (ushort)_productData.ProductStock;

                foreach (var property in _productData.PropertyInputs)
                {
                    productData.Properties[property.Key] = property.Value;
                }

                Marker.OnPageNavigated?.Invoke(ViewType.ManagerViewInformationPage, true, true);
            }
        }
    }
}