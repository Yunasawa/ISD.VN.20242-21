using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class ManagerChat
    {
        private class Controller : PageController
        {
            private ManagerChat _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerChat;
            }
        }
    }
}