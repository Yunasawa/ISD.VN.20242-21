using System;

namespace YNL.Checkotel
{
    public class RoomDescription
    {
        public RoomProperty Property { get; set; } = new();
        public RoomFacility Facility { get; set; } = new();
        public RoomRestriction Restriction { get; set; } = new();
    }

    public class RoomProperty
    {
        public ushort MaxOccupancy { get; set; } = 5;
        public ushort NumberOfBeds { get; set; } = 2;
        public Room.BedType BedType { get; set; } = new();
        public Room.ViewType ViewType { get; set; } = Room.ViewType.Ocean;
    }

    public class RoomRestriction
    {
        public bool Refundable { get; set; } = false;
        public ushort MinStay { get; set; } = 1;
        public ushort MaxStay { get; set; } = 10;
    }

    [Flags]
    public enum RoomFacility : uint
    {
        None = 0,

        HasPrivateWifi = 1 << 0,
        HasTV = 1 << 1,
        HasBalcony = 1 << 2,
        HasKitchen = 1 << 3,
        HasBathTub = 1 << 4,
        HasBrailleSignage = 1 << 5,
        HasAirConditioning = 1 << 6,
        HasHeating = 1 << 7,

        IsPetFriendly = 1 << 8,
        IsSmokingAllowed = 1 << 9,
        IsWheelchairFriendly = 1 << 10,
    }

    [Flags]
    public enum HotelFacility : uint
    {
        None = 0,

        HasPool = 1 << 0,
        HasGym = 1 << 1,
        HasSpa = 1 << 2,
        HasRestaurant = 1 << 3,
        HasBar = 1 << 4,
        HasParrkingLots = 1 << 5,
        HasFreeBreakfast = 1 << 6,
        HasEVChargingStation = 1 << 7,
        HasElevator = 1 << 9,

        IsPetFriendly = 1 << 8,
    }
}
