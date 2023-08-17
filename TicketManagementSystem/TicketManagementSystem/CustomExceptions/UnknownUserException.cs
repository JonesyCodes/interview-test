using System;

namespace TicketManagementSystem.CustomExceptions
{
    public class UnknownUserException : Exception
    {
        public UnknownUserException(string message) : base(message)
        {
        }
    }
}
