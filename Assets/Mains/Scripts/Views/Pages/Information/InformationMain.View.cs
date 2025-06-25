using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class InformationMain
    {
        private class View : PageView
        {

            public AudioSource AudioSource;

            private InformationMain _b;

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

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as InformationMain;
            }

            public override void Collect(VisualElement root)
            {
                _backButton = root.Q("TopBar").Q("BackButton");
                _backButton.RegisterCallback<PointerUpEvent>(OnClicked_BackButton);

                _shareButton = root.Q("TopBar").Q("ShareButton");

                _priceField = new(root.Q("BottomBar").Q("PriceField"));

                _streamField = new(root.Q("BottomBar").Q("StreamField"), AudioSource);

                var contentContainer = root.Q("ContentScroll").Q("unity-content-container");

                _imageView = contentContainer.Q("ImageView");

                _nameView = new(contentContainer.Q("NameView"));

                _amountField = new(contentContainer.Q("QuantityField"));

                _genreField = new(contentContainer.Q("GenreField"));

                _reviewView = new(contentContainer);

                _descriptionField = new(contentContainer);
            }

            public override void Refresh()
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
                _b.OnBackButtonClicked?.Invoke();
            }
        }
    }

    public partial class InformationMain
    {
        public class PriceField
        {
            private UID _accountID => Main.Runtime.Data.AccountID;
            private SerializableDictionary<UID, List<UID>> _cartedProducts => Main.Runtime.Data.CartedProducts;
            private SerializableDictionary<UID, List<UID>> _productCollection => Main.Runtime.Data.ProductCollection;

            public Action OnOpenTimeRangePage;

            private Label _originalPrice;
            private Label _lastPrice;
            private Button _addToCartButton;

            private UID _uid;

            public PriceField(VisualElement field)
            {
                _originalPrice = field.Q("PriceArea").Q("OriginalPrice") as Label;

                _lastPrice = field.Q("PriceArea").Q("LastPrice") as Label;

                _addToCartButton = field.Q("AddToCartButton") as Button;
                _addToCartButton.RegisterCallback<PointerUpEvent>(OnClicked_ChooseButton);
            }

            public void Apply(UID id)
            {
                _uid = id;
                var product = Main.Database.Products[id];

                var discounrt = 20;

                _originalPrice.SetDisplay(discounrt > 0 && product.IsFree == false ? DisplayStyle.Flex : DisplayStyle.None);
                _originalPrice.SetText($"From <color=#7f7f7f><s>${product.Price:0.00}</s></color>");
                _lastPrice.SetText(product.IsFree ? "FREE" : $"${product.Price * (1 - discounrt / 100f):0.00}");

                var isCarted = false;

                if (_cartedProducts.TryGetValue(_accountID, out var products))
                {
                    isCarted = products.Contains(_uid);
                }

                _uid.SetCartButtonStatus(_addToCartButton);
            }

            private void OnClicked_ChooseButton(PointerUpEvent evt)
            {
                var existCart = _cartedProducts.TryGetValue(_accountID, out var cart);
                var existCollection = _productCollection.TryGetValue(_accountID, out var collection);

                var product = Main.Database.Products[_uid];

                var isCollected = existCollection ? collection.Contains(_uid) : false;

                if (product.IsFree && isCollected == false)
                {
                    if (existCollection)
                    {
                        collection.Add(_uid);
                    }
                    else
                    {
                        _productCollection.Add(_accountID, new() { _uid });
                    }
                }
                else if (existCart)
                {
                    if (cart.Remove(_uid) == false)
                    {
                        cart.Add(_uid);
                    }
                }
                else
                {
                    _cartedProducts.Add(_accountID, new() { _uid });
                }

                _uid.SetCartButtonStatus(_addToCartButton);

                Marker.OnRuntimeSavingRequested?.Invoke();
            }
        }


        public class NameView
        {
            private VisualElement _badgeField;
            private VisualElement _productType;
            private Label _nameText;
            private Label _creatorText;

            public NameView(VisualElement contentContainer)
            {
                _badgeField = contentContainer.Q("BadgeField");
                _productType = contentContainer.Q("NameField").Q("ProductType");
                _nameText = contentContainer.Q("NameField").Q("NameText") as Label;
                _creatorText = contentContainer.Q("CreatorText") as Label;
            }

            public void Apply(Product.Data product)
            {
                _productType.SetBackgroundImage(Main.Resources.Icons[product.Type.ToString()]);
                _nameText.SetText(product.Title);
                _creatorText.SetText($"by <b>{string.Join(", ", product.Creators)}</b>");
            }
        }

        public class AmountField
        {
            private Label _soldText;
            private Label _quantityText;

            public AmountField(VisualElement field)
            {
                _soldText = field.Q("SoldText") as Label;
                _quantityText = field.Q("QuantityText") as Label;
            }

            public void Apply(Product.Data product)
            {
                _soldText.SetText($"<b>Sold amount:</b> <color=#DEF95D>{product.SoldAmount}</color>");
                _quantityText.SetText($"<b>Available in stock:</b> <color=#DEF95D>{product.Quantity}</color>");
            }
        }

        public class GenreField
        {
            private VisualElement _genreContainer;

            public GenreField(VisualElement container)
            {
                _genreContainer = container.Q("GenreContainer");
            }

            public void Apply(Product.Data product)
            {
                _genreContainer.Clear();

                foreach (var genre in product.Genres)
                {
                    var genreButton = new GenreButtonItemUI();
                    genreButton.Apply(genre);
                    _genreContainer.AddElements(genreButton);
                }
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
                var product = Main.Database.Products[id];
                var review = product.Review;
                var feedbacks = product.Review.Feedbacks.Keys.ToArray();

                _reviewAmount.SetText($"({review.FeebackAmount} reviews)");

                bool emptyFeedback = review.FeebackAmount == 0;

                _emptyLabel.SetDisplay(emptyFeedback ? DisplayStyle.Flex : DisplayStyle.None);
                _reviewScroll.SetDisplay(emptyFeedback ? DisplayStyle.None : DisplayStyle.Flex);

                _ratingText.SetText(review.AverageTotalRating.ToString());

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
                Marker.OnPageNavigated?.Invoke(ViewType.InformationViewReviewPage, true, true);
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

            public void Apply(UID id)
            {
                var description = id.GetDescriptionText();

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

        public class StreamField
        {
            public VisualElement Field;
            private VisualElement _playButton;
            private Slider _progressSlider;
            private Label _timeLabel;

            private AudioSource _audioSource;
            private bool _isPlaying = false;
            private float _lastRecord = 0;

            public StreamField(VisualElement field, AudioSource source)
            {
                Field = field;
                _audioSource = source;

                _playButton = field.Q("PlayButton");
                _playButton.RegisterCallback<PointerUpEvent>(OnClicked_PlayButton);

                _progressSlider = field.Q("ProgressSlider") as Slider;
                _progressSlider.RegisterValueChangedCallback(OnValueChanged_ProgressSlider);

                _timeLabel = field.Q("TimeLabel") as Label;
            }

            public void Apply(UID id)
            {
                id.LoadCloudAudioAsync(ApplyAudioClip);

                _progressSlider.value = 0;
                _timeLabel.SetText(id.GetDurationText());
            }

            private void OnValueChanged_ProgressSlider(ChangeEvent<float> evt)
            {
                if (_audioSource.clip != null)
                {
                    _audioSource.time = evt.newValue;

                    float remainTime = _audioSource.clip.length - _audioSource.time;
                    _timeLabel.SetTimeText(remainTime);
                }
            }

            private void OnClicked_PlayButton(PointerUpEvent evt)
            {
                _isPlaying = !_isPlaying;

                if (_isPlaying)
                {
                    _playButton.SetBackgroundImage(Main.Resources.Icons["Pause"]);
                    _audioSource.Play();
                    UpdateSliderAsync().Forget();
                }
                else
                {
                    _playButton.SetBackgroundImage(Main.Resources.Icons["Play"]);
                    _lastRecord = _audioSource.time;
                    _audioSource.Pause();
                }
            }

            private void ApplyAudioClip(AudioClip clip)
            {
                _audioSource.clip = clip;

                _timeLabel.SetTimeText(clip.length);
                _progressSlider.lowValue = 0;
                _progressSlider.highValue = _audioSource.clip.length;
            }

            private async UniTaskVoid UpdateSliderAsync()
            {
                while (true)
                {
                    _progressSlider.value = _audioSource.time;

                    float remainTime = _audioSource.clip.length - _audioSource.time;
                    _timeLabel.SetTimeText(remainTime);

                    if (!_audioSource.isPlaying)
                    {
                        _progressSlider.value = _lastRecord;
                        _timeLabel.SetTimeText(remainTime);
                        _playButton.SetBackgroundImage(Main.Resources.Icons["Play"]);
                        return;
                    }

                    await UniTask.Delay(100);
                }
            }
        }
    }
}