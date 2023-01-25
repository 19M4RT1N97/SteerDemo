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
        
        public float Steering { get; set; }
        
        public float Speed { get; set; }
        
        public float Brake { get; set;}
        
        public float Throttle { get; set; }
        
        public float Distance { get; set; }
        
        public float RoadPercentage { get; set; }

        public override string GetCsv()
        {
            return $"{_datestamp};{_timeStamp};{CurveAngle};{Speed};{Steering};{Brake};{Throttle};{Distance};{RoadPercentage};";
        }

        public override string GetCsvHeaders()
        {
            return $"{nameof(_datestamp)};{nameof(_timeStamp)};{nameof(CurveAngle)};{nameof(Speed)};{nameof(Steering)};{nameof(Brake)};{nameof(Throttle)};{nameof(Distance)};{nameof(RoadPercentage)}";
        }
    }
}