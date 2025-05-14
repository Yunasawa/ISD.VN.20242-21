using System;
using System.Collections.Generic;

namespace YNL.Checkotel
{
    public class RoomCategorization
    {
        public Dictionary<UID, RoomUnit> Rooms { get; set; } = new(); // Key is ID of RoomUnit
    }

    public class RoomUnit
    {
        public UID ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public RoomDescription Description { get; set; } = new();
        public RoomPrice Price { get; set; } = new();
        public Dictionary<ushort, RoomStatus> Rooms { get; set; } = new();
    }

    public class RoomStatus
    {
        public bool IsAvailable { get; set; } = true;
    }

    public class RoomPrice
    {
        public decimal BasePrice { get; set; } = 0m; 
        public string Currency { get; set; } = "VND";
        public decimal Discount { get; set; } = 0m;   
        public decimal FinalPrice
        {
            get
            {
                return Math.Max(BasePrice - Discount, 0);
            }
        }
    }
}
