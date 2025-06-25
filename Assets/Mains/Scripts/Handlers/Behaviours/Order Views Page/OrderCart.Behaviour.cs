using System;
using UnityEngine;

namespace YNL.JAMOS
{
    public partial class OrderCart : PageBehaviour
    {
        public Action OnBackButtonClicked;

        [SerializeField] private AudioSource _audioSource;

        private View _view;
        private Controller _controller;

        protected override void Construct()
        {
            _view = ViewFactory.CreateView<View>(Root, this);
            _controller = ViewFactory.CreateController<Controller>(this);

            _view.AudioSource = _audioSource;
        }

        protected override void Refresh()
        {
            _view.Refresh();
        }
    }
}