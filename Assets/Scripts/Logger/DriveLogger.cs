using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Logger
{
    public class DriveLogger : MonoBehaviour
    {
        [SerializeField] private int LogInterval;
        [SerializeField] private string FileName;
        private static List<DriverLog> DriverLog { get; set; }
        private VehicleControl _vehicle;
        private int? _currentStreet;
        private int _currLogInterval;

        public void Start()
        {
            _vehicle = gameObject.GetComponent<VehicleControl>();
            ResetLogging();
        }

        public void FixedUpdate()
        {
            if (_currentStreet.HasValue
                && ++_currLogInterval >= LogInterval)
            {
                DriverLog.Add(new DriverLog
                {
                    CurveId = _currentStreet.Value,
                    Speed = _vehicle.speed,
                    Steer = _vehicle.steer
                });
                _currLogInterval = 0;
            }
        }
        
        public void ResetLogging()
        {
            DriverLog = new List<DriverLog>();
            _currentStreet = null;
            _currLogInterval = 0;
        }

        private void FinishDriverLog()
        {
            var filePath = FileName;
            Log(filePath);
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Finish"))
            {
                FinishDriverLog();
                ResetLogging();
            }
            else if (other.CompareTag("Start"))
            {
                _currentStreet = other.GetComponent<StartBoxScript>().GetStreetId();
            }
        }
    }
}