namespace MediaStore
{
    public class CartContainer
    {
        public List<CartItem> Items { get; set; } = new();
    }

    public class CartItem
    {
        public UID ProductID { get; set; }
        public Quantity Quantity { get; set; } = 1;
    }
}
