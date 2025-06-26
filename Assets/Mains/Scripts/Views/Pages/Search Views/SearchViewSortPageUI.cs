using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class SearchViewSortPageUI : PageBehaviour
    {

        private VisualElement _background;
        private VisualElement _page;
        private VisualElement _sortingPage;
        private VisualElement _labelField;
        private VisualElement _applyButton;

        private Dictionary<SortType, SortItem> _sortingItems = new();
        private SortType _selectedSortType = SortType.ByTitleAToZ;

        protected override void VirtualAwake()
        {
            SortItem.OnSelected += OnSortItemSelected;
        }

        private void OnDestroy()
        {
            SortItem.OnSelected -= OnSortItemSelected;
        }

        protected override void Construct()
        {
            _background = Root.Q("ScreenBackground");
            _background.RegisterCallback<PointerUpEvent>(OnClicked_CloseButton);
            _page = Root.Q("SortingPage");

            _sortingPage = Root.Q("SortingPage");

            _labelField = _sortingPage.Q("LabelField");
            _labelField.RegisterCallback<PointerUpEvent>(OnClicked_CloseButton);

            _applyButton = _sortingPage.Q("Toolbar").Q("ApplyButton");
            _applyButton.RegisterCallback<PointerUpEvent>(OnClicked_ApplyButton);

            var container = _sortingPage.Q("SortSelectionArea").Q("unity-content-container");

            foreach (SortType type in Enum.GetValues(typeof(SortType)))
            {
                var item = container.Q(type.ToString());
                var sortItem = new SortItem(item, type);
                _sortingItems.Add(type, sortItem);
            }
        }

        protected override void Begin()
        {
            _sortingItems[SortType.ByTitleAToZ].OnSelected_SortItem();
        }

        protected override void Refresh()
        {
            _sortingItems[SortType.ByTitleAToZ].OnSelected_SortItem();
        }

        public override void OnPageOpened(bool isOpen, bool needRefresh = true)
        {
            if (isOpen)
            {
                _background.SetPickingMode(PickingMode.Position);
                _background.SetBackgroundColor(new Color(0.0865f, 0.0865f, 0.0865f, 0.725f));
                _page.SetTranslate(0, 0, true);
            }
            else
            {
                _background.SetPickingMode(PickingMode.Ignore);
                _background.SetBackgroundColor(Color.clear);
                _page.SetTranslate(0, 100, true);
            }

            if (isOpen && needRefresh) Refresh();
        }

        private void OnClicked_CloseButton(PointerUpEvent evt)
        {
            OnPageOpened(false);
        }

        private void OnClicked_ApplyButton(PointerUpEvent evt)
        {
            Marker.OnSearchResultSorted?.Invoke(_selectedSortType);
            OnPageOpened(false);
        }

        private void OnSortItemSelected(SortType type)
        {
            _selectedSortType = type;
        }
    }
}