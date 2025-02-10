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

    private Color distanceColor = new Color(0f, 0f, 0f, 1f); 

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

    public Vector2 ProjectOntoSubspace(Vector4 P) 
    {
        return new Vector2(P.x, P.y);
    }

    public float DistanceFromViewport(Vector4 point) 
    {
        return (new Vector2(point.z, point.w) + Vector2.one).magnitude;
    }

    private void Start() 
    {
        Refresh();
    }

    private void RenderVertices(Vector4[] vertices, int[,] edges, Color color) 
    {
        Vector4[] rotatedVertices = vertices
            .AsEnumerable()
            .Select(vertex => vertex.Rotate(Rotation.PlaneOfRototation.XY, xySlider.value))
            .Select(vertex => vertex.Rotate(Rotation.PlaneOfRototation.XZ, xzSlider.value))
            .Select(vertex => vertex.Rotate(Rotation.PlaneOfRototation.XW, xwSlider.value))
            .Select(vertex => vertex.Rotate(Rotation.PlaneOfRototation.YZ, yzSlider.value))
            .Select(vertex => vertex.Rotate(Rotation.PlaneOfRototation.YW, ywSlider.value))
            .Select(vertex => vertex.Rotate(Rotation.PlaneOfRototation.ZW, zwSlider.value))
            .ToArray();

        Vector2[] projectedVertices = rotatedVertices
            .AsEnumerable()
            .Select(vertex => ProjectOntoSubspace(vertex))
            .ToArray();

        for (int i = 0; i < projectedVertices.Length; i++)
        {
            float distance = DistanceFromViewport(rotatedVertices[i]);
            InstantiateVertex(projectedVertices[i], GetVertexColor(color, distance), distance);
        }
        
        for (int i = 0; i < edges.GetLength(0); i++)
        {
            float distanceA = DistanceFromViewport(rotatedVertices[edges[i, 0]]);
            float distanceB = DistanceFromViewport(rotatedVertices[edges[i, 1]]);
            
            InstantiateEdge(projectedVertices[edges[i, 0]], projectedVertices[edges[i, 1]], 
                GetVertexColor(color, distanceA), GetVertexColor(color, distanceB), (distanceA + distanceB) / 2f);
        }
    }
    private Color GetVertexColor(Color color, float distance) 
    {
        return Color.Lerp(color, distanceColor, 1 - 1f / (distance + 1));
    }

    public void Refresh() 
    {
        foreach (Transform child in transform) 
        {
            Destroy(child.gameObject);
        }

        RenderVertices(vertices, edges, Color.white);
        RenderVertices(new Vector4[] { Vector4.zero, new Vector4(1, 0, 0, 0) }, new int[,] { {0, 1} }, Color.red);
        RenderVertices(new Vector4[] { Vector4.zero, new Vector4(0, 1, 0, 0) }, new int[,] { {0, 1} }, Color.green);
        RenderVertices(new Vector4[] { Vector4.zero, new Vector4(0, 0, 1, 0) }, new int[,] { {0, 1} }, Color.blue);
        RenderVertices(new Vector4[] { Vector4.zero, new Vector4(0, 0, 0, 1) }, new int[,] { {0, 1} }, Color.yellow);

        RenderVertices(new Vector4[] { Vector4.one * 0.5f }, new int[,] { } , Color.cyan);

        InstantiateVertex(Vector2.zero, Color.black, DistanceFromViewport(Vector4.zero), 1);
    }

    private void InstantiateVertex(Vector2 vertex, Color color, float distance, int orderIncrease = 0) 
    {
        GameObject point = Instantiate(pointPrefab, (Vector3)vertex, Quaternion.identity);
        point.transform.SetParent(transform);

        SpriteRenderer spriteRenderer = point.GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;

        spriteRenderer.sortingOrder = (int)(-distance * 1000) + orderIncrease;
    }

    private void InstantiateEdge(Vector2 a, Vector2 b, Color fromColor, Color toColor, float distance) 
    {
        GameObject line = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        line.transform.SetParent(transform);

        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        Vector3[] lines = new Vector3[] { (Vector3)a, (Vector3)b };

        lineRenderer.SetPosition(0, lines[0]);
        lineRenderer.SetPosition(1, lines[1]);

        Gradient gradient = new();
        GradientColorKey[] colors = new[] { new GradientColorKey(fromColor, 0f), new GradientColorKey(toColor, 1f) };
        GradientAlphaKey[] alphas = new[] { new GradientAlphaKey(1f, 0f) };
        gradient.SetKeys(colors, alphas);
        lineRenderer.colorGradient = gradient;

        lineRenderer.sortingOrder = (int)(-distance * 1000);
    }
}