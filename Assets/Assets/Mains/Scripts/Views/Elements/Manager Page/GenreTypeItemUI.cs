using System;
using UnityEngine.Analytics;
using UnityEngine.UIElements;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class GenreTypeItemUI : VisualElement
    {
        public Action<bool, ushort> OnSelected { get; set; }
        public ushort Value => _value;

        private const string _rootClass = "genre-type-item";
        private const string _genreTextClass = _rootClass + "__genre-text";
        private const string _checkBoxClass = _rootClass + "__check-box";
        private const string _selected = "selected";

        private Label _genreText;
        private VisualElement _checkBox;

        private string _genre;
        private ushort _value;
        private bool _isSelected = false;

        public GenreTypeItemUI(string genre, ushort value)
        {
            _genre = genre;
            _value = value;

            this.AddStyle(Main.Resources.Styles["StyleVariableUI"]);
            this.AddStyle(Main.Resources.Styles["GenreTypeItemUI"]);
            this.AddClass(_rootClass);
            this.RegisterCallback<PointerUpEvent>(OnSelected_GenreItem);

            _genreText = new Label(genre.AddSpace()).AddClass(_genreTextClass);
            _checkBox = new VisualElement().AddClass(_checkBoxClass);

            this.AddElements(_genreText, _checkBox);
        }

        public void Select()
        {
            OnSelected_GenreItem(null);
        }

        private void OnSelected_GenreItem(PointerUpEvent evt)
        {
            _isSelected = !_isSelected;

            this.EnableClass(_isSelected, _selected);
            _genreText.EnableClass(_isSelected, _selected);
            _checkBox.EnableClass(_isSelected, _selected);

            OnSelected?.Invoke(_isSelected, _value);
        }
    }
}