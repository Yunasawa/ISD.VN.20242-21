using System.Linq;
using YNL.Utilities.Addons;

namespace YNL.JAMOS
{
    public static partial class Function
    {
        private static SerializableDictionary<UID, Book.Data> _books => Main.Database.Books;

        public static UID[] GetNewProductsList()
        {
            return _books.Select(p => p.Key).ToArray();
        }
    }
}