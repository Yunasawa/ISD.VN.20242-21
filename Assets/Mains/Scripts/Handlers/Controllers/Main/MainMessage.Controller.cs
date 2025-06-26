using System.Collections.Generic;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;

namespace YNL.JAMOS
{
    public partial class MainMessage
    {
        private class Controller : PageController
        {
            private SerializableDictionary<UID, MessageList> _messageLists => Main.Runtime.Data.Messages;

            private MainMessage _b;

            private List<MessageItem> _messages;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as MainMessage;

                _b.OnNewMessageSent += OnNewMessageSent;
            }

            public override void Refresh()
            {
                UpdateMessageList();

                _b.OnMessageListRefreshed?.Invoke(_messages);
            }

            private void OnNewMessageSent(string message)
            {
                var item = new MessageItem()
                {
                    Type = Main.Database.Accounts[Main.Runtime.Data.AccountID].Type,
                    Message = message
                };

                if (_messageLists.TryGetValue(Main.Runtime.Data.AccountID, out var messages))
                {
                    messages.Messages.Add(item);
                }
                else
                {
                    var newMessages = new MessageList();
                    newMessages.Messages.Add(item);
                    _messageLists.Add(Main.Runtime.Data.AccountID, newMessages);
                }

                UpdateMessageList();

                _b.OnMessageListRefreshed?.Invoke(_messages);
            }

            private void UpdateMessageList()
            {
                var accountID = Main.Runtime.Data.AccountID;
                var messages = Main.Runtime.Data.Messages;

                if (messages.ContainsKey(accountID) == false)
                {
                    _messages = new();
                    return;
                }

                _messages =  messages[accountID].Messages;
            }
        }
    }
}