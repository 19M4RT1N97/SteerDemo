using System;
using UnityEngine;

namespace Logger
{
    public class DriverLog : LogBase
    {
        public DriverLog()
        {
            _datestamp = DateTime.Now.ToString("dd.MM.yyyy");
            _timeStamp = DateTime.Now.ToString("hh:mm:ss.fff");
        }

        private readonly string _datestamp;
        
        private readonly string _timeStamp;
        
        public int CurveAngle { get; set; }
        public float Steer { get; set; }
        public float Speed { get; set; }
        public float Distance { get; set; }
        public float RoadPercentage { get; set; }

        public override string GetCsv()
        {
            return $"{_datestamp};{_timeStamp};{CurveAngle};{Speed};{Steer};{Distance};{RoadPercentage};";
        }

        public override string GetCsvHeaders()
        {
            return "Date;Time;CurveAngle;Speed;Steering;Distance;RoadPercent;";
        }
    }
}