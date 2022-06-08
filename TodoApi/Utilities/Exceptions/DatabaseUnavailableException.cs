namespace TodoApi.Utilities.Exceptions
{
    /// <summary>
    /// Exception to throw 503 status code
    /// </summary>
    [Serializable]
    public class DatabaseUnavailableException : Exception, IHttpResponseException
    {
        public DatabaseUnavailableException(string message)
        {
            Value = new(status: 503, error: "Database connection error", message: message);
        }
        public HttpResponseExceptionValue Value { get; set; }
    }

}
