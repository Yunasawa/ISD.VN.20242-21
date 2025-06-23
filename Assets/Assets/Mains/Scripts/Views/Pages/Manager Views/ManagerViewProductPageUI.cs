using System.Linq;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class ManagerViewProductPageUI : ViewPageUI
    {
        private SerializableDictionary<UID, Product.Data> _products => Main.Database.Products;

        private ListView _productList;
        private Label _productAmount;
        private Button _addButton;

        private UID[] _productIDs;

        protected override void Collect()
        {
            Root.Remove(Root.Q("ProductScroll"));

            _productList = Root.Q<ListView>("ProductList");

            _productAmount = Root.Q("ManagerField").Q<Label>("InventoryAmount");

            _addButton = Root.Q("ManagerField").Q<Button>("AddProduct");
        }

        protected override void Initialize()
        {
            _productList.Q("unity-content-container").SetFlexGrow(1);
            _productList.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
            _productList.itemsSource = _productIDs = _products.Keys.ToArray();
            _productList.makeItem = () => new SearchingResultItemUI();
            _productList.bindItem = (element, index) =>
            {
                var item = element as SearchingResultItemUI;
                if (item != null)
                {
                    item.Apply(_productIDs[index]);
                    item.SetAsProductItem();
                }
            };

            _addButton.RegisterCallback<PointerUpEvent>(OnClicked_AddProduct);

            Refresh();
        }

        protected override void Refresh()
        {
            _productIDs = _products.Keys.ToArray();
            _productList.RebuildListView(_productIDs);

            _productAmount.SetText($"Product: <color=#DEF95D>{_products.Count}</color>");
        }

        private void OnClicked_AddProduct(PointerUpEvent evt)
        {
            Marker.OnPageNavigated?.Invoke(ViewType.ManagerViewAddPage, true, true);
        }
    }
}