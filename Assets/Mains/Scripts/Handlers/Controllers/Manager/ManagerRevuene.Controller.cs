using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class ManagerRevuene
    {
        private class Controller : PageController
        {
            private ManagerRevuene _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerRevuene;
            }
        }
    }
}