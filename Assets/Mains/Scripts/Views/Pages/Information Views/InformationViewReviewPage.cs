using UnityEngine;
using UnityEngine.UIElements;

namespace YNL.Checkotel
{
    public partial class InformationViewReviewPage : MonoBehaviour, ICollectible, IInitializable
    {
        private VisualElement _root;

        private VisualElement _backButton;
        private RatingView _ratingView;
        private VisualElement _reviewScroll;

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

            _ratingView = new(_root.Q("TopBar").Q("RatingView"));

            _reviewScroll = _root.Q("ContentScroll").Q("unity-content-container");

            Initialize();
        }

        public void Initialize()
        {
            _reviewScroll.Clear();
            for (byte i = 0; i < 10; i++)
            {
                var reviewItem = new ReviewResultItemUI();
                _reviewScroll.Add(reviewItem);

                if (i == 0) reviewItem.SetAsFirstItem();
            }
        }
    }
}