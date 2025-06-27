using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class ManagerUpdate
    {
        private class Controller : PageController
        {
            private ManagerUpdate _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerUpdate;
            }
        }
    }
}