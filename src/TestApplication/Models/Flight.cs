using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CsvHelper;
using CsvHelper.Configuration;

namespace TestApplication.Models
{
	public class Flight
	{
		public string DepartureAirportCode { get; set; }
		public string DestinationAirportCode { get; set; }
		public string FlightNumber { get; set; }
		public string DepartureTime { get; set; }
		public string ArrivalTime { get; set; }
		public Decimal MainCabinPrice { get; set; }
		public Decimal FirstClassPrice { get; set; }
	}


	public sealed class FlightMap : CsvClassMap<Flight>
	{
		public FlightMap()
		{
			Map(m => m.DepartureAirportCode).Name("From");
			Map(m => m.DestinationAirportCode).Name("To");
			Map(m => m.FlightNumber);
			Map(m => m.DepartureTime).Name("Departs");
			Map(m => m.ArrivalTime).Name("Arrives");
			Map(m => m.MainCabinPrice);
			Map(m => m.FirstClassPrice);
		}
	}
}