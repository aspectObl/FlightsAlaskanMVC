using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestApplication.Models;

namespace TestApplication.ViewModel
{
	[Bind()]
	public class FlightsFilterResults
	{
		public FlightsFilterResults()
		{
			Flights = new List<Flight>();
		}

		public string DepartureAirportCode { get; set; }
		public string DepartureAirportDescription { get; set; }
		public string DestinationAirportCode { get; set; }
		public string DestinationAirportDescription {get;set;}
		public List<Flight> Flights { get; set; }
		public string SortBy { get; set; }
		public bool IsDescending { get; set; }
	}
}