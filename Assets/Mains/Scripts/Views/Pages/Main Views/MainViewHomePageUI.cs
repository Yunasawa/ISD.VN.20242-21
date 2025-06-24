using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class MainViewHomePageUI : ViewPageUI
    {
        private VisualElement _searchField;
        private VisualElement _notificationButton;
        private VisualElement _cartButton;
        private ScrollView _pageScroll;
        private List<VisualElement> _imageSlides = new();

        private List<ProductPreviewListUI> _previewLists = new();

        protected override void Collect()
        {
            _searchField = Root.Q("TopBar").Q("SearchField");
            _searchField.RegisterCallback<PointerUpEvent>(OnClicked_SearchField);

            _notificationButton = Root.Q("TopBar").Q("NotificationButton");
            _notificationButton.RegisterCallback<PointerUpEvent>(OnClicked_NotificationButton);

            _cartButton = Root.Q("TopBar").Q("CartButton");
            _cartButton.RegisterCallback<PointerUpEvent>(OnClicked_CartButton);

            _pageScroll = Root.Q("ScrollView") as ScrollView;

            _imageSlides.Clear();
            var imageSlide = _pageScroll.Q("NotableContent").Q("ImageSlide");
            for (int i = 0; i < 3; i++)
            {
                _imageSlides.Add(imageSlide.Q($"Slide{i + 1}"));
            }

            var sampleList = _pageScroll.Q("SampleList");
            _pageScroll.Remove(sampleList);
        }

        protected override void Initialize()
        {
            _previewLists.Add(new ProductPreviewListUI(PreviewListFilterType.NewProducts));
            _previewLists.Add(new ProductPreviewListUI(PreviewListFilterType.BestSellers));
            _previewLists.Add(new ProductPreviewListUI(PreviewListFilterType.HighRated));
            _previewLists.Add(new ProductPreviewListUI(PreviewListFilterType.MostPopular));
            foreach (var list in _previewLists) _pageScroll.Insert(1, list);
        }

        protected override void Refresh()
        {
            var images = Main.Database.Images.Values.ToList();
            foreach (var slide in _imageSlides)
            {
                slide.SetBackgroundImage(images.GetRandom());
            }
        }

        private void OnClicked_SearchField(PointerUpEvent evt)
        {
            Marker.OnPageNavigated?.Invoke(ViewType.SearchViewMainPage, true, true);
        }

        private void OnClicked_NotificationButton(PointerUpEvent evt)
        {
            Marker.OnNotificationViewOpened?.Invoke();
        }

        private void OnClicked_CartButton(PointerUpEvent evt)
        {
            Marker.OnPageNavigated?.Invoke(ViewType.OrderViewCartPage, true, true);
        }
    }
}