using System;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class SigningViewPreferencePageUI : ViewPageUI
    {
        private VisualElement _bookGenreContainer;
        private VisualElement _musicGenreContainer;
        private VisualElement _movieGenreContainer;
        private VisualElement _nextButton;

        protected override void Collect()
        {
            var preferenceContainer = Root.Q("PreferenceScroll").Q("unity-content-container");

            _bookGenreContainer = preferenceContainer.Q("BookGenre").Q("Container");
            _musicGenreContainer = preferenceContainer.Q("MusicGenre").Q("Container");
            _movieGenreContainer = preferenceContainer.Q("MovieGenre").Q("Container");

            _nextButton = Root.Q("ToolBar").Q("NextButton");
            _nextButton.RegisterCallback<PointerUpEvent>(OnClicked_NextButton);
        }

        protected override void Initialize()
        {
            _bookGenreContainer.Clear();

            foreach (BookGenre genre in Enum.GetValues(typeof(BookGenre)))
            {
                if (genre == BookGenre.None) continue;

                var item = new GenrePreferenceItemUI(genre.ToString());
                _bookGenreContainer.AddElements(item);
            }

            _musicGenreContainer.Clear();

            foreach (MusicGenre genre in Enum.GetValues(typeof(MusicGenre)))
            {
                if (genre == MusicGenre.None) continue;

                var item = new GenrePreferenceItemUI(genre.ToString());
                _musicGenreContainer.AddElements(item);
            }

            _movieGenreContainer.Clear();

            foreach (MovieGenre genre in Enum.GetValues(typeof(MovieGenre)))
            {
                if (genre == MovieGenre.None) continue;

                var item = new GenrePreferenceItemUI(genre.ToString());
                _movieGenreContainer.AddElements(item);
            }
        }

        private void OnClicked_NextButton(PointerUpEvent evt)
        {
            Marker.OnViewPageSwitched?.Invoke(ViewType.MainViewHomePage, true, true);
        }
    }
}