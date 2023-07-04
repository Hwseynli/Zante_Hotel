using System;
namespace Zante_Hotel.Utilities.Exceptions
{
	public class BadRequestException:Exception
	{
		public static string Message = "Xeta bas verdi";
		public BadRequestException():base(Message)
		{
		}
        public BadRequestException(string message) : base(message)
        {
        }
    }
}

