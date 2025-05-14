using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.Checkotel
{
    public class HotelPreviewListUI : VisualElement
    {
        private const string _rootClass = "hotel-preview-list";
        private const string _labelFieldClass = _rootClass + "__label-field";
        private const string _labelClass = _rootClass + "__label";
        private const string _seeMoreButtonClass = _rootClass + "__see-more-button";
        private const string _previewListClass = _rootClass + "__preview-list";

        private VisualElement _labelField;
        private Label _label;
        private Label _seeMoreButton;
        private ScrollView _previewList;

        public HotelPreviewListUI(string label)
        {
            var items = new string[] { "Item 1", "Item 2", "Item 3", "Item 4", "Item 5" };

            this.AddStyle(Main.Resources.Styles["StyleVariableUI"]);
            this.AddStyle(Main.Resources.Styles["HotelPreviewListUI"]);
            this.AddClass(_rootClass);

            _labelField = new();
            _labelField.AddToClassList(_labelFieldClass);
            this.AddElements(_labelField);

            _label = new(label);
            _label.AddClass(_labelClass);
            _labelField.AddElements(_label);

            _seeMoreButton = new("See more");
            _seeMoreButton.AddClass(_seeMoreButtonClass);
            _labelField.AddElements(_seeMoreButton);

            _previewList = new();
            _previewList.AddClass(_previewListClass);
            this.AddElements(_previewList);
            _previewList.mode = ScrollViewMode.Horizontal;
            _previewList.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
            _previewList.verticalScrollerVisibility = ScrollerVisibility.Hidden;

            for (int i = 0; i < 10; i++)
            {
                _previewList.AddElements(new HotelPreviewItemUI());
            }
        }
    }
}