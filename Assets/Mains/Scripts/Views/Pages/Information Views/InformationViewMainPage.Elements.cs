using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class InformationViewMainPage
    {
        public class PriceField
        {
            public Action OnOpenTimeRangePage;

            private VisualElement _timeField;
            private VisualElement _timeIcon;
            private Label _timeText;
            private Label _originalPrice;
            private Label _lastPrice;
            private Button _chooseButton;

            private UID _hotelID;

            public PriceField(VisualElement root)
            {
                var bottomBar = root.Q("BottomBar");

                _timeField = bottomBar.Q("TimeField");
                _timeField.RegisterCallback<PointerUpEvent>(OnClicked_TimeField);

                _timeIcon = _timeField.Q("Icon");

                _timeText = _timeField.Q("Text") as Label;

                var priceArea = bottomBar.Q("PriceArea");

                _originalPrice = priceArea.Q("PriceField").Q("OriginalPrice") as Label;

                _lastPrice = priceArea.Q("PriceField").Q("LastPrice") as Label;

                _chooseButton = priceArea.Q("ChooseButton") as Button;
                _chooseButton.RegisterCallback<PointerUpEvent>(OnClicked_ChooseButton);
            }

            public void Apply(UID hotelID)
            {
                _hotelID = hotelID;

            }

            private void OnClicked_TimeField(PointerUpEvent evt)
            {
                OnOpenTimeRangePage?.Invoke();
            }

            private void OnClicked_ChooseButton(PointerUpEvent evt)
            {
                //Marker.OnViewPageSwitched?.Invoke(ViewType.InformationViewRoomPage, true, false);
                Marker.OnHotelRoomsDisplayed?.Invoke(_hotelID);
            }
        }

        public class ImageView
        {
            private List<VisualElement> _roomImages = new();
            private VisualElement _fadeItem;
            private Label _amountText;

            public ImageView(VisualElement imageView)
            {
                for (byte i = 0; i < 4; i++) _roomImages.Add(imageView.Q($"Image{i}"));

                _fadeItem = imageView.Q("Image3").Q("Fade");
                _amountText = imageView.Q("Image3").Q("Text") as Label;
            }

            public async UniTaskVoid Apply()
            {

            }
        }

        public class NameView
        {
            private VisualElement _badgeField;
            private Label _nameText;
            private Label _addressText;

            public NameView(VisualElement contentContainer)
            {
                _badgeField = contentContainer.Q("BadgeField");
                _nameText = contentContainer.Q("NameText") as Label;
                _addressText = contentContainer.Q("AddressText") as Label;
            }

            public void Apply(string name, string address)
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
            private Label _emptyLabel;
            private VisualElement _ellipsis;

            private List<FeedbackPreviewItemUI> _feedbackItems = new();

            public ReviewView(VisualElement contentContainer)
            {
                var reviewView = contentContainer.Q("ReviewView").AddStyle(Main.Resources.Styles["GlobalStyleUI"]);
                var scoreField = reviewView.Q("ScoreField");

                _ratingText = scoreField.Q("Rating") as Label;
                _reviewAmount = scoreField.Q("Amount") as Label;
                _seeMoreButton = scoreField.Q("SeeMore") as Label;
                _seeMoreButton.RegisterCallback<PointerUpEvent>(OnClicked_SeeMoreButton);

                _emptyLabel = reviewView.Q("EmptyLabel") as Label;

                _reviewScroll = reviewView.Q("ReviewScroll") as ScrollView;
                _reviewScroll.Clear();

                for (byte i = 0; i < 5; i++)
                {
                    var feedbackItem = new FeedbackPreviewItemUI();
                    _feedbackItems.Add(feedbackItem);
                    _reviewScroll.AddElements(feedbackItem);
                }

                _ellipsis = new VisualElement().AddClass("review-view-ellipsis").SetBackgroundImage(Main.Resources.Icons["Ellipsis"]);
                _reviewScroll.AddElements(_ellipsis);
            }

            public void Apply(UID id)
            {
                var unit = Main.Database.Books[id];
                var review = unit.Review;
                var feedbacks = unit.Review.Feedbacks.Keys.ToArray();

                _reviewAmount.SetText($"({review.FeebackAmount} reviews)");

                bool emptyFeedback = review.FeebackAmount == 0;

                _emptyLabel.SetDisplay(emptyFeedback ? DisplayStyle.Flex : DisplayStyle.None);
                _reviewScroll.SetDisplay(emptyFeedback ? DisplayStyle.None : DisplayStyle.Flex);

                if (!emptyFeedback)
                {
                    for (byte i = 0; i < 5; i++)
                    {
                        if (i < feedbacks.Length)
                        {
                            _feedbackItems[i].SetDisplay(DisplayStyle.Flex);
                            _feedbackItems[i].Apply(id, feedbacks[i]);
                        }
                        else
                        {
                            _feedbackItems[i].SetDisplay(DisplayStyle.None);
                        }
                    }
                }
            }

            private void OnClicked_SeeMoreButton(PointerUpEvent evt)
            {
                Marker.OnViewPageSwitched?.Invoke(ViewType.InformationViewReviewPage, true, false);
            }
        }

        public class DescriptionField
        {
            private Label _descriptionText;

            private bool _isExpanded = false;

            public DescriptionField(VisualElement contentContainer)
            {
                _descriptionText = contentContainer.Q("DescriptionField").Q("DescriptionText") as Label;
                _descriptionText.RegisterCallback<PointerUpEvent>(OnClicked_DescriptionText);
            }

            public void Apply(string description)
            {
                _descriptionText.SetText(description == string.Empty ? "<color=#808080>No description!</color>" : description);
            }

            private void OnClicked_DescriptionText(PointerUpEvent evt)
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
    }
}