using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.Checkotel
{
    public partial class InformationViewReviewPage
    {
        public class RatingView : IRefreshable
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

                Refresh();
            }

            public void Refresh()
            {
                _ratingText.SetText(((4.2f + 4.5f + 4.3f) / 3).ToString("0.0"));
                _rankText.SetText("Exceptional");
                _amountText.SetText("123 reviews");

                _cleanlinessScore.Text.SetText("4.2");
                _cleanlinessScore.Bar.SetWidth(4.2f / 5 * 100, true);

                _facilitiesScore.Text.SetText("4.5");
                _facilitiesScore.Bar.SetWidth(4.5f / 5 * 100, true);

                _serviceScore.Text.SetText("4.3");
                _serviceScore.Bar.SetWidth(4.3f / 5 * 100, true);
            }
        }
    }
}