using System;
using DefaultNamespace;
using Logger;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

public class SpawnScript : MonoBehaviour
{
    private Vector3 _startPosition = new Vector3(0, 0.2f, 10f);
    [SerializeField] private LayerMask _roadLayer;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private SimpleVisualizer _visualizer;
    private StartBoxScript _start;
    private GameObject _vehicle;
    private DriveLogger _logger;
    private Random _random;
    private DateTime? _startTime;
    private RCC_CarControllerV3 _carController;

    public void Start()
    {
        _start = GameObject.Find("Start").GetComponent<StartBoxScript>();
        _random = new Random();
        SpawnNewVehicle();
    }

    private void FixedUpdate()
    {
        if (!_startTime.HasValue && _logger._currentStreetAngle.HasValue)
        {
            _startTime = DateTime.Now;
        }else if(_startTime.HasValue && DateTime.Now.Subtract(_startTime.Value).TotalMinutes > 1)
        {
            _logger.ResetLogging();
            SpawnNewVehicle();
        }
        if (_vehicle &&
            !Physics.Raycast(_vehicle.transform.position, Vector3.down, 50, _roadLayer))
        {
            _logger.ResetLogging();
            SpawnNewVehicle();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.CompareTag("Car"))
        {
            SpawnNewVehicle();
            _logger.FinishDriverLog();
            _logger.ResetLogging();
        }
    }

    private void SpawnNewVehicle()
    {
        Destroy(_vehicle);
        _vehicle = Instantiate(_prefab, _startPosition, Quaternion.Euler(Vector3.zero));
        _carController = _vehicle.GetComponent<RCC_CarControllerV3>();
        _logger = _vehicle.GetComponent<DriveLogger>();
        
        // var angle = _random.Next(0, 36)*5-90;//[-90, 90]
        var angle = _random.Next(0, 20)*5-50;//[-50, 50]
        
        // _visualizer.VisualizeSequence(angle, 10-(int)math.sqrt(Math.Pow(angle/10,2)));
        _visualizer.VisualizeSequence(angle, 10-(int)math.sqrt(Math.Pow(angle/10,2)));
        _start.StreetAngle = angle;
        _startTime = null;
    }
}
