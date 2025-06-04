using System;
using System.Collections.Generic;
using YNL.Utilities.Addons;

namespace YNL.JAMOS
{
    public delegate void OnHotelFacilitiesDisplayed(UID hotelID);
    public delegate void OnHotelRoomsDisplayed(UID hotelID);
    public delegate void OnPaymentRequested(UID hotelID, UID roomID);

    public delegate void OnSearchResultFiltered(MRange priceRange, Product.Type productType, RatingScoreType ratingScore, List<string> genres);

    public static partial class Marker
    {
        public static Action OnDatabaseSerializationDone { get; set; }
        public static Action OnRuntimeSavingRequested { get; set; }

        public static Action<ViewType, bool, bool> OnPageNavigated { get; set; }

        public static Action<bool, bool> OnPageBacked { get; set; }

        public static Action OnNotificationViewOpened { get; set; }
        public static Action<string> OnAddressSearchSubmitted { get; set; }

        public static Action OnSearchResultSorted { get; set; }
        public static OnSearchResultFiltered OnSearchResultFiltered { get; set; }
        public static OnHotelFacilitiesDisplayed OnHotelFacilitiesDisplayed { get; set; }
        public static OnHotelRoomsDisplayed OnHotelRoomsDisplayed { get; set; }
        public static OnPaymentRequested OnPaymentRequested { get; set; }

        public static Action<string> OnGenreSearchRequested { get; set; }
    }
}