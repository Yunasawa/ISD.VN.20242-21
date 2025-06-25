using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class InformationViewReviewPage : PageBehaviour
    {
        private VisualElement _backButton;
        private RatingView _ratingView;
        private ScrollView _reviewScroll;
        private ListView _reviewList;

        private UID _uid;
        private List<UID> _feedbackIDs = new();

        protected override void Construct()
        {
            _backButton = Root.Q("TopBar").Q("LabelField");
            _backButton.RegisterCallback<PointerUpEvent>(OnClicked_BackButton);

            _ratingView = new(Root.Q("TopBar").Q("RatingView"));

            _reviewScroll = Root.Q("ContentScroll") as ScrollView;
            Root.Remove(_reviewScroll);

            _reviewList = Root.Q("ContentList") as ListView;
            _reviewList.Q("unity-content-container").SetFlexGrow(1);
            _reviewList.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
            _reviewList.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            _reviewList.itemsSource = _feedbackIDs;
            _reviewList.makeItem = () => new FeedbackResultItemUI();
            _reviewList.bindItem = (element, index) =>
            {
                var item = element as FeedbackResultItemUI;
                item.Apply(_uid, _feedbackIDs[index]);
            };
        }

        protected override void Refresh()
        {
            _uid = Main.Runtime.SelectedProduct;
            if (!Main.Database.Products.TryGetValue(_uid, out var product)) return;

            _ratingView.Apply(product);

            _feedbackIDs = product.Review.Feedbacks.Keys.ToList();

            bool emptyFeedback = product.Review.FeebackAmount == 0;

            if (!emptyFeedback) RebuildHistoryList();
        }

        private void OnClicked_BackButton(PointerUpEvent evt)
        {
            Marker.OnPageNavigated?.Invoke(ViewType.InformationViewMainPage, true, false);
        }

        private void RebuildHistoryList()
        {
            _reviewList.itemsSource = null;
            _reviewList.itemsSource = _feedbackIDs;
            _reviewList.Rebuild();
        }
    }
}