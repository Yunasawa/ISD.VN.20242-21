using System;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public enum PaymentMethod : byte { CreditCard, VISA, Paypal, PayAtPlace}

    public class InformationViewPaymentPageUI : ViewPageUI
    {
        protected override void VirtualAwake()
        {
        }

        private void OnDestroy()
        {
        }

        protected override void Collect()
        {
            
        }
    }
}