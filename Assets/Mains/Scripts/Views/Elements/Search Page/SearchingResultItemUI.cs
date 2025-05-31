using System;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using YNL.Utilities.Addons;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
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
    
        public class ProductTypeUI : VisualElement
        {
            private const string _elementClass = _rootClass + "__product-type";
            private const string _iconClass = _rootClass + "__product-icon";

            private VisualElement _icon;

            public ProductTypeUI()
            {
                this.AddClass(_elementClass);

                _icon = new VisualElement().AddClass(_iconClass);
                this.AddElements(_icon);
            }

            public void Apply(Product.Data product)
            {
                _icon.SetBackgroundImage(Main.Resources.Icons[product.Type.ToString()]);
            }
        }

        public class NameFieldUI : VisualElement
        {
            private const string _elementClass = _rootClass + "__name-field";
            private const string _nameTextClass = _rootClass + "__name-text";

            private Label _nameText;

            public NameFieldUI()
            {
                this.SetName("NameField").AddClass(_elementClass);

                _nameText = new Label().SetName("NameText").AddClass(_nameTextClass);
                this.Add(_nameText);
            }

            public void Apply(string name)
            {
                _nameText.SetText(name);
            }
        }
    
        public class CreatorFieldUI : VisualElement
        {
            private const string _elementClass = _rootClass + "__creator-field";
            private const string _creatorAreaClass = _rootClass + "__creator-area";
            private const string _creatorIconClass = _rootClass + "__creator-icon";
            private const string _creatorTextClass = _rootClass + "__creator-text";
            private const string _ratingFieldClass = _rootClass + "__rating-field";
            private const string _ratingIconClass = _rootClass + "__rating-icon";
            private const string _ratingTextClass = _rootClass + "__rating-text";

            private VisualElement _creatorArea;
            private VisualElement _creatorIcon;
            private Label _creatorText;
            private VisualElement _ratingField;
            private VisualElement _ratingIcon;
            private Label _ratingText;

            public CreatorFieldUI()
            {
                this.AddClass(_elementClass);

                _creatorArea = new VisualElement().AddClass(_creatorAreaClass);
                this.AddElements(_creatorArea);

                _creatorIcon = new VisualElement().AddClass(_creatorIconClass);
                _creatorText = new Label().AddClass(_creatorTextClass);
                _creatorArea.AddElements(_creatorIcon, _creatorText);

                _ratingField = new VisualElement().AddClass(_ratingFieldClass);
                this.Add(_ratingField);

                _ratingText = new Label().AddClass(_ratingTextClass);
                _ratingField.Add(_ratingText);

                _ratingIcon = new VisualElement().AddClass(_ratingIconClass);
                _ratingField.Add(_ratingIcon);
            }

            public void Apply(Product.Data product)
            {
                _creatorText.SetText(string.Join(", ", product.Creators));

                string ratingScoreText = product.Review.AverageTotalRating == -1 ? "-" : product.Review.AverageTotalRating.ToString();

                _ratingText.SetText($"<b>{ratingScoreText}</b> ({product.Review.FeebackAmount})");
            }
        }

        public class GenreFieldUI : VisualElement
        {
            private const string _elementClass = _rootClass + "__genre-field";

            public GenreFieldUI()
            {
                this.AddClass(_elementClass);
            }

            public void Apply(Product.Data product)
            {
                this.Clear();

                foreach (var genre in product.Genres)
                {
                    var genreItem = new GenreItemUI(genre.Trim());
                    this.Add(genreItem);
                }
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
            private Label _priceText;
            private VisualElement _discountField;
            private VisualElement _discountIcon;
            private Label _discountText;
            private Label _stockText;

            public PriceFieldUI()
            {
                this.AddClass(_elementClass);

                _originalField = new VisualElement().AddClass(_originalFieldClass);
                this.AddElements(_originalField);

                _priceText = new Label().AddClass(_originalPriceClass);
                _originalField.AddElements(_priceText);

                _discountField = new VisualElement().AddClass(_discountFieldClass);
                _originalField.AddElements(_discountField);

                _discountIcon = new VisualElement().AddClass(_discountIconClass);
                _discountField.AddElements(_discountIcon);

                _discountText = new Label().AddClass(_discountTextClass);
                _discountField.AddElements(_discountText);

                _stockText = new Label().AddClass(_lastPriceClass);
                this.AddElements(_stockText);
            }

            public void Apply(Product.Data product)
            {
                var discount = 20;

                _priceText.SetDisplay(discount > 0 ? DisplayStyle.Flex : DisplayStyle.None);
                _discountField.SetDisplay(discount > 0 ? DisplayStyle.Flex : DisplayStyle.None);

                var lastPrice = product.Price * (1 - discount / 100f);

                _priceText.SetText($"<b><color=#DEF95D>{lastPrice:0.00}$</color></b> <s>{product.Price:0.00}$</s>");
                _discountText.SetText($"-{discount}%");
                _stockText.SetText($"• Only {UnityEngine.Random.Range(10, 50)} copies in stock");
            }
        }

        public class GenreItemUI : Label
        {
            private const string _elementClass = _rootClass + "__genre-item";

            public GenreItemUI(string genre) : base(genre.AddSpace())
            {
                this.AddClass(_elementClass);
            }
        }
    }

    public partial class SearchingResultItemUI : VisualElement, IRefreshable
    {
        public static Action<UID> OnSelected { get; set; }

        protected const string _rootClass = "searching-result-item";
        protected const string _previewAreaClass = _rootClass + "__preview-area";
        protected const string _infoAreaClass = _rootClass + "__info-area";
        protected const string _shortenClass = "shorten";

        private VisualElement _previewArea;
        private ProductTypeUI _productType;
        private VisualElement _infoArea;
        private NameFieldUI _nameField;
        private CreatorFieldUI _creatorField;
        private GenreFieldUI _genreField;
        private PriceFieldUI _priceField;

        private UID _productID;

        public SearchingResultItemUI()
        {
            this.AddStyle(Main.Resources.Styles["StyleVariableUI"]);
            this.AddStyle(Main.Resources.Styles["SearchingResultItemUI"]);
            this.AddClass(_rootClass).SetMarginBottom(50);
            this.RegisterCallback<PointerUpEvent>(OnSelected_ResultItem);

            _previewArea = new VisualElement().AddClass(_previewAreaClass);

            _productType = new();
            _previewArea.AddElements(_productType);

            _infoArea = new VisualElement().AddClass(_infoAreaClass);
            this.AddElements(_previewArea, _infoArea);

            _nameField = new();
            _infoArea.AddElements(_nameField);

            _creatorField = new();
            _infoArea.AddElements(_creatorField);

            _genreField = new();
            _infoArea.AddElements(_genreField);

            _priceField = new();
            _infoArea.AddElements(_priceField);
        }

        public void Refresh()
        {

        }

        public void Apply(UID id)
        {
            _productID = id;

            var product = Main.Database.Products[id];

            _previewArea.ApplyCloudImageAsync(id.GetImageURL());

            _nameField.Apply(product.Title);
            _creatorField.Apply(product);
            _priceField.Apply(product);
            _productType.Apply(product);
            _genreField.Apply(product);

            this.EnableClass(product.Type == Product.Type.CD || product.Type == Product.Type.LP, _shortenClass);
        }

        private void OnSelected_ResultItem(PointerUpEvent evt)
        {
            Main.Runtime.SelectedProduct = _productID;

            Marker.OnViewPageSwitched?.Invoke(ViewType.InformationViewMainPage, true, true);
        }
    }
}