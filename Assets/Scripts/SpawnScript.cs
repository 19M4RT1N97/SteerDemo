using System;
using System.Collections.Generic;
using DefaultNamespace;
using Logger;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public delegate void NewVehicle(GameObject vehicle);

public class SpawnScript : MonoBehaviour
{
    private Vector3 _startPosition = new Vector3(0, 0.2f, 10f);
    [SerializeField] private LayerMask _roadLayer;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private SimpleVisualizer _visualizer;
    [SerializeField] private GameObject _recommendedText;
    private DriveLogger _logger;
    private Random _random;
    private DateTime? _startTime;
    private Text _recommendation;
    private List<int> speeds;
    
    [HideInInspector]
    public GameObject Vehicle { get; set; }
    [HideInInspector]
    public int Angle { get; set; }

    public static event NewVehicle sendNewVehicle;
    public void Start()
    {
        _random = new Random();
        speeds = new List<int> { 50, 70, 100 };

        SpawnNewVehicle();
    }

    public void AddListenerToVehicleEvent(Action<GameObject> vehicleInfo)
    {
        sendNewVehicle += new NewVehicle(vehicleInfo);
    }
    
    private void FixedUpdate()
    {
        if (!_startTime.HasValue && _logger != null && _logger.CurrentStreetAngle.HasValue)
        {
            _startTime = DateTime.Now;
        }else if(_startTime.HasValue && DateTime.Now.Subtract(_startTime.Value).TotalMinutes > 1)
        {
            _logger.ResetLogging();
            SpawnNewVehicle();
        }
        if (Vehicle &&
            !Physics.Raycast(Vehicle.transform.position, Vector3.down, 50, _roadLayer))
        {
            _logger.ResetLogging();
            SpawnNewVehicle();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.CompareTag("Car"))
        {
            _logger.FinishDriverLog();
            SpawnNewVehicle();
        }
    }

    private void SpawnNewVehicle()
    {
        Destroy(Vehicle);
        Vehicle = Instantiate(_prefab, _startPosition, Quaternion.Euler(Vector3.zero));
        _logger = Vehicle.GetComponent<DriveLogger>();
        
        //Angle = _random.Next(0, 36)*5-90;//[-90, 90]
        //Angle = _random.Next(0, 20)*5-50;//[-50, 50]
        Angle = _random.Next(0, 30)*5-75;//[-75, 75]
        
        // _visualizer.VisualizeSequence(angle, 10-Math.Abs(angle/10));
        _visualizer.Visualize(Angle, 4);
        _startTime = null;

        if (_recommendation == null)
        {
            _recommendation = _recommendedText.GetComponent<Text>();
        }

        _recommendation.text = speeds[_random.Next(0, 3)].ToString();

        _logger.SetLoggingValues(Vehicle.GetComponent<RCC_CarControllerV3>(), Angle, Int32.Parse(_recommendation.text));
        if (sendNewVehicle != null) sendNewVehicle.Invoke(Vehicle);
    }
}
