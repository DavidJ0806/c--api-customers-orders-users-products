namespace TodoApi.Utilities.Exceptions
{
    /// <summary>
    /// A Http response class to represent a json body for exceptions
    /// </summary>
    public class HttpResponseExceptionValue
    {
        public DateTime Timestamp { get; set; }
        public int Status { get; set; }
        public string? Error { get; set; }
        public string? ErrorMessage { get; set; }

        public HttpResponseExceptionValue()
        {
            Timestamp = DateTime.UtcNow;
        }
        public HttpResponseExceptionValue(int status, string error, string message)
        {
            Timestamp = DateTime.UtcNow;
            Status = status;
            Error = error;
            ErrorMessage = message;
        }
    }
}
