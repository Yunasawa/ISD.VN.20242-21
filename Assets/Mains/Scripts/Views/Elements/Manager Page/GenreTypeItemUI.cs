using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class GenreTypeItemUI : VisualElement
    {
        private const string _rootClass = "genre-type-item";

        public GenreTypeItemUI()
        {
            this.AddStyle(Main.Resources.Styles["StyleVariableUI"]);
            this.AddStyle(Main.Resources.Styles["GenreTypeItemUI"]);
            this.AddClass(_rootClass);
        }
    }
}