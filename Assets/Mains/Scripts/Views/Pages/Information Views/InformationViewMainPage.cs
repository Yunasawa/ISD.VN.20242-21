using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.Checkotel
{
    public partial class InformationViewMainPage
    {
        public class NameView
        {
            private VisualElement _nameView;
            private VisualElement _badgeField;
            private Label _nameText;
            private Label _addressText;

            public NameView(VisualElement contentContainer)
            {
                _nameView = contentContainer.Q("NameView");
                _badgeField = contentContainer.Q("BadgeField");
                _nameText = contentContainer.Q("NameText") as Label;
                _addressText = contentContainer.Q("AddressText") as Label;
            }

            public void Update(string name, string address)
            {
                _nameText.SetText(name);
                _addressText.SetText(address);
            }
        }
    
        public class ReviewView
        {
            private Label _ratingText;
            private Label _reviewAmount;
            private Label _seeMoreButton;
            private ScrollView _reviewScroll;

            public ReviewView(VisualElement contentContainer)
            {
                var reviewView = contentContainer.Q("ReviewView");
                var scoreField = reviewView.Q("ScoreField");

                _ratingText = scoreField.Q("Rating") as Label;
                _reviewAmount = scoreField.Q("Amount") as Label;
                _seeMoreButton = scoreField.Q("SeeMore") as Label;

                _reviewScroll = reviewView.Q("ReviewScroll") as ScrollView;
            }

            public void Update(float rating, int amount)
            {
                _ratingText.SetText(rating.ToString());
                _reviewAmount.SetText(amount.ToString());
            }
        }
    
        public class FacilityView
        {
            private Label _seeMoreButton;
            private VisualElement _facilityField;

            public FacilityView(VisualElement contentContainer)
            {
                var facilityView = contentContainer.Q("FacilityView");

                _seeMoreButton = facilityView.Q("LabelField").Q("SeeMore") as Label;
                _facilityField = facilityView.Q("FacilityField");
            }
        }
    
        public class DescriptionField
        {
            private Label _descriptionText;

            private bool _isExpanded = false;

            public DescriptionField(VisualElement contentContainer)
            {
                _descriptionText = contentContainer.Q("DescriptionField").Q("DescriptionText") as Label;
                _descriptionText.RegisterCallback<PointerDownEvent>(OnClicked_DescriptionText);
            }

            public void Update(string description)
            {
                _descriptionText.SetText(description);
            }

            private void OnClicked_DescriptionText(PointerDownEvent evt)
            {
                _isExpanded = !_isExpanded;

                if (_isExpanded)
                {
                    _descriptionText.SetMaxHeight(StyleKeyword.Auto);
                }
                else
                {
                    _descriptionText.SetMaxHeight(175);
                }
            }
        }
    
        public class TimeField
        {

        }
    }

    public partial class InformationViewMainPage : MonoBehaviour, ICollectible, IInitializable
    {
        private VisualElement _root;

        private VisualElement _backButton;
        private VisualElement _favoriteButton;
        private VisualElement _shareButton;
        private PriceField _priceField;
        private VisualElement _imageView;
        private NameView _nameView;
        private ReviewView _reviewView;
        private FacilityView _facilityView;
        private DescriptionField _descriptionField;
        private VisualElement _timeField;
        private VisualElement _hotelPolicy;
        private VisualElement _cancellationPolicy;

        private void Awake()
        {
            Marker.OnSystemStart += Collect;
        }

        private void OnDestroy()
        {
            Marker.OnSystemStart -= Collect;
        }

        public void Collect()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _backButton = _root.Q("TopBar").Q("BackButton");
            _favoriteButton = _root.Q("TopBar").Q("FavoriteButton");
            _shareButton = _root.Q("TopBar").Q("ShareButton");

            _priceField = new(_root);

            var contentContainer = _root.Q("ContentScroll").Q("unity-content-container");

            _imageView = contentContainer.Q("ImageView");

            _nameView = new(contentContainer);

            _reviewView = new(contentContainer);

            _facilityView = new(contentContainer);

            _descriptionField = new(contentContainer);

            _timeField = contentContainer.Q("TimeField");

            _hotelPolicy = contentContainer.Q("HotelPolicy");

            _cancellationPolicy = contentContainer.Q("CancellationPolicy");

        }

        public void Initialize()
        {
            
        }
    }
}