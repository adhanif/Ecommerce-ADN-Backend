using System.Net;

namespace Ecommerce.Core.src.Common
{
    public class AppException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public AppException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        #region Custom Exceptions
        public static AppException BadRequest(string message = "Bad Request 1234") // For testing purposes first
        {
            return new AppException(HttpStatusCode.BadRequest, message)
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = message
            };
        }

        public static AppException NotFound(string message = "Not Found")
        {
            return new AppException(HttpStatusCode.BadRequest, message)
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = message
            };
        }

        public static AppException Unauthorized(string message = "Unauthorized")
        {
            return new AppException(HttpStatusCode.BadRequest, message)
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Message = message
            };
        }

        public static AppException InternalServerError(string message = "Internal Server Error")
        {
            return new AppException(HttpStatusCode.BadRequest, message)
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = message
            };
        }

        public static AppException InvalidLoginCredentialsException(string message = "Invalid Login Credentials")
        {
            return new AppException(HttpStatusCode.BadRequest, message)
            {
                StatusCode = HttpStatusCode.BadRequest, // 400
                Message = message
            };
        }

        public static AppException NotLoginException(string message = "User is not logged in")
        {
            return new AppException(HttpStatusCode.BadRequest, message)
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = message
            };
        }

        public static AppException DuplicateEmailException(string message = "Email is already in use, please choose another email")
        {
            return new AppException(HttpStatusCode.BadRequest, message)
            {
                StatusCode = HttpStatusCode.Conflict, // 409
                Message = message
            };
        }

        public static AppException DuplicateProductTitleException(string message = "Product title is already in use, please choose another title")
        {
            return new AppException(HttpStatusCode.BadRequest, message)
            {
                StatusCode = HttpStatusCode.Conflict, // 409
                Message = message
            };
        }

        public static AppException DuplicateCategotyNameException(string message = "Category name is already in use, please choose another name")
        {
            return new AppException(HttpStatusCode.BadRequest, message)
            {
                StatusCode = HttpStatusCode.Conflict, // 409
                Message = message
            };
        }

        public static AppException ReviewRatingException(string message = "Rating must be between 1 and 5")
        {
            return new AppException(HttpStatusCode.BadRequest, message)
            {
                StatusCode = HttpStatusCode.BadRequest, // 400
                Message = message
            };
        }

        public static AppException InvalidInputException(string message = "Invalid input")
        {
            return new AppException(HttpStatusCode.BadRequest, message)
            {
                StatusCode = HttpStatusCode.BadRequest, // 400
                Message = message
            };
        }
        #endregion

    }
}