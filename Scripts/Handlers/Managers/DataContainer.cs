namespace MediaStore
{
    public class DataContainer
    {
        public UID AccountID = new();
        public static List<Account> Accounts = new();

        public static MediaContainer MediaContainer = new();
        public static CartContainer CartContainer = new();
        public static OrderDiary OrderDiary = new();
    }
}
