using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MyLowPolyWater : MonoBehaviour {

    void Start() {

        //MakeMeshLowPoly();
    }

    /// Rearranges the mesh vertices to create a 'low poly' effect
    /// For each vertex, uv1 and uv2 will contain the position of the two other vertices in its triangle.
    [ContextMenu("Make mesh low poly")]
    private void MakeMeshLowPoly() {
        
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

        Vector3[] originalVertices = mesh.vertices;

        int[] triangles = mesh.triangles;
        Vector3[] vertices = new Vector3[triangles.Length];
        List<Vector3> uv1 = Enumerable.Repeat(Vector3.zero, vertices.Length).ToList();
        List<Vector3> uv2 = Enumerable.Repeat(Vector3.zero, vertices.Length).ToList();

        for (int i = 0; i + 2 < triangles.Length; i += 3) {
            
            vertices[i + 0] = originalVertices[triangles[i + 0]];
            vertices[i + 1] = originalVertices[triangles[i + 1]];
            vertices[i + 2] = originalVertices[triangles[i + 2]];

            uv1[i + 0] = vertices[i + 1];
            uv2[i + 0] = vertices[i + 2];

            uv1[i + 1] = vertices[i + 2];
            uv2[i + 1] = vertices[i + 0];

            uv1[i + 2] = vertices[i + 0];
            uv2[i + 2] = vertices[i + 1];

            triangles[i + 0] = i + 0;
            triangles[i + 1] = i + 1;
            triangles[i + 2] = i + 2;
        }

        //Update the gameobject's mesh with new vertices
        mesh.vertices = vertices;
        mesh.SetTriangles(triangles, 0);
        mesh.SetUVs(1, uv1);
        mesh.SetUVs(2, uv2);

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}
