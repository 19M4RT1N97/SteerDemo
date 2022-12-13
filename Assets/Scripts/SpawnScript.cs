using System;
using System.Collections.Generic;
using Logger;
using UnityEngine;
using Random = System.Random;

public class SpawnScript : MonoBehaviour
{
    private Vector3 _startPosition = new Vector3(2.5f, 0.2f, -95f);
    [SerializeField] private LayerMask _roadLayer;
    private GameObject _vehicle;
    private DriveLogger _logger;
    private GameObject _ogCar;


    public void Start()
    {
        _ogCar = GameObject.Find("RallyCarOG");
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
        if (other.CompareTag("Finish"))
        {
            SpawnNewVehicle();
        }
    }

    private void SpawnNewVehicle()
    {
        Destroy(_vehicle);
        _vehicle = Instantiate(_ogCar, _startPosition, Quaternion.Euler(Vector3.zero));
        _vehicle.GetComponent<VehicleControl>().activeControl = true;
        _logger = _vehicle.GetComponent<DriveLogger>();
    }
}
