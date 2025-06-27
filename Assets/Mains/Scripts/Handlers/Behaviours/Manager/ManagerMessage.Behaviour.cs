using System;
using UnityEngine;

namespace YNL.JAMOS
{
    public partial class ManagerMessage : PageBehaviour
    {
        private View _view;
        private Controller _controller;

        protected override void Construct()
        {
            RegisterView(_view = ViewFactory.CreateView<View>(Root, this));
            RegisterController(_controller = ViewFactory.CreateController<Controller>(this));
        }
    }
}