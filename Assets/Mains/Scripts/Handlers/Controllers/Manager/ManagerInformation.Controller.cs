using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class ManagerInformation
    {
        private class Controller : PageController
        {
            private ManagerInformation _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerInformation;
            }
        }
    }
}