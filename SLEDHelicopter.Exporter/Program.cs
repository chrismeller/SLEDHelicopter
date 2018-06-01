using SLEDHelicopter.Client;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure;
using SLEDHelicopter.Client.Exception;
using SLEDHelicopter.Domain;
using Microsoft.WindowsAzure.Storage;
using SLEDHelicopter.Atom;
using SLEDHelicopter.Client.DTOs;

namespace SLEDHelicopter.Exporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Run().ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine("Done!");
        }

        public static async Task Run()
        {
			//await DownloadOne();
			//await DownloadAll();
			//await ShowFlights();
			await Update();

			//await GenerateFeed();
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

        public static async Task Update()
        {
	        using (var service = new FlightService())
	        {
		        var latest = await service.GetLatest();

				// if there is no latest flight, start from the beginning
		        if (latest == null)
		        {
			        await DownloadAll();
			        await GenerateFeed();
			        return;
		        }

		        var year = Convert.ToInt32(latest.Split('-').First());
		        var num = Convert.ToInt32(latest.Split('-').Last());

		        Console.WriteLine("Starting update from {0}-{1}", year, (num + 1));

		        await DownloadAll(year, num + 1);

		        await GenerateFeed();
	        }
        }

	    public static async Task GenerateFeed()
	    {
		    using (var service = new FlightService())
		    {
			    var flights = await service.GetFlightsForAtom(50);

			    var items = flights.Select(x => new AtomFeedItem()
			    {
				    Id = Guid.Parse(Md5(x.LogNumber)),
				    Link = new Uri("http://services.sled.sc.gov/Aviation/CopViewPublic2.aspx?LOGID=" + x.LogNumber),
				    Summary = BuildDescription(x),
				    Title = "SLED Helicopter Flight " + x.LogNumber,
				    LastUpdated = x.CompletedAt,
				    AuthorName = "South Carolina Law Enforcement Division",
				    Category = $"Aircraft N{x.Aircraft}",
			    }).ToList();

			    var f = new AtomFeed()
			    {
				    AuthorName = "South Carolina Law Enforcement Division",
				    Id = Guid.Parse("EFF7C905-20B3-49B7-9503-D67E181289A2"),
				    LastUpdated = DateTimeOffset.UtcNow,
				    Link = new Uri("http://www.sled.sc.gov"),
				    //LinkSelf = new Uri("http://www.sled.sc.gov"),
				    Title = "South Carolina Law Enforcement Division Helicopter Flight Log",
				    Items = items
			    };

			    var feed = AtomFeedGenerator.GenerateFeed(f);

			    await UploadFile("sledflights.xml", feed, "text/xml");
			}
	    }

	    private static string BuildDescription(SledFlight flight)
	    {
		    var time = String.Join(flight.StartedAt.ToString("h"),
			    (flight.StartedAt.ToString("mm") == "00") ? "" : flight.StartedAt.ToString(":mm"), flight.StartedAt.ToString("tt").ToLower());
		    var date = flight.StartedAt.ToString("MMM d");

		    var message = "";
		    if (flight.FlirUsed.ToUpper() == "Y" && flight.MicrowaveUsed.ToUpper() == "Y")
		    {
			    message = "Both FLIR and Microwave were used";
		    }
			else if (flight.FlirUsed.ToUpper() != "Y" && flight.MicrowaveUsed.ToUpper() != "Y")
		    {
			    message = "Neither FLIR nor Microwave were used";
		    }
			else if (flight.FlirUsed.ToUpper() == "Y")
		    {
			    message = "FLIR was used";
		    }
		    else
		    {
			    message = "Microwave was used";
		    }

		    if (flight.WeaponInvolved.ToUpper() == "Y")
		    {
			    message = message + " and a weapon was involved.";
		    }
		    else
		    {
			    message = message + " and weapons were not involved.";
		    }

		    return
			    $"At {time} on {date} I took off to assist {flight.RequestingAgency} with {flight.Nature1} in {flight.County} County. It took {flight.Duration} hours and {flight.TotalFuel} gallons of fuel. {message}";
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

	    private static async Task UploadFile(string filename, string contents, string contentType)
	    {
		    var storageAccount =
			    CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
		    var blobClient = storageAccount.CreateCloudBlobClient();
		    var blobContainer = blobClient.GetContainerReference("opinions");

		    var blob = blobContainer.GetBlockBlobReference(filename);

		    await blob.UploadTextAsync(contents);

		    if (blob.Properties.ContentType != contentType)
		    {
			    blob.Properties.ContentType = contentType;

			    await blob.SetPropertiesAsync();
		    }
	    }

	    private static string Md5(string input)
	    {
		    using (var md5 = MD5.Create())
		    {
			    var bytes = Encoding.UTF8.GetBytes(input);
			    var hashBytes = md5.ComputeHash(bytes);

			    var bc = BitConverter.ToString(hashBytes);

			    // lowercase it and get a standardized 8-4-4-4-12 grouping
			    // in opposite order so we don't have to offset for the -'s we've already added
			    bc = bc.ToLower().Replace("-", "").Insert(20, "-").Insert(16, "-").Insert(12, "-").Insert(8, "-");

			    return bc;
		    }
	    }
	}
}
