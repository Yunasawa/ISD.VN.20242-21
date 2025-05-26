using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public class MainViewHomePageUI : ViewPageUI
    {
        private VisualElement _searchField;
        private VisualElement _notificationButton;
        private VisualElement _cartButton;
        private ScrollView _pageScroll;

        private List<ProductPreviewListUI> _previewLists = new();

        protected override void Collect()
        {
            _searchField = Root.Q("TopBar").Q("SearchField");
            _searchField.RegisterCallback<PointerUpEvent>(OnClicked__SearchField);

            _notificationButton = Root.Q("TopBar").Q("NotificationButton");
            _notificationButton.RegisterCallback<PointerUpEvent>(OnClicked__NotificationButton);

            _cartButton = Root.Q("TopBar").Q("CartButton");
            _cartButton.RegisterCallback<PointerUpEvent>(OnClicked__CartButton);

            _pageScroll = Root.Q("ScrollView") as ScrollView;

            var sampleList = _pageScroll.Q("SampleList");
            _pageScroll.Remove(sampleList);
        }

        protected override void Initialize()
        {
            _previewLists.Add(new ProductPreviewListUI(PreviewListFilterType.NewProducts));
            //_previewLists.Add(new HotelPreviewListUI(PreviewListFilterType.LuxuryStays, true));
            //_previewLists.Add(new HotelPreviewListUI(PreviewListFilterType.ExceptionalChoices , true));
            //_previewLists.Add(new HotelPreviewListUI(PreviewListFilterType.HighRated));
            //_previewLists.Add(new HotelPreviewListUI(PreviewListFilterType.NewHotels, true).SetAsLastItem());
            foreach (var list in _previewLists) _pageScroll.Insert(1, list);
        }

        protected override void Refresh()
        {
            //foreach (var list in _previewLists) list.Refresh();
        }

        private void OnClicked__SearchField(PointerUpEvent evt)
        {
            Marker.OnViewPageSwitched?.Invoke(ViewType.SearchViewMainPage, true, false);
        }

        private void OnClicked__NotificationButton(PointerUpEvent evt)
        {
            Marker.OnNotificationViewOpened?.Invoke();
        }

        private void OnClicked__CartButton(PointerUpEvent evt)
        {

        }
    }
}