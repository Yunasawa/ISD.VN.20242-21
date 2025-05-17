using System.Collections.Generic;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.Checkotel
{
    public class ReviewResultItemUI : VisualElement, IInitializable, IRefreshable
    {
        private const string _rootClass = "review-result-item";
        private const string _infoFieldClass = _rootClass + "__info-field";
        private const string _accountPictureClass = _rootClass + "__account-picture";
        private const string _accountTextClass = _rootClass + "__account-text";
        private const string _starFieldClass = _rootClass + "__star-field";
        private const string _starIconClass = _rootClass + "__star-icon";
        private const string _commentTextClass = _rootClass + "__comment-text";
        private const string _toolFieldClass = _rootClass + "__tool-field";
        private const string _timeStampClass = _rootClass + "__time-stamp";
        private const string _likeFieldClass = _rootClass + "__like-field";
        private const string _likeAmountClass = _rootClass + "__like-amount";
        private const string _likeIconClass = _rootClass + "__like-icon";
        
        private const string _firstItemClass = "first-item";

        private VisualElement _accountPicture;
        private Label _accountText;
        private VisualElement _starField;
        private Label _commentText;
        private Label _timeStamp;
        private Label _likeAmount;
        private VisualElement _likeIcon;
        private List<VisualElement> _starIcons = new();

        public ReviewResultItemUI()
        {
            this.AddStyle(Main.Resources.Styles["StyleVariableUI"]);
            this.AddStyle(Main.Resources.Styles["ReviewResultItemUI"]);
            this.AddClass(_rootClass);

            var infoField = new VisualElement().AddClass(_infoFieldClass);
            this.AddElements(infoField);

            _accountPicture = new VisualElement().AddClass(_accountPictureClass);
            infoField.AddElements(_accountPicture);

            _accountText = new Label().AddClass(_accountTextClass);
            infoField.AddElements(_accountText);

            _starField = new VisualElement().AddClass(_starFieldClass);
            infoField.AddElements(_starField);

            _commentText = new Label().AddClass(_commentTextClass);
            this.AddElements(_commentText);

            var toolField = new VisualElement().AddClass(_toolFieldClass);
            this.AddElements(toolField);

            _timeStamp = new Label().AddClass(_timeStampClass);
            toolField.AddElements(_timeStamp);

            var likeField = new VisualElement().AddClass(_likeFieldClass);
            toolField.AddElements(likeField);

            _likeAmount = new Label().AddClass(_likeAmountClass);
            likeField.AddElements(_likeAmount);

            _likeIcon = new VisualElement().AddClass(_likeIconClass);
            likeField.AddElements(_likeIcon);

            Initialize();
        }

        public void Initialize()
        {
            for (byte i = 0; i < 5; i++)
            {
                var starIcon = new VisualElement().AddClass(_starIconClass);

                _starIcons.Add(starIcon);
                _starField.AddElements(starIcon);
            }

            Refresh();
        }

        public void Refresh()
        {
            _accountText.SetText("Sam Winchester\r\n<size=35><color=#808080>Room 302</color></size>");

            _commentText.SetText("I had a wonderful stay at this hotel! The staff were incredibly welcoming and attentive, ensuring every aspect of my visit was comfortable.");
            _timeStamp.SetText("Created on 12/05");

            _likeAmount.SetText("1,233");
        }

        public void SetAsFirstItem()
        {
            this.EnableClass(true, _firstItemClass);
        }
    }
}