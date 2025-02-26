﻿namespace ZumZumFood.Application.Utils.Common
{
    public static class Constant
    {
        public const string SYSADMIN = "ADMIN";
        //PAGESIZE DEFAULT
        public const int DEFAULT_PAGESIZE = 15;
        // Status Redis connect
        public static bool IsRedisConnectedStatic { get; set; } = false;
        // 
        public const string ENABLE = "Enable";      // Kích hoạt tài khoản
        public const string BLOCK = "Block";       // Khóa tài khoản
        public const string SUSPENDED = "Suspended"; // Đình chỉ tài khoản

        // Defalut password
        public const string DEFAULT_PASSWORD = "12345678";

        // ParaType Parameter
        public const string PARA_TYPE_INTEGER = "INTEGER";
        public const string PARA_TYPE_CODE = "CODE";
        public const string PARA_TYPE_STRING = "STRING";
        public const string PARA_TYPE_BOOLEAN = "BOOLEAN";
        public const string PARA_TYPE_LONG_TEXT = "LONG_TEXT";
        public const string PARA_TYPE_NUMBER = "NUMBER";

        // Banner Type
        public const string BANNER_TYPE_APP = "APP";
        public const string BANNER_TYPE_RESTAURANT = "RESTAURANT";

        // Cart Type Update
        public const string CART_UPDATE_MINUS = "minus";
        public const string CART_UPDATE_PLUS = "plus";

        // Status order
        public const int DEFAULT_STATUS_ORDER = 1;

        // RabbitMQ
        public const string HNXSettingId = "HNX";
        public const string FixReceiveSettingId = "FixReceive";

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
