using UnityEngine.UIElements;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class GenreButtonItemUI : Label
    {
        private const string _rootClass = "genre-button-item";

        private string _genre;

        public GenreButtonItemUI()
        {
            this.AddStyle(Main.Resources.Styles["StyleVariableUI"]);
            this.AddStyle(Main.Resources.Styles["FacilityListItemUI"]);
            this.AddClass(_rootClass);
            this.RegisterCallback<PointerUpEvent>(OnClicked_GenreButton);
        }

        public void Apply(string genre)
        {
            _genre = genre;
            this.SetText(genre.AddSpaces());
        }

        private void OnClicked_GenreButton(PointerUpEvent evt)
        {
            Main.Runtime.SearchingGenre = _genre;
            Marker.OnGenreSearchRequested?.Invoke(_genre);
            Marker.OnViewPageSwitched?.Invoke(ViewType.SearchViewResultPage, true, false);
        }
    }
}