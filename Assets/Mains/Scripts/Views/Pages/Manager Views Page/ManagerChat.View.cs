using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class ManagerChat
    {
        private class View : PageView
        {
            private ManagerChat _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerChat;
            }

            public override void Collect(VisualElement root)
            {

            }
        }
    }
}
