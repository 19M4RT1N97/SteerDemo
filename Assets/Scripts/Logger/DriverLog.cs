using System;
using UnityEngine;

namespace Logger
{
    public class DriverLog : LogBase
    {
        public DriverLog()
        {
            _timeStamp = DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss.fff");
        }
        
        private readonly string _timeStamp;
        
        public int CurveId { get; set; }
        public float Steer { get; set; }
        public float Speed { get; set; }

        public override string GetCsv()
        {
            return $"{_timeStamp};{CurveId};{Speed};{Steer};";
        }

        public override string GetCsvHeaders()
        {
            return "TimeStamp;CurveId;Speed;Steering;";
        }
    }
}