using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quad : MonoBehaviour
{
    void Start()
    {
        // 新しいMeshを作成
        // test
        Mesh mesh = new Mesh();

        // 頂点座標を設定
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(-0.5f, -0.5f, 0f),
            new Vector3(0.5f, -0.5f, 0f), 
            new Vector3(-0.5f, 0.5f, 0f),
            new Vector3(0.5f, 0.5f, 0f)
        };
        mesh.vertices = vertices;

        // 頂点インデックスを設定
        int[] tris = new int[6] 
        {
            0, 2, 1,
            1, 2, 3
        };
        mesh.triangles = tris;

        // これ以降の理解は甘くても一旦問題なさそう

        // UVを設定
        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;

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
}
