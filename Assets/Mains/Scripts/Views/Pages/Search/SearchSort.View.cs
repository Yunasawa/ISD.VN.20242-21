using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class SearchSort
    {
        private class View : PageView
        {
            private SearchSort _b;

            private VisualElement _background;
            private VisualElement _page;
            private VisualElement _sortingPage;
            private VisualElement _labelField;
            private VisualElement _applyButton;

            private Dictionary<SortType, SortItem> _sortingItems = new();

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as SearchSort;

                _b.OnViewOpenRequested += OnViewOpenRequested;
            }

            public override void Collect(VisualElement root)
            {
                _background = root.Q("ScreenBackground");
                _background.RegisterCallback<PointerUpEvent>(OnClicked_CloseButton);
                _page = root.Q("SortingPage");

                _sortingPage = root.Q("SortingPage");

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

            public override void Begin()
            {
                _sortingItems[SortType.ByTitleAToZ].OnSelected_SortItem();
            }

            public override void Refresh()
            {
                _sortingItems[SortType.ByTitleAToZ].OnSelected_SortItem();
            }

            private void OnViewOpenRequested(bool isOpen)
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
            }

            private void OnClicked_CloseButton(PointerUpEvent evt)
            {
                _b.OnPageOpened(false);
            }

            private void OnClicked_ApplyButton(PointerUpEvent evt)
            {
                _b.OnPageOpened(false);

                _b.OnApplyButtonClicked?.Invoke();
            }
        }
    }

    public partial class SearchSort
    {
        public class SortItem
        {
            public static Action<SortType> OnSelected { get; set; }

            private VisualElement _item;
            private VisualElement _checkBox;

            private SortType _type;
            private bool _isSelected;

            public SortItem(VisualElement item, SortType type)
            {
                _type = type;
                _item = item;
                _item.RegisterCallback<PointerUpEvent>(OnSelected_SortItem);

                _checkBox = _item.Q("Toggle");

                OnSelected += UpdateOnSelected;
            }
            ~SortItem()
            {
                OnSelected -= UpdateOnSelected;
            }

            public void OnSelected_SortItem(PointerUpEvent evt = null)
            {
                _isSelected = true;
                UpdateUI();

                OnSelected?.Invoke(_type);
            }

            private void UpdateOnSelected(SortType type)
            {
                if (_type == type) return;

                _isSelected = false;
                UpdateUI();
            }

            private void UpdateUI()
            {
                _checkBox.SetBackgroundImageTintColor(_isSelected ? "#DEF95D".ToColor() : Color.white);
                _checkBox.SetBackgroundImage(Main.Resources.Icons[_isSelected ? "Check" : "Uncheck"]);
            }
        }
    }
}
