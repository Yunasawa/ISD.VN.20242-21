using UnityEngine;
using UnityEngine.UIElements;

namespace YNL.Checkotel
{
    public class MainViewHomePageUI : MonoBehaviour, ICollectible
    {
        private VisualElement _root;

        private VisualElement _navigationBar;
        private VisualElement _searchField;
        private VisualElement _notificationButton;
        private ScrollView _avalableFilterScroll;
        private ScrollView _typeFilterScroll;
        private ScrollView _pageScroll;

        private void Awake()
        {
            Marker.OnSystemStart += Collect;
        }

        private void OnDestroy()
        {
            Marker.OnSystemStart -= Collect;
        }

        public void Collect()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _navigationBar = _root.Q("NavigationBar");
            _navigationBar.Clear();

            _navigationBar.Add(new HomeNavigationButton(Main.Resources.Icons["Home"], "Home", true, HomeNavigationType.Home));
            _navigationBar.Add(new HomeNavigationButton(Main.Resources.Icons["Star"], "Favorite", false, HomeNavigationType.Favorite));
            _navigationBar.Add(new HomeNavigationButton(Main.Resources.Icons["Suggestion"], "Suggestion", false, HomeNavigationType.Suggestion));
            _navigationBar.Add(new HomeNavigationButton(Main.Resources.Icons["Account"], "Account", false, HomeNavigationType.Account));

            _searchField = _root.Q("TopBar").Q("SearchField");
            _searchField.RegisterCallback<PointerDownEvent>(OnClicked__SearchField);

            _notificationButton = _root.Q("TopBar").Q("NotificationButton");
            _notificationButton.RegisterCallback<PointerDownEvent>(OnClicked__NotificationButton);

            _pageScroll = _root.Q("ScrollView") as ScrollView;
            _pageScroll.Add(new HotelPreviewListUI("Most Popular"));
            _pageScroll.Add(new HotelPreviewListUI("New Hotels"));

            _avalableFilterScroll = _pageScroll.Q("BackgroudContainer").Q("AvalableScroll") as ScrollView;
            _avalableFilterScroll.Clear();

            _avalableFilterScroll.Add(new SuggestFilterButtonUI(true, Main.Resources.Icons["Star"], "Romantic", SuggestFilterType.Romantic));
            _avalableFilterScroll.Add(new SuggestFilterButtonUI(true, Main.Resources.Icons["Star"], "With Pool", SuggestFilterType.WithPool));
            _avalableFilterScroll.Add(new SuggestFilterButtonUI(true, Main.Resources.Icons["Star"], "Homestay", SuggestFilterType.Homestay));

            _typeFilterScroll = _pageScroll.Q("BackgroudContainer").Q("FilterScroll") as ScrollView;
            _typeFilterScroll.Clear();

            _typeFilterScroll.Add(new SuggestFilterButtonUI(false, Main.Resources.Icons["Star"], "<b>Near You</b>\n<size=30>One step to the sky</size>", SuggestFilterType.NearYou));
            _typeFilterScroll.Add(new SuggestFilterButtonUI(false, Main.Resources.Icons["Star"], "<b>Hourly</b>\n<size=30>Enjoy every seconds</size>", SuggestFilterType.Hourly));
            _typeFilterScroll.Add(new SuggestFilterButtonUI(false, Main.Resources.Icons["Star"], "<b>Overnight</b>\n<size=30>Comfort like home</size>", SuggestFilterType.Overnight));
            _typeFilterScroll.Add(new SuggestFilterButtonUI(false, Main.Resources.Icons["Star"], "<b>Daily</b>\n<size=30>Every day is joyful</size>", SuggestFilterType.Daily));
        }

        private void OnClicked__SearchField(PointerDownEvent evt)
        {
            Marker.OnSearchViewOpened?.Invoke();
        }

        private void OnClicked__NotificationButton(PointerDownEvent evt)
        {
            Marker.OnNotificationViewOpened?.Invoke();
        }
    }
}