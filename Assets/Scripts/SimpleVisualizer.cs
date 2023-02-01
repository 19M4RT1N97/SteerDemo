using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class SimpleVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject _finish;
        [SerializeField] private GameObject _debug;
        private float _length = 20f;
        private SplineDone _spline;
        private SplineMesh _splineMesh;
        private Vector3 _startPosition = new Vector3(0f, 0f, 90f);

        private List<GameObject> debugList;
        private void Awake()
        {
            _splineMesh = gameObject.AddComponent<SplineMesh>();
            _spline = gameObject.AddComponent<SplineDone>();
            debugList = new List<GameObject>();
        }

        public void VisualizeSequence(int angle, int iterationAmount)
        { 
            //cleanup
            _spline.ClearAnchors();
            if (debugList.Any())
                foreach (var d in debugList)
                    Destroy(d);
            debugList.Clear();
            
            //create spline
            var currentPosition = _startPosition;
            var direction = Vector3.forward;
            var sideways = Vector3.back;
            
            _spline.AddAnchor(_startPosition, sideways);
            for (int i = 0; i < iterationAmount; i++)
            {
                if (i != 0)
                {
                    direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                    sideways = Quaternion.AngleAxis(angle, Vector3.up) * sideways;
                }
                currentPosition += direction * _length;
                _spline.AddAnchor(currentPosition, sideways);
            }
            currentPosition += direction * _length/2;
            _spline.AddAnchor(currentPosition, sideways);
            _finish.transform.position = currentPosition + new Vector3(0,5,0) + direction;
            _finish.transform.rotation = Quaternion.Euler(new Vector3(0,(iterationAmount-1)*angle,0));
            currentPosition += direction * _length/4;
            _spline.AddAnchor(currentPosition, sideways);
            
            //update mesh
            _splineMesh.spline = _spline;
            _splineMesh.UpdateMesh();
            
            //debug
            //_spline.SetupPointList();
            //PrintDebug(_spline.GetPointList().Select(a => a.position));
            // PrintDebug(_spline.GetAnchorList().Select(a => a.position));
        }

        public SplineReturn GetSplineData(GameObject car)
        {
            var carTranform = car.transform;

            var pointList = _spline.GetPointList();
            int pointIndex = 0;
            float distance = float.PositiveInfinity;
            for(int i = 0; i< pointList.Count; i++)
            {
                var point = pointList[i];
                var currentDistence = Vector3.Distance(carTranform.position, point.position);
                if (currentDistence < distance)
                {
                    pointIndex = i;
                    distance = currentDistence;
                }
            }

            return new SplineReturn
            {
                Distance = distance,
                Point = pointList[pointIndex],
                RoadPercent = ((float)pointIndex/pointList.Count)*100
            };
        }

        public struct SplineReturn
        {
            public float Distance { get; set; }
            public SplineDone.Point Point { get; set; }
            public float RoadPercent { get; set; }
            
        }

        private void PrintDebug(IEnumerable<Vector3> posList)
        {
            foreach (var p in posList)
            {
                debugList.Add(GameObject.Instantiate(_debug, p, Quaternion.identity));
            }
        }
    }
}