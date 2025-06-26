using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class SigningStarting : MonoBehaviour
    {
        private VisualElement _root;
        private VisualElement _ground;
        private VisualElement _background;

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _background = _root.Q("ScreenBackground");
            _background.RegisterCallback<TransitionEndEvent>(OnTransitionEnded);

            _ground = _root.Q("Ground");

            Marker.OnClosingStartingPageRequested += OnClosingStartingPageRequested;
        }

        private void OnDestroy()
        {
            Marker.OnClosingStartingPageRequested -= OnClosingStartingPageRequested;
        }

        private void OnClosingStartingPageRequested()
        {
            ClosePage().Forget();
        }

        private async UniTaskVoid ClosePage()
        {
            await UniTask.WaitForSeconds(1);

            _background.SetBackgroundColor(Color.clear);
            _ground.SetOpacity(0);
        }

        private void OnTransitionEnded(TransitionEndEvent evt)
        {
            this.gameObject.SetActive(false);
        }
    }
}