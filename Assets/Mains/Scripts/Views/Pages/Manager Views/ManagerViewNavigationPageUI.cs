using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class ManagerViewNavigationPageUI : ViewPageUI
    {
        private VisualElement _navigationBar;

        protected override void VirtualAwake()
        {
            Marker.OnSignedInOrSignedUp += OnSignedInOrSignedUp;
        }

        private void OnDestroy()
        {
            Marker.OnSignedInOrSignedUp -= OnSignedInOrSignedUp;
        }

        protected override void Collect()
        {
            _navigationBar = Root.Q("NavigationBar");
        }

        protected override void Initialize()
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

            if (accountType == AccountType.Customer)
            {
                Root.SetDisplay(DisplayStyle.None);
            }
        }
    }
}