using System;
using System.Collections.Generic;
using UnityEngine;

public class SplineDone : MonoBehaviour {
    public event EventHandler OnDirty;
    
    [SerializeField] private Vector3 normal = new Vector3(0, 1, 0);
    [SerializeField] private bool closedLoop;
    [SerializeField] public List<Anchor> anchorList;

    private float moveDistance;
    private float pointAmountInCurve;
    private float pointAmountPerUnitInCurve = 2f;


    private List<Point> pointList;
    private float splineLength;

    private void Awake() {
        // splineLength = GetSplineLength();
        // SetupPointList();
    }

    private void Start() {
        //PrintPath();
    }

    private Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t) {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, t);
    }

    private Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t) {
        Vector3 abc = QuadraticLerp(a, b, c, t);
        Vector3 bcd = QuadraticLerp(b, c, d, t);

        return Vector3.Lerp(abc, bcd, t);
    }

    public Vector3 GetPositionAt(float t) {
        if (t == 1) {
            // Full position, special case
            Anchor anchorA, anchorB;
            if (closedLoop) {
                anchorA = anchorList[anchorList.Count - 1];
                anchorB = anchorList[0];
            } else {
                anchorA = anchorList[anchorList.Count - 2];
                anchorB = anchorList[anchorList.Count - 1];
            }
            return transform.position + CubicLerp(anchorA.position, anchorA.handleBPosition, anchorB.handleAPosition, anchorB.position, t);
        } else {
            int addClosedLoop = (closedLoop ? 1 : 0);
            float tFull = t * (anchorList.Count - 1 + addClosedLoop);
            int anchorIndex = Mathf.FloorToInt(tFull);
            float tAnchor = tFull - anchorIndex;

            Anchor anchorA, anchorB;

            if (anchorIndex < anchorList.Count - 1) {
                anchorA = anchorList[anchorIndex + 0];
                anchorB = anchorList[anchorIndex + 1];
            } else {
                // anchorIndex is final one, either don't link to "next" one or loop back
                if (closedLoop) {
                    anchorA = anchorList[anchorList.Count - 1];
                    anchorB = anchorList[0];
                } else {
                    anchorA = anchorList[anchorIndex - 1];
                    anchorB = anchorList[anchorIndex + 0];
                    tAnchor = 1f;
                }
            }

            return transform.position + CubicLerp(anchorA.position, anchorA.handleBPosition, anchorB.handleAPosition, anchorB.position, tAnchor);
        }
    }

    public List<Point> SetupPointList() {
        pointList = new List<Point>();
        pointAmountInCurve = pointAmountPerUnitInCurve * GetSplineLength();
        for (float t = 0; t < 1f; t += 1f / pointAmountInCurve) {
            pointList.Add(new Point {
                t = t,
                position = GetPositionAt(t),
                normal = normal,
            });
        }

        pointList.Add(new Point {
            t = 1f,
            position = GetPositionAt(1f),
        });

        UpdateForwardVectors();

        return pointList;
    }

    public void UpdateForwardVectors() {
        // Set forward vectors
        for (int i = 0; i < pointList.Count - 1; i++) {
            pointList[i].forward = (pointList[i + 1].position - pointList[i].position).normalized;
        }
        // Set final forward vector
        if (closedLoop) {
            pointList[pointList.Count - 1].forward = pointList[0].forward;
        } else {
            pointList[pointList.Count - 1].forward = pointList[pointList.Count - 2].forward;
        }
    }

    // private void PrintPath() {
    //     foreach (Point point in pointList) {
    //         Transform dotTransform = Instantiate(dots, point.position, Quaternion.identity);
    //         FunctionUpdater.Create(() => {
    //             dotTransform.position = point.position;
    //         });
    //     }
    // }

    public float GetSplineLength(float stepSize = .01f) {
        float splineLength = 0f;

        Vector3 lastPosition = GetPositionAt(0f);

        for (float t = 0; t < 1f; t += stepSize) {
            splineLength += Vector3.Distance(lastPosition, GetPositionAt(t));

            lastPosition = GetPositionAt(t);
        }

        splineLength += Vector3.Distance(lastPosition, GetPositionAt(1f));

        return splineLength;
    }

    public void AddAnchor(Vector3 newPos, Vector3 direction) {
        if (anchorList == null) anchorList = new List<Anchor>();
        
        anchorList.Add(new Anchor {
                position = newPos,
                handleAPosition = newPos+direction*1.3f,
                handleBPosition = newPos-direction*1.3f
            });
    }


    public List<Point> GetPointList() {
        return pointList;
    }

    public void ClearAnchors()
    {
        if (anchorList != null)
        {
            anchorList.Clear();
        }
    }


    [Serializable]
    public class Point {
        public float t;
        public Vector3 position;
        public Vector3 forward;
        public Vector3 normal;
    }

    [Serializable]
    public class Anchor {
        public Vector3 position;
        public Vector3 handleAPosition;
        public Vector3 handleBPosition;
    }
}
