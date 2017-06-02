using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using CsvHelper;
using CsvHelper.TypeConversion;
using TestApplication.Models;
using TestApplication.Repositories;
using TestApplication.ViewModel;

namespace TestApplication.Controllers
{
	[HandleError]
	public class FlightsController : Controller
	{
		private IFlightsRepository _flightsRepository;


		public FlightsController()
		{
			_flightsRepository = new FlightsRepository();
		}

		public FlightsController(IFlightsRepository flightsRepository) //For Dependency Injection
		{
			_flightsRepository = flightsRepository;
		}

		public ActionResult Search()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Search(FlightsFilterResults flightsFilterResults)
		{
			if (flightsFilterResults.DepartureAirportCode == null || flightsFilterResults.DestinationAirportCode == null)
				return View(flightsFilterResults);

			var airportCodes = GetAirportCodes(); //Cached data since data is very close to being constant
			
			var flights = _flightsRepository.GetAllFlights().ToList();

			var filteredFlights = flights.Where(x => x.DepartureAirportCode.ToLowerInvariant() == flightsFilterResults.DepartureAirportCode.ToLowerInvariant()
													&& x.DestinationAirportCode.ToLowerInvariant() == flightsFilterResults.DestinationAirportCode.ToLowerInvariant()).ToList();


			string departureAirportDescription, destinationAirportDescription;

			airportCodes.TryGetValue(flightsFilterResults.DepartureAirportCode.ToUpperInvariant(), out departureAirportDescription);
			airportCodes.TryGetValue(flightsFilterResults.DestinationAirportCode.ToUpperInvariant(), out destinationAirportDescription);

			flightsFilterResults.DepartureAirportDescription = departureAirportDescription ?? flightsFilterResults.DepartureAirportCode;
			flightsFilterResults.DestinationAirportDescription = destinationAirportDescription ?? flightsFilterResults.DestinationAirportCode;

			if (flightsFilterResults.SortBy != null)
			{

				var sortBy = flightsFilterResults.SortBy;

				if (filteredFlights != null && filteredFlights.Count > 0)
				{
					switch (sortBy)
					{
						case "DPT":
							filteredFlights = filteredFlights.OrderBy(x =>
							{
								var dateTime = DateTime.ParseExact(DateTime.MinValue.ToShortDateString() + " " + x.DepartureTime, "M/d/yyyy h:mm tt", null);
								return dateTime;
							}).ToList();
							break;
						case "MCP":
							filteredFlights = filteredFlights.OrderBy(x => x.MainCabinPrice).ToList(); ;
							break;
						case "FCP":
							filteredFlights = filteredFlights.OrderBy(x => x.FirstClassPrice).ToList();
							break;
					}

					if (flightsFilterResults.IsDescending) filteredFlights.Reverse();
				}
			}

			flightsFilterResults.Flights = filteredFlights;

			return View(flightsFilterResults);
		}

		private Dictionary<string, string> GetAirportCodes()
		{
			var airportCodes = Session["AirportCodes"]; //Can be also stored in cache instead as data is not user specific
			if (airportCodes == null)
			{
				airportCodes = _flightsRepository.GetAllAirports();
				Session["AirportCodes"] = airportCodes;
			}
			return airportCodes as Dictionary<string, string>;
		}
		
		/// <summary>
		/// Json End Point 
		/// Can also change it to Http GET instead of POST
		/// </summary>
		/// <param name="flightsFilterResults"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult SearchFlights(FlightsFilterResults flightsFilterResults)
		{
			if (flightsFilterResults.DepartureAirportCode == null || flightsFilterResults.DestinationAirportCode == null)
				return Json(flightsFilterResults);

			var airportCodes = GetAirportCodes(); //Cached data since data is very close to being constant

			var flights = _flightsRepository.GetAllFlights().ToList();

			var filteredFlights = flights.Where(x => x.DepartureAirportCode.ToLowerInvariant() == flightsFilterResults.DepartureAirportCode.ToLowerInvariant()
													&& x.DestinationAirportCode.ToLowerInvariant() == flightsFilterResults.DestinationAirportCode.ToLowerInvariant()).ToList();


			string departureAirportDescription, destinationAirportDescription;

			airportCodes.TryGetValue(flightsFilterResults.DepartureAirportCode.ToUpperInvariant(), out departureAirportDescription);
			airportCodes.TryGetValue(flightsFilterResults.DestinationAirportCode.ToUpperInvariant(), out destinationAirportDescription);

			flightsFilterResults.DepartureAirportDescription = departureAirportDescription ?? flightsFilterResults.DepartureAirportCode;
			flightsFilterResults.DestinationAirportDescription = destinationAirportDescription ?? flightsFilterResults.DestinationAirportCode;

			if (flightsFilterResults.SortBy != null)
			{

				var sortBy = flightsFilterResults.SortBy;

				if (filteredFlights != null && filteredFlights.Count > 0)
				{
					switch (sortBy)
					{
						case "DPT":
							filteredFlights = filteredFlights.OrderBy(x =>
							{
								var dateTime = DateTime.ParseExact(DateTime.MinValue.ToShortDateString() + " " + x.DepartureTime, "M/d/yyyy h:mm tt", null);
								return dateTime;
							}).ToList();
							break;
						case "MCP":
							filteredFlights = filteredFlights.OrderBy(x => x.MainCabinPrice).ToList(); ;
							break;
						case "FCP":
							filteredFlights = filteredFlights.OrderBy(x => x.FirstClassPrice).ToList();
							break;
					}

					if (flightsFilterResults.IsDescending) filteredFlights.Reverse();
				}
			}

			flightsFilterResults.Flights = filteredFlights;

			return Json(flightsFilterResults);
		}
	}

}