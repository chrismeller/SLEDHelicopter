using SLEDHelicopter.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using SLEDHelicopter.Client.Exception;
using SLEDHelicopter.Domain;

namespace SLEDHelicopter.Exporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Run().ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine("Done!");
            Console.ReadKey();
        }

        public static async Task Run()
        {
            //await DownloadOne();
            await DownloadAll(2015, 1438);
            //await ShowFlights();
        }

        public static async Task ShowFlights()
        {
            var service = new FlightService();
            var flights = await service.GetFlights();

            foreach (var flight in flights)
            {
                Console.WriteLine("Flight {0} from {1} at {2} to {3} at {4}.", flight.LogNumber, flight.FromDate, flight.FromTime, flight.ToDate, flight.ToTime);
            }
        }

        public static async Task DownloadOne()
        {
            var client = new SledClient();
            try
            {
                var flight = await client.Fetch("1993-1100");

                var service = new FlightService();
                await service.AddFlight(flight);
            }
            catch (FlightDoesNotExistException)
            {
                Console.WriteLine("That flight does not exist!");
            }
        }

        public static async Task DownloadAll(int? firstYear = 1993, int firstNum = 1001, int? lastYear = null, int? lastNum = null)
        {
            var client = new SledClient();
            var service = new FlightService();

            var keepGoing = true;
            var year = firstYear;
            var num = firstNum;
            var numAttempts = 0;
            var yearAttempts = 0;
            var threshold = 5;
            do
            {
                try
                {
                    Console.Write("Trying to get flight {0}-{1}... ", year, num);

                    var flight = await client.Fetch(String.Format("{0}-{1}", year, num));
                    Console.Write("Scraped! ");

                    await service.AddFlight(flight);
                    Console.WriteLine("Saved!");

                    // reset our bad record counters
                    numAttempts = 0;
                    yearAttempts = 0;

                    // bump the record number
                    num++;
                }
                catch (FlightDoesNotExistException)
                {
                    Console.WriteLine("Flight not found.");

                    num++;

                    numAttempts++;

                    // if we've tried as many records as we're allowed we should have hit the last record - reset the num and bump to the next year
                    if (numAttempts >= threshold)
                    {
                        Console.WriteLine("Threshold reached. Moving to next year.");

                        year++;
                        num = 1001;

                        // we only bump year attempts when we increment years
                        yearAttempts++;

                        // reset the num attempts so we try at least 5 per year
                        numAttempts = 0;
                    }

                    if (yearAttempts >= threshold)
                    {
                        Console.WriteLine("Threshold reached. We're done.");
                        keepGoing = false;
                    }
                }

                // if we've hit the declared limit, give up
                if (lastYear.HasValue && lastNum.HasValue && year >= lastYear && num >= lastNum)
                {
                    keepGoing = false;
                }

                if (keepGoing)
                {
                    Thread.Sleep(500);
                }
            } while (keepGoing);
        }
    }
}
