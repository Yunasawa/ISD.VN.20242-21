using System.Collections.Generic;

namespace YNL.Checkotel
{
    public class HotelContainer
    {
        public Dictionary<UID, HotelUnit> Hotels = new();    // Key is ID of HotelUnit
    }

    public class HotelUnit
    {
        public UID ID { get; set; }
        public HotelDescription Description { get; set; } = new();
        public HotelReview Review { get; set; } = new();
        public RoomCategorization Rooms { get; set; } = new();
    }

    public class HotelDescription
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
