using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class MessageBoxItemUI : VisualElement
    {
        private SerializableDictionary<UID, Account> _accounts => Main.Database.Accounts;

        private const string _rootClass = "message-box-item";
        private const string _avatarClass = _rootClass + "__avatar";
        private const string _nameFieldClass = _rootClass + "__name-field";
        private const string _messageFieldClass = _rootClass + "__message-field";

        private VisualElement _avatarField;
        private Label _nameField;
        private Label _messageField;

        private UID _messageID;

        public MessageBoxItemUI()
        {
            this.AddStyle(Main.Resources.Styles["StyleVariableUI"]);
            this.AddStyle(Main.Resources.Styles["MessageBoxItemUI"]);
            this.AddClass(_rootClass);
            this.RegisterCallback<PointerUpEvent>(OnClicked_MessageBox);

            _avatarField = new VisualElement().AddClass(_avatarClass);

            _nameField = new Label().AddClass(_nameFieldClass);
            var space = new VisualElement().SetFlexGrow(1);
            _messageField = new Label().AddClass(_messageFieldClass);
            var contentField = new VisualElement().SetFlexGrow(1).AddElements(_nameField, space, _messageField);

            this.AddElements(_avatarField, contentField);
        }

        public void Apply(UID id)
        {
            if (Main.Runtime.Data.AccountID == -1) return;

            _messageID = id;

            var account = _accounts[id];
            var messages = Main.Runtime.Data.Messages[id].Messages;

            var lastMessage = messages[^1];
            var selfMessage = _accounts[Main.Runtime.Data.AccountID].Type == lastMessage.Type;
            var selfText = selfMessage ? "You: " : "";

            _nameField.SetText(account.Name);
            _messageField.SetText($"{selfText}{lastMessage.Message}");
        }

        private void OnClicked_MessageBox(PointerUpEvent evt)
        {
            Marker.OnChatBoxOpened?.Invoke(_messageID);
            Marker.OnPageNavigated?.Invoke(ViewType.ManagerViewChatPage, true, true);
        }
    }
}