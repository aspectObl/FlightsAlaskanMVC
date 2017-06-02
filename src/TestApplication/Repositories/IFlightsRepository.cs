using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestApplication.Models;

namespace TestApplication.Repositories
{
	public interface IFlightsRepository
	{
		Dictionary<string, string> GetAllAirports();
		IEnumerable<Flight> GetAllFlights();
	}
}