using System.Collections.Generic;
using UnityEngine;

public class Cylinder : MonoBehaviour
{
    // 高さ
    private const float Height = 1f;
    // 底面の頂点数
    private const int Segments = 8;
    // 半径
    private const float Radius = 1f;

    private void Start()
    {
        Mesh mesh = new Mesh();
        
        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();
        var triangles = new List<int>();
        
        // top の高さと bottom の高さを定義
        float top = Height * 0.5f, bottom = -Height * 0.5f;
        
        // 側面の頂点情報を計算
        GenerateCap(Segments, top, bottom, Radius, vertices, uvs, normals, true);
        
        var len = (Segments + 1) * 2;
        for (int i = 0; i < Segments + 1; i++)
        {
            int idx = i * 2;
            int a = idx, b = idx + 1, c = (idx + 2) % len, d = (idx + 3) % len;
            triangles.Add(a);
            triangles.Add(c);
            triangles.Add(b);
            triangles.Add(d);
            triangles.Add(b);
            triangles.Add(c);
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        // 法線を計算
        mesh.RecalculateNormals();

        // MeshFilterを追加
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        // マテリアルを取得
        Material material = Resources.Load<Material>("Grad");

        // MeshRendererを追加
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
    }

    private void GenerateCap(
        int segments,
        float top,
        float bottom,
        float radius,
        List<Vector3> vertices,
        List<Vector2> uvs,
        List<Vector3> normals,
        bool side
    ) {
        for (int i = 0; i < segments + 1; i++) {
            // 角度（比率）
            float ratio = (float)i / segments;
            // 角度（ラジアン）
            float rad = ratio * Mathf.PI * 2;
            // cos, sin の値 [0-1]
            float cos = Mathf.Cos(rad), sin = Mathf.Sin(rad);
            // cos, sin を半径で乗算することで x, z 座標の値を計算
            float x = cos * radius, z = sin * radius;
            // x, z 座標に対応する top, bottom の2頂点の座標
            Vector3 tp = new Vector3(x, top, z), bp = new Vector3(x, bottom, z);
            
            vertices.Add(tp);
            uvs.Add(new Vector2(ratio, 1f));
            vertices.Add(bp);
            uvs.Add(new Vector2(ratio, 0f));
            
            if(side) {  // 側面なら
                var normal = new Vector3(cos, 0f, sin);
                // 2つの三角形に対応する法線ベクトルを追加
                normals.Add(normal);
                normals.Add(normal);
            } else {  // 上蓋 or 下蓋なら
                // 上蓋に対応する上向きのベクトルを追加
                normals.Add(new Vector3(0f, 1f, 0f));
                // 下蓋に対応する下向きのベクトルを追加
                normals.Add(new Vector3(0f, -1f, 0f));
            }
        }
    }
}