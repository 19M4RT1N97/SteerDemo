using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DriveLogger
{
    public class DrivingLog
    {
        private const string _logPath = "DriveLogs/";
        private static List<DriverLog> DriverLog { get; set; }

        private static readonly DrivingLog Instance = null;

        private DrivingLog()
        {
            DriverLog = new List<DriverLog>();
        }

        public static DrivingLog GetInstance() => Instance ?? new DrivingLog();

        public void AddDriverLog(DriverLog x) => DriverLog.Add(x);

        public void FinishDriverLog()
        {
            var filePath = _logPath + "DriverLog.csv";
            var newFile = File.Exists(filePath);
            var csv = new StringBuilder();

            if (!newFile)
            {
                csv.AppendLine(DriveLogger.DriverLog.GetCSVHeaders());
            }
            foreach (var l in DriverLog)
            {
                csv.AppendLine(l.getCSV());
            }
            
            if (newFile)
            {
                File.AppendAllText(filePath, csv.ToString());
            }
            else
            {
                File.WriteAllText(filePath, csv.ToString());
            }
        }
    }
}