namespace MediaStore
{
    public class CartList
    {
        public List<CartItem> Items { get; set; } = new();
    }

    public class CartItem
    {
        public UID ProductID { get; set; }
        public Quantity Quantity { get; set; } = 1;
    }
}
