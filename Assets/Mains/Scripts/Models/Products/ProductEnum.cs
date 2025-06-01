using System;

namespace YNL.JAMOS
{
    [Flags]
    public enum BookGenre : ushort
    {
        None = 0,
        Fiction = 1 << 0,
        NonFiction = 1 << 1,
        Mystery = 1 << 2,
        Thriller = 1 << 3,
        Fantasy = 1 << 4,
        ScienceFiction = 1 << 5,
        Romance = 1 << 6,
        Horror = 1 << 7,
        Historical = 1 << 8,
        Biography = 1 << 9,
        Poetry = 1 << 10,
        Drama = 1 << 11,
        Adventure = 1 << 12,
        Crime = 1 << 13,
        TrueCrime = 1 << 14,
        YoungAdult = 1 << 15
    }

    [Flags]
    public enum MusicGenre : ushort
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

    [Flags]
    public enum MovieGenre : ushort
    {
        None = 0,
        Action = 1 << 0,
        Adventure = 1 << 1,
        Comedy = 1 << 2,
        Drama = 1 << 3,
        Fantasy = 1 << 4,
        Horror = 1 << 5,
        Mystery = 1 << 6,
        Thriller = 1 << 7,
        ScienceFiction = 1 << 8,
        Animation = 1 << 9,
        Family = 1 << 10,
        Musical = 1 << 11,
        Documentary = 1 << 12,
        Crime = 1 << 13,
        War = 1 << 14,
        Romance = 1 << 15
    }
}