using System;

namespace SLEDHelicopter.Data.Models
{
    public class Flight
    {
        public string LogNumber { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
		public DateTimeOffset StartedAt { get; set; }
		public DateTimeOffset CompletedAt { get; set; }
        public string Aircraft { get; set; }
        public string RequestingAgency { get; set; }
        public string CallStatus { get; set; }
        public string Nature1 { get; set; }
        public string Nature2 { get; set; }
        public string Nature3 { get; set; }
        public string County { get; set; }
        public string Pilot1 { get; set; }
        public string Pilot2 { get; set; }
        public string PicTime1 { get; set; }
        public string Nvg1 { get; set; }
        public string PicTime2 { get; set; }
        public string Nvg2 { get; set; }
        public string InsHours1 { get; set; }
        public string InsApp1 { get; set; }
        public string InsHours2 { get; set; }
        public string InsApp2 { get; set; }
        public string Crew { get; set; }
        public string Result { get; set; }
        public string FlirUsed { get; set; }
        public string MicrowaveUsed { get; set; }
        public string WeaponInvolved { get; set; }
        public string TotalFuel { get; set; }
        public string Recoveries { get; set; }
        public string Arrest { get; set; }
        public decimal HobbsIn { get; set; }
        public decimal HobbsOut { get; set; }
        public Int32 StartCounter_Initial { get; set; }
        public Int32 StartCounter_Additional { get; set; }
        public Int32 StartCounter_Total { get; set; }
        public Int32 RinCount_Initial { get; set; }
        public Int32 RinCount_Additional { get; set; }
        public Int32 RinCount_Total { get; set; }
        public decimal Duration { get; set; }
        public decimal AcTotal { get; set; }
        public Int32 Hoist_Initial { get; set; }
        public Int32 Hoist_Additional { get; set; }
        public Int32 Hoist_Total { get; set; }
        public Int32 Takeoff_Initial { get; set; }
        public Int32 Takeoff_Additional { get; set; }
        public Int32 Takeoff_Total { get; set; }
        public string InspDue { get; set; }
        public string MaintenanceRequired { get; set; }
        public string ApprovedBy { get; set; }
    }
}