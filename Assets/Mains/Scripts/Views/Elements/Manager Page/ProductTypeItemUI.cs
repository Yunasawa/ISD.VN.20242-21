using System;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class ProductTypeItemUI : VisualElement
    {
        public Action<Product.Type> OnSelected { get; set; }
        public Product.Type Type => _type;

        private const string _rootClass = "product-type-item";
        private const string _iconClass = _rootClass + "__icon";
        private const string _labelClass = _rootClass + "__label";

        private Product.Type _type;

        public ProductTypeItemUI(Product.Type type)
        {
            _type = type;

            this.AddStyle(Main.Resources.Styles["StyleVariableUI"]);
            this.AddStyle(Main.Resources.Styles["ProductTypeItemUI"]);
            this.AddClass(_rootClass);
            this.RegisterCallback<PointerUpEvent>(OnClicked_Item);

            var typeString = type.ToString();
            var icon = new VisualElement().AddClass(_iconClass).SetBackgroundImage(Main.Resources.Icons[typeString]);
            var label = new Label(typeString).AddClass(_labelClass);
            this.AddElements(icon, label);
        }

        public void Select() => OnClicked_Item(null);

        private void OnClicked_Item(PointerUpEvent evt)
        {
            OnSelected?.Invoke(_type);

            evt?.StopPropagation();
        }
    }
}