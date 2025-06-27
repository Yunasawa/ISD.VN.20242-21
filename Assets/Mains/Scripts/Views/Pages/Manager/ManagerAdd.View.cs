using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;
using System.Globalization;

namespace YNL.JAMOS
{
    public partial class ManagerAdd
    {
        private class View : PageView
        {
            private ManagerAdd _b;

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

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerAdd;

                _b.OnDataFieldReseted += OnDataFieldReseted; 
            }

            public override void Collect(VisualElement root)
            {
                var scrollContainer = root.Q("MainScroll").Q("unity-content-container");

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

                var bottomField = root.Q("BottomField");
                var cancelButton = bottomField.Q<Button>("CancelButton");
                cancelButton.clicked += OnClicked_CancelButton;
                var saveAsDraftButton = bottomField.Q<Button>("SaveAsDraftButton");
                saveAsDraftButton.clicked += OnClicked_SaveAsDraftButton;
                var addButton = bottomField.Q<Button>("AddButton");
                addButton.clicked += OnClicked_AddButton;
            }

            private void OnValueChanged_NameInput(ChangeEvent<string> evt)
            {
                _b.OnPropertyValueChanged?.Invoke("Name", evt.newValue);
            }

            private void OnValueChanged_CreatorsInput(ChangeEvent<string> evt)
            {
                _b.OnPropertyValueChanged?.Invoke("Creators", evt.newValue);
            }

            private void OnValueChanged_DateInput(ChangeEvent<string> evt)
            {
                _b.OnPropertyValueChanged?.Invoke("PublicationDate", evt.newValue);
            }

            private void OnValueChanged_DescriptionInput(ChangeEvent<string> evt)
            {
                _b.OnPropertyValueChanged?.Invoke("Description", evt.newValue);
            }

            private void OnValueChanged_PriceInput(ChangeEvent<float> evt)
            {
                _b.OnPropertyValueChanged?.Invoke("Price", evt.newValue.ToString());
            }

            private void OnvalueChanged_StockInput(ChangeEvent<int> evt)
            {
                _b.OnPropertyValueChanged?.Invoke("Stock", evt.newValue.ToString());
            }

            private void OnTypeSelected(Product.Type type)
            {
                _b.OnPropertyValueChanged?.Invoke("Type", type.ToString());

                _genreTypeField.RecreateGenreItems(type);
                _productPropertyField.RecreatePropertyItems(type);
            }

            private void OnGenreSelected(bool isSelected, ushort genre)
            {
                _b.OnGenreValueChanged?.Invoke(isSelected, genre);
            }

            private void OnPropertyChanged(Product.Property property, string value)
            {
                _b.OnPropertyValueChanged?.Invoke(property.ToString(), value);
            }

            private void OnClicked_CancelButton()
            {
                Begin();
                Marker.OnPageNavigated?.Invoke(ViewType.ManagerViewProductPage, true, true);
            }

            private void OnClicked_SaveAsDraftButton()
            {
                Marker.OnPageNavigated?.Invoke(ViewType.ManagerViewProductPage, true, true);
            }

            private void OnClicked_AddButton()
            {
                _b.OnAddButtonClicked?.Invoke();
            }

