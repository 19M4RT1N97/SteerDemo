using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Rules;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = System.Random;

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

        public void VisualizeSequence(int angle, string sequence)
        {
            if (prevStreet.Count > 0)
            {
                foreach (var go in prevStreet)
                {
                    Destroy(go);
                }
            }
            _positions.Clear();

            Stack<AgentParameters> savePoints = new Stack<AgentParameters>();
            var currentPosition = _startPosition;
            var direction = Vector3.forward;
            _positions.Add(currentPosition);

            foreach (var letter in sequence)
            {
                switch ((EncodingLetters)letter)
                {
                    case EncodingLetters.unknown:
                        break;
                    case EncodingLetters.save:
                        savePoints.Push(new AgentParameters
                        {
                            position = currentPosition,
                            direction = direction, 
                            length = _length
                        });
                        break;
                    case EncodingLetters.load:
                        if (savePoints.Count>0)
                        {
                            var agentParameter = savePoints.Pop();
                            currentPosition = agentParameter.position;
                            direction = agentParameter.direction;
                            _length = agentParameter.length;
                        }
                        else
                        {
                            throw new System.Exception("no savepoints");
                        }
                        break;
                    case EncodingLetters.draw:
                        currentPosition += direction * _length;
                        var roadTile = Instantiate(prefab, currentPosition, Quaternion.Euler(0f,angle*_positions.Count, 0f));
                        prevStreet.Add(roadTile);
                        _positions.Add(currentPosition);
                        break;
                    case EncodingLetters.turnRight:
                        direction = Quaternion.AngleAxis(angle, Vector3.up)*direction;
                        break;
                    case EncodingLetters.turnLeft:
                        direction = Quaternion.AngleAxis(angle, Vector3.up)*direction;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var lastPart = prevStreet.Last();
            _finish.transform.position = lastPart.transform.position;
            _finish.transform.rotation = lastPart.transform.rotation;
        }
    }
}