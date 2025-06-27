using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class ManagerProduct
    {
        private class View : PageView
        {
            private ManagerProduct _b;

            private ListView _productList;
            private Label _productAmount;
            private Button _addButton;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerProduct;

                _b.OnProductListDisplayed += OnProductListDisplayed;
            }

            public override void Collect(VisualElement root)
            {
                root.Remove(root.Q("ProductScroll"));

                _productList = root.Q<ListView>("ProductList");

                _productAmount = root.Q("ManagerField").Q<Label>("InventoryAmount");

                _addButton = root.Q("ManagerField").Q<Button>("AddProduct");
            }

            public override void Begin()
            {
                _productList.Q("unity-content-container").SetFlexGrow(1);
                _productList.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
                _productList.makeItem = () => new SearchingResultItemUI();

                _addButton.RegisterCallback<PointerUpEvent>(OnClicked_AddProduct);
            }

            private void OnClicked_AddProduct(PointerUpEvent evt)
            {
                Marker.OnPageNavigated?.Invoke(ViewType.ManagerViewAddPage, true, true);
            }

            private void OnProductListDisplayed(UID[] uids)
            {
                _productList.itemsSource = uids;
                _productList.bindItem = (element, index) =>
                {
                    var item = element as SearchingResultItemUI;
                    if (item != null)
                    {
                        item.Apply(uids[index]);
                        item.SetAsProductItem();
                    }
                };

                _productAmount.SetText($"Product: <color=#DEF95D>{uids.Length}</color>");
            }
        }
    }
}
