using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using CsvHelper;
using TestApplication.Models;

namespace TestApplication.Repositories
{
	public class FlightsRepository : IFlightsRepository
	{

		public Dictionary<string, string> GetAllAirports()
		{
			var airportCodes = new Dictionary<string, string>();
			var airportCodesPath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, ConfigurationManager.AppSettings["AirportsFile"]);
			try
			{
				using (TextReader reader = System.IO.File.OpenText(airportCodesPath))
				{
					CsvHelper.CsvParser parser = new CsvParser(reader);
					CsvReader csvReader = new CsvReader(parser);

					var codeNames = csvReader.GetRecords<CodeName>().ToList();

					foreach (var codeName in codeNames)
					{
						if (!airportCodes.ContainsKey(codeName.Code))
						{
							airportCodes.Add(codeName.Code, codeName.Name);
						}
					}
				}
			}
			catch (FileNotFoundException ex)
			{
				//Log exception
			}
			catch (Exception ex)
			{
			}
			return airportCodes;
		}

		public IEnumerable<Flight> GetAllFlights()
		{
			var flights = new List<Flight>();

			var path = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, ConfigurationManager.AppSettings["FlightsFile"]);
			try
			{
				using (TextReader reader = System.IO.File.OpenText(path))
				{
					CsvHelper.CsvParser parser = new CsvParser(reader);
					CsvReader csvReader = new CsvReader(parser);
					csvReader.Configuration.RegisterClassMap(new FlightMap());
					flights = csvReader.GetRecords<Flight>().ToList();
				}
			}
			catch (FileNotFoundException ex)
			{
				//Log exception
			}
			catch (Exception ex)
			{
			}
			return flights;
		}
	}
}