using UnityEngine;
using UnityEngine.UIElements;

namespace YNL.Checkotel
{
    public class SearchViewAddressPage : MonoBehaviour, ICollectible
    {
        private VisualElement _root;

        private VisualElement _closeButton;
        private TextField _addressInput;
        private VisualElement _nearMeButton;
        private ScrollView _historyScroll;

        private void Awake()
        {
            Marker.OnSystemStart += Collect;
        }

        private void OnDestroy()
        {
            Marker.OnSystemStart -= Collect;
        }

        public void Collect()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _closeButton = _root.Q("LabelField").Q("CloseButton");
            _closeButton.RegisterCallback<PointerDownEvent>(OnClicked_CloseButton);

            _addressInput = _root.Q("SearchField").Q("SearchInput") as TextField;
            _addressInput.RegisterValueChangedCallback(OnValueChanged_AddressInput);

            _nearMeButton = _root.Q("NearMeButton");
            _nearMeButton.RegisterCallback<PointerDownEvent>(OnClicked_NearMeButton);

            _historyScroll = _root.Q("SearchingHistory").Q("HistoryScroll") as ScrollView;
        }

        private void OnClicked_CloseButton(PointerDownEvent evt)
        {
        }

        private void OnValueChanged_AddressInput(ChangeEvent<string> evt)
        {

        }

        private void OnClicked_NearMeButton(PointerDownEvent evt)
        {
        }
    }
}