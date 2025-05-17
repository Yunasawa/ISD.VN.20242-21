using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.Checkotel
{
    public class SearchViewMainPage : MonoBehaviour, ICollectible
    {
        private VisualElement _root;

        private VisualElement _closeButton;
        private VisualElement _addressButton;
        private VisualElement _stayTypeField;
        private VisualElement _roomTypeField;
        private VisualElement _timeRangeButton;
        private Label _checkInTime;
        private Label _checkOutTime;
        private VisualElement _searchButton;

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

            _addressButton = _root.Q("SearchField");
            _addressButton.RegisterCallback<PointerDownEvent>(OnClicked_AddressButton);

            _stayTypeField = _root.Q("StayTypeField").Q("TimeField");
            _stayTypeField.Clear();
            _stayTypeField.Add(new SearchingSelectTypeUI("Hourly", "Hourly", "Hourly", true).SetStayType(SearchingStayType.Hourly));
            _stayTypeField.Add(new SearchingSelectTypeUI("Overnight", "Overnight (Filled)", "Overnight", false).SetStayType(SearchingStayType.Overnight));
            _stayTypeField.Add(new SearchingSelectTypeUI("Daily", "Daily", "Daily", false).SetStayType(SearchingStayType.Daily));

            _roomTypeField = _root.Q("RoomTypeField").Q("TypeField");
            _roomTypeField.Clear();
            _roomTypeField.Add(new SearchingSelectTypeUI("Bed", "Bed (Filled)", "Standard", true).SetRoomType(SearchingRoomType.Standard));
            _roomTypeField.Add(new SearchingSelectTypeUI("Family", "Family (Filled)", "Family", false).SetRoomType(SearchingRoomType.Family));
            _roomTypeField.Add(new SearchingSelectTypeUI("Business", "Business", "Business", false).SetRoomType(SearchingRoomType.Business));

            var timeRangeField = _root.Q("TimeRangeField");

            _timeRangeButton = timeRangeField.Q("TimeFIeld");
            _timeRangeButton.RegisterCallback<PointerDownEvent>(OnClicked_TimeRangeButton);

            _checkInTime = _timeRangeButton.Q("CheckInField").Q("Time") as Label;
            _checkInTime.RegisterCallback<PointerDownEvent>(OnClicked_CheckingButton);

            _checkOutTime = _timeRangeButton.Q("CheckOutField").Q("Time") as Label;
            _checkOutTime.RegisterCallback<PointerDownEvent>(OnClicked_CheckingButton);

            _searchButton = _root.Q("SearchButton");
            _searchButton.RegisterCallback<PointerDownEvent>(OnClicked_SearchButton);
        }

        private void OnClicked_CloseButton(PointerDownEvent evt)
        {

        }

        private void OnClicked_AddressButton(PointerDownEvent evt)
        {
        }

        private void OnClicked_TimeRangeButton(PointerDownEvent evt)
        {
        }

        private void OnClicked_CheckingButton(PointerDownEvent evt)
        {
        }

        private void OnClicked_SearchButton(PointerDownEvent evt)
        {
        }
    }
}