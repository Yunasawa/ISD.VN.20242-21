using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class MainViewNavigationPageUI : PageBehaviour
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

        protected override void Construct()
        {
            _navigationBar = Root.Q("NavigationBar");
        }

        protected override void Initialize()
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

            Root.SetDisplay(accountType == AccountType.Manager ? DisplayStyle.None : DisplayStyle.Flex);
        }
    }
}