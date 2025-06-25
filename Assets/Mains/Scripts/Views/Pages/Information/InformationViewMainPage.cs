using System;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;
using static YNL.JAMOS.InformationViewMainPage;

namespace YNL.JAMOS
{
    public partial class InformationViewMainPage : PageBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        private VisualElement _backButton;
        private VisualElement _shareButton;

        private PriceField _priceField;
        private StreamField _streamField;

        private VisualElement _imageView;
        private NameView _nameView;
        private AmountField _amountField;
        private GenreField _genreField;
        private ReviewView _reviewView;
        private DescriptionField _descriptionField;

        private UID _uid => Main.Runtime.SelectedProduct;

        protected override void Construct()
        { 
            _backButton = Root.Q("TopBar").Q("BackButton");
            _backButton.RegisterCallback<PointerUpEvent>(OnClicked_BackButton);

            _shareButton = Root.Q("TopBar").Q("ShareButton");

            _priceField = new(Root.Q("BottomBar").Q("PriceField"));

            _streamField = new(Root.Q("BottomBar").Q("StreamField"), _audioSource);

            var contentContainer = Root.Q("ContentScroll").Q("unity-content-container");

            _imageView = contentContainer.Q("ImageView");

            _nameView = new(contentContainer.Q("NameView"));

            _amountField = new(contentContainer.Q("QuantityField"));

            _genreField = new(contentContainer.Q("GenreField"));

            _reviewView = new(contentContainer);

            _descriptionField = new(contentContainer);
        }

        protected override void Refresh()
        {
            if (!Main.Database.Products.TryGetValue(_uid, out var product)) return;

            _imageView.ApplyCloudImageAsync(_uid);
            _nameView.Apply(product);
            _amountField.Apply(product);
            _genreField.Apply(product);
            _descriptionField.Apply(_uid);
            _reviewView.Apply(_uid);

            _priceField.Apply(_uid);

            _streamField.Field.SetDisplay(product.HasStreamer ? DisplayStyle.Flex : DisplayStyle.None);
            if (product.HasStreamer) _streamField.Apply(_uid);
        }

        private void OnClicked_BackButton(PointerUpEvent evt)
        {
            Marker.OnPageBacked?.Invoke(true, false);
        }
    }
}