using System.Collections.Concurrent;
using System.Security.Principal;

namespace MediaStore
{
    public class DataContainer
    {
        public UID AccountID = new();
        public static List<Account> Accounts = new();

        public ConcurrentDictionary<UID, MediaUnit> MediaUnits = new();
        public OrderDiary OrderDiary = new();
    }
}