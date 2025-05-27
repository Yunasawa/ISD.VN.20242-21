using System;

namespace YNL.JAMOS
{
    public class LP
    {
        public enum Format : byte { Standard, Limited }

        [Flags]
        public enum Genre : ushort
        {
            None = 0,
            Pop = 1 << 0,
            Rock = 1 << 1,
            HipHop = 1 << 2,
            Rap = 1 << 3,
            RnB = 1 << 4,
            Soul = 1 << 5,
            Jazz = 1 << 6,
            Blues = 1 << 7,
            Country = 1 << 8,
            Folk = 1 << 9,
            Classical = 1 << 10,
            Electronic = 1 << 11,
            Reggae = 1 << 12,
            Latin = 1 << 13,
            Soundtrack = 1 << 14,
            Instrumental = 1 << 15
        }

        [System.Serializable]
        public class Data
        {
            public string Album;
            public string Duration;
            public Format Format;
            public Genre Genre;
        }
    }
}