using System.Collections.Generic;
using System.Security.Cryptography;
using DefaultNamespace;
using Logger;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class SpawnScript : MonoBehaviour
{
    private Vector3 _startPosition = new Vector3(0, 0.15f, 10f);
    [SerializeField] private LayerMask _roadLayer;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private SimpleVisualizer _visualizer;
    [SerializeField] private List<string> _sequences;
    private StartBoxScript _start;
    private GameObject _vehicle;
    private DriveLogger _logger;

    private Random _random;
    private float angle;

    public void Start()
    {
        _start = GameObject.Find("Start").GetComponent<StartBoxScript>();
        _random = new Random();
        SpawnNewVehicle();
    }

    private void FixedUpdate()
    {
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
        _vehicle.GetComponent<VehicleControl>().activeControl = true;
        _logger = _vehicle.GetComponent<DriveLogger>();
        var index = _random.Next(0, _sequences.Count);
        var angle = index * 5 - 90;
        _visualizer.VisualizeSequence(angle, _sequences[index]);
        _start.StreetAngle = angle;

    }
}
