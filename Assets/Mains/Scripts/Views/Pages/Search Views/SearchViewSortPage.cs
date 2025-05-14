using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace YNL.Checkotel
{
    public partial class SearchViewSortPage : MonoBehaviour, ICollectible, IInitializable, IRefreshable
    {
        public SortingSelectionType SortingType;

        private VisualElement _root;
        private VisualElement _sortingPage;
        private VisualElement _closeButton;
        private VisualElement _applyButton;
        private VisualElement _sortSelectionArea;

        private List<SortingSelectionItemUI> _sortingItemUI = new();

        private void Awake()
        {
            Marker.OnSystemStart += Collect;
            SortingSelectionItemUI.OnSelected += OnSortingTypeSelected;
        }

        private void OnDestroy()
        {
            Marker.OnSystemStart -= Collect;
            SortingSelectionItemUI.OnSelected -= OnSortingTypeSelected;
        }

        public void Collect()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _sortingPage = _root.Q("SortingPage");

            _closeButton = _sortingPage.Q("LabelField").Q("CloseButton");
            _closeButton.RegisterCallback<PointerDownEvent>(OnClicked_CloseButton);

            _applyButton = _sortingPage.Q("Toolbar").Q("ApplyButton");
            _applyButton.RegisterCallback<PointerDownEvent>(OnClicked_ApplyButton);

            _sortSelectionArea = _sortingPage.Q("SortSelectionArea");
            _sortSelectionArea.Clear();

            Initialize();
        }

        public void Initialize()
        {
            var sortingTypes = Enum.GetValues(typeof(SortingSelectionType)) as SortingSelectionType[];

            for (byte i = 0; i < sortingTypes.Length; i++)
            {
                var item = new SortingSelectionItemUI(sortingTypes[i]);
                if (i == sortingTypes.Length - 1) item.SetAsLastItem();

                _sortingItemUI.Add(item);
                _sortSelectionArea.Add(item);
            }

            Refresh();
        }

        public void Refresh()
        {
            _sortingItemUI[0].OnClicked__Toggle();
        }

        private void OnClicked_CloseButton(PointerDownEvent evt)
        {
        }

        private void OnClicked_ApplyButton(PointerDownEvent evt)
        {
        }

        private void OnSortingTypeSelected(SortingSelectionType type)
        {
            SortingType = type;
        }
    }
}