using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.Extensions;

namespace YNL.JAMOS
{
    public partial class ManagerViewAddPageUI : ViewPageUI
    {
        private SerializableDictionary<UID, Product.Data> _products => Main.Database.Products;

        private GenreTypeField _genreTypeField;
        private PropertyField _propertyField;

        private Texture2D _productImage;
        private string _productName;
        private string _productCreators;
        private Product.Type _productType;
        private ushort _productGenres;
        private string _productDescription;
        private float _productPrice;
        private int _productStock;
        private string _publicationDate;
        private Dictionary<Product.Property, string> _propertyInputs = new();

        protected override void Collect()
        {
            var scrollContainer = Root.Q("MainScroll").Q("unity-content-container");

            var informationField = scrollContainer.Q("InformationField");
            var nameInput = informationField.Q("InputField").Q("NameField").Q<TextField>("Input");
            nameInput.RegisterValueChangedCallback(OnValueChanged_NameInput);
            var creatorsInput = informationField.Q("InputField").Q("CreatorsField").Q<TextField>("Input");
            creatorsInput.RegisterValueChangedCallback(OnValueChanged_CreatorsInput);

            var detailField = scrollContainer.Q("DetailField");
            var productTypeField = new ProductTypeField(detailField.Q("TypeField"));
            productTypeField.OnTypeSelected = OnTypeSelected;
            _genreTypeField = new(detailField.Q("GenreField"));
            _genreTypeField.OnGenreSelected = OnGenreSelected;
            var dateInput = detailField.Q("DateField").Q<TextField>("Input");
            dateInput.RegisterValueChangedCallback(OnValueChanged_DateInput);
            var descriptionInput = detailField.Q("DescriptionField").Q<TextField>("Input");
            descriptionInput.RegisterValueChangedCallback(OnValueChanged_DescriptionInput);

            var storeField = scrollContainer.Q("StoreField");
            var priceInput = storeField.Q("PriceField").Q<FloatField>("Input");
            priceInput.RegisterValueChangedCallback(OnValueChanged_PriceInput);
            var stockInput = storeField.Q("StockField").Q<IntegerField>("Input");
            stockInput.RegisterValueChangedCallback(OnvalueChanged_StockInput);

            _propertyField = new(scrollContainer.Q("PropertyField"));
            _propertyField.OnPropertyChanged = OnPropertyChanged;

            var bottomField = Root.Q("BottomField");
            var cancelButton = bottomField.Q<Button>("CancelButton");
            cancelButton.clicked += OnClicked_CancelButton;
            var saveAsDraftButton = bottomField.Q<Button>("SaveAsDraftButton");
            saveAsDraftButton.clicked += OnClicked_SaveAsDraftButton;
            var addButton = bottomField.Q<Button>("AddButton");
            addButton.clicked += OnClicked_AddButton;
        }

        protected override void Refresh()
        {
            _productImage = null;
            _productName = string.Empty;
            _productCreators = string.Empty;
            _productType = Product.Type.None;
            _productGenres = 0;
            _publicationDate = string.Empty;
            _productDescription = string.Empty;
            _productPrice = 0.0f;
            _productStock = 0;
            _propertyInputs = new();
        }

        private void OnValueChanged_NameInput(ChangeEvent<string> evt)
        {
            _productName = evt.newValue;
        }

        private void OnValueChanged_CreatorsInput(ChangeEvent<string> evt)
        {
            _productCreators = evt.newValue;
        }

        private void OnValueChanged_DateInput(ChangeEvent<string> evt)
        {
            _publicationDate = evt.newValue.Trim();
        }

        private void OnValueChanged_DescriptionInput(ChangeEvent<string> evt)
        {
            _productDescription = evt.newValue;
        }

        private void OnValueChanged_PriceInput(ChangeEvent<float> evt)
        {
            _productPrice = evt.newValue;
        }

        private void OnvalueChanged_StockInput(ChangeEvent<int> evt)
        {
            _productStock = evt.newValue;
        }

        private void OnTypeSelected(Product.Type type)
        {
            _productType = type;

            _genreTypeField.RecreateGenreItems(type);
            _propertyField.RecreatePropertyItems(type);
        }

        private void OnGenreSelected(bool isSelected, ushort genre)
        {
            if (isSelected)
            {
                _productGenres |= genre;
            }
            else
            {
                _productGenres &= (ushort)~genre;
            }

            MDebug.Log(_productGenres);
        }

        private void OnPropertyChanged(Product.Property property, string value)
        {
            _propertyInputs[property] = value;

            MDebug.Log($"{property}: {value}");
        }

        private void OnClicked_CancelButton()
        {
            Initialize();
            Marker.OnPageNavigated?.Invoke(ViewType.ManagerViewProductPage, true, true);
        }

        private void OnClicked_SaveAsDraftButton()
        {
            Marker.OnPageNavigated?.Invoke(ViewType.ManagerViewProductPage, true, true);
        }

        private void OnClicked_AddButton()
        {
            var productData = new Product.Data();
            productData.Type = _productType;
            productData.Title = _productName;
            productData.Genres = _productType.GetProductGenresString(_productGenres);
            productData.Creators = _productCreators.Split(",").Select(s => s.Trim()).ToArray();
            productData.PublicationDate = new(DateTime.ParseExact(_publicationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            productData.Description = _productDescription;
            productData.Price = _productPrice;
            productData.Quantity = (ushort)_productStock;

            foreach (var property in _propertyInputs)
            {
                productData.Properties.Add(property.Key, property.Value);
            }

            _products.Add((int)_productType * 10000000 + _products.Count, productData);
        }
    }
}