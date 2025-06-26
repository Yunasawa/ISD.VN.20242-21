using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class MainHome
    {
        private class Controller : PageController
        {
            private MainHome _b;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as MainHome;
            }
        }
    }
}