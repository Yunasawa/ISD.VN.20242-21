using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class ManagerViewChatPageUI : PageBehaviour
    {
        private SerializableDictionary<UID, Account> _accounts => Main.Database.Accounts;
        private SerializableDictionary<UID, MessageList> _messageLists => Main.Runtime.Data.Messages;


        private VisualElement _avatarField;
        private Label _nameField;
        private ListView _messageList;
        private TextField _messageInput;
        private VisualElement _sendButton;

        private UID _accountID;
        private List<MessageItem> _messages = new();

        protected override void VirtualAwake()
        {
            Marker.OnChatBoxOpened += OnChatBoxOpened;
        }

        private void OnDestroy()
        {
            Marker.OnChatBoxOpened -= OnChatBoxOpened;
        }

        protected override void Construct()
        {
            var labelField = Root.Q("LabelField");
            var backButton = labelField.Q("BackButton");
            backButton.RegisterCallback<PointerUpEvent>(OnClicked_BackButton);
            _avatarField = labelField.Q("Avatar");
            _nameField = labelField.Q<Label>("NameField");

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
            if (_messageLists.ContainsKey(_accountID) == false) return;

            var account = _accounts[_accountID];
            _messages = _messageLists[_accountID].Messages;

            _nameField.SetText(account.Name);

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
            var item = new MessageItem()
            {
                Type = _accounts[Main.Runtime.Data.AccountID].Type,
                Message = _messageInput.value
            };
            _messageLists[_accountID].Messages.Add(item);
            _messageList.RebuildListView(_messages);

            _messageInput.SetValueWithoutNotify(string.Empty);

            Marker.OnRuntimeSavingRequested?.Invoke();
        }

        private void OnChatBoxOpened(UID accountID)
        {
            _accountID = accountID;
        }
    }
}