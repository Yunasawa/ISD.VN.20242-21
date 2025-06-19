using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class MessageListItemUI : VisualElement
    {
        private const string _rootClass = "message-list-item";
        private const string _messageClass = _rootClass + "__message";
        private const string _otherClass = "other";

        private Label _message;

        public MessageListItemUI()
        {
            this.AddStyle(Main.Resources.Styles["StyleVariableUI"]);
            this.AddStyle(Main.Resources.Styles["MessageListItemUI"]);
            this.AddClass(_rootClass);

            _message = new Label().AddClass(_messageClass);
            this.AddElements(_message);
        }

        public void Apply(AccountType type, string message)
        {
            bool isOther = type != Main.Runtime.AccountType;

            this.EnableClass(isOther, _otherClass);
            _message.EnableClass(isOther, _otherClass);

            _message.SetText(message);
        }
    }
}