using UnityEngine;
using YNL.Utilities.Addons;

namespace YNL.JAMOS
{
	public class ViewManager : MonoBehaviour
	{
		public ViewType PreviousViewType;
		public ViewType CurrentViewType;
        public bool IsAbleToMovePage = true;

		public SerializableDictionary<ViewType, ViewPageUI> Pages = new();

        public void Awake()
        {
            Marker.OnPageNavigated += OnPageNavigated;
            Marker.OnPageBacked += OnPageBacked;
        }

        private void OnDestroy()
        {
            Marker.OnPageNavigated -= OnPageNavigated;
            Marker.OnPageBacked -= OnPageBacked;
        }

        private void OnPageNavigated(ViewType type, bool hidePreviousPage, bool needRefresh)
        {
            if (!IsAbleToMovePage) return;

            if (hidePreviousPage)
            {
                Pages[CurrentViewType].DisplayView(false, needRefresh);
            }

            if (InvalidBackPage(CurrentViewType)) PreviousViewType = CurrentViewType;
            CurrentViewType = type;

            Pages[CurrentViewType].DisplayView(true, needRefresh);
        }

        private void OnPageBacked(bool hidePreviousPage, bool needRefresh)
        {
            OnPageNavigated(PreviousViewType, hidePreviousPage, needRefresh);
        }

        private bool InvalidBackPage(ViewType page)
        {
            return page switch
            {
                ViewType.InformationViewReviewPage => false,
                ViewType.InformationViewMainPage => false,
                _ => true
            };
        }
    }

    public enum ViewType : byte
    {
        SigningViewSignUpPage = 0, 
        SigningViewSignInPage = 1,
        SigningViewPreferencePage = 2,
        
        MainViewHomePage = 3,
        MainViewMessagePage = 4,
        MainViewOrderPage = 5,
        MainViewAccountPage = 6,
        
        SearchViewMainPage = 7,
        SearchViewResultPage = 8,
        
        InformationViewMainPage = 9,
        InformationViewReviewPage = 10,

        OrderViewCartPage = 11,
        OrderViewPaymentPage = 12,
        OrderViewDeliveryPage = 13,
        OrderViewResultPage = 14,

        ManagerViewProductPage = 15,
        ManagerViewOrderPage = 16,
        ManagerViewRevenuePage = 17,
        ManagerViewAccountPage = 18,
    }
}