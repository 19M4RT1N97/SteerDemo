using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DriveLogger
{
    public class Test : MonoBehaviour
    {
        private DriveLogger _logger;
        
        // Start is called before the first frame update
        void Start()
        {
            _logger = gameObject.GetComponent<DriveLogger>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Finish"))
            {
                _logger.FinishDriverLog();
            }
        }
    }
}
