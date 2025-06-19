using System;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class PropertyInputFieldUI : VisualElement
    {
        public Action<Product.Property, string> OnPropertyChanged { get; set; }

        private const string _rootClass = "property-input-field";
        private const string _labelClass = _rootClass + "__label";
        private const string _inputClass = _rootClass + "__input";

        private Product.Property _property;

        public PropertyInputFieldUI(Product.Property property)
        {
            _property = property;

            this.AddStyle(Main.Resources.Styles["StyleVariableUI"]);
            this.AddStyle(Main.Resources.Styles["PropertyInputFieldUI"]);
            this.AddClass(_rootClass);

            var label = new Label(property.ToString().AddSpace()).AddClass(_labelClass);

            var input = new TextField(string.Empty).AddClass(_inputClass);
            input.RegisterValueChangedCallback(OnValueChanged_Input);
        }

        private void OnValueChanged_Input(ChangeEvent<string> evt)
        {
            OnPropertyChanged?.Invoke(_property, evt.newValue);
        }
    }
}