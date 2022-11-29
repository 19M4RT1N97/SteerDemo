namespace DriveLogger
{
    public class DriverLog
    {
        public int DriverId { get; set; }
        public int CurveId { get; set; }

        public string getCSV()
        {
            return $"{DriverId};{CurveId}";
        }

        public static string GetCSVHeaders()
        {
            return $"DriverId;CurveId";
        }
    }
}