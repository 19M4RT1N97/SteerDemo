using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DriveLogger
{
    public class DriveLogger : MonoBehaviour
    {
        [SerializeField] private int LogInterval;
        [SerializeField] private string FileName; 
        
        private const string logPath = "DriveLogs/";
        private static List<DriverLog> DriverLog { get; set; }

        private int _currLogInterval;
        public void Start()
        {
            DriverLog = new List<DriverLog>();
            _currLogInterval = 0;
        }

        public void FixedUpdate()
        {
            if (++_currLogInterval >= LogInterval)
            {
                AddDriverLog(new DriverLog{DriverId = 1, CurveId = 1});
                _currLogInterval = 0;
            }
        }

        private void AddDriverLog(DriverLog x) => DriverLog.Add(x);

        public void FinishDriverLog()
        {
            var filePath = logPath + FileName;
            Log(filePath);
            DriverLog.Clear();
        }

        private void Log(string path)
        {
            var newFile = File.Exists(path);
            var csv = new StringBuilder();

            if (!newFile)
            {
                csv.AppendLine(DriverLog.First().GetCsvHeaders());
            }

            foreach (var l in DriverLog)
            {
                csv.AppendLine(l.GetCsv());
            }

            if (newFile)
            {
                File.AppendAllText(path, csv.ToString());
            }
            else
            {
                File.WriteAllText(path, csv.ToString());
            }
        }
    }
}