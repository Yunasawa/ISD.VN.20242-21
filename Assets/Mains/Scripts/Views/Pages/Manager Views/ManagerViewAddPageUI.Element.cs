using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class ManagerViewAddPageUI
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
            private readonly HashSet<string> _genreValues = new();

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

            public PropertyField(VisualElement field)
            {
                _propertyField = field;

                RecreatePropertyItems(Product.Type.None);
            }

            public void RecreatePropertyItems(Product.Type type)
            {
                _propertyField.Clear();

                var properties = type.GetProductProperties();

                foreach (var property in properties)
                {
                    var field = new PropertyInputFieldUI(property);
                    field.OnPropertyChanged = OnPropertyValueChanged;
                    if (property == properties[0]) field.SetAsFirstItem();
                    _propertyField.Add(field);
                }
            }

            private void OnPropertyValueChanged(Product.Property property, string value)
            {
                OnPropertyChanged?.Invoke(property, value);
            }
        }
    }
}