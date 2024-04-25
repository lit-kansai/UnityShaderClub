using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPlane : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();
        int widthSegments = 5;
        int heightSegments = 6;
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();

        float winv = 1f / (widthSegments - 1);
        float hinv = 1f / (heightSegments - 1);

        for(int y = 0; y < heightSegments; y++) {
            float ry = y * hinv;

            for(int x = 0; x < widthSegments; x++) {
                var rx = x * winv;

                vertices.Add(new Vector3(
                    (rx - 0.5f) * widthSegments,
                    Depth(rx,ry),
                    (0.5f - ry) * heightSegments
                ));
                uv.Add(new Vector2(rx, ry));
                normals.Add(new Vector3(0f, 1f, 0f));
            }
        }

        List<int> triangles = new List<int>();
        for(int y = 0; y < heightSegments - 1; y++) {
            for(int x = 0; x< widthSegments - 1; x++) {
                int index = y * widthSegments + x;
                int a = index;
                int b = index + 1;
                int c = index + 1 + widthSegments;
                int d = index + widthSegments;

                triangles.Add(a);
                triangles.Add(b);
                triangles.Add(c);

                triangles.Add(c);
                triangles.Add(d);
                triangles.Add(a);
            }
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();

        // 法線を計算
        mesh.RecalculateNormals();

        // MeshFilterを追加
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        // MeshRendererを追加
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

    }

    float Depth(float u, float v) {
        float uScale = 1.2f;
        float vScale = 1.5f;
        float uOffset = 0f;
        float vOffset = 0f;
        return Mathf.PerlinNoise(u * uScale + uOffset, v * vScale + vOffset) * 3;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
