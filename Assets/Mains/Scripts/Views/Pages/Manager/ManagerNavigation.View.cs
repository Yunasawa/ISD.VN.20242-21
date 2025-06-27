using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class ManagerNavigation
    {
        private class View : PageView
        {
            private ManagerNavigation _b;

            private VisualElement _root;
            private VisualElement _navigationBar;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerNavigation;

                Marker.OnSignedInOrSignedUp += OnSignedInOrSignedUp;
            }

            ~View()
            {
                Marker.OnSignedInOrSignedUp -= OnSignedInOrSignedUp;
            }

            public override void Collect(VisualElement root)
            {
                _root = root;

                _navigationBar = root.Q("NavigationBar");
            }

            public override void Begin()
            {
                _navigationBar.Clear();
                _navigationBar.Add(new HomeNavigationButton(Main.Resources.Icons["Product"], "Product", true, ViewType.ManagerViewProductPage));
                _navigationBar.Add(new HomeNavigationButton(Main.Resources.Icons["Message"], "Message", false, ViewType.ManagerViewMessagePage));
                _navigationBar.Add(new HomeNavigationButton(Main.Resources.Icons["Cart"], "Order", false, ViewType.ManagerViewOrderPage));
                _navigationBar.Add(new HomeNavigationButton(Main.Resources.Icons["Revenue"], "Revenue", false, ViewType.ManagerViewRevenuePage));
                _navigationBar.Add(new HomeNavigationButton(Main.Resources.Icons["Account"], "Account", false, ViewType.ManagerViewAccountPage));
            }

            private void OnSignedInOrSignedUp()
            {
                var accountType = Main.Database.Accounts[Main.Runtime.Data.AccountID].Type;

                _root.SetDisplay(accountType == AccountType.Manager ? DisplayStyle.Flex : DisplayStyle.None);
            }
        }
    }
}
