using System.Collections.Generic;
using Logger;
using UnityEngine;
using Random = System.Random;

public class SpawnScript : MonoBehaviour
{
    [SerializeField] private List<Vector3> _startPositions;
    [SerializeField] private LayerMask _roadLayer;
    private VehicleControl _vehicleControl;
    private Rigidbody _vehicleBody;
    private DriveLogger _logger;
    private Random _random;
    

    private void Start()
    {
        _vehicleControl = gameObject.GetComponent<VehicleControl>();
        _vehicleBody = gameObject.GetComponent<Rigidbody>();
        _logger = gameObject.GetComponent<DriveLogger>();
        _random = new Random();
    }

    private void FixedUpdate()
    {
        if (!Physics.Raycast(this.gameObject.transform.position, Vector3.down, 50, _roadLayer))
        {
            _logger.ResetLogging();
            RelocateVehicle();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            RelocateVehicle();
        }
    }

    private void RelocateVehicle()
    {
        _vehicleControl.transform.localPosition = _startPositions[_random.Next(0, _startPositions.Count)];
        _vehicleControl.transform.rotation = Quaternion.Euler(Vector3.zero);
        _vehicleControl.ResetCar();
        _vehicleBody.angularVelocity = Vector3.zero;
        _vehicleBody.velocity = Vector3.zero;
    }
}
