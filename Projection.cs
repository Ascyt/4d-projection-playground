using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Projection : MonoBehaviour 
{
    [SerializeField]
    private GameObject pointPrefab;
    [SerializeField]
    private GameObject linePrefab;

    // Sliders for 4D planes of rotation
    [SerializeField]
    private Slider xySlider;
    [SerializeField]
    private Slider xzSlider;
    [SerializeField]
    private Slider xwSlider;
    [SerializeField]
    private Slider yzSlider;
    [SerializeField]
    private Slider ywSlider;
    [SerializeField]
    private Slider zwSlider;

    // 4D hypercube vertices
    Vector4[] vertices = new Vector4[] 
    { 
        new Vector4(-1, -1, -1, -1), // 0
        new Vector4(1, -1, -1, -1), // 1
        new Vector4(1, 1, -1, -1), // 2
        new Vector4(-1, 1, -1, -1), // 3
        new Vector4(-1, 1, 1, -1), // 4
        new Vector4(1, 1, 1, -1), // 5
        new Vector4(1, -1, 1, -1), // 6
        new Vector4(-1, -1, 1, -1),  // 7
        
        new Vector4(-1, -1, -1, 1), // 8
        new Vector4(1, -1, -1, 1), // 9
        new Vector4(1, 1, -1, 1), // 10
        new Vector4(-1, 1, -1, 1), // 11
        new Vector4(-1, 1, 1, 1), // 12
        new Vector4(1, 1, 1, 1), // 13
        new Vector4(1, -1, 1, 1), // 14
        new Vector4(-1, -1, 1, 1)  // 15
    };
    // Edges
    int[,] edges = new int[,] 
    {
        { 0, 1 }, { 1, 2 }, { 2, 3 }, { 3, 0 }, // First square
        { 4, 5 }, { 5, 6 }, { 6, 7 }, { 7, 4 }, // Second square
        { 0, 7 }, { 1, 6 }, { 2, 5 }, { 3, 4 }, // Connecting first and second square to form a cube

        { 8, 9 }, { 9, 10 }, { 10, 11 }, { 11, 8 }, // First square
        { 12, 13 }, { 13, 14 }, { 14, 15 }, { 15, 12 }, // Second Square
        { 8, 15 }, { 9, 14 }, { 10, 13 }, { 11, 12 }, // Connecting first and second square to form a cube

        { 0, 8 }, { 1, 9 }, { 2, 10 }, { 3, 11 }, // Connecting the two cubes
        { 4, 12 }, { 5, 13 }, { 6, 14 }, { 7, 15 } // Connecting the two cubes
    };

    public Vector2 ProjectOntoSubspace(Vector4 P, Vector4 v, Vector4 w) 
    {
        float Pv = Vector4.Dot(P, v);
        float Pw = Vector4.Dot(P, w);

        float v2 = Vector4.Dot(v, v);
        float w2 = Vector4.Dot(w, w);

        return new Vector2(Pv / v2, Pw / w2);
    }

    private void Start() 
    {
        //Refresh();
    }

    public void Refresh() 
    {
        Vector4 subspaceV = new Vector4(1, 0, 0, 0);
        Vector4 subspaceW = new Vector4(0, 1, 0, 0);

        Vector4[] rotatedVertices = vertices
            .AsEnumerable()
            .Select(vertex => vertex.Rotate(Rotation.PlaneOfRototation.XY, xySlider.value))
            .Select(vertex => vertex.Rotate(Rotation.PlaneOfRototation.XZ, xzSlider.value))
            .Select(vertex => vertex.Rotate(Rotation.PlaneOfRototation.XW, xwSlider.value))
            .Select(vertex => vertex.Rotate(Rotation.PlaneOfRototation.YZ, yzSlider.value))
            .Select(vertex => vertex.Rotate(Rotation.PlaneOfRototation.YW, ywSlider.value))
            .Select(vertex => vertex.Rotate(Rotation.PlaneOfRototation.ZW, zwSlider.value))
            .ToArray();

        foreach (Transform child in transform) 
        {
            Destroy(child.gameObject);
        }

        Vector2[] projectedVertices = rotatedVertices
            .AsEnumerable()
            .Select(vertex => ProjectOntoSubspace(vertex, subspaceV, subspaceW))
            .ToArray();

        for (int i = 0; i < projectedVertices.Length; i++)
        {
            Vector2 projectedVertex = projectedVertices[i];

            GameObject point = Instantiate(pointPrefab, (Vector3)projectedVertex, Quaternion.identity);

            point.transform.SetParent(transform);
        }
        
        for (int i = 0; i < edges.GetLength(0); i++)
        {
            InstantiateEdge(projectedVertices[edges[i, 0]], projectedVertices[edges[i, 1]]);
        }

        //InstantiateEdge(projectedVertices[projectedVertices.Length - 1], projectedVertices[0]);
    }

    private void InstantiateEdge(Vector2 a, Vector2 b) 
    {
        GameObject line = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        Vector3[] lines = new Vector3[] { (Vector3)a, (Vector3)b };

        lineRenderer.SetPosition(0, lines[0]);
        lineRenderer.SetPosition(1, lines[1]);
        line.transform.SetParent(transform);
    }
}