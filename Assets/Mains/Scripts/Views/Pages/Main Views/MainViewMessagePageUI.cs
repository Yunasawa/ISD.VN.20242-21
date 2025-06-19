using System.Collections.Generic;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class MainViewMessagePageUI : ViewPageUI
    {
        private List<MessageItem> _messages => Main.Runtime.Data.Messages[Main.Runtime.Data.AccountID].Messages;

        private ListView _messageList;
        private TextField _messageInput;
        private VisualElement _sendButton;

        protected override void Collect()
        {
            Root.Remove(Root.Q("ChatScroll"));
            _messageList = Root.Q<ListView>("ChatList");

            var chatField = Root.Q("ChatField");
            _messageInput = chatField.Q<TextField>("ChatInput");
            _sendButton = chatField.Q("ToolField").Q("SendButton");
        }

        protected override void Initialize()
        {
            _messageList.Q("unity-content-container").SetFlexGrow(1);
            _messageList.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
            _messageList.itemsSource = _messages;
            _messageList.makeItem = () => new MessageListItemUI();
            _messageList.bindItem = (element, index) =>
            {
                var item = element as MessageListItemUI;
                if (item != null)
                {
                    var message = _messages[index];
                    item.Apply(message.Type, message.Message);
                }
            };

            _messageInput.SetValueWithoutNotify(string.Empty);

            _sendButton.RegisterCallback<PointerUpEvent>(OnClicked_SendButton);
        }

        protected override void Refresh()
        {
            _messageList.RebuildListView(_messages);
        }

        private void OnClicked_SendButton(PointerUpEvent evt)
        {
            var item = new MessageItem()
            {
                Type = Main.Database.Accounts[Main.Runtime.Data.AccountID].Type,
                Message = _messageInput.value
            };
            Main.Runtime.Data.Messages[Main.Runtime.Data.AccountID].Messages.Add(item);
            _messageList.RebuildListView(_messages);
        }
    }
}