using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class SimpleVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject _finish;
        [SerializeField] private GameObject prefab;
        private float _length = 30f;
        private SplineDone _spline;
        private SplineMesh _splineMesh;
        private Vector3 _startPosition = new Vector3(0f, 0f, 90f);
        private GameObject _endStreight;
        private void Awake()
        {
            _splineMesh = gameObject.AddComponent<SplineMesh>();
            _spline = gameObject.AddComponent<SplineDone>();
        }

        public void VisualizeSequence(int angle, int iterationAmount)
        { 
            //cleanup
            _spline.ClearAnchors();
            Destroy(_endStreight);
            
            //create spline
            _spline.AddAnchor(_startPosition);
            var currentPosition = _startPosition;
            var direction = Vector3.forward;
            
            for (int i = 0; i < iterationAmount; i++)
            {
                if (i != 0)
                {
                    direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                }
                currentPosition += direction * _length;
                _spline.AddAnchor(currentPosition);
            }
            
            //update mesh
            _splineMesh.spline = _spline;
            _splineMesh.UpdateMesh();
            
            //create prefabs and do finishing touches
            _endStreight = Instantiate(prefab, currentPosition + direction * (prefab.transform.localScale.z/2),
                Quaternion.Euler(new Vector3(0, (iterationAmount - 1) * angle, 0)));
            
            _finish.transform.position = _endStreight.transform.position + new Vector3(0,5,0) + direction * (prefab.transform.localScale.z/2);
            _finish.transform.rotation = Quaternion.Euler(new Vector3(0,(iterationAmount-1)*angle,0));
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
    }
}