using System.Collections.Generic;
using YNL.Utilities.Addons;

namespace YNL.JAMOS
{
    public partial class ManagerChat
    {
        private class Controller : PageController
        {
            private SerializableDictionary<UID, Account> _accounts => Main.Database.Accounts;
            private SerializableDictionary<UID, MessageList> _messageLists => Main.Runtime.Data.Messages;
             
            private ManagerChat _b;

            private UID _accountID;
            private List<MessageItem> _messages = new();

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerChat;

                _b.OnNewMessageRequested += OnNewMessageRequested;

                Marker.OnChatBoxOpened += OnChatBoxOpened;
            }

            ~Controller()
            {
                Marker.OnChatBoxOpened -= OnChatBoxOpened;
            }

            public override void Refresh()
            {
                if (_messageLists.ContainsKey(_accountID) == false) return;

                var account = _accounts[_accountID];
                _messages = _messageLists[_accountID].Messages;

                _b.OnMessageListDisplayed?.Invoke(_messages);
            }

            private void OnNewMessageRequested(string message)
            {
                var item = new MessageItem()
                {
                    Type = _accounts[Main.Runtime.Data.AccountID].Type,
                    Message = message
                };
                _messageLists[_accountID].Messages.Add(item);

                Marker.OnRuntimeSavingRequested?.Invoke();

                _b.OnMessageListDisplayed?.Invoke(_messages);
            }

            private void OnChatBoxOpened(UID accountID)
            {
                _accountID = accountID;
            }
        }
    }
}