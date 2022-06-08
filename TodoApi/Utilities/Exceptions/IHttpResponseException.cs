namespace TodoApi.Utilities.Exceptions
{
    /// <summary>
    /// abstraction layer for HttpResponseExceptionValue
    /// </summary>
    public interface IHttpResponseException
    {
        public HttpResponseExceptionValue Value { get; set; }
    }
}
