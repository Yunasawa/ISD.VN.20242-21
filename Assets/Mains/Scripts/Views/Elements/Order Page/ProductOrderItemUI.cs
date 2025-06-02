using System;
using System.Linq;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class ProductOrderItemUI
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

            public CreatorFieldUI()
            {
                this.AddClass($"{_rootClass}__creator-field");

                _creatorName = new VisualElement().AddClass($"{_rootClass}__creator-name");

                _creatorIcon = new VisualElement().AddClass($"{_rootClass}__creator-icon");
                _creatorText = new Label().AddClass($"{_rootClass}__creator-text");
                _creatorName.AddElements(_creatorIcon, _creatorText);

                this.AddElements(_creatorName);
            }

            public void Apply(Product.Data product)
            {
                _creatorText.SetText(string.Join(", ", product.Creators));
            }
        }

        public class PriceFieldUI : VisualElement
        {
            private Label _originalText;
            private VisualElement _discountField;
            private VisualElement _discountIcon;
            private Label _discountText;

            public PriceFieldUI()
            {
                this.AddClass($"{_rootClass}__price-field");

                _originalText = new Label().AddClass($"{_rootClass}__original-text");

                _discountField = new VisualElement().AddClass($"{_rootClass}__discount-field");

                _discountIcon = new VisualElement().AddClass($"{_rootClass}__discount-icon");
                _discountText = new Label().AddClass($"{_rootClass}__discount-text");
                _discountField.AddElements(_discountIcon, _discountText);

                this.AddElements(_originalText, _discountField);
            }

            public void Apply(UID id)
            {
                var product = Main.Database.Products[id];
                int discount = 0;

                _discountField.SetDisplay(discount > 0 && product.IsFree == false ? DisplayStyle.Flex : DisplayStyle.None);
                _discountText.SetText($"-{discount}%");

                string price = $"<b><color=#DEF95D>{product.Price * (1 - discount / 100f):0.00}$</color></b>";
                if (discount > 0) price += $"<s>{product.Price:0.00}$</s>";

                if (product.IsFree) price = $"<b><color=#DEF95D>FREE</color></b>";

                _originalText.SetText(price);
            }
        }
    }

    public partial class ProductOrderItemUI : VisualElement
    {
        public static Action OnAmountAdjusted { get; set; }

        protected const string _rootClass = "product-order-item";

        private VisualElement _previewImage;
        private VisualElement _infoArea;
        private NameFieldUI _nameField;
        private CreatorFieldUI _creatorField;
        private VisualElement _spaceElement;
        private PriceFieldUI _priceField;
        private Label _amountLabel;

        private UID _uid;

        public ProductOrderItemUI()
        {
            this.AddStyle(Main.Resources.Styles["StyleVariableUI"]);
            this.AddStyle(Main.Resources.Styles["ProductOrderItemUI"]);
            this.AddClass(_rootClass);
            this.RegisterCallback<PointerUpEvent>(OnClicked_CartItem);

            _previewImage = new VisualElement().AddClass($"{_rootClass}__preview-image");
            _infoArea = new VisualElement().AddClass($"{_rootClass}__info-area");
            this.AddElements(_previewImage, _infoArea);

            _nameField = new();
            _creatorField = new();
            _spaceElement = new VisualElement().AddClass($"{_rootClass}__space-element");
            _priceField = new();
            _amountLabel = new Label().AddClass($"{_rootClass}__amount-label");

            _infoArea.AddElements(_nameField, _creatorField, _spaceElement, _priceField, _amountLabel);
        }

        public void Apply(UID id)
        {
            _uid = id;
            var product = Main.Database.Products[id];
            var amount = Main.Runtime.OrderedAmounts[id];

            _previewImage.ApplyCloudImageAsync(id.GetImageURL());
            _nameField.Apply(product);
            _creatorField.Apply(product);
            _priceField.Apply(id);
            _amountLabel.SetText(amount.ToString());
        }

        private void OnClicked_CartItem(PointerUpEvent evt)
        {
            Main.Runtime.SelectedProduct = _uid;

            Marker.OnPageNavigated?.Invoke(ViewType.InformationViewMainPage, true, true);
        }
    }
}