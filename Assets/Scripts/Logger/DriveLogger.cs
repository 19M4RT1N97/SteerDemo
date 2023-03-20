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
        private static List<DriverLog> DriverLog; 
        private RCC_CarControllerV3 _vehicle;
        private SimpleVisualizer _visualizer;
        private bool _logging;
     
        public int? CurrentStreetAngle;
        public int? RecSpeed;
        
        public void Start()
        {
            _vehicle = gameObject.GetComponent<RCC_CarControllerV3>();
            _visualizer = GameObject.Find("SimpleVisualizer").GetComponent<SimpleVisualizer>();
        }

        public void FixedUpdate()
        {Debug.Log(CurrentStreetAngle.HasValue);
            if (_logging)
            {
                if (DriverLog == null)
                    DriverLog = new List<DriverLog>();
                if (CurrentStreetAngle.HasValue && RecSpeed.HasValue)
                {
                    var statistics = _visualizer.GetSplineData(_vehicle.gameObject);
                    DriverLog.Add(new DriverLog
                    {
                        CurveAngle = CurrentStreetAngle.Value,
                        Speed = _vehicle.speed,
                        RecSpeed = RecSpeed.Value,
                        Steering = _vehicle.inputs.steerInput,
                        Brake = _vehicle.inputs.brakeInput,
                        Throttle = _vehicle.inputs.throttleInput,
                        Distance = statistics.Distance,
                        RoadPercentage = statistics.RoadPercent
                    });
                }
            }
        }
        
        public void ResetLogging()
        {
            DriverLog = new List<DriverLog>();
            CurrentStreetAngle = null;
            RecSpeed = null;
            _vehicle = null;
            _logging = false;
        }

        public void FinishDriverLog()
        {
            var filePath = FileName;
            Log(filePath);
            ResetLogging();
        }

        public void SetLoggingValues(RCC_CarControllerV3 vehicle, int angle, int recSpeed)
        {
            _vehicle = vehicle;
            CurrentStreetAngle = angle;
            RecSpeed = recSpeed;
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

        public void OnTriggerEnter(Collider other)
        {
            if (other.name.Equals("Start"))
            {
                _logging = true;
            }
        }
    }
}