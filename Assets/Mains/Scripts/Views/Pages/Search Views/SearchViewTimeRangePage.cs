using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace YNL.Checkotel
{
    public partial class SearchViewTimeRangePage : MonoBehaviour, ICollectible
    {
        public (DateTime CheckInTime, byte Duration) TimeRange;

        private VisualElement _root;

        private VisualElement _closeButton;
        private Label _checkInTime;
        private Label _checkOutTime;
        private Label _durationTime;
        private VisualElement _cancelButton;
        private VisualElement _applyButton;

        private HourlyPage _hourlyPage;

        private void Awake()
        {
            Marker.OnSystemStart += Collect;
            Marker.OnTimeRangeChanged += OnTimeRangeChanged;
        }

        private void OnDestroy()
        {
            Marker.OnSystemStart -= Collect;
            Marker.OnTimeRangeChanged -= OnTimeRangeChanged;
        }

        public void Collect()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _closeButton = _root.Q("LabelField").Q("CloseButton");
            _closeButton.RegisterCallback<PointerDownEvent>(OnClicked_CloseButton);

            var resultField = _root.Q("ResultField");

            _checkInTime = resultField.Q("CheckInField").Q("Time") as Label;
            _checkOutTime = resultField.Q("CheckOutField").Q("Time") as Label;
            _durationTime = resultField.Q("DurationField").Q("Time") as Label;

            _cancelButton = _root.Q("ToolBar").Q("CancelButton");
            _cancelButton.RegisterCallback<PointerDownEvent>(OnClicked_CancelButton);

            _applyButton = _root.Q("ToolBar").Q("ApplyButton");
            _applyButton.RegisterCallback<PointerDownEvent>(OnClicked_ApplyButton);

            _hourlyPage = new HourlyPage(_root);
        }

        private void OnClicked_CloseButton(PointerDownEvent evt)
        {

        }

        private void OnClicked_CancelButton(PointerDownEvent evt)
        {

        }

        private void OnClicked_ApplyButton(PointerDownEvent evt)
        {

        }

        private void OnTimeRangeChanged(DateTime checkInTime, byte duration)
        {
            _checkInTime.text = checkInTime.ToString("dd/MM, HH:mm");
            _checkOutTime.text = checkInTime.AddHours(duration).ToString("dd/MM, HH:mm");
            _durationTime.text = duration == 1 ? "1 hour" : $"{duration} hours";

            TimeRange = (checkInTime, duration);
        }
    }
}