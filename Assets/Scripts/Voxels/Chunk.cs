using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Chunk : MonoBehaviour
{
    public static int chunkSize = 16;
    public bool update = true;

    private MeshFilter filter;
    private MeshCollider coll;
    private Block[,,] blocks;

    private void Start()
    {
        filter = gameObject.GetComponent<MeshFilter>();
        coll = gameObject.GetComponent<MeshCollider>();
        //past here is just to set up an e
        blocks = new Block[chunkSize, chunkSize, chunkSize];
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    blocks[x, y, z] = new BlockAir();
                }
            }
        }
        blocks[3, 5, 2] = new Block();
        blocks[8, 6, 3] = new Block();

        UpdateChunk();
    }

    void Update()
    {

    }

    public Block GetBlock(int x, int y, int z)
    {
        return blocks[x, y, z];
    }
    //Updates contents of chunk
    void UpdateChunk()
    {
        MeshData meshdata = new MeshData();
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    meshdata = blocks[x, y, z].BlockData(this, x, y, z, meshdata);
                }
            }
        }
        RenderMesh(meshdata);
    }
    //Sends the calculated mesh info to mesh renderer and collider
    void RenderMesh(MeshData meshData)
    {
        filter.mesh.Clear();
        filter.mesh.vertices = meshData.vertices.ToArray();
        filter.mesh.triangles = meshData.triangles.ToArray();
    }
}
