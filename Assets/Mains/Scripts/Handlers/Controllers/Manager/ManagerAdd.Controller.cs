using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using YNL.Utilities.Addons;

namespace YNL.JAMOS
{
    public partial class ManagerAdd
    {
        public class ProductWrapper
        {
            public string ProductName;
            public string ProductCreators;
            public Product.Type ProductType;
            public ushort ProductGenres;
            public string ProductDescription;
            public float ProductPrice;
            public int ProductStock;
            public string PublicationDate;
            public Dictionary<Product.Property, string> PropertyInputs = new();

            public void Reset()
            {
                ProductName = string.Empty;
                ProductCreators = string.Empty;
                ProductType = default;
                ProductGenres = 0;
                ProductDescription = string.Empty;
                ProductPrice = 0f;
                ProductStock = 0;
                PublicationDate = string.Empty;
                PropertyInputs.Clear();
            }
        }

        private class Controller : PageController
        {
            private SerializableDictionary<UID, Product.Data> _products => Main.Database.Products;
            private SerializableDictionary<UID, Product.Data> _createdProducts => Main.Runtime.Data.CreatedProducts;

            private ManagerAdd _b;

            private ProductWrapper _productData = new();

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerAdd ;

                _b.OnPropertyValueChanged += OnPropertyValueChanged;
                _b.OnGenreValueChanged += OnGenreValueChanged;
                _b.OnAddButtonClicked += OnAddButtonClicked;
            }

            public override void Begin()
            {
                _productData.Reset();
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

            public void OnGenreValueChanged(bool isSelected, ushort genre)
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
        
            public void OnAddButtonClicked()
            {
                var productData = new Product.Data();
                productData.Type = _productData.ProductType;
                productData.Title = _productData.ProductName;
                productData.Genres = _productData.ProductType.GetProductGenresString(_productData.ProductGenres);
                productData.Creators = _productData.ProductCreators.Split(',').Select(s => s.Trim()).ToArray();
                productData.PublicationDate = new(DateTime.ParseExact(_productData.PublicationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                productData.Description = _productData.ProductDescription;
                productData.Price = _productData.ProductPrice;
                productData.Quantity = (ushort)_productData.ProductStock;

                foreach (var property in _productData.PropertyInputs)
                {
                    productData.Properties.Add(property.Key, property.Value);
                }

                var id = (int)_productData.ProductType * 10000000 + _products.Count;

                _createdProducts.Add(id.ToString(), productData);
                _products.Insert(0, id.ToString(), productData);

                Marker.OnRuntimeSavingRequested?.Invoke();

                Begin();
                Marker.OnPageNavigated?.Invoke(ViewType.ManagerViewProductPage, true, true);
            }
        }
    }
}