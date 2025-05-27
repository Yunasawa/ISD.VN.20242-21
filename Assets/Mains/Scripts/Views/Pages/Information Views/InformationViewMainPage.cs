using System;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class InformationViewMainPage : ViewPageUI
    {
        private VisualElement _backButton;
        private VisualElement _favoriteButton;
        private VisualElement _shareButton;

        private PriceField _priceField;

        private ImageView _imageView;
        private NameView _nameView;
        private ReviewView _reviewView;
        private DescriptionField _descriptionField;

        private UID _hotelID;

        protected override void VirtualAwake()
        {
            Marker.OnHotelInformationDisplayed += OnHotelInformationDisplayed;
        }

        private void OnDestroy()
        {
            Marker.OnHotelInformationDisplayed -= OnHotelInformationDisplayed;
        }

        protected override void Collect()
        { 
            _backButton = Root.Q("TopBar").Q("BackButton");
            _backButton.RegisterCallback<PointerUpEvent>(OnClicked_BackButton);

            _favoriteButton = Root.Q("TopBar").Q("FavoriteButton");
            _favoriteButton.RegisterCallback<PointerUpEvent>(OnClicked_FavoriteButton);

            _shareButton = Root.Q("TopBar").Q("ShareButton");

            _priceField = new(Root);

            var contentContainer = Root.Q("ContentScroll").Q("unity-content-container");

            _imageView = new(contentContainer.Q("ImageView"));

            _nameView = new(contentContainer.Q("NameView"));

            _reviewView = new(contentContainer);

            _descriptionField = new(contentContainer);
        }

        private void OnClicked_BackButton(PointerUpEvent evt)
        {
            Marker.OnViewPageSwitched?.Invoke(ViewType.MainViewHomePage, true, false);
        }

        private void OnClicked_FavoriteButton(PointerUpEvent evt)
        {

        }

        private void OnTimeRangeSubmitted()
        {      
            _priceField.Apply(_hotelID);
        }

        private void OnHotelInformationDisplayed(UID id, bool isSearchResult)
        {

        }
    }
}