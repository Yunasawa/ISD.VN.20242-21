using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public enum PreviewListFilterType : byte
    {
        NewProducts, BestSellers, TrendingNow, LimitedEdition, MostPopular
    }

    public class ProductPreviewListUI : VisualElement, IRefreshable
    {
        private const string _rootClass = "product-preview-list";
        private const string _labelFieldClass = _rootClass + "__label-field";
        private const string _labelClass = _rootClass + "__label";
        private const string _seeMoreButtonClass = _rootClass + "__see-more-button";
        private const string _previewListClass = _rootClass + "__preview-list";

        private VisualElement _labelField;
        private Label _label;
        private Label _seeMoreButton;
        private ScrollView _previewList;

        private List<ProductPreviewItemUI> _previewItems = new();

        public ProductPreviewListUI(PreviewListFilterType type, bool isMini = false)
        {
            this.AddStyle(Main.Resources.Styles["StyleVariableUI"]);
            this.AddStyle(Main.Resources.Styles["ProductPreviewListUI"]);
            this.AddClass(_rootClass);

            _labelField = new VisualElement().AddClass(_labelFieldClass);
            this.AddElements(_labelField);

            _label = new Label(type.ToSentenceCase()).AddClass(_labelClass);
            _labelField.AddElements(_label);

            _seeMoreButton = new Label("See more").AddClass(_seeMoreButtonClass);
            _labelField.AddElements(_seeMoreButton);

            _previewList = new ScrollView().AddClass(_previewListClass);
            _previewList.mode = ScrollViewMode.Horizontal;
            _previewList.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
            _previewList.verticalScrollerVisibility = ScrollerVisibility.Hidden;
            this.AddElements(_previewList);

            Initialize(type, isMini);
        }

        private void Initialize(PreviewListFilterType type, bool isMini)
        {
            CreatePreviewItems(type, isMini).Forget();
        }

        public void Refresh()
        {
            foreach (var item in _previewItems) item.Refresh();
        }

        public ProductPreviewListUI SetAsLastItem()
        {
            this.SetMarginBottom(275);

            return this;
        }

        private UID[] GetPreviewItems(PreviewListFilterType type)
        {
            return type switch
            {
                PreviewListFilterType.NewProducts => Function.GetNewProductsList(),
                _ => null
            };
        }

        private async UniTaskVoid CreatePreviewItems(PreviewListFilterType type, bool isMini)
        {
            var previewItems = GetPreviewItems(type);

            for (int i = 0; i < previewItems.Length; i++)
            {
                await UniTask.Yield();

                var previewItem = new ProductPreviewItemUI(previewItems[i], isMini).SetAsLastItem(i == previewItems.Length - 1);
                _previewItems.Add(previewItem);
                _previewList.AddElements(previewItem);
            }
        }
    }
}