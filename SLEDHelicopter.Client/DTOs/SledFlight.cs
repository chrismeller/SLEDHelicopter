using System;

namespace SLEDHelicopter.Client.DTOs
{
    public class SledFlight
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
        public Counter StartCounter { get; set; }
        public Counter RinCount { get; set; }
        public decimal Duration { get; set; }
        public decimal AcTotal { get; set; }
        public Counter Hoist { get; set; }
        public Counter Takeoff { get; set; }
        public string InspDue { get; set; }
        public string MaintenanceRequired { get; set; }
        public string ApprovedBy { get; set; }
    }

    public class Counter
    {
        public Int32 Initial { get; set; }
        public Int32 Additional { get; set; }
        public Int32 Total { get; set; }
    }
}