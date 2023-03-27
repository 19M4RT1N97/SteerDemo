using System;
using System.Globalization;

namespace Logger
{
    public class DriverLog : LogBase
    {
        public DriverLog()
        {
            TimeStamp = DateTime.Now.ToString("HH:mm:ss.fff");
        }
        private readonly string TimeStamp;
        
        public int CurveAngle { get; set; }
        
        public float Steering { get; set; }
        
        public int RecSpeed { get; set; }
        
        public float Speed { get; set; }
        
        public float Brake { get; set;}
        
        public float Throttle { get; set; }
        
        public float Distance { get; set; }
        
        public float RoadPercentage { get; set; }

        public override string GetCsv()
        {
            return $"{TimeStamp};{Speed.ToString(CultureInfo.InvariantCulture).Replace(".",",")};{RecSpeed};{CurveAngle};{Brake};{Throttle.ToString(CultureInfo.InvariantCulture).Replace(".",",")};{Distance.ToString(CultureInfo.InvariantCulture).Replace(".",",")};{RoadPercentage.ToString(CultureInfo.InvariantCulture).Replace(".",",")};{Steering.ToString(CultureInfo.InvariantCulture).Replace(".",",")}";
        }

        public override string GetCsvHeaders()
        {
            return $"{nameof(TimeStamp)};{nameof(Speed)};{nameof(RecSpeed)};{nameof(CurveAngle)};{nameof(Brake)};{nameof(Throttle)};{nameof(Distance)};{nameof(RoadPercentage)};y;";
        }
    }
}