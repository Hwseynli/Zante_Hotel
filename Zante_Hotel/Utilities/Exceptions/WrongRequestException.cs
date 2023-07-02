using System;
namespace Zante_Hotel.Utilities.Exceptions
{
    public class WrongRequestException : Exception
    {
        public WrongRequestException(string message) : base(message)
        {

        }

    }
}

