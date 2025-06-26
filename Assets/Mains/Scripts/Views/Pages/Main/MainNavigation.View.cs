using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class MainNavigation
    {
        private class View : PageView
        {
            private MainNavigation _b;

            private VisualElement _root;
            private VisualElement _navigationBar;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as MainNavigation;

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
                _navigationBar.Add(new HomeNavigationButton(Main.Resources.Icons["Home"], "Home", true, ViewType.MainViewHomePage));
                _navigationBar.Add(new HomeNavigationButton(Main.Resources.Icons["Message"], "Message", false, ViewType.MainViewMessagePage));
                _navigationBar.Add(new HomeNavigationButton(Main.Resources.Icons["Cart"], "Order", false, ViewType.MainViewOrderPage));
                _navigationBar.Add(new HomeNavigationButton(Main.Resources.Icons["Account"], "Account", false, ViewType.MainViewAccountPage));
            }

            private void OnSignedInOrSignedUp()
            {
                var accountType = Main.Database.Accounts[Main.Runtime.Data.AccountID].Type;

                _root.SetDisplay(accountType == AccountType.Manager ? DisplayStyle.None : DisplayStyle.Flex);
            }
        }
    }
}
