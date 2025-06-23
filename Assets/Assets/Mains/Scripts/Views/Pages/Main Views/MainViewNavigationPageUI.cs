using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public class MainViewNavigationPageUI : ViewPageUI
    {
        private VisualElement _navigationBar;

        protected override void Collect()
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
    }
}