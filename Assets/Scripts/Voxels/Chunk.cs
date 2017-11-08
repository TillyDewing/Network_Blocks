//William Dewing 2017
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Chunk : MonoBehaviour
{
    public Block[,,] blocks = new Block[chunkSize, chunkSize, chunkSize];
    public Dictionary<WorldPos, Block> modifiedBlocks = new Dictionary<WorldPos, Block>();
    public static int chunkSize = 16;
    public bool update = false;
    public bool rendered;
    public World world;
    public WorldPos pos;
    public byte numClientsLoaded = 0;
    MeshFilter filter;
    MeshCollider coll;


    // Use this for initialization
    void Start()
    {
        filter = gameObject.GetComponent<MeshFilter>();
        coll = gameObject.GetComponent<MeshCollider>();
    }

    //Update is called once per frame
    void Update()
    {
        if (update)
        {
            update = false;
            UpdateChunk();
        }
    }
    //Gets block at a given possition within the chunk
    public Block GetBlock(int x, int y, int z)
    {
        if (InRange(x) && InRange(y) && InRange(z))
        {
            return blocks[x, y, z];
        }
        return world.GetBlock(pos.x + x, pos.y + y, pos.z + z);
    }

    public static bool InRange(int index)
    {
        if (index < 0 || index >= chunkSize)
            return false;
        return true;
    }
    //Sets block at a given possition within the chunk
    public void SetBlock(int x, int y, int z, Block block, bool natural = false)
    {
        if (InRange(x) && InRange(y) && InRange(z))
        {
            blocks[x, y, z] = block;
            blocks[x, y, z].changed = true;
            //Keeps modified blocks in a seperate list so they can be sent to the client
            if (!natural)
            {
                if (modifiedBlocks.ContainsKey(new WorldPos(x, y, z)))
                {
                    modifiedBlocks[new WorldPos(x, y, z)] = block;
                }
                else
                {
                    modifiedBlocks.Add(new WorldPos(x, y, z), block);
                }
            }
        }
        else
        {
            world.SetBlock(pos.x + x, pos.y + y, pos.z + z, block);
        }
    }

    // Updates the chunk based on its contents
    void UpdateChunk()
    {
        rendered = true;

        MeshData meshData = new MeshData();
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    meshData = blocks[x, y, z].Blockdata(this, x, y, z, meshData);
                }
            }
        }

        RenderMesh(meshData);
    }

    // Sends the calculated mesh information to the mesh and collision components
    void RenderMesh(MeshData meshData)
    {
        filter.mesh.Clear();
        filter.mesh.vertices = meshData.vertices.ToArray();
        filter.mesh.triangles = meshData.triangles.ToArray();

        filter.mesh.uv = meshData.uv.ToArray();
        filter.mesh.RecalculateNormals();

        coll.sharedMesh = null;
        Mesh mesh = new Mesh();
        mesh.vertices = meshData.colVertices.ToArray();
        mesh.triangles = meshData.colTriangles.ToArray();
        mesh.RecalculateNormals();

        coll.sharedMesh = mesh;
    }

    public void SetBlocksUnmodified()
    {
        foreach (Block block in blocks)
        {
            block.changed = false;
        }

        modifiedBlocks = new Dictionary<WorldPos, Block>();
    }
}