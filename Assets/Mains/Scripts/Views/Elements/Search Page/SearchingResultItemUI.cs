using System;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.Checkotel
{
    public partial class SearchingResultItemUI
    {
        public class HotTagUI : VisualElement
        {
            private const string _elementClass = _rootClass + "__hot-tag";
            private const string _iconClass = _elementClass + "-icon";
            private const string _textClass = _elementClass + "-text";

            private VisualElement _icon;
            private VisualElement _text;

            public HotTagUI()
            {
                this.AddClass(_elementClass);

                _icon = new VisualElement().AddClass(_iconClass);
                _text = new Label("Hot").AddClass(_textClass);
                this.AddElements(_icon, _text);
            }
        }
    
        public class NameFieldUI : VisualElement
        {
            private const string _elementClass = _rootClass + "__name-field";
            private const string _nameTextClass = _rootClass + "__name-text";
            private const string _ratingFieldClass = _rootClass + "__rating-field";
            private const string _ratingIconClass = _rootClass + "__rating-icon";
            private const string _ratingTextClass = _rootClass + "__rating-text";

            private Label _nameText;
            private VisualElement _ratingField;
            private VisualElement _ratingIcon;
            private Label _ratingText;

            public NameFieldUI(string name, (float score, int amount) rating)
            {
                this.AddClass(_elementClass);

                _nameText = new Label(name).AddClass(_nameTextClass);
                this.Add(_nameText);

                _ratingField = new VisualElement().AddClass(_ratingFieldClass);
                this.Add(_ratingField);

                _ratingIcon = new VisualElement().AddClass(_ratingIconClass);
                _ratingField.Add(_ratingIcon);

                _ratingText = new Label($"<b>{rating.score}</b> ({rating.amount})").AddClass(_ratingTextClass);
                _ratingField.Add(_ratingText);
            }
        }
    
        public class AddressFieldUI : VisualElement
        {
            private const string _elementClass = _rootClass + "__address-field";
            private const string _iconClass = _rootClass + "__address-icon";
            private const string _textClass = _rootClass + "__address-text";

            private VisualElement _icon;
            private VisualElement _text;

            public AddressFieldUI(string address)
            {
                this.AddClass(_elementClass);

                _icon = new VisualElement().AddClass(_iconClass);
                _text = new Label(address).AddClass(_textClass);
                this.AddElements(_icon, _text);
            }
        }

        public class PriceFieldUI : VisualElement
        {
            private const string _elementClass = _rootClass + "__price-field";
            private const string _originalFieldClass = _rootClass + "__original-field";
            private const string _originalPriceClass = _rootClass + "__original-price";
            private const string _discountFieldClass = _rootClass + "__discount-field";
            private const string _discountIconClass = _rootClass + "__discount-icon";
            private const string _discountTextClass = _rootClass + "__discount-text";
            private const string _lastPriceClass = _rootClass + "__last-price";

            private VisualElement _originalField;
            private Label _originalPrice;
            private VisualElement _discountField;
            private VisualElement _discountIcon;
            private Label _discountText;
            private Label _lastPrice;

            public PriceFieldUI(float price, int discount, byte duration, byte roomAmount)
            {
                this.AddClass(_elementClass);

                _originalField = new VisualElement().AddClass(_originalFieldClass);
                this.AddElements(_originalField);

                _originalPrice = new Label($"Only <s>{Extension.Value.ToClosestPrice(price)}$</s>").AddClass(_originalPriceClass);
                _originalField.AddElements(_originalPrice);

                _discountField = new VisualElement().AddClass(_discountFieldClass);
                _originalField.AddElements(_discountField);

                _discountIcon = new VisualElement().AddClass(_discountIconClass);
                _discountField.AddElements(_discountIcon);

                _discountText = new Label($"Discount {discount}%").AddClass(_discountTextClass);
                _discountField.AddElements(_discountText);

                float lastPrice = Extension.Value.ToClosestPrice(price * (1 - discount / 100f));
                string priceText = $"<b><color=#FED1A7>{lastPrice}$</color></b> <size=45>/ {duration} hours • <color=#75caff>Only {roomAmount} room left</color></size>";

                _lastPrice = new Label(priceText).AddClass(_lastPriceClass);
                this.AddElements(_lastPrice);
            }
        }
    }

    public partial class SearchingResultItemUI : VisualElement, IInitializable, IRefreshable
    {
        public static Action<UID> OnSelected { get; set; }

        protected const string _rootClass = "searching-result-item";
        protected const string _previewAreaClass = _rootClass + "__preview-area";
        protected const string _infoAreaClass = _rootClass + "__info-area";
        protected const string _spaceFieldClass = _rootClass + "__space-field";

        private VisualElement _previewArea;
        private HotTagUI _hotTag;
        private VisualElement _infoArea;
        private NameFieldUI _nameField;
        private AddressFieldUI _addressField;
        private VisualElement _spaceField;
        private PriceFieldUI _priceField;

        public SearchingResultItemUI()
        {
            this.AddStyle(Main.Resources.Styles["StyleVariableUI"]);
            this.AddStyle(Main.Resources.Styles["SearchingResultItemUI"]);
            this.AddClass(_rootClass);
            this.RegisterCallback<PointerDownEvent>(OnSelected_ResultItem);

            _previewArea = new VisualElement().AddClass(_previewAreaClass);
            _infoArea = new VisualElement().AddClass(_infoAreaClass);
            this.AddElements(_previewArea, _infoArea);

            _hotTag = new();
            _previewArea.AddElements(_hotTag);

            _nameField = new("Ami House", (4.5f, 120));
            _infoArea.AddElements(_nameField);

            _addressField = new("120 Locust Ln, Schoharie, New York, USA");
            _infoArea.AddElements(_addressField);

            _spaceField = new VisualElement().AddClass(_spaceFieldClass);
            _infoArea.AddElements(_spaceField);

            _priceField = new(20, 35, 1, 5);
            _infoArea.AddElements(_priceField);
        }

        public void Initialize()
        {

        }

        public void Refresh()
        {

        }

        private void OnSelected_ResultItem(PointerDownEvent evt)
        {

        }
    }
}