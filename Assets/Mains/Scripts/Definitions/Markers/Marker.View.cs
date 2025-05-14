using System;

namespace YNL.Checkotel
{
    public static partial class Marker
    {
        public static Action<HomeNavigationType> OnHomeNavigated { get; set; }
        public static Action OnSearchViewOpened { get; set; }
        public static Action OnNotificationViewOpened { get; set; }
        public static Action<SuggestFilterType> OnSuggestFilterSelected { get; set; }
        public static Action<DateTime, byte> OnTimeRangeChanged { get; set; }
    }
}