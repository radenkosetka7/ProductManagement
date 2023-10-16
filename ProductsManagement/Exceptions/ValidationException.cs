namespace ProductsManagement.Exceptions
{
    public class ValidationException : Exception
    {
        public int StatusCode { get; }
        public ValidationException() : base() { }

        public ValidationException(string message,int statusCode) : base(message) 
        {
            StatusCode = statusCode;
        }
    }
}
