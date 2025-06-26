using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class ManagerAccount
    {
        private class Controller : PageController
        {
            private InformationMain _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as InformationMain;
            }
        }
    }
}