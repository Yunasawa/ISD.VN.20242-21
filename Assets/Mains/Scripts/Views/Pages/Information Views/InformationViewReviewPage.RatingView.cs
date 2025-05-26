using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class InformationViewReviewPage
    {
        public class RatingView
        {
            private Label _ratingText;
            private Label _rankText;
            private Label _amountText;
            private (VisualElement Bar, Label Text) _cleanlinessScore;
            private (VisualElement Bar, Label Text) _facilitiesScore;
            private (VisualElement Bar, Label Text) _serviceScore;

            public RatingView(VisualElement view)
            {
                var ratingField = view.Q("RatingField");
                _ratingText = ratingField.Q("RatingText") as Label;
                _rankText = ratingField.Q("RankText") as Label;
                _amountText = ratingField.Q("AmountText") as Label;

                var cleanlinessScore = view.Q("ScoreView").Q("CleanlinessScore");
                var facilitiesScore = view.Q("ScoreView").Q("FacilitiesScore");
                var serviceScore = view.Q("ScoreView").Q("ServiceScore");

                _cleanlinessScore = (cleanlinessScore.Q("ScoreLine").Q("LineFill"), cleanlinessScore.Q("ScoreText") as Label);
                _facilitiesScore = (facilitiesScore.Q("ScoreLine").Q("LineFill"), facilitiesScore.Q("ScoreText") as Label);
                _serviceScore = (serviceScore.Q("ScoreLine").Q("LineFill"), serviceScore.Q("ScoreText") as Label);
            }

            public void Apply()
            {

            }
        }
    }
}