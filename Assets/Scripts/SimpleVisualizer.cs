using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class SimpleVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject _finish;
        private List<Vector3> _positions;
        private Vector3 _startPosition = new Vector3(0f, 0f, 25f);
        private float _length = 8f;
        private List<GameObject> prevStreet;
        public GameObject prefab;

        private void Start()
        {
            _positions = new List<Vector3>();
        }

        public void VisualizeSequence(int angle, int iterationAmount)
        {
            if (prevStreet.Count > 0)
            {
                foreach (var go in prevStreet)
                {
                    Destroy(go);
                }
            }
            prevStreet.Clear();
            _positions.Clear();
            var currentPosition = _startPosition;
            var direction = Vector3.forward;
            _positions.Add(currentPosition);

            for (int i = 0; i < iterationAmount; i++)
            {
                //Draw
                currentPosition += direction * _length;
                var roadTile = Instantiate(prefab, currentPosition, Quaternion.Euler(0f, angle * _positions.Count, 0f));
                prevStreet.Add(roadTile);
                _positions.Add(currentPosition);
                
                //Turn
                direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
            }

            var lastPart = prevStreet.Last();
            _finish.transform.position = lastPart.transform.position + direction * (_length / 2);
            _finish.transform.rotation = lastPart.transform.rotation;
        }
    }
}