using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class MainViewMessagePageUI : PageBehaviour
    {
        private List<MessageItem> _messages
        {
            get
            {
                if (Main.Runtime.Data.Messages.ContainsKey(Main.Runtime.Data.AccountID) == false)
                {
                    return new();
                }

                return Main.Runtime.Data.Messages[Main.Runtime.Data.AccountID].Messages;
            }
        }

        private ListView _messageList;
        private TextField _messageInput;
        private VisualElement _sendButton;

        protected override void Construct()
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
            _messageInput.RegisterCallback<KeyDownEvent>(OnKey_MessageInput, TrickleDown.TrickleDown);

            _sendButton.RegisterCallback<PointerUpEvent>(OnClicked_SendButton);
        }

        protected override void Refresh()
        {
            _messageList.RebuildListView(_messages);
        }

        private void OnKey_MessageInput(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
            {
                if (string.IsNullOrEmpty(_messageInput.value)) return;

                SendMessage();
                evt.StopPropagation();
            }
        }

        private void OnClicked_SendButton(PointerUpEvent evt)
        {
            SendMessage();
        }

        private void SendMessage()
        {
            var item = new MessageItem()
            {
                Type = Main.Database.Accounts[Main.Runtime.Data.AccountID].Type,
                Message = _messageInput.value
            };

            Main.Runtime.Data.Messages[Main.Runtime.Data.AccountID].Messages.Add(item);
            _messageList.RebuildListView(_messages);
            _messageInput.SetValueWithoutNotify(string.Empty);
        }
    }
}