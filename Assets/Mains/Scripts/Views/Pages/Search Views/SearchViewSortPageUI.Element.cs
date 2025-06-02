using System;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class SearchViewSortPageUI
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

                Main.Runtime.SelectedSortType = _type;

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