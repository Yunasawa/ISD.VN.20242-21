using System.Collections.Generic;
using System.Linq;

namespace YNL.JAMOS
{
    public partial class ManagerMessage
    {
        private class Controller : PageController
        {
            private Dictionary<UID, MessageList>.KeyCollection _messageKeys => Main.Runtime.Data.Messages.Keys;

            private ManagerMessage _b;

            private UID[] _messageIDs;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerMessage;
            }

            public override void Refresh()
            {
                _messageIDs = _messageKeys.ToArray();

                _b.OnChatListDisplayed?.Invoke(_messageIDs);
            }
        }
    }
}