using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.Checkotel
{
    public class SearchViewResultPage : MonoBehaviour, ICollectible, IRefreshable
    {
        private VisualElement _root;

        private VisualElement _resultPage;
        private ScrollView _resultScroll;
        private ListView _resultList;

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

            _resultPage = _root.Q("SearchingResultPage");

            _resultScroll = _root.Q("ResultScroll") as ScrollView;
            _resultPage.Remove(_resultScroll);

            _resultList = new ListView().SetFlexGrow(1).SetMarginTop(50);
            _resultList.Add(_resultScroll);

            Refresh();
        }

        public void Refresh()
        {
            bool[] list = new bool[10];

            _resultList.dataSource = list;
        }
    }
}