using System;

namespace RestApiExample.Exceptions
{
    /// <summary>
    /// Thrown when a requested entity is not found.
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException() { }
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}