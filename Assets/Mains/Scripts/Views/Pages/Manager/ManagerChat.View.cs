using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class ManagerChat
    {
        private class View : PageView
        {
            private ManagerChat _b;

            private VisualElement _avatarField;
            private Label _nameField;
            private ListView _messageList;
            private TextField _messageInput;
            private VisualElement _sendButton;

            private UID _accountID;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerChat;

                _b.OnMessageListDisplayed += OnMessageListDisplayed;

                Marker.OnChatBoxOpened += OnChatBoxOpened;
            }

            ~View()
            {
                Marker.OnChatBoxOpened -= OnChatBoxOpened;
            }

            public override void Collect(VisualElement root)
            {
                var labelField = root.Q("LabelField");
                var backButton = labelField.Q("BackButton");
                backButton.RegisterCallback<PointerUpEvent>(OnClicked_BackButton);
                _avatarField = labelField.Q("Avatar");
                _nameField = labelField.Q<Label>("NameField");

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

            private void OnMessageListDisplayed(List<MessageItem> messages)
            {
                _messageList.itemsSource = messages;
                _messageList.bindItem = (element, index) =>
                {
                    var item = element as MessageListItemUI;
                    if (item != null)
                    {
                        var message = messages[index];
                        item.Apply(message.Type, message.Message);
                    }
                };
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

            private void OnClicked_BackButton(PointerUpEvent evt)
            {
                Marker.OnPageNavigated?.Invoke(ViewType.ManagerViewMessagePage, true, true);
            }

            private void OnClicked_SendButton(PointerUpEvent evt)
            {
                SendMessage();
            }

            private void SendMessage()
            {
                _b.OnNewMessageRequested?.Invoke(_messageInput.value);

                _messageInput.SetValueWithoutNotify(string.Empty);
            }

            private void OnChatBoxOpened(UID accountID)
            {
                _accountID = accountID;
            }
        }
    }
}
