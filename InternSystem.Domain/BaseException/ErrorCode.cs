namespace InternSystem.Domain.BaseException
{
    public class ErrorCode
    {
        public const string BadRequest = "Bad Request";
        public const string UnAuthenticated = "Un-Authenticate";
        public const string UnAuthorized = "Forbidden";
        public const string NotFound = "Not Found";
        public const string Unknown = "Oops! Something went wrong, please try again later.";
        public const string NotUnique = "The resource is already, please try another.";
        public const string TokenExpired = "The Token is already expired.";
        public const string TokenInvalid = "The Token is invalid.";
        public const string Validated = "Validated.";
        public const string InvalidInput = "Invaid input";
        public const string Duplicate = "Duplicate";
    }
}
