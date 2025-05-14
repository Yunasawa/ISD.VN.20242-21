using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.Checkotel
{
    public enum SuggestFilterType : byte
    {
        Romantic, WithPool, Homestay,
        NearYou, Hourly, Overnight, Daily,
    }

    public class SuggestFilterButtonUI : VisualElement
    {
        private const string _rootClass = "suggest-filter-button";
        private const string _iconClass = _rootClass + "__icon";
        private const string _labelClass = _rootClass + "__label";
        private const string _typeFilterClass = "type-filter";

        private VisualElement _icon;
        private Label _label;

        private SuggestFilterType _type;

        public SuggestFilterButtonUI(bool isAvailableFilter, Texture2D icon, string label, SuggestFilterType type)
        {
            _type = type;

            this.AddStyle(Main.Resources.Styles["StyleVariableUI"]);
            this.AddStyle(Main.Resources.Styles["SuggestFilterButtonUI"]);
            this.AddClass(_rootClass).EnableClass(!isAvailableFilter, _typeFilterClass);

            _icon = new();
            _icon.AddClass(_iconClass).EnableClass(!isAvailableFilter, _typeFilterClass);
            _icon.SetBackgroundImage(icon);
            this.AddElements(_icon);

            _label = new();
            _label.AddClass(_labelClass).SetText(label);
            this.AddElements(_label);


            this.RegisterCallback<PointerDownEvent>(OnClicked_Button);
        }

        private void OnClicked_Button(PointerDownEvent evt)
        {
            Marker.OnSuggestFilterSelected?.Invoke(_type);
        }
    }
}