using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class ManagerViewUpdatePageUI : ViewPageUI
    {
        private SerializableDictionary<UID, Product.Data> _products => Main.Database.Products;

        private VisualElement _productImage;
        private TextField _productNameField;
        private TextField _productCreatorsField;
        private ProductTypeField _productTypeField;
        private GenreTypeField _genreTypeField;
        private TextField _productPublicationDate;
        private TextField _productDescriptionField;
        private FloatField _productPriceField;
        private IntegerField _productStockField;
        private PropertyField _productPropertyField;

        private string _productName;
        private string _productCreators;
        private Product.Type _productType;
        private ushort _productGenres;
        private string _productDescription;
        private float _productPrice;
        private int _productStock;
        private string _publicationDate;
        private Dictionary<Product.Property, string> _propertyInputs = new();

        private UID _productID;

        protected override void VirtualAwake()
        {
            Marker.OnProductUpdatingRequested += OnProductUpdatingRequested;
        }

        private void OnDestroy()
        {
            Marker.OnProductUpdatingRequested -= OnProductUpdatingRequested;
        }

        protected override void Collect()
        {
            var scrollContainer = Root.Q("MainScroll").Q("unity-content-container");

            var informationField = scrollContainer.Q("InformationField");
            _productImage = informationField.Q("ImageField");
            _productNameField = informationField.Q("InputField").Q("NameField").Q<TextField>("Input");
            _productNameField.RegisterValueChangedCallback(OnValueChanged_NameInput);
            _productCreatorsField = informationField.Q("InputField").Q("CreatorsField").Q<TextField>("Input");
            _productCreatorsField.RegisterValueChangedCallback(OnValueChanged_CreatorsInput);

            var detailField = scrollContainer.Q("DetailField");
            _productTypeField = new(detailField.Q("TypeField"));
            _productTypeField.OnTypeSelected = OnTypeSelected;
            _genreTypeField = new(detailField.Q("GenreField"));
            _genreTypeField.OnGenreSelected = OnGenreSelected;
            _productPublicationDate = detailField.Q("DateField").Q<TextField>("Input");
            _productPublicationDate.RegisterValueChangedCallback(OnValueChanged_DateInput);
            _productDescriptionField = detailField.Q("DescriptionField").Q<TextField>("Input");
            _productDescriptionField.RegisterValueChangedCallback(OnValueChanged_DescriptionInput);

            var storeField = scrollContainer.Q("StoreField");
            _productPriceField = storeField.Q("PriceField").Q<FloatField>("Input");
            _productPriceField.RegisterValueChangedCallback(OnValueChanged_PriceInput);
            _productStockField = storeField.Q("StockField").Q<IntegerField>("Input");
            _productStockField.RegisterValueChangedCallback(OnvalueChanged_StockInput);

            _productPropertyField = new(scrollContainer.Q("PropertyField"));
            _productPropertyField.OnPropertyChanged = OnPropertyChanged;

            var bottomField = Root.Q("BottomField");
            var cancelButton = bottomField.Q<Button>("CancelButton");
            cancelButton.clicked += OnClicked_CancelButton;
            var updateButton = bottomField.Q<Button>("UpdateButton");
            updateButton.clicked += OnClicked_UpdateButton;
        }

        protected override void Initialize()
        {
            _productID = 10000000;
            Refresh();
        }

        protected override void Refresh()
        {
            var product = _products[_productID];

            _productImage.ApplyCloudImageAsync(_productID.GetImageURL());
            _productNameField.SetText(_productName = product.Title);
            _productCreatorsField.SetText(_productCreators = string.Join(", ", product.Creators));
            _productTypeField.SetValue(_productType = product.Type);
            _genreTypeField.SetValue(_productType, _productGenres = 0b0000000000001010);
            _productPublicationDate.SetText($"{_publicationDate = product.PublicationDate.Value} (dd/mm/yyyy)");
            _productDescription = product.Description;
            _productDescriptionField.SetText(_productID.GetDescriptionText());
            _productPriceField.value = _productPrice = product.Price;
            _productStockField.value = _productStock = product.Quantity;

            _propertyInputs.Clear();
            foreach (var kvp in product.Properties)
            {
                _propertyInputs.Add(kvp.Key, kvp.Value);
            }
            _productPropertyField.SetValue(_productType, _propertyInputs);
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
            _publicationDate = evt.newValue.RemoveWord("(dd/mm/yyyy)").Trim();
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

            _genreTypeField.ResetValue();
            _genreTypeField.RecreateGenreItems(type);
            _productPropertyField.RecreatePropertyItems(type);
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
        }

        private void OnPropertyChanged(Product.Property property, string value)
        {
            _propertyInputs[property] = value;
        }

        private void OnClicked_CancelButton()
        {
            Initialize();
            Marker.OnPageNavigated?.Invoke(ViewType.ManagerViewProductPage, true, true);
        }

        private void OnClicked_UpdateButton()
        {
            var productData = _products[_productID];
            productData.Type = _productType;
            productData.Title = _productName;
            productData.Genres = _productType.GetProductGenresString(_productGenres);
            productData.Creators = _productCreators.Split(",").Select(s => s.Trim()).ToArray();
            productData.PublicationDate = new(DateTime.ParseExact(_publicationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            productData.Description = _productDescription.Replace("\r\n", "#");
            productData.Price = _productPrice;
            productData.Quantity = (ushort)_productStock;

            foreach (var property in _propertyInputs)
            {
                productData.Properties[property.Key] = property.Value;
            }

            Marker.OnPageNavigated?.Invoke(ViewType.ManagerViewInformationPage, true, true);
        }

        private void OnProductUpdatingRequested(UID productID)
        {
            _productID = productID;
        }
    }
}