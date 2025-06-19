using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class ProductTypeItemUI : VisualElement
    {
        private const string _rootClass = "product-type-item";

        public ProductTypeItemUI()
        {
            this.AddStyle(Main.Resources.Styles["StyleVariableUI"]);
            this.AddStyle(Main.Resources.Styles["ProductTypeItemUI"]);
            this.AddClass(_rootClass);
        }
    }
}