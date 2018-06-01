using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SLEDHelicopter.Client.DTOs;
using SLEDHelicopter.Data;
using SLEDHelicopter.Data.Models;

namespace SLEDHelicopter.Domain
{
    public class FlightService : IDisposable
    {
        private ApplicationDbConnection _db = new ApplicationDbConnection();

        public async Task<List<SledFlight>> GetFlights()
        {
            var connection = await _db.Get();

            var dbFlights = await connection.QueryAsync<Flight>("select * from Flights");

            var flights = dbFlights.Select(x => new SledFlight()
            {
                LogNumber = x.LogNumber,
                FromDate = x.FromDate,
                FromTime = x.FromTime,
                ToDate = x.ToDate,
                ToTime = x.ToTime
            });

            return flights.ToList();
        }

	    public async Task<List<SledFlight>> GetFlightsForAtom(int num)
	    {
		    var connection = await _db.Get();

		    var dbFlights = await connection.QueryAsync<Flight>("select * from Flights order by lognumber desc");

		    var flights = dbFlights.Take(num).Select(x => new SledFlight()
		    {
			    LogNumber = x.LogNumber,
			    StartedAt = x.StartedAt,
				CompletedAt = x.CompletedAt,
			    RequestingAgency = x.RequestingAgency,
			    Nature1 = x.Nature1,
			    Nature2 = x.Nature2,
			    Nature3 = x.Nature3,
			    County = x.County,
			    Duration = x.Duration,
			    TotalFuel = x.TotalFuel,
			    FlirUsed = x.FlirUsed,
			    MicrowaveUsed = x.MicrowaveUsed,
			    WeaponInvolved = x.WeaponInvolved,
				Aircraft = x.Aircraft,
		    });

		    return flights.ToList();
	    }

        public async Task<string> GetLatest()
        {
            var connection = await _db.Get();

            var latest = await connection.QueryAsync<string>("select max(lognumber) from flights");

            return latest.First();
        }

        public async Task AddFlight(SledFlight flight)
        {
            var connection = await _db.Get();

            var dbFlight = new Flight()
            {
                LogNumber = flight.LogNumber,
                FromDate = flight.FromDate,
                ToDate = flight.ToDate,
                FromTime = flight.FromTime,
                ToTime = flight.ToTime,
				StartedAt = flight.StartedAt,
				CompletedAt = flight.CompletedAt,
                Aircraft = flight.Aircraft,
                RequestingAgency = flight.RequestingAgency,
                CallStatus = flight.CallStatus,
                Nature1 = flight.Nature1,
                Nature2 = flight.Nature2,
                Nature3 = flight.Nature3,
                County = flight.County,
                Pilot1 = flight.Pilot1,
                Pilot2 = flight.Pilot2,
                PicTime1 = flight.PicTime1,
                Nvg1 = flight.Nvg1,
                PicTime2 = flight.PicTime2,
                Nvg2 = flight.Nvg2,
                InsHours1 = flight.InsHours1,
                InsApp1 = flight.InsApp1,
                InsHours2 = flight.InsHours2,
                InsApp2 = flight.InsApp2,
                Crew = flight.Crew,
                Result = flight.Result,
                FlirUsed = flight.FlirUsed,
                MicrowaveUsed = flight.MicrowaveUsed,
                WeaponInvolved = flight.WeaponInvolved,
                TotalFuel = flight.TotalFuel,
                Recoveries = flight.Recoveries,
                Arrest = flight.Arrest,
                HobbsIn = flight.HobbsIn,
                HobbsOut = flight.HobbsOut,
                StartCounter_Initial = flight.StartCounter.Initial,
                StartCounter_Additional = flight.StartCounter.Additional,
                StartCounter_Total = flight.StartCounter.Total,
                RinCount_Initial = flight.RinCount.Initial,
                RinCount_Additional = flight.RinCount.Additional,
                RinCount_Total = flight.RinCount.Total,
                Duration = flight.Duration,
                AcTotal = flight.AcTotal,
                Hoist_Initial = flight.Hoist.Initial,
                Hoist_Additional = flight.Hoist.Additional,
                Hoist_Total = flight.Hoist.Total,
                Takeoff_Initial = flight.Takeoff.Initial,
                Takeoff_Additional = flight.Takeoff.Additional,
                Takeoff_Total = flight.Takeoff.Total,
                InspDue = flight.InspDue,
                MaintenanceRequired = flight.MaintenanceRequired,
                ApprovedBy = flight.ApprovedBy
            };

            #region insert query

            await connection.ExecuteAsync(new CommandDefinition(@"
insert into Flights (
    LogNumber, FromDate, ToDate, FromTime, ToTime, StartedAt, CompletedAt, Aircraft, RequestingAgency, CallStatus, Nature1, Nature2, Nature3, County, Pilot1, Pilot2, PicTime1, Nvg1, PicTime2, Nvg2, InsHours1, InsApp1, InsHours2, InsApp2, Crew, Result, FlirUsed, MicrowaveUsed, WeaponInvolved, TotalFuel, Recoveries, Arrest, HobbsIn, HobbsOut, StartCounter_Initial, StartCounter_Additional, StartCounter_Total, RinCount_Initial, RinCount_Additional, RinCount_Total, Duration, AcTotal, Hoist_Initial, Hoist_Additional, Hoist_Total, Takeoff_Initial, Takeoff_Additional, Takeoff_Total, InspDue, MaintenanceRequired, ApprovedBy
) values (
    @LogNumber, @FromDate, @ToDate, @FromTime, @ToTime, @StartedAt, @CompletedAt, @Aircraft, @RequestingAgency, @CallStatus, @Nature1, @Nature2, @Nature3, @County, @Pilot1, @Pilot2, @PicTime1, @Nvg1, @PicTime2, @Nvg2, @InsHours1, @InsApp1, @InsHours2, @InsApp2, @Crew, @Result, @FlirUsed, @MicrowaveUsed, @WeaponInvolved, @TotalFuel, @Recoveries, @Arrest, @HobbsIn, @HobbsOut, @StartCounter_Initial, @StartCounter_Additional, @StartCounter_Total, @RinCount_Initial, @RinCount_Additional, @RinCount_Total, @Duration, @AcTotal, @Hoist_Initial, @Hoist_Additional, @Hoist_Total, @Takeoff_Initial, @Takeoff_Additional, @Takeoff_Total, @InspDue, @MaintenanceRequired, @ApprovedBy
);
", dbFlight));

            #endregion insert query

        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db = null;
            }
        }
    }
}