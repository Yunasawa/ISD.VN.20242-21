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
        private const string _firstClass = "first";

        private TextField _inputField;

        private Product.Property _property;

        public PropertyInputFieldUI(Product.Property property)
        {
            _property = property;

            this.AddStyle(Main.Resources.Styles["StyleVariableUI"]);
            this.AddStyle(Main.Resources.Styles["PropertyInputFieldUI"]);
            this.AddClass(_rootClass);

            var label = new Label(property.ToString().AddSpace()).AddClass(_labelClass);

            _inputField = new TextField(string.Empty).AddClass(_inputClass);
            _inputField.textEdition.placeholder = "...";
            _inputField.RegisterValueChangedCallback(OnValueChanged_Input);

            this.AddElements(label, _inputField);
        }

        public void SetValue(string value)
        {
            _inputField.SetValueWithoutNotify(value);
        }

        public void SetAsFirstItem()
        {
            this.EnableClass(_firstClass);
        }

        private void OnValueChanged_Input(ChangeEvent<string> evt)
        {
            OnPropertyChanged?.Invoke(_property, evt.newValue);
        }
    }
}