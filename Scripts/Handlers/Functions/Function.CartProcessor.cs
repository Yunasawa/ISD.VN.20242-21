namespace MediaStore
{
    public partial class Function
    {
        public class CartProcessor
        {
            public void ModifyProduct(UID productID, Quantity quantity)
            {
                // if product exists in the cart, update its quantity
                // else add it to the cart
            }

            public void RemoveProduct(UID product)
            {
                // if product exists in the cart, remove it
            }
        }
    }
}
