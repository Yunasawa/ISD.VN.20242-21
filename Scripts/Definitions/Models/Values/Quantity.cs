namespace MediaStore
{
    public struct Quantity
    {
        private uint _quantity;

        public static implicit operator uint(Quantity quantity) => quantity._quantity;
        public static implicit operator Quantity(uint quantity) => new() { _quantity = quantity };
    }
}
