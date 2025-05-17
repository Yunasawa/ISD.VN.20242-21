using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.Checkotel
{
    public enum HomeNavigationType : byte
    {
        Home, Favorite, Suggestion, Account
    }

    public class HomeNavigationButton : VisualElement
    {
        private const string _rootClass = "home-navigation-button";
        private const string _iconClass = _rootClass + "__icon";
        private const string _labelClass = _rootClass + "__label";
        private const string _selected = "selected";

        private VisualElement _icon;
        private Label _label;

        private bool _isSelected;
        private HomeNavigationType _type;

        public HomeNavigationButton(Texture2D icon, string label, bool isSelected, HomeNavigationType type)
        {
            _isSelected = isSelected;
            _type = type;

            this.AddStyle(Main.Resources.Styles["StyleVariableUI"]);
            this.AddStyle(Main.Resources.Styles["HomeNavigationButtonUI"]);
            this.AddClass(_rootClass);

            _icon = new();
            _icon.AddClass(_iconClass);
            _icon.style.backgroundImage = icon;
            this.AddElements(_icon);

            _label = new();
            _label.AddClass(_labelClass);
            _label.text = label;
            this.AddElements(_label);

            UpdateUI();

            this.RegisterCallback<PointerDownEvent>(OnClicked_Button);

            Marker.OnHomeNavigated += RecheckUI;
        }
        ~HomeNavigationButton()
        {
            Marker.OnHomeNavigated -= RecheckUI;
        }

        private void UpdateUI()
        {
            this.EnableClass(_isSelected, _selected);
            _icon.EnableClass(_isSelected, _selected);
            _label.EnableClass(_isSelected, _selected);
        }

        private void OnClicked_Button(PointerDownEvent evt)
        {
            Marker.OnHomeNavigated?.Invoke(_type);

            _isSelected = true;
            UpdateUI();
        }

        private void RecheckUI(HomeNavigationType type)
        {
            if (_type == type) return;

            _isSelected = false;
            UpdateUI();
        }
    }
}