using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public abstract class PageBehaviour : MonoBehaviour
    {
        [SerializeField] private bool _hideOnAwake = true;
        [SerializeField] protected bool _isPopupPage = false;
        public VisualElement Root;

        private PageView _view;
        private PageController _controller;

        private bool _isDisplayThisTime = false;

        private void Awake()
        {
            Root = GetComponent<UIDocument>().rootVisualElement;
            Root.SetTransitionProperty("translate");
            Root.RegisterCallback<TransitionEndEvent>(OnTransitionEnded);

            VirtualAwake();

            Construct();

            if (!_isPopupPage)
            {
                Root.SetTranslate(_hideOnAwake ? 100 : 0, 0, true);
                Root.SetTransitionDuration(0.2f);
            }
            else
            {
                OnPageOpened(false);
            }

            _isDisplayThisTime = _hideOnAwake;

            Marker.OnDatabaseSerializationDone += OnDatabaseSerializationDone;
        }

        private void OnDestroy()
        {
            Marker.OnDatabaseSerializationDone -= OnDatabaseSerializationDone;
        }

        protected virtual void VirtualAwake() { }

        protected virtual void Construct() { }
        protected virtual void Begin()
        {
            _view?.Begin();
            _controller?.Begin();
        }
        protected virtual void Refresh()
        {
            _view?.Refresh();
            _controller?.Refresh();
        }

        public void RegisterView(PageView view) => _view = view;
        public void RegisterController(PageController controller) => _controller = controller;

        public void DisplayView(bool display, bool needRefresh = true)
        {
            Root.SetDisplay(DisplayStyle.Flex);
            Root.SetTranslate(display ? 0 : -100, 0, true);

            _isDisplayThisTime = display;

            if (Main.View != null) Main.View.IsAbleToMovePage = false;

            if (display && needRefresh)
            {
                Refresh();
            }
        }

        public virtual void OnPageOpened(bool isOpen, bool needRefresh = true)
        {
            Root.SetTranslate(0, isOpen ? 0 : 100, true);

            if (isOpen && needRefresh)
            {
                Refresh();
            }
        }

        private void OnDatabaseSerializationDone()
        {
            Begin();
            Refresh();
        }

        private void OnTransitionEnded(TransitionEndEvent evt)
        {
            if (_isDisplayThisTime) return;

            Root.SetDisplay(DisplayStyle.None);
            Root.SetTranslate(100, 0, true);

            _isDisplayThisTime = true;

            if (Main.View != null)
            {
                Main.View.IsAbleToMovePage = true;
            }
        }
    }
}