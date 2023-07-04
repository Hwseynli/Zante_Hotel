using System;
namespace Zante_Hotel.Utilities.Exceptions
{
	public class NotFoundException:Exception
	{
		public static string Message = "Tapilmadi";
		public NotFoundException():base(Message)
		{
		}
        public NotFoundException(string	message):base(message)
        {
        }
    }
}

