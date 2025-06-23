using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class OrderViewResultPageUI : ViewPageUI
    {
        private Label _codeLabel;

        protected override void VirtualAwake()
        {
            Marker.OnOrderCodeCreated += OnOrderCodeCreated;
        }

        private void OnDestroy()
        {
            Marker.OnOrderCodeCreated -= OnOrderCodeCreated;
        }

        protected override void Collect()
        {
            var informationField = Root.Q("InformationField");

            _codeLabel = informationField.Q<Label>("CodeLabel");

            var buttonField = informationField.Q("ButtonField");

            var homePageButton = buttonField.Q<Button>("HomePageButton");
            homePageButton.clicked += OnClicked_HomePageButton;

            var orderPageButton = buttonField.Q<Button>("OrderPageButton");
            orderPageButton.clicked += OnClicked_OrderPageButton;
        }

        private void OnClicked_HomePageButton()
        {
            Marker.OnPageNavigated?.Invoke(ViewType.MainViewHomePage, true, false);
        }

        private void OnClicked_OrderPageButton()
        {
            Marker.OnPageNavigated?.Invoke(ViewType.MainViewOrderPage, true, true);
        }

        private void OnOrderCodeCreated(string code)
        {
            _codeLabel.SetText(code);
        }
    }
}