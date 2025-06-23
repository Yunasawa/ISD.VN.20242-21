using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class ProductGenreItemUI : VisualElement
    {
        public static Action<string> OnToggled { get; set; }

        private const string _rootClass = "toggle-property-item";
        private const string _labelClass = _rootClass + "__label";
        private const string _toggleClass = _rootClass + "__toggle";
        private const string _selected = "selected";

        private Label _label;
        private VisualElement _toggle;

        private List<string> _selectedGenres = new();
        private string _genre;
        private bool _isSelected = false;

        public ProductGenreItemUI(string genre, List<string> selectedGenres)
        {
            _genre = genre;
            _selectedGenres = selectedGenres;

            this.AddStyle(Main.Resources.Styles["StyleVariableUI"]);
            this.AddStyle(Main.Resources.Styles["ProductGenreItemUI"]);
            this.AddClass(_rootClass);
            this.RegisterCallback<PointerUpEvent>(OnClicked__Toggle);

            _label = new(_genre.AddSpaces());
            _label.AddClass(_labelClass);
            this.AddElements(_label);

            _toggle = new();
            _toggle.AddClass(_toggleClass);
            this.AddElements(_toggle);

            _isSelected = (_selectedGenres.IsNullOrEmpty() && genre == "None") || (_selectedGenres.Contains(genre));
            UpdateUI();

            OnToggled += UpdateOnSelected;
        }
        ~ProductGenreItemUI()
        {
            OnToggled -= UpdateOnSelected;
        }

        public void SetAsLastItem()
        {
            this.style.borderBottomWidth = 0;
        }

        public void OnClicked__Toggle(PointerUpEvent evt = null)
        {
            if (_genre == "None")
            {
                _isSelected = true;
                _selectedGenres.Clear();
            }
            else
            {
                _isSelected = !_isSelected;

                if (_isSelected)
                {
                    _selectedGenres.Add(_genre);
                }
                else
                {
                    _selectedGenres.Remove(_genre);
                }
            }

            OnToggled?.Invoke(_genre);

            UpdateUI();
        }

        private void UpdateOnSelected(string genre)
        {
            if (genre == "None" && _genre != "None")
            {
                _isSelected = false;
            }
            if (_genre == "None" && !_selectedGenres.IsNullOrEmpty())
            {
                _isSelected = false;
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            _toggle.EnableClass(_isSelected, _selected);
        }
    }
}