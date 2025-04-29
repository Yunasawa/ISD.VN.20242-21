namespace MediaStore
{
    public partial class Function
    {
        public class MediaReviewer
        {
            public static void ModifyFeedback(UID productID, UID userID, ReviewRating rating, string comment)
            {
                // if feedback exists, update it
                // else add it to the product's feedback list
            }

            public static void RemoveFeedback(UID productID, UID userID)
            {
                // if feedback exists, remove it
            }
        }
    }
}
