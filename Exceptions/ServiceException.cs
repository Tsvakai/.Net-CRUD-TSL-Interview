using System;

namespace RestApiExample.Exceptions
{
    /// <summary>
    /// Thrown for unexpected service errors, wraps lower-level exceptions.
    /// </summary>
    public class ServiceException : Exception
    {
        public ServiceException() { }
        public ServiceException(string message) : base(message) { }
        public ServiceException(string message, Exception inner) : base(message, inner) { }
    }
}