using System;
using UnityEngine;
using DriveLogger;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var logger = DrivingLog.GetInstance();
        logger.AddDriverLog(new DriverLog{DriverId = 1, CurveId = 1});
        logger.AddDriverLog(new DriverLog{DriverId = 1, CurveId = 2});
        
        logger.FinishDriverLog();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
