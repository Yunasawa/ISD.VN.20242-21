using System;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class SigningPreference
    {
        private class View : PageView
        {
            private SigningPreference _b;

            private VisualElement _bookGenreContainer;
            private VisualElement _musicGenreContainer;
            private VisualElement _movieGenreContainer;
            private VisualElement _nextButton;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as SigningPreference;
            }

            public override void Collect(VisualElement root)
            {
                var preferenceContainer = root.Q("PreferenceScroll").Q("unity-content-container");

                _bookGenreContainer = preferenceContainer.Q("BookGenre").Q("Container");
                _musicGenreContainer = preferenceContainer.Q("MusicGenre").Q("Container");
                _movieGenreContainer = preferenceContainer.Q("MovieGenre").Q("Container");

                _nextButton = root.Q("ToolBar").Q("NextButton");
                _nextButton.RegisterCallback<PointerUpEvent>(OnClicked_NextButton);
            }

            public override void Begin()
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
                Marker.OnPageNavigated?.Invoke(ViewType.MainViewHomePage, true, true);
            }
        }
    }
}
