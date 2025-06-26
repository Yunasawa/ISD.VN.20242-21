using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class InformationReview
    {
        private class View : PageView
        {
            private InformationReview _b;

            private VisualElement _backButton;
            private RatingView _ratingView;
            private ScrollView _reviewScroll;
            private ListView _reviewList;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as InformationReview;

                _b.OnDataRefreshed += OnDataRefreshed;
            }

            public override void Collect(VisualElement root)
            {
                _backButton = root.Q("TopBar").Q("LabelField");
                _backButton.RegisterCallback<PointerUpEvent>(OnClicked_BackButton);

                _ratingView = new(root.Q("TopBar").Q("RatingView"));

                _reviewScroll = root.Q("ContentScroll") as ScrollView;
                root.Remove(_reviewScroll);

                _reviewList = root.Q("ContentList") as ListView;
            }

            public override void Begin()
            {
                _reviewList.Q("unity-content-container").SetFlexGrow(1);
                _reviewList.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
                _reviewList.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
                _reviewList.makeItem = () => new FeedbackResultItemUI();
            }

            private void OnClicked_BackButton(PointerUpEvent evt)
            {
                Marker.OnPageNavigated?.Invoke(ViewType.InformationViewMainPage, true, false);
            }

            private void OnDataRefreshed(UID id, Product.Data product, List<UID> feedbackIDs)
            {
                _ratingView.Apply(product);

                bool emptyFeedback = product.Review.FeebackAmount == 0;

                if (!emptyFeedback)
                {
                    _reviewList.bindItem = (element, index) =>
                    {
                        var item = element as FeedbackResultItemUI;
                        if (index < feedbackIDs.Count)
                        {
                            item.Apply(id, feedbackIDs[index]);
                        }
                    };
                    _reviewList.RebuildListView(feedbackIDs);
                }
            }
        }
    }

    public partial class InformationReview
    {
        public class RatingView
        {
            private Label _ratingText;
            private Label _rankText;
            private Label _amountText;
            private (VisualElement bar, Label text) _statusScore;
            private (VisualElement bar, Label text) _qualityScore;
            private (VisualElement bar, Label text) _usabilityScore;

            public RatingView(VisualElement view)
            {
                var ratingField = view.Q("RatingField");
                _ratingText = ratingField.Q("RatingText") as Label;
                _rankText = ratingField.Q("RankText") as Label;
                _amountText = ratingField.Q("AmountText") as Label;

                var cleanlinessScore = view.Q("ScoreView").Q("CleanlinessScore");
                var facilitiesScore = view.Q("ScoreView").Q("FacilitiesScore");
                var serviceScore = view.Q("ScoreView").Q("ServiceScore");

                _statusScore = (cleanlinessScore.Q("ScoreLine").Q("LineFill"), cleanlinessScore.Q("ScoreText") as Label);
                _qualityScore = (facilitiesScore.Q("ScoreLine").Q("LineFill"), facilitiesScore.Q("ScoreText") as Label);
                _usabilityScore = (serviceScore.Q("ScoreLine").Q("LineFill"), serviceScore.Q("ScoreText") as Label);
            }

            public void Apply(Product.Data product)
            {
                var review = product.Review;

                _ratingText.SetText(review.AverageTotalRating.ToString());
                _rankText.SetText(review.ToRankText());
                _amountText.SetText($"{review.FeebackAmount} feedbacks");

                SetBarScore(_statusScore, review.AverageRating(RatingType.Status));
                SetBarScore(_qualityScore, review.AverageRating(RatingType.Quality));
                SetBarScore(_usabilityScore, review.AverageRating(RatingType.Usability));
            }

            private void SetBarScore((VisualElement bar, Label text) score, ReviewRating rating)
            {
                score.text.SetText(rating.ToString());
                score.bar.SetWidth(rating == -1 ? 0 : rating / 5 * 100, true);
            }
        }
    }
}