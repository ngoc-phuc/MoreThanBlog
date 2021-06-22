namespace Core.Errors
{
    public class ErrorCode
    {
        public const string InvalidArgumentException = "Invalid Argument Exception";
        public const string BadRequest = "Bad Request";
        public const string Unauthorized = "Unauthorized";
        public const string Forbidden = "Forbidden";
        public const string ResourceNotFound = "Resource Not Found";
        public const string RequestTimeout = "Request Timeout";
        public const string UnprocessableEntity = "Unprocessable Entity";
        public const string SystemError = "System Error";
        public const string AnUnexpectedErrorOccurred = "AnUnexpectedErrorOccurred";
        public const string Unknown = "Unknown";
        public const string InvalidParamters = "Invalid Paramters";
        public const string FileNotExist = "File not exist";

        //user
        public const string UserNotFound = "User Not Found";
        public const string WrongPassword = "Wrong Password";
        public const string DuplicateName = "Duplicate name, please try again.";

        //Blog
        public const string DuplicateTitle = "Duplicate title, please try again.";
        public const string BlogNotFound = "Blog not found.";
    }
}