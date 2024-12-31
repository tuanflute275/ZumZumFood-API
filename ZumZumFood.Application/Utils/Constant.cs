namespace ZumZumFood.Application.Utils
{
    public static class Constant
    {
        public const string SYSADMIN = "ADMIN";
        //PAGESIZE DEFAULT
        public const int DEFAULT_PAGESIZE = 15;

        // 
        public const string ENABLE = "Enable";      // Kích hoạt tài khoản
        public const string BLOCK = "Block";       // Khóa tài khoản
        public const string SUSPENDED = "Suspended"; // Đình chỉ tài khoản

        // STATUS
        public const string PENDING = "Pending";
        public const string COMPLETED = "Completed";
        public const string PAID = "Paid";
        public const string FAILED = "Failed";
        // PAYMENT
        public const string NOTE_AWAIT = "Awaiting payment";
        public const string NOTE_PAID = "Payment processed successfully";
        public const string NOTE_FAILED = "Transfer failed";

        // Success messages
        public const string CreateSuccess = "Successfully created!";
        public const string UpdateSuccess = "Successfully updated!";
        public const string DeleteSuccess = "Successfully deleted!";

        // Error messages
        public const string CreateError = "Failed to create, please try again.";
        public const string UpdateError = "Failed to update, please try again.";
        public const string DeleteError = "Failed to delete, please try again.";

        // General messages
        public const string NotFound = "Data not found.";
        public const string InvalidData = "Invalid data.";
        public const string OperationFailed = "Operation failed. Please try again!";
        public const string InvalidForm = "Invalid data. Please correct the errors and try again.";
    }
}