            private void OnDataFieldReseted(ProductWrapper wraper)
            {
                _productNameField.SetText(wraper.ProductName);
                _productCreatorsField.SetText(wraper.ProductCreators);
                _productTypeField.SetValue(wraper.ProductType);
                _genreTypeField.SetValue(wraper.ProductType, wraper.ProductGenres);
                _productPublicationDate.SetText(wraper.PublicationDate);
                _productDescriptionField.SetText(wraper.ProductDescription);
                _productPriceField.value = wraper.ProductPrice;
                _productStockField.value = wraper.ProductStock;
                _productPropertyField.SetValue(wraper.ProductType, wraper.PropertyInputs);
            }
        }
    }

    public partial class ManagerAdd
    {
        public class ProductTypeField
        {
            public Action<Product.Type> OnTypeSelected { get; set; }

            private VisualElement _typeField;
            private VisualElement _typeIcon;
            private Label _typeText;
            private VisualElement _typeArrow;
            private VisualElement _typeDropdown;

            private int _dropdownHeight;

            public ProductTypeField(VisualElement typeField)
            {
                _typeField = typeField.Q("EnumField");
                _typeField.RegisterCallback<PointerUpEvent>(OnClicked_TypeField);

                _typeIcon = _typeField.Q("TypeIcon");
                _typeText = _typeField.Q<Label>("TypeLabel");
                _typeArrow = _typeField.Q("ExpandArrow");

                _typeDropdown = typeField.Q("EnumDropdown");
                _typeDropdown.RegisterCallback<FocusOutEvent>(OnFocusOut_TypeDropdown);

                Initialize();
            }

            private void Initialize()
            {
                _dropdownHeight = 100 + (Enum.GetValues(typeof(Product.Type)).Length - 1) * 90 + 30;

                _typeDropdown.Clear();

                foreach (Product.Type type in Enum.GetValues(typeof(Product.Type)))
                {
                    if (type == Product.Type.None) continue;

                    var item = new ProductTypeItemUI(type);
                    item.OnSelected = OnProductTypeSelected;
                    _typeDropdown.Add(item);
                }

                _typeDropdown.Add(new VisualElement().SetHeight(30));
            }

            public void SetValue(Product.Type type)
            {
                foreach (var child in _typeDropdown.Children())
                {
                    if (child is not ProductTypeItemUI) continue;

                    var item = child as ProductTypeItemUI;
                    if (item.Type == type)
                    {
                        item.Select();
                        return;
                    }
                }

                _typeIcon.SetBackgroundImage(Main.Resources.Icons["None"]);
                _typeText.SetText("...");
            }

            private void OnClicked_TypeField(PointerUpEvent evt)
            {
                OpenDropdown(true);

                _typeDropdown.Focus();
            }

            private void OnFocusOut_TypeDropdown(FocusOutEvent evt)
            {
                OpenDropdown(false);
            }

            private void OpenDropdown(bool isOpen)
            {
                _typeDropdown.SetHeight(isOpen ? _dropdownHeight : 100);
                _typeArrow.SetRotate(isOpen ? 90 : 270, AngleUnit.Degree);
            }

            private void OnProductTypeSelected(Product.Type type)
            {
                var typeString = type.ToString();

                _typeIcon.SetBackgroundImage(Main.Resources.Icons[typeString]);
                _typeText.SetText(typeString);

                OpenDropdown(false);

                OnTypeSelected?.Invoke(type);
            }
        }

        public class GenreTypeField : VisualElement
        {
            public Action<bool, ushort> OnGenreSelected { get; set; }

            private VisualElement _typeField;
            private Label _typeText;
            private VisualElement _typeArrow;
            private VisualElement _typeDropdown;

            private Product.Type _productType;
            private ushort _genreValue;
            private readonly HashSet<string> _genreValues = new();
            private bool _isGenreSelected = false;

            public GenreTypeField(VisualElement genreField)
            {
                _typeField = genreField.Q("EnumField");
                _typeField.RegisterCallback<PointerUpEvent>(OnClicked_TypeField);

                _typeText = _typeField.Q<Label>("TypeLabel");
                _typeArrow = _typeField.Q("ExpandArrow");

                _typeDropdown = genreField.Q("EnumDropdown");
                _typeDropdown.RegisterCallback<FocusOutEvent>(OnFocusOut_TypeDropdown);

                Initialize();
            }

            private void Initialize()
            {
                RecreateGenreItems(Product.Type.None);
            }

            public void SetValue(Product.Type type, ushort genre)
            {
                _genreValue = genre;
                _isGenreSelected = false;

                RecreateGenreItems(type);

                if (_isGenreSelected == false)
                {
                    _typeText.SetText("<color=#909090>None</color>");
                }
            }

            public void ResetValue()
            {
                _genreValue = 0;
            }

            public void RecreateGenreItems(Product.Type productType)
            {
                _productType = productType;

                _typeDropdown.Clear();

                if (productType == Product.Type.None)
                {
                    var message = new Label("Please select <b><color=#DEF95D>Product Type</color></b> first");
                    message
                        .SetHeight(50).SetMargin(0, 25, 0, 25).SetPadding(0).SetFont(Main.Resources.Fonts["Nunito"])
                        .SetColor("#FFFFFF").SetTextAlign(TextAnchor.MiddleLeft).SetFontSize(35);

                    _typeDropdown.Add(message);
                }
                else
                {
                    _typeDropdown.AddElements(new VisualElement().SetWidth(100, true).SetHeight(30));

                    var genres = productType.GetGenreType();

                    foreach (var genre in Enum.GetValues(genres))
                    {
                        if ((ushort)genre == 0) continue;

                        var item = new GenreTypeItemUI(genre.ToString(), (ushort)genre);
                        item.OnSelected = OnProductTypeSelected;
                        if ((_genreValue & item.Value) == item.Value)
                        {
                            item.Select();
                            _isGenreSelected = true;
                        }
                        _typeDropdown.Add(item);
                    }

                    _typeDropdown.AddElements(new VisualElement().SetWidth(100, true).SetHeight(30));
                }
            }

            private void OnClicked_TypeField(PointerUpEvent evt)
            {
                OpenDropdown(true);

                _typeDropdown.Focus();
            }

            private void OnFocusOut_TypeDropdown(FocusOutEvent evt)
            {
                OpenDropdown(false);
            }

            private void OpenDropdown(bool isOpen)
            {
                if (isOpen) _typeDropdown.SetHeight(StyleKeyword.Auto);
                else _typeDropdown.SetHeight(0);

                _typeArrow.SetRotate(isOpen ? 90 : 270, AngleUnit.Degree);
            }

            private void OnProductTypeSelected(bool isSelected, ushort value)
            {
                var genre = _productType.ToGenreText(value).AddSpace();

                if (isSelected)
                {
                    _genreValues.Add(genre);
                }
                else
                {
                    _genreValues.Remove(genre);
                }

                if (_genreValues.Count == 0)
                {
                    _typeText.SetText("<color=#909090>None</color>");
                }
                else
                {
                    _typeText.SetText(string.Join(", ", _genreValues));
                }

                OnGenreSelected?.Invoke(isSelected, value);
            }
        }

        public class PropertyField
        {
            public Action<Product.Property, string> OnPropertyChanged { get; set; }

            private VisualElement _propertyField;
            private Dictionary<Product.Property, PropertyInputFieldUI> _fields = new();

            public PropertyField(VisualElement field)
            {
                _propertyField = field;

                RecreatePropertyItems(Product.Type.None);
            }

            public void SetValue(Product.Type type, Dictionary<Product.Property, string> properties)
            {
                RecreatePropertyItems(type);

                foreach (var property in properties)
                {
                    _fields[property.Key].SetValue(property.Value);
                }
            }

            public void RecreatePropertyItems(Product.Type type)
            {
                _propertyField.Clear();
                _fields.Clear();

                var properties = type.GetProductProperties();

                foreach (var property in properties)
                {
                    var field = new PropertyInputFieldUI(property);
                    field.OnPropertyChanged = OnPropertyValueChanged;
                    if (property == properties[0]) field.SetAsFirstItem();
                    _propertyField.Add(field);
                    _fields.Add(property, field);
                }
            }

            private void OnPropertyValueChanged(Product.Property property, string value)
            {
                OnPropertyChanged?.Invoke(property, value);
            }
        }
    }
}
