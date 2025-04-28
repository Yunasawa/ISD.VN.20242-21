namespace MediaStore
{
    public static partial class Event
    {
        public static Action<AccountVerificationResult>? OnAccountVerificated { get; set; }
        public static Action<AccountDeletionResult>? OnAccountDeleted { get; set; }
    }
}
