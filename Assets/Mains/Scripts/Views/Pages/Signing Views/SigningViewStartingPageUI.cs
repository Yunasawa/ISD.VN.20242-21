using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class SigningViewStartingPageUI : MonoBehaviour
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

            Marker.OnDatabaseSerializationDone += OnDatabaseSerializationDone().Forget;
        }

        private void OnDestroy()
        {
            Marker.OnDatabaseSerializationDone -= OnDatabaseSerializationDone().Forget;
        }

        private async UniTaskVoid OnDatabaseSerializationDone()
        {
            await UniTask.WaitForSeconds(3);

            _background.SetBackgroundColor(Color.clear);
            _ground.SetOpacity(0);
        }

        private void OnTransitionEnded(TransitionEndEvent evt)
        {
            this.gameObject.SetActive(false);
        }
    }
}