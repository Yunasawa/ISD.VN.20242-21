using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class ManagerRevenue
    {
        private class Controller : PageController
        {
            private ManagerRevenue _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerRevenue;
            }
        }
    }
}