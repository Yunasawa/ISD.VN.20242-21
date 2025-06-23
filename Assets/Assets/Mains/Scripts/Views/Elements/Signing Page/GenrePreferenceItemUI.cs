using UnityEngine.UIElements;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class GenrePreferenceItemUI : VisualElement
    {
        private const string _rootClass = "genre-preference-item";
        private const string _genreTextClass = _rootClass + "__genre-text";
        private const string _checkBoxClass = _rootClass + "__check-box";
        private const string _selected ="selected";

        private Label _genreText;
        private VisualElement _checkBox;

        private string _genre;
        private bool _isSelected = false;

        public GenrePreferenceItemUI(string genre)
        {
            _genre = genre;

            this.AddStyle(Main.Resources.Styles["StyleVariableUI"]);
            this.AddStyle(Main.Resources.Styles["GenrePreferenceItemUI"]);
            this.AddClass(_rootClass);
            this.RegisterCallback<PointerUpEvent>(OnSelected_GenreItem);

            _genreText = new Label(genre.AddSpaces()).AddClass(_genreTextClass);
            _checkBox = new VisualElement().AddClass(_checkBoxClass);

            this.AddElements(_genreText, _checkBox);
        }
    
        private void OnSelected_GenreItem(PointerUpEvent evt)
        {
            _isSelected = !_isSelected;

            this.EnableClass(_isSelected, _selected);
            _genreText.EnableClass(_isSelected, _selected);
            _checkBox.EnableClass(_isSelected, _selected);

            if (_isSelected)
            {
                Main.Runtime.Data.FavoriteGenres.Add(_genre);
            }
            else
            {
                Main.Runtime.Data.FavoriteGenres.Remove(_genre);
            }
        }
    }
}