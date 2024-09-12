using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralSphere : MonoBehaviour
{
    public int latitudeSegments = 24;
    public int longitudeSegments = 24;
    public float radius = 1f;

    public void GenerateSphere()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mesh.name = "Procedural Sphere";

        // Set the radius of sphere collider to match the generated sphere
        GetComponent<SphereCollider>().radius = radius;

        Vector3[] vertices = new Vector3[(latitudeSegments + 1) * (longitudeSegments + 1)];
        Vector3[] normals = new Vector3[vertices.Length];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[latitudeSegments * longitudeSegments * 6];

        float pi = Mathf.PI;
        float twoPi = pi * 2f;

        for (int lat = 0; lat <= latitudeSegments; lat++)
        {
            float latAngle = pi * lat / latitudeSegments;
            float y = Mathf.Cos(latAngle);
            float sinLat = Mathf.Sin(latAngle);

            for (int lon = 0; lon <= longitudeSegments; lon++)
            {
                float lonAngle = twoPi * lon / longitudeSegments;
                float x = sinLat * Mathf.Cos(lonAngle);
                float z = sinLat * Mathf.Sin(lonAngle);

                int index = lat * (longitudeSegments + 1) + lon;

                vertices[index] = new Vector3(x, y, z) * radius;
                normals[index] = vertices[index].normalized;
                uv[index] = new Vector2((float)lon / longitudeSegments, (float)lat / latitudeSegments);
            }
        }

        int triangleIndex = 0;
        for (int lat = 0; lat < latitudeSegments; lat++)
        {
            for (int lon = 0; lon < longitudeSegments; lon++)
            {
                int current = lat * (longitudeSegments + 1) + lon;
                int next = current + longitudeSegments + 1;

                triangles[triangleIndex++] = current;
                triangles[triangleIndex++] = current + 1;
                triangles[triangleIndex++] = next;

                triangles[triangleIndex++] = next;
                triangles[triangleIndex++] = current + 1;
                triangles[triangleIndex++] = next + 1;

            }
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        meshFilter.mesh = mesh;
    }
}

[CustomEditor(typeof(ProceduralSphere))]
public class ProceduralSphereEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ProceduralSphere myScript = (ProceduralSphere)target;
        if (GUILayout.Button("Generate Sphere"))
        {
            myScript.GenerateSphere();
        }
    }
}
