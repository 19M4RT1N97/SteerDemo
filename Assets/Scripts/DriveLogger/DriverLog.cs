using System;
using UnityEngine;

namespace DriveLogger
{
    public class DriverLog : LogBase
    {
        public DriverLog()
        {
            _timeStamp = DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss.fff");
        }
        
        private readonly string _timeStamp;
        public int DriverId { get; set; }
        public int CurveId { get; set; }

        public override string GetCsv()
        {
            return $"{_timeStamp};{DriverId};{CurveId}";
        }

        public override string GetCsvHeaders()
        {
            return "TimeStamp;DriverId;CurveId";
        }
    }
}