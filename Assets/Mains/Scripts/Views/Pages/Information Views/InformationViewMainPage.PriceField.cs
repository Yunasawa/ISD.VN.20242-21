using UnityEngine.UIElements;

namespace YNL.Checkotel
{
    public partial class InformationViewMainPage
    {
        public enum PriceFieldType : byte
        {
            HourlyTime, OvernightTime, DailyTime
        }

        public class PriceField : ICollectible
        {
            private VisualElement _root;

            private VisualElement _timeField;
            private VisualElement _timeIcon;
            private Label _timeText;
            private Label _originalPrice;
            private Label _lastPrice;
            private Button _chooseButton;

            public PriceField(VisualElement root)
            {
                _root = root;

                Collect();
            }

            public void Collect()
            {
                var bottomBar = _root.Q("BottomBar");

                _timeField = bottomBar.Q("TimeField");

                _timeIcon = _timeField.Q("Icon");

                _timeText = _timeField.Q("Text") as Label;

                var priceArea = bottomBar.Q("PriceArea");

                _originalPrice = priceArea.Q("PriceField").Q("OriginalPrice") as Label;

                _lastPrice = priceArea.Q("PriceField").Q("LastPrice") as Label;

                _chooseButton = _lastPrice.Q("ChooseButton") as Button;
            }
        }
    }
}