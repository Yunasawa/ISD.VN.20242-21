using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class ManagerMessage
    {
        private class View : PageView
        {
            private ManagerMessage _b;

            private ListView _messageList;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerMessage;

                _b.OnChatListDisplayed += OnChatListDisplayed;
            }

            public override void Collect(VisualElement root)
            {
                root.Remove(root.Q("MessageScroll"));

                _messageList = root.Q<ListView>("MessageList");
            }

            public override void Begin()
            {
                _messageList.Q("unity-content-container").SetFlexGrow(1);
                _messageList.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
                _messageList.makeItem = () => new MessageBoxItemUI();
            }

            private void OnChatListDisplayed(UID[] uids)
            {
                _messageList.itemsSource = uids;
                _messageList.bindItem = (element, index) =>
                {
                    var item = element as MessageBoxItemUI;
                    if (item != null)
                    {
                        var messageID = uids[index];
                        item.Apply(messageID);
                    }
                };
            }
        }
    }
}
