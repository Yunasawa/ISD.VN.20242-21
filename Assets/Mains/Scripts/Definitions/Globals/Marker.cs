using System;
using System.Collections.Generic;
using YNL.Utilities.Addons;

namespace YNL.JAMOS
{
    public delegate void OnSearchResultSorted(SortType sortType);
    public delegate void OnSearchResultFiltered(MRange priceRange, Product.Type productType, RatingScoreType ratingScore, List<string> genres);

    public static partial class Marker
    {
        public static Action OnDatabaseSerializationDone { get; set; }
        public static Action OnRuntimeSavingRequested { get; set; }
        public static Action OnSignedInOrSignedUp { get; set; }

        public static Action<ViewType, bool, bool> OnPageNavigated { get; set; }

        public static Action<bool, bool> OnPageBacked { get; set; }

        public static Action OnNotificationViewOpened { get; set; }
        public static Action<string> OnAddressSearchSubmitted { get; set; }

        public static OnSearchResultSorted OnSearchResultSorted { get; set; }
        public static OnSearchResultFiltered OnSearchResultFiltered { get; set; }

        public static Action<string> OnGenreSearchRequested { get; set; }
        public static Action<string, Product.Type> OnSearchingInputEntered { get; set; }

        public static Action<string> OnOrderCodeCreated { get; set; }

        public static Action<UID> OnProductUpdatingRequested { get; set; }
        public static Action<UID> OnChatBoxOpened { get; set; }
        public static Action<string, uint> OnVNPayPaymentRequested { get; set; }
        public static Action<DeliveryType> OnDeliveryTypeSelected { get; set; }
        public static Action<Dictionary<DeliveryType, float>> OnDeliveryChargeCalculated { get; set; }
        public static Action OnNewOrderRequested { get; set; }
    }
}