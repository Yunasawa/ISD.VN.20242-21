using System;
using UnityEngine;

namespace YNL.JAMOS
{
    public partial class OrderResult : PageBehaviour
    {
        public Action OnBackButtonClicked;

        [SerializeField] private AudioSource _audioSource;

        private View _view;
        private Controller _controller;

        protected override void Construct()
        {
            _view = ViewFactory.CreateView<View>(Root, this);
            _controller = ViewFactory.CreateController<Controller>(this);
        }

        protected override void Refresh()
        {
            _view.Refresh();
        }
    }
}