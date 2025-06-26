using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class MainMessage
    {
        private class View : PageView
        {
            private MainMessage _b;

            private ListView _messageList;
            private TextField _messageInput;
            private VisualElement _sendButton;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as MainMessage;

                _b.OnMessageListRefreshed += OnMessageListRefreshed;
            }

            public override void Collect(VisualElement root)
            {
                root.Remove(root.Q("ChatScroll"));
                _messageList = root.Q<ListView>("ChatList");

                var chatField = root.Q("ChatField");
                _messageInput = chatField.Q<TextField>("ChatInput");
                _sendButton = chatField.Q("ToolField").Q("SendButton");
            }

            public override void Begin()
            {
                _messageList.Q("unity-content-container").SetFlexGrow(1);
                _messageList.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
                _messageList.makeItem = () => new MessageListItemUI();

                _messageInput.SetValueWithoutNotify(string.Empty);
                _messageInput.RegisterCallback<KeyDownEvent>(OnKey_MessageInput, TrickleDown.TrickleDown);

                _sendButton.RegisterCallback<PointerUpEvent>(OnClicked_SendButton);
            }

            private void OnMessageListRefreshed(List<MessageItem> messages)
            {
                _messageList.bindItem = (element, index) =>
                {
                    var item = element as MessageListItemUI;
                    if (item != null)
                    {
                        var message = messages[index];
                        item.Apply(message.Type, message.Message);
                    }
                };
                _messageList.RebuildListView(messages);

                _messageInput.SetValueWithoutNotify(string.Empty);
            }

            private void OnKey_MessageInput(KeyDownEvent evt)
            {
                if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
                {
                    if (string.IsNullOrEmpty(_messageInput.value)) return;

                    _b.OnNewMessageSent?.Invoke(_messageInput.value);
                    evt.StopPropagation();
                }
            }

            private void OnClicked_SendButton(PointerUpEvent evt)
            {
                _b.OnNewMessageSent?.Invoke(_messageInput.value);
            }
        }
    }
}
