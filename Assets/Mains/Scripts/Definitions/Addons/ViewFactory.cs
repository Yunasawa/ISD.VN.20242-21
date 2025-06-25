using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public abstract class PageView
    {
        public abstract void Initialize(PageBehaviour behaviour);
        public abstract void Collect(VisualElement root);
    }

    public abstract class PageController
    {
        public abstract void Initialize(PageBehaviour behaviour);
    }

    public static class ViewFactory
    {
        public static T CreateView<T>(VisualElement root, PageBehaviour behaviour) where T : PageView, new()
        {
            var view = new T();
            view.Initialize(behaviour);
            view.Collect(root);
            return view;
        }

        public static T CreateController<T>(PageBehaviour behaviour) where T : PageController, new()
        {
            var controller = new T();
            controller.Initialize(behaviour);
            return controller;
        }
    }
}