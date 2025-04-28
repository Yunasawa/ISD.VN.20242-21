namespace MediaStore
{
    public class OrderDiary
    {
        public Dictionary<OrderStatus, List<UID>> Orders { get; set; } = new();
    }

    public class OrderSession
    {
        public UID SessionID { get; set; }
        public DateTime OrderTime { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public List<OrderItem> Items { get; set; } = new();
    }

    public class OrderItem
    {
        public UID MediaID { get; set; }
        public int Quantity { get; set; }
        public MediaPrice Price { get; set; } = new();
    }

    public enum OrderStatus
    {
        Pending,
        Completed,
        Cancelled
    }
}
