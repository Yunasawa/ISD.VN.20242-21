using System;
using System.Linq;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class ProductCartItemUI
    {
        public class NameFieldUI : VisualElement
        {
            private VisualElement _typeIcon;
            private Label _nameText;

            public NameFieldUI()
            {
                this.AddClass($"{_rootClass}__name-field");

                _typeIcon = new VisualElement().AddClass($"{_rootClass}__type-icon");
                _nameText = new Label().AddClass($"{_rootClass}__name-text");
                this.AddElements(_typeIcon, _nameText);
            }

            public void Apply(Product.Data product)
            {
                _typeIcon.SetBackgroundImage(Main.Resources.Icons[product.Type.ToString()]);
                _nameText.SetText(product.Title);
            }
        }
    
        public class CreatorFieldUI : VisualElement
        {
            private VisualElement _creatorName;
            private VisualElement _creatorIcon;
            private Label _creatorText;
            private VisualElement _ratingField;
            private Label _ratingText;
            private VisualElement _ratingIcon;

            public CreatorFieldUI()
            {
                this.AddClass($"{_rootClass}__creator-field");

                _creatorName = new VisualElement().AddClass($"{_rootClass}__creator-name");

                _creatorIcon = new VisualElement().AddClass($"{_rootClass}__creator-icon");
                _creatorText = new Label().AddClass($"{_rootClass}__creator-text");
                _creatorName.AddElements(_creatorIcon, _creatorText);

                _ratingField = new VisualElement().AddClass($"{_rootClass}__rating-field");

                _ratingText = new Label().AddClass($"{_rootClass}__rating-text");
                _ratingIcon = new VisualElement().AddClass($"{_rootClass}__rating-icon");
                _ratingField.AddElements(_ratingText, _ratingIcon);

                this.AddElements(_creatorName, _ratingField);
            }

            public void Apply(Product.Data product)
            {
                _creatorText.SetText(string.Join(", ", product.Creators));
                _ratingText.SetText($"<b>{product.Review.AverageTotalRating}</b> ({product.Review.FeebackAmount})");
            }
        }
    
        public class PriceFieldUI : VisualElement
        {
            private VisualElement _originalField;
            private VisualElement _discountField;
            private VisualElement _discountIcon;
            private Label _discountText;
            private Label _stockText;

            public PriceFieldUI()
            {
                this.AddClass($"{_rootClass}__price-field");

                _originalField = new VisualElement().AddClass($"{_rootClass}__original-field");

                _discountField = new VisualElement().AddClass($"{_rootClass}__discount-field");

                _discountIcon = new VisualElement().AddClass($"{_rootClass}__discount-icon");
                _discountText = new Label().AddClass($"{_rootClass}__discount-text");
                _discountField.AddElements(_discountIcon, _discountText);

                _stockText = new Label().AddClass($"{_rootClass}__stock-text");

                _originalField.AddElements(_discountField, _stockText);
                this.AddElements(_originalField);
            }

            public void Apply(UID id)
            {
                var product = Main.Database.Products[id];
                int discount = Main.Runtime.Discount;
               
                _discountField.SetDisplay(discount > 0 && product.IsFree == false ? DisplayStyle.Flex : DisplayStyle.None);
                _discountText.SetText($"-{discount}%");

                _stockText.SetText($"• Only {product.Quantity} copies in stock");
            }
        }
    
        public class AmountFieldUI : VisualElement
        {
            private SerializableDictionary<UID, uint> _orderedAmounts => Main.Runtime.OrderedAmounts;

            private Label _originalText;
            private VisualElement _removeButton;
            private Label _amountText;
            private VisualElement _addButton;

            private UID _uid;

            public AmountFieldUI()
            {
                this.AddClass($"{_rootClass}__amount-field");

                _originalText = new Label().AddClass($"{_rootClass}__original-text");
                _amountText = new Label("0").AddClass($"{_rootClass}__amount-text");
                
                _removeButton = new VisualElement().AddClass($"{_rootClass}__remove-button");
                _removeButton.RegisterCallback<PointerUpEvent>(OnClicked_RemoveButton);
                
                _addButton = new VisualElement().AddClass($"{_rootClass}__add-button");
                _addButton.RegisterCallback<PointerUpEvent>(OnClicked_AddButton);

                this.AddElements(_addButton, _amountText, _removeButton, _originalText);
            }

            public void Apply(UID id)
            {
                _uid = id;

                var product = Main.Database.Products[id];
                int discount = Main.Runtime.Discount;

                string price = $"<b><color=#DEF95D>{product.Price * (1 - discount / 100f):0.00}$</color></b>";
                if (discount > 0) price += $"<s>{product.Price:0.00}$</s>";

                if (product.IsFree) price = $"<b><color=#DEF95D>FREE</color></b>";

                _originalText.SetText(price);

                UpdateAmountText();
            }

            private void OnClicked_AddButton(PointerUpEvent evt)
            {
                _uid.AdjustOrderedAmount(true);
                UpdateAmountText();

                OnAmountAdjusted?.Invoke();

                evt.StopImmediatePropagation();
            }

            private void OnClicked_RemoveButton(PointerUpEvent evt)
            {
                _uid.AdjustOrderedAmount(false);
                UpdateAmountText();

                OnAmountAdjusted?.Invoke();

                evt.StopImmediatePropagation();
            }

            private void UpdateAmountText()
            {
                if (_orderedAmounts.ContainsKey(_uid))
                {
                    _amountText.SetText(_orderedAmounts[_uid].ToString());
                }
                else
                {
                    _amountText.SetText("0");
                }
            }
        }
    }

    public partial class ProductCartItemUI : VisualElement
    {
        public static Action OnAmountAdjusted { get; set; }

        protected const string _rootClass = "product-cart-item";

        private VisualElement _previewImage;
        private VisualElement _infoArea;
        private NameFieldUI _nameField;
        private CreatorFieldUI _creatorField;
        private VisualElement _spaceElement;
        private PriceFieldUI _priceField;
        private AmountFieldUI _amountField;

        private UID _uid;

        public ProductCartItemUI()
        {
            this.AddStyle(Main.Resources.Styles["StyleVariableUI"]);
            this.AddStyle(Main.Resources.Styles["ProductCartItemUI"]);
            this.AddClass(_rootClass);
            this.RegisterCallback<PointerUpEvent>(OnClicked_CartItem);

            _previewImage = new VisualElement().AddClass($"{_rootClass}__preview-image");
            _infoArea = new VisualElement().AddClass($"{_rootClass}__info-area");
            this.AddElements(_previewImage, _infoArea);

            _nameField = new();
            _creatorField = new();
            _spaceElement = new VisualElement().AddClass($"{_rootClass}__space-element");
            _priceField = new();
            _amountField = new AmountFieldUI();

            _infoArea.AddElements(_nameField, _creatorField, _spaceElement, _priceField, _amountField);
        }

        public void Apply(UID id)
        {
            _uid = id;
            var proiduct = Main.Database.Products[id];

            _previewImage.ApplyCloudImageAsync(id.GetImageURL());
            _nameField.Apply(proiduct);
            _creatorField.Apply(proiduct);
            _priceField.Apply(id);
            _amountField.Apply(id);
        }
    
        private void OnClicked_CartItem(PointerUpEvent evt)
        {
            Main.Runtime.SelectedProduct = _uid;

            Marker.OnPageNavigated?.Invoke(ViewType.InformationViewMainPage, true, true);
        }
    }
}