using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdater : MonoBehaviour
{
    public GameObject Recommendation;
    private RCC_CarControllerV3 _vehicleController;
    private Text _currentText;
    private Text _recommendation;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Finish").GetComponent<SpawnScript>().AddListenerToVehicleEvent(GetVehicleInfo);
        _currentText = GetComponent<Text>();
        _recommendation = Recommendation.GetComponent<Text>();
    }

    private void FixedUpdate()
    {
        UpdateText();
    }

    public void GetVehicleInfo(GameObject vehicle)
    {
        _vehicleController = vehicle.GetComponent<RCC_CarControllerV3>();
        UpdateText();
    }

    private void UpdateText()
    {
        if (_recommendation == null)
        {
            _recommendation = Recommendation.GetComponent<Text>();
        }
        
        if (_vehicleController.speed < float.Parse(_recommendation.text) * 0.9 ||
            float.Parse(_recommendation.text) * 1.1 < _vehicleController.speed)
        {
            _currentText.color = Color.red;
        }
        else
        {
            _currentText.color = Color.green;
        }
        _currentText.text = _vehicleController.speed.ToString("N0");
    }
}
