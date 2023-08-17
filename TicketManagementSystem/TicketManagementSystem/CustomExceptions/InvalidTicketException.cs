using System;

namespace TicketManagementSystem.CustomExceptions
{
    public class InvalidTicketException : Exception
    {
        public InvalidTicketException(string message) : base(message)
        {
        }
    }
}
