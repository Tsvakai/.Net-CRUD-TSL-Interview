using System;

namespace RestApiExample.Exceptions
{
    /// <summary>
    /// Thrown when validation of input data fails.
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException() { }
        public ValidationException(string message) : base(message) { }
        public ValidationException(string message, Exception inner) : base(message, inner) { }
    }
}