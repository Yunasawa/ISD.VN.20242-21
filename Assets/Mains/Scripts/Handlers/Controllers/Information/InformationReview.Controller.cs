using System.Collections.Generic;
using System.Linq;

namespace YNL.JAMOS
{
    public partial class InformationReview
    {
        private class Controller : PageController
        {
            private InformationReview _b;

            private UID _uid;
            private List<UID> _feedbackIDs = new();

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as InformationReview;
            }

            public override void Refresh()
            {
                _uid = Main.Runtime.SelectedProduct;
                if (!Main.Database.Products.TryGetValue(_uid, out var product))
                {
                    return;
                }

                _feedbackIDs = product.Review.Feedbacks.Keys.ToList();

                _b.OnDataRefreshed?.Invoke(_uid, product, _feedbackIDs);
            }
        }
    }
}