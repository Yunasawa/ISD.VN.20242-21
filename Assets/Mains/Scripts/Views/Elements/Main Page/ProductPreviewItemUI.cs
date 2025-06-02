using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class ProductPreviewItemUI : VisualElement, IRefreshable
    {
        private const string _rootClass = "hotel-preview-item";
        private const string _previewImageClass = _rootClass + "__preview-image";
        private const string _productTypeClass = _rootClass + "__product-type";
        private const string _productIconClass = _rootClass + "__product-icon";
        private const string _productDurationClass = _rootClass + "__product-duration";
        private const string _previewInfoClass = _rootClass + "__preview-info";
        private const string _nameLabelClass = _rootClass + "__name-label";
        private const string _creatorFieldClass = _rootClass + "__creator-field";
        private const string _creatorIconClass = _rootClass + "__creator-icon";
        private const string _creatorTextClass = _rootClass + "__creator-text";
        private const string _priceFieldClass = _rootClass + "__price-field";
        private const string _priceTextClass = _rootClass + "__price-text";
        private const string _ratingFieldClass = _rootClass + "__rating-field";
        private const string _ratingIconClass = _rootClass + "__rating-icon";
        private const string _ratingTextClass = _rootClass + "__rating-text";
        private const string _spaceClass = _rootClass + "__space";
        private const string _miniClass = "mini";

        private VisualElement _previewImage;
        private VisualElement _productType;
        private VisualElement _productIcon;
        private Label _productDuration;
        private VisualElement _previewInfo;
        private Label _nameLabel;
        private VisualElement _creatorField;
        private VisualElement _creatorIcon;
        private Label _creatorText;
        private VisualElement _space;
        private VisualElement _priceField;
        private Label _priceText;
        private VisualElement _ratingField;
        private VisualElement _ratingIcon;
        private Label _ratingText;

        private UID _uid;

        public ProductPreviewItemUI(UID productID, bool isMini = false)
        {
            _uid = productID;

            Initialize(isMini);
            Apply(productID);
        }

        private void Initialize(bool isMini)
        {
            this.AddStyle(Main.Resources.Styles["StyleVariableUI"]);
            this.AddStyle(Main.Resources.Styles["ProductPreviewItemUI"]);
            this.AddClass(_rootClass).EnableClass(isMini, _miniClass);
            this.RegisterCallback<PointerUpEvent>(OnClicked_PreviewItem);

            _previewImage = new VisualElement().AddClass(_previewImageClass);
            this.AddElements(_previewImage);

            _productType = new VisualElement().AddClass(_productTypeClass);
            _previewImage.AddElements(_productType);

            _productIcon = new VisualElement().AddClass(_productIconClass);
            _productType.AddElements(_productIcon);

            _productDuration = new Label().AddClass(_productDurationClass);
            _previewImage.AddElements(_productDuration);

            _previewInfo = new VisualElement().AddClass(_previewInfoClass);
            this.AddElements(_previewInfo);

            _nameLabel = new Label().AddClass(_nameLabelClass);
            _previewInfo.AddElements(_nameLabel);

            _creatorField = new VisualElement().AddClass(_creatorFieldClass);
            if (!isMini) _previewInfo.AddElements(_creatorField);

            _creatorIcon = new VisualElement().AddClass(_creatorIconClass);
            _creatorField.AddElements(_creatorIcon);

            _creatorText = new Label().AddClass(_creatorTextClass);
            _creatorField.AddElements(_creatorText);

            _space = new VisualElement().AddClass(_spaceClass);
            _previewInfo.AddElements(_space);

            _priceField = new VisualElement().AddClass(_priceFieldClass);
            _previewInfo.AddElements(_priceField);

            _priceText = new Label().AddClass(_priceTextClass);
            _priceField.AddElements(_priceText);

            _ratingField = new VisualElement().AddClass(_ratingFieldClass);
            if (isMini) _previewInfo.Insert(1, _ratingField);
            else _priceField.AddElements(_ratingField);

            _ratingIcon = new VisualElement().AddClass(_ratingIconClass);
            _ratingField.AddElements(_ratingIcon);

            _ratingText = new Label().AddClass(_ratingTextClass);
            _ratingField.AddElements(_ratingText);
        }

        private void Apply(UID id)
        {
            var product = Main.Database.Products[id];
            int discount = 45;

            _previewImage.ApplyCloudImageAsync(id.GetImageURL());

            _productIcon.SetBackgroundImage(Main.Resources.Icons[product.Type.ToString()]);
            _productDuration.SetText(id.GetDurationText());

            _nameLabel.text = product.Title;
            _creatorText.text = string.Join(", ", product.Creators);

            var priceText = $"<b><color=#DEF95D>${product.Price * (1 - (discount / 100f)):0.00}</color></b>";
            if (discount > 0) priceText += $" <s>${product.Price:0.00}</s>";
            if (product.IsFree) priceText = "<b><color=#DEF95D>FREE</color></b>";

            _priceText.SetText(priceText);

            _ratingText.text = $"<b>{product.Review.AverageTotalRating}</b> ({product.Review.FeebackAmount})";
        }

        public void Refresh()
        {
            Apply(_uid);
        }

        public ProductPreviewItemUI SetAsLastItem(bool set = true)
        {
            if (!set) return this;

            this.SetMarginRight(50);

            return this;
        }

        private void OnClicked_PreviewItem(PointerUpEvent evt)
        {
            Main.Runtime.SelectedProduct = _uid;
            Marker.OnPageNavigated?.Invoke(ViewType.InformationViewMainPage, true, true);
            Marker.OnHotelFacilitiesDisplayed?.Invoke(_uid);
        }
    }
}