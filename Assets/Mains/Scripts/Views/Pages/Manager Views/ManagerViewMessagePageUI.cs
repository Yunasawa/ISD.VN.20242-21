using System.Collections.Generic;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class ManagerViewMessagePageUI : ViewPageUI
    {
        private Dictionary<UID, MessageList>.KeyCollection _messageKeys => Main.Runtime.Data.Messages.Keys;

        private ListView _messageList;

        private UID[] _messageIDs;

        protected override void Collect()
        {
            Root.Remove(Root.Q("MessageScroll"));

            _messageList = Root.Q<ListView>("MessageList");
        }

        protected override void Initialize()
        {
            _messageList.Q("unity-content-container").SetFlexGrow(1);
            _messageList.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
            _messageList.itemsSource = _messageIDs;
            _messageList.makeItem = () => new MessageBoxItemUI();
            _messageList.bindItem = (element, index) =>
            {
                var item = element as MessageBoxItemUI;
                if (item != null)
                {
                    var messageID = _messageIDs[index];
                    item.Apply(messageID);
                }
            };
        }

        protected override void Refresh()
        {
            _messageIDs = _messageKeys.ToArray();

            _messageList.RebuildListView(_messageIDs);
        }
    }
}