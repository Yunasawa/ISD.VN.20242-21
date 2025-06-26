using System;
using UnityEngine;

namespace YNL.JAMOS
{
    public partial class ManagerRevuene : PageBehaviour
    {
        public Action OnBackButtonClicked;

        [SerializeField] private AudioSource _audioSource;

        private View _view;
        private Controller _controller;

        protected override void Construct()
        {
            RegisterView(_view = ViewFactory.CreateView<View>(Root, this));
            RegisterController(_controller = ViewFactory.CreateController<Controller>(this));
        }

        protected override void Refresh()
        {
            _view.Refresh();
        }
    }
}