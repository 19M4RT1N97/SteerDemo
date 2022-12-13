using System;
using System.Collections.Generic;
using System.Linq;
using Rules;
using UnityEngine;

namespace DefaultNamespace
{
    public class SimpleVisualizer : MonoBehaviour
    {
        public StreetGen LSystem;
        private List<Vector3> positions;
        public GameObject prefab;
        public Material lineMaterial;

        private int length = 8;
        public int Length
        {
            get => length > 0 ? length : 0;
            set => length = value;
        }
        private float angle = 90;
        
        private void Start()
        {
            positions = new List<Vector3>();
            var sequence = LSystem.GenerateSentence();
            VisualizeSequence(sequence);
        }

        private void VisualizeSequence(string sequence)
        {
            Stack<AgentParameters> savePoints = new Stack<AgentParameters>();
            var currentPosition = Vector3.zero;
            var direction = Vector3.forward;
            var tempPosition = Vector3.zero;
            positions.Add(currentPosition);

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
                            length = Length
                        });
                        break;
                    case EncodingLetters.load:
                        if (savePoints.Count>0)
                        {
                            var agentParameter = savePoints.Pop();
                            currentPosition = agentParameter.position;
                            direction = agentParameter.direction;
                            Length = agentParameter.length;
                        }
                        else
                        {
                            throw new System.Exception("no savepoints");
                        }
                        break;
                    case EncodingLetters.draw:
                        tempPosition = currentPosition;
                        currentPosition += direction * Length;
                        DrawLine(tempPosition, currentPosition, Color.red);
                        Length -= 2;
                        positions.Add(currentPosition);
                        break;
                    case EncodingLetters.turnRight:
                        direction = Quaternion.AngleAxis(angle, Vector3.up)*direction;
                        break;
                    case EncodingLetters.turnLeft:
                        direction = Quaternion.AngleAxis(-angle, Vector3.up)*direction;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            foreach (var position in positions)
            {
                Instantiate(prefab, position, Quaternion.identity);
            }
        }

        private void DrawLine(Vector3 start, Vector3 end, Color c)
        {
            GameObject line = new GameObject("Line");
            line.transform.position = start;
            var lineRenderer = line.AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            lineRenderer.startColor = c;
            lineRenderer.endColor = c;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
        }
    }
}