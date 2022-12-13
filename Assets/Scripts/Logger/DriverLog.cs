using System;
using UnityEngine;

namespace Logger
{
    public class DriverLog : LogBase
    {
        public DriverLog()
        {
            _datestamp = DateTime.Now.ToString("dd.mm.yyyy");
            _timeStamp = DateTime.Now.ToString("hh:mm:ss.fff");
        }

        private readonly string _datestamp;
        
        private readonly string _timeStamp;
        
        public int CurveId { get; set; }
        public float Steer { get; set; }
        public float Speed { get; set; }

        public override string GetCsv()
        {
            return $"{_datestamp};{_timeStamp};{CurveId};{Speed};{Steer};";
        }

        public override string GetCsvHeaders()
        {
            return "Date;Time;CurveId;Speed;Steering;";
        }
    }
}