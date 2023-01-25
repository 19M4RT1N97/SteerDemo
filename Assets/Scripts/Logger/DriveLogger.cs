using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DefaultNamespace;
using UnityEditor.SceneManagement;
using UnityEngine;


namespace Logger
{
    public class DriveLogger : MonoBehaviour
    {
        [SerializeField] private string FileName;
        private static List<DriverLog> DriverLog { get; set; }
        private RCC_CarControllerV3 _vehicle;
        public int? _currentStreetAngle;
        private GameObject _car;
        private SimpleVisualizer _visualizer;
        
        public void Start()
        {
            _vehicle = gameObject.GetComponent<RCC_CarControllerV3>();
            
            _visualizer = GameObject.Find("SimpleVisualizer").GetComponent<SimpleVisualizer>();
            ResetLogging();
        }

        public void FixedUpdate()
        {
            if (_currentStreetAngle.HasValue)
            {
                var statistics = _visualizer.GetSplineData(_car);
                DriverLog.Add(new DriverLog
                {
                    CurveAngle = _currentStreetAngle.Value,
                    Speed = _vehicle.speed,
                    Steering = _vehicle.steerInput,
                    Brake = _vehicle.brakeInput,
                    Distance = statistics.Distance,
                    RoadPercentage = statistics.RoadPercent
                });
            }
        }
        
        public void ResetLogging()
        {
            DriverLog = new List<DriverLog>();
            _currentStreetAngle = null;
        }

        public void FinishDriverLog()
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
            if (other.CompareTag("Start"))
            {
                var start = other.GetComponent<StartBoxScript>();
                _currentStreetAngle = start.StreetAngle;
                _car = start.Car;
            }
        }
    }
}