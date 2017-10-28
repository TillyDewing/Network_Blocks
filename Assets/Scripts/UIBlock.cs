using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class UIBlock : MonoBehaviour
{
    MeshFilter filter;

    void Start()
    {
        filter = gameObject.GetComponent<MeshFilter>();
        RenderMesh(new BlockGrass());
    }

    void RenderMesh(Block block)
    {
        MeshData meshData = new MeshData();
        meshData = block.BlockData(meshData);
        filter.mesh.Clear();
        filter.mesh.vertices = meshData.vertices.ToArray();
        filter.mesh.triangles = meshData.triangles.ToArray();

        filter.mesh.uv = meshData.uv.ToArray();
        filter.mesh.RecalculateNormals();
    }
}
