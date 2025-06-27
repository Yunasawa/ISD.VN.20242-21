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

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerChat;
            }
        }
    }
}