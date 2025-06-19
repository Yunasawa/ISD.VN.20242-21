using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;

namespace YNL.JAMOS
{
    public partial class ManagerViewAddPageUI : ViewPageUI
    {
        private SerializableDictionary<UID, Product.Data> _products => Main.Database.Products;

        private Texture2D _productImage;
        private string _productName;
        private string _productCreators;
        private Product.Type _productType;
        private string _productGenres;
        private string _productDescription;
        private float _productPrice;
        private int _productStock;
        private DateTime _publicationDate;
        private Dictionary<Product.Property, string> _propertyInputs = new();

        protected override void Collect()
        {
            var scrollContainer = Root.Q("MainScroll").Q("unity-content-container");

            var informationField = scrollContainer.Q("InformationField");
            var nameInput = informationField.Q("DetailField").Q("NameField").Q<TextField>("Input");
            nameInput.RegisterValueChangedCallback(OnValueChanged_NameInput);
            var creatorsInput = informationField.Q("DetailField").Q("CreatorsField").Q<TextField>("Input");
            creatorsInput.RegisterValueChangedCallback(OnValueChanged_CreatorsInput);

            var detailField = scrollContainer.Q("DetailField");

            var descriptionInput = detailField.Q("DescriptionField").Q<TextField>("Input");
            descriptionInput.RegisterValueChangedCallback(OnValueChanged_DescriptionInput);

            var storeField = scrollContainer.Q("StoreField");
            var priceInput = storeField.Q("PriceField").Q<FloatField>("Input");
            priceInput.RegisterValueChangedCallback(OnValueChanged_PriceInput);
            var stockInput = storeField.Q("StockField").Q<IntegerField>("Input");
            stockInput.RegisterValueChangedCallback(OnvalueChanged_StockInput);

            var bottomField = Root.Q("BottomField");
            var cancelButton = bottomField.Q<Button>("CancelButton");
            cancelButton.clicked += OnClicked_CancelButton;
            var saveAsDraftButton = bottomField.Q<Button>("SaveAsDraftButton");
            saveAsDraftButton.clicked += OnClicked_SaveAsDraftButton;
            var addButton = bottomField.Q<Button>("AddButton");
            addButton.clicked += OnClicked_AddButton;
        }

        protected override void Initialize()
        {
            _productImage = null;
            _productName = "...";
            _productCreators = "...";
            _productType = Product.Type.None;
            _productGenres = "...";
            _productDescription = string.Empty;
            _productPrice = 0.0f;
            _productStock = 0;
        }

        private void OnValueChanged_NameInput(ChangeEvent<string> evt)
        {
            _productName = evt.newValue;
        }

        private void OnValueChanged_CreatorsInput(ChangeEvent<string> evt)
        {
            _productCreators = evt.newValue;
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
            var productDate = new Product.Data();
            productDate.Type = _productType;
            productDate.Title = _productName;
            productDate.Creators = _productCreators.Split(",").Select(s => s.Trim()).ToArray();
            productDate.Description = _productDescription;
            productDate.Price = _productPrice;
            productDate.Quantity = (ushort)_productStock;

            _products.Add(_products.Count, productDate);
        }

        private void OnPropertyChanged(Product.Property property, string value)
        {
            _propertyInputs[property] = value;
        }
    }
}