using UnityEngine;
using UnityEditor;

public class TerrainToMeshOnly : MonoBehaviour
{
    [MenuItem("Tools/Terrain/Convert Selected Terrain to Mesh (Clean)")]
    static void ConvertTerrainToMesh()
    {
        Terrain terrain = Selection.activeGameObject?.GetComponent<Terrain>();
        if (terrain == null)
        {
            Debug.LogError("No Terrain selected!");
            return;
        }

        TerrainData data = terrain.terrainData;
        Vector3 size = data.size;
        int resolution = data.heightmapResolution;

        Vector3[] vertices = new Vector3[resolution * resolution];
        Vector2[] uvs = new Vector2[vertices.Length];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float height = data.GetHeight(x, y);
                int i = y * resolution + x;

                float xPos = ((float)x / (resolution - 1)) * size.x;
                float zPos = ((float)y / (resolution - 1)) * size.z;

                vertices[i] = new Vector3(xPos, height, zPos);
                uvs[i] = new Vector2((float)x / (resolution - 1), (float)y / (resolution - 1));
            }
        }

        int triIndex = 0;
        for (int y = 0; y < resolution - 1; y++)
        {
            for (int x = 0; x < resolution - 1; x++)
            {
                int i = y * resolution + x;

                triangles[triIndex++] = i;
                triangles[triIndex++] = i + resolution;
                triangles[triIndex++] = i + resolution + 1;

                triangles[triIndex++] = i;
                triangles[triIndex++] = i + resolution + 1;
                triangles[triIndex++] = i + 1;
            }
        }

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; // stöd för stora mesh
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GameObject meshGO = new GameObject("TerrainMesh");
        meshGO.transform.position = terrain.transform.position;

        MeshFilter mf = meshGO.AddComponent<MeshFilter>();
        mf.sharedMesh = mesh;

        MeshRenderer mr = meshGO.AddComponent<MeshRenderer>();
        mr.sharedMaterial = new Material(Shader.Find("Standard")); // tomt material, bara för att något ska synas

        Debug.Log("Terrain converted to mesh.");
    }
}
