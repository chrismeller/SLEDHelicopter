using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using NodaTime;
using NodaTime.TimeZones;
using SLEDHelicopter.Client.DTOs;
using SLEDHelicopter.Client.Exception;

namespace SLEDHelicopter.Client
{
    public class SledClient
    {
        // http://services.sled.sc.gov/Aviation/CopViewPublic2.aspx?LOGID=1993-1001
        private const string BaseUrl = "http://services.sled.sc.gov/Aviation/CopViewPublic2.aspx";

        public async Task<SledFlight> Fetch(string logId)
        {
            using (var http = new HttpClient())
            {
                var response = await http.GetStringAsync(string.Format("{0}?LOGID={1}", BaseUrl, logId));

                // stupid webforms app - everything is a 200 status code
                if (response.Contains("Please contact the System Administrator"))
                {
                    throw new FlightDoesNotExistException();
                }

                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(response);

                var logNumber = doc.DocumentNode.SelectSingleNode("//input[ @id='txtLog' ]")
                    .GetAttributeValue("value", "");
                var fromDate = doc.DocumentNode.SelectSingleNode("//input[ @id='txtFromDate' ]")
                    .GetAttributeValue("value", "");
                var toDate = doc.DocumentNode.SelectSingleNode("//input[ @id='txtToDate' ]")
                    .GetAttributeValue("value", "");
                var fromTime = doc.DocumentNode.SelectSingleNode("//input[ @id='txtFromTime' ]")
                    .GetAttributeValue("value", "");
                var toTime = doc.DocumentNode.SelectSingleNode("//input[ @id='txtToTime' ]")
                    .GetAttributeValue("value", "");
                var aircraft = doc.DocumentNode.SelectSingleNode("//input[ @id='txtAirCraft' ]")
                    .GetAttributeValue("value", "");
                var requestingAgency = doc.DocumentNode.SelectSingleNode("//input[ @id='txtAgency' ]")
                    .GetAttributeValue("value", "");
                var callStatus = doc.DocumentNode.SelectSingleNode("//input[ @id='txtCallStatus' ]")
                    .GetAttributeValue("value", "");
                var nature1 = doc.DocumentNode.SelectSingleNode("//input[ @id='txtNature1' ]")
                    .GetAttributeValue("value", "");
                var nature2 = doc.DocumentNode.SelectSingleNode("//input[ @id='txtNature2' ]")
                    .GetAttributeValue("value", "");
                var nature3 = doc.DocumentNode.SelectSingleNode("//input[ @id='txtNature3' ]")
                    .GetAttributeValue("value", "");
                var county = doc.DocumentNode.SelectSingleNode("//input[ @id='txtCounty' ]")
                    .GetAttributeValue("value", "");
                var pilot1 = doc.DocumentNode.SelectSingleNode("//input[ @id='txtPilot1' ]")
                    .GetAttributeValue("value", "");
                var pilot2 = doc.DocumentNode.SelectSingleNode("//input[ @id='txtPilot2' ]")
                    .GetAttributeValue("value", "");
                var picTime1 = doc.DocumentNode.SelectSingleNode("//input[ @id='txtPicTime1' ]")
                    .GetAttributeValue("value", "");
                var nvg1 = doc.DocumentNode.SelectSingleNode("//input[ @id='txtNVG1' ]").GetAttributeValue("value", "");
                var picTime2 = doc.DocumentNode.SelectSingleNode("//input[ @id='txtPicTime1' ]")
                    .GetAttributeValue("value", "");
                var nvg2 = doc.DocumentNode.SelectSingleNode("//input[ @id='txtNVG2' ]").GetAttributeValue("value", "");
                var insHours1 = doc.DocumentNode.SelectSingleNode("//input[ @id='txtInsHrs1' ]")
                    .GetAttributeValue("value", "");
                var insApp1 = doc.DocumentNode.SelectSingleNode("//input[ @id='txtInsApp1' ]")
                    .GetAttributeValue("value", "");
                var insHours2 = doc.DocumentNode.SelectSingleNode("//input[ @id='txtInsHrs2' ]")
                    .GetAttributeValue("value", "");
                var insApp2 = doc.DocumentNode.SelectSingleNode("//input[ @id='txtInsApp2' ]")
                    .GetAttributeValue("value", "");
                var crew = doc.DocumentNode.SelectSingleNode("//span[ @id='lblCrewList' ]")
                    .GetAttributeValue("value", "");
                var result = doc.DocumentNode.SelectSingleNode("//input[ @id='txtResult' ]")
                    .GetAttributeValue("value", "");
                var flirUsed = doc.DocumentNode.SelectSingleNode("//input[ @id='txtFlir' ]")
                    .GetAttributeValue("value", "");
                var microwaveUsed = doc.DocumentNode.SelectSingleNode("//input[ @id='txtMicrowave' ]")
                    .GetAttributeValue("value", "");
                var weaponInvolved = doc.DocumentNode.SelectSingleNode("//input[ @id='txtWeaponInvolved' ]")
                    .GetAttributeValue("value", "");
                var totalFuel = doc.DocumentNode.SelectSingleNode("//input[ @id='txtFuel' ]")
                    .GetAttributeValue("value", "");
                var recoveries = doc.DocumentNode.SelectSingleNode("//input[ @id='txtRecover' ]")
                    .GetAttributeValue("value", "");
                var arrest = doc.DocumentNode.SelectSingleNode("//input[ @id='txtArrest' ]")
                    .GetAttributeValue("value", "");
                var hobbsIn = Convert.ToDecimal(doc.DocumentNode.SelectSingleNode("//input[ @id='txtArrival' ]")
                    .GetAttributeValue("value", "0"));
                var hobbsOut = Convert.ToDecimal(doc.DocumentNode.SelectSingleNode("//input[ @id='txtDepart' ]")
                    .GetAttributeValue("value", "0"));
                var startCounter = Convert.ToInt32(doc.DocumentNode.SelectSingleNode("//input[ @id='txtStartCnt' ]")
                    .GetAttributeValue("value", "0"));
                var startCounterAdd = Convert.ToInt32(doc.DocumentNode.SelectSingleNode("//input[ @id='txtStartCntAdd' ]")
                    .GetAttributeValue("value", "0"));
                var startCounterTotal = Convert.ToInt32(doc.DocumentNode.SelectSingleNode("//input[ @id='txtTotStart' ]")
                    .GetAttributeValue("value", "0"));
                var rinCount = Convert.ToInt32(doc.DocumentNode.SelectSingleNode("//input[ @id='txtRin' ]")
                    .GetAttributeValue("value", "0"));
                var rinCountAdd = Convert.ToInt32(doc.DocumentNode.SelectSingleNode("//input[ @id='txtRinAdd' ]")
                    .GetAttributeValue("value", "0"));
                var rinCountTotal = Convert.ToInt32(doc.DocumentNode.SelectSingleNode("//input[ @id='txtTotRin' ]")
                    .GetAttributeValue("value", "0"));
                var duration = Convert.ToDecimal(doc.DocumentNode.SelectSingleNode("//input[ @id='txtDuration' ]")
                    .GetAttributeValue("value", "0"));
                var acTotal = Convert.ToDecimal(doc.DocumentNode.SelectSingleNode("//input[ @id='txtHobbsTotal' ]")
                    .GetAttributeValue("value", "0"));
                var hoist = Convert.ToInt32(doc.DocumentNode.SelectSingleNode("//input[ @id='txtHoist' ]")
                    .GetAttributeValue("value", "0"));
                var hoistAdd = Convert.ToInt32(doc.DocumentNode.SelectSingleNode("//input[ @id='txtHoistAdd' ]")
                    .GetAttributeValue("value", "0"));
                var hoistTotal = Convert.ToInt32(doc.DocumentNode.SelectSingleNode("//input[ @id='txtTotHoist' ]")
                    .GetAttributeValue("value", "0"));
                var takeoff = Convert.ToInt32(doc.DocumentNode.SelectSingleNode("//input[ @id='txtTakeOff' ]")
                    .GetAttributeValue("value", "0"));
                var takeoffAdd = Convert.ToInt32(doc.DocumentNode.SelectSingleNode("//input[ @id='txtTakeOffAdd' ]")
                    .GetAttributeValue("value", "0"));
                var takeoffTotal = Convert.ToInt32(doc.DocumentNode.SelectSingleNode("//input[ @id='txtTotTakeOff' ]")
                    .GetAttributeValue("value", "0"));
                var inspDue = doc.DocumentNode.SelectSingleNode("//input[ @id='txtInsp' ]")
                    .GetAttributeValue("value", "");
                var maintenanceRequired = doc.DocumentNode.SelectSingleNode("//span[ @id='lblMainReq' ]")
                    .GetAttributeValue("value", "");
                var approvedBy = doc.DocumentNode.SelectSingleNode("//input[ @id='txtApproved' ]")
                    .GetAttributeValue("value", "");

                var from = DateTimeOffset.ParseExact(String.Format("{0} {1}", fromDate, fromTime), "MM/dd/yyyy HHmm", new DateTimeFormatInfo());
                var fromLocal = new LocalDateTime(from.Year, from.Month, from.Day, from.Hour, from.Minute);
                var fromZoned = fromLocal.InZone(DateTimeZoneProviders.Tzdb["America/New_York"],
                    Resolvers.LenientResolver);

                var to = DateTimeOffset.ParseExact(String.Format("{0} {1}", toDate, toTime), "MM/dd/yyyy HHmm",
                    new DateTimeFormatInfo());
                var toLocal = new LocalDateTime(to.Year, to.Month, to.Day, to.Hour, to.Minute);
                var toZoned = toLocal.InZone(DateTimeZoneProviders.Tzdb["America/New_York"], Resolvers.LenientResolver);

                var flight = new SledFlight()
                {
                    LogNumber = logNumber,
                    FromDate = fromDate,
                    ToDate = toDate,
                    FromTime = fromTime,
                    ToTime = toTime,
                    Aircraft = aircraft,
                    RequestingAgency = requestingAgency,
                    CallStatus = callStatus,
                    Nature1 = nature1,
                    Nature2 = nature2,
                    Nature3 = nature3,
                    County = county,
                    Pilot1 = pilot1,
                    Pilot2 = pilot2,
                    PicTime1 = picTime1,
                    PicTime2 = picTime2,
                    Nvg1 = nvg1,
                    Nvg2 = nvg2,
                    InsHours1 = insHours1,
                    InsHours2 = insHours2,
                    InsApp1 = insApp1,
                    InsApp2 = insApp2,
                    Crew = crew,
                    Result = result,
                    FlirUsed = flirUsed,
                    MicrowaveUsed = microwaveUsed,
                    WeaponInvolved = weaponInvolved,
                    TotalFuel = totalFuel,
                    Recoveries = recoveries,
                    Arrest = arrest,
                    HobbsIn = hobbsIn,
                    HobbsOut = hobbsOut,
                    StartCounter = new Counter()
                    {
                        Initial = startCounter,
                        Additional = startCounterAdd,
                        Total = startCounterTotal
                    },
                    RinCount = new Counter()
                    {
                        Initial = rinCount,
                        Additional = rinCountAdd,
                        Total = rinCountTotal
                    },
                    Duration = duration,
                    AcTotal = acTotal,
                    Hoist = new Counter()
                    {
                        Initial = hoist,
                        Additional = hoistAdd,
                        Total = hoistTotal
                    },
                    Takeoff = new Counter()
                    {
                        Initial = takeoff,
                        Additional = takeoffAdd,
                        Total = takeoffTotal
                    },
                    InspDue = inspDue,
                    MaintenanceRequired = maintenanceRequired,
                    ApprovedBy = approvedBy,
                    From = fromZoned.ToDateTimeOffset(),
                    To = toZoned.ToDateTimeOffset()
                };

                return flight;

                //Console.WriteLine("Log Number: {0}", logNumber);
                //Console.WriteLine("From Date: {0}", fromDate);
                //Console.WriteLine("To Date: {0}", toDate);
                //Console.WriteLine("From Time: {0}", fromTime);
                //Console.WriteLine("To Time: {0}", toTime);
                //Console.WriteLine("Aircraft: {0}", aircraft);
                //Console.WriteLine("Requesting Agency: {0}", requestingAgency);
                //Console.WriteLine("Call Status: {0}", callStatus);
                //Console.WriteLine("Nature 1: {0}", nature1);
                //Console.WriteLine("Nature 2: {0}", nature2);
                //Console.WriteLine("Nature 3: {0}", nature3);
                //Console.WriteLine("County: {0}", county);
                //Console.WriteLine("Pilot 1: {0}", pilot1);
                //Console.WriteLine("Pilot 2: {0}", pilot2);
                //Console.WriteLine("PIC Time 1: {0}", picTime1);
                //Console.WriteLine("NVG1: {0}", nvg1);
                //Console.WriteLine("PIC Time 2: {0}", picTime2);
                //Console.WriteLine("NVG2: {0}", nvg2);
                //Console.WriteLine("Ins Hours 1: {0}", insHours1);
                //Console.WriteLine("Ins App 1: {0}", insApp1);
                //Console.WriteLine("Ins Hours 2: {0}", insHours2);
                //Console.WriteLine("Ins App 2: {0}", insApp2);
                //Console.WriteLine("Crew: {0}", crew);
                //Console.WriteLine("Result: {0}", result);
                //Console.WriteLine("FLIR Used: {0}", flirUsed);
                //Console.WriteLine("Microwave Used: {0}", microwaveUsed);
                //Console.WriteLine("Weapon Involved: {0}", weaponInvolved);
                //Console.WriteLine("Total Fuel: {0}", totalFuel);
                //Console.WriteLine("Recoveries: {0}", recoveries);
                //Console.WriteLine("Arrest: {0}", arrest);
                //Console.WriteLine("HOBBS In: {0}", hobbsIn);
                //Console.WriteLine("HOBBS Out: {0}", hobbsOut);
                //Console.WriteLine("Start Counter: {0} + {1} = {2}", startCounter, startCounterAdd, startCounterTotal);
                //Console.WriteLine("RIN Count: {0} + {1} = {2}", rinCount, rinCountAdd, rinCountTotal);
                //Console.WriteLine("Duration: {0}", duration);
                //Console.WriteLine("A/C Total: {0}", acTotal);
                //Console.WriteLine("Hoist: {0} + {1} = {2}", hoist, hoistAdd, hoistTotal);
                //Console.WriteLine("Takeoff: {0} + {1} = {2}", takeoff, takeoffAdd, takeoffTotal);
                //Console.WriteLine("Insp Due: {0}", inspDue);
                //Console.WriteLine("Maintenance Required: {0}", maintenanceRequired);
                //Console.WriteLine("Approved By: {0}", approvedBy);
            }
        }
    }
}
