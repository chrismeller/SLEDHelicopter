using System;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace SLEDHelicopter.Data
{
    public class ApplicationDbConnection : IDisposable
    {
        private DbConnection _connection { get; set; }

        public async Task<DbConnection> Get()
        {
            if (_connection == null)
            {
                _connection = new SQLiteConnection(@"Data Source=app.sqlite3;Version=3;");

                await CrappyMigrate();
            }

            return _connection;
        }

        private async Task CrappyMigrate()
        {
            var tables =
                await _connection.QueryAsync<string>(new CommandDefinition("select name from sqlite_master where type = @type",
                    new {type = "table"}));

            if (tables.Contains("Flights") == false)
            {
                #region create query
                await _connection.ExecuteAsync(@"
create table Flights (
	LogNumber text, 
	FromDate text, 
	ToDate text, 
	FromTime text, 
	ToTime text, 
	StartedAt text,
	CompletedAt text,
	Aircraft text, 
	RequestingAgency text, 
	CallStatus text, 
	Nature1 text, 
	Nature2 text, 
	Nature3 text, 
	County text, 
	Pilot1 text, 
	Pilot2 text, 
	PicTime1 text, 
	Nvg1 text, 
	PicTime2 text, 
	Nvg2 text, 
	InsHours1 text, 
	InsApp1 text, 
	InsHours2 text, 
	InsApp2 text, 
	Crew text, 
	Result text, 
	FlirUsed text, 
	MicrowaveUsed text, 
	WeaponInvolved text, 
	TotalFuel text, 
	Recoveries text, 
	Arrest text, 
	HobbsIn numeric, 
	HobbsOut numeric, 
	StartCounter_Initial number, 
	StartCounter_Additional number, 
	StartCounter_Total number, 
	RinCount_Initial number, 
	RinCount_Additional number, 
	RinCount_Total number, 
	Duration numeric, 
	AcTotal number, 
	Hoist_Initial number, 
	Hoist_Additional number, 
	Hoist_Total number, 
	Takeoff_Initial number, 
	Takeoff_Additional number, 
	Takeoff_Total number, 
	InspDue text, 
	MaintenanceRequired text, 
	ApprovedBy text
);
");
                #endregion

                await _connection.ExecuteAsync("create unique index UK_FLIGHTS_LOGNUMBER on Flights ( LogNumber );");
            }
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection = null;
            }
        }
    }
}