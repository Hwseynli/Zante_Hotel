using System;
using AutoMapper;

namespace Zante_Hotel
{
	public class MapperProfile:Profile
	{
		public MapperProfile()
		{
			CreateMap<Reservation, ReservationVM>();
			CreateMap<ReservationVM, Reservation>()
				.ForMember(d => d.FullName, o => o.MapFrom(s => s.Name + " " + s.Surname));
        }
	}
}

