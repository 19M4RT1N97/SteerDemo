using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SplineMesh : MonoBehaviour {

    public SplineDone spline;
    [SerializeField] private float meshWidth = 10f;

    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    private void Awake() {
        if (spline == null) spline = GetComponent<SplineDone>();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        transform.position = Vector3.zero;
    }

    private void Start() {
        transform.position = spline.transform.position;

        UpdateMesh();

        spline.OnDirty += Spline_OnDirty;
    }

    private void Spline_OnDirty(object sender, EventArgs e) {
        UpdateMesh();
    }

    public void UpdateMesh() {
        if (mesh != null) {
            mesh.Clear();
            Destroy(mesh);
            mesh = null;
        }

        spline.SetupPointList();
        spline.UpdateForwardVectors();
        List<SplineDone.Point> pointList = spline.GetPointList();
        if (pointList.Count > 2) {
            SplineDone.Point point = pointList[0];
            SplineDone.Point secondPoint = pointList[1];
            mesh = MeshUtils.CreateLineMesh(point.position - transform.position, secondPoint.position - transform.position, point.normal, meshWidth);

            for (int i = 2; i < pointList.Count; i++) {
                SplineDone.Point thisPoint = pointList[i];
                MeshUtils.AddLinePoint(mesh, thisPoint.position - transform.position, thisPoint.forward, point.normal, meshWidth);
            }
            
            meshFilter.mesh = mesh;
            meshFilter.gameObject.layer = 3; //road layer
            meshCollider.sharedMesh = mesh;
            meshCollider.gameObject.layer = 3; //road layer
        }
    }

}
