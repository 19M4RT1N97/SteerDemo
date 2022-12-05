using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace DriveLogger
{
    public class DriveLogger : MonoBehaviour
    {
        [SerializeField] private int LogInterval;
        [SerializeField] private string FileName;
        [SerializeField] private List<Vector3> _startPositions;
        private static List<DriverLog> DriverLog { get; set; }
        private VehicleControl _vehicle;
        private int? _currentStreet;
        private int _currLogInterval;
        private Random _random;

        public void Start()
        {
            _vehicle = gameObject.GetComponent<VehicleControl>();
            ResetLogging();
            _random = new Random();
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
                RelocateVehicle();
            }else if (other.CompareTag("Start"))
            {
                _currentStreet = other.GetComponent<StartBoxScript>().GetStreetId();
            }
        }

        private void RelocateVehicle()
        {
            _vehicle.transform.localPosition = _startPositions[_random.Next(0, _startPositions.Count)];
            _vehicle.accel = 0f;
            _vehicle.steer = 0f;
            _vehicle.currentGear = 0;
        }

        private void ResetLogging()
        {
            DriverLog = new List<DriverLog>();
            _currentStreet = null;
            _currLogInterval = 0;
        }
    }
}