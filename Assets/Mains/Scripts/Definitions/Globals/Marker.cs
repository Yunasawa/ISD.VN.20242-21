using NUnit.Framework.Constraints;
using System;
using YNL.Utilities.Addons;

namespace YNL.JAMOS
{
    public delegate void OnSearchResultSorted(SortType typw);
    public delegate void OnHotelFacilitiesDisplayed(UID hotelID);
    public delegate void OnHotelRoomsDisplayed(UID hotelID);
    public delegate void OnPaymentRequested(UID hotelID, UID roomID);

    public static partial class Marker
    {
        public static Action OnDatabaseSerializationDone { get; set; }
        public static Action OnRuntimeSavingRequested { get; set; }

        public static Action<ViewType, bool, bool> OnPageNavigated { get; set; }

        public static Action<bool, bool> OnPageBacked { get; set; }

        public static Action OnNotificationViewOpened { get; set; }
        public static Action<SuggestFilterType> OnSuggestFilterSelected { get; set; }

        public static Action<string> OnAddressSearchSubmitted { get; set; }

        public static OnSearchResultSorted OnSearchResultSorted { get; set; }
        public static OnHotelFacilitiesDisplayed OnHotelFacilitiesDisplayed { get; set; }
        public static OnHotelRoomsDisplayed OnHotelRoomsDisplayed { get; set; }
        public static OnPaymentRequested OnPaymentRequested { get; set; }

        public static Action<string> OnGenreSearchRequested { get; set; }
    }
}