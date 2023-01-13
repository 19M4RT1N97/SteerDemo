using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class SimpleVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject _finish;
        
        private List<Vector3> _positions;
        //street
        // private Vector3 _startPosition = new Vector3(0f, 0f, 25f);
        
        private float _length = 10f;
        private List<GameObject> prevStreet;
        public GameObject prefab;

        private SplineDone _spline;
        private SplineMesh _splineMesh;
        //spline
        private Vector3 _startPosition = new Vector3(0f, 0f, 30f);

        private void Awake()
        {
            _splineMesh = gameObject.AddComponent<SplineMesh>();
            _spline = gameObject.AddComponent<SplineDone>();
        }

        private void Start()
        {
            _positions = new List<Vector3>();
        }

        public void VisualizeSequence(int angle, int iterationAmount)
        {
            // if (prevStreet.Count > 0)
            // {
            //     foreach (var go in prevStreet)
            //     {
            //         Destroy(go);
            //     }
            // }
            // prevStreet.Clear();
            // _positions.Clear();
            // var currentPosition = _startPosition;
            // var direction = Vector3.forward;
            // _positions.Add(currentPosition);
            //
            // for (int i = 0; i < iterationAmount; i++)
            // {
            //     //Draw
            //     currentPosition += direction * _length;
            //     var roadTile = Instantiate(prefab, currentPosition, Quaternion.Euler(0f, angle * _positions.Count, 0f));
            //     prevStreet.Add(roadTile);
            //     _positions.Add(currentPosition);
            //     
            //     //Turn
            //     direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
            // }
            //
            // var lastPart = prevStreet.Last();
            // _finish.transform.position = lastPart.transform.position + direction * (_length / 2) + new Vector3(0,5,0);
            // _finish.transform.rotation = lastPart.transform.rotation;
            
            
            _spline.ClearAnchors();
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

            foreach (var v in _spline.GetAnchorList())
            {
                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = v.position;
            }
            _splineMesh.spline = _spline;
            _splineMesh.UpdateMesh();
            var last = _spline.GetAnchorList().Last();
            _finish.transform.position = last.position + new Vector3(0,5,0);
            _finish.transform.rotation = Quaternion.Euler(direction);
        }
    }
}