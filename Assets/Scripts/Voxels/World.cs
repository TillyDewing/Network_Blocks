//William Dewing 2017
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Handles chunks in world space allows you to edit blocks at world points rather than at points within a chunk
public class World : MonoBehaviour
{
    public string worldName = "World";
    public static int seed = 0;
    public static Vector3 spawnPos = new Vector3(0, 50, 0);
    //List of all loaded chunks
    public Dictionary<WorldPos, Chunk> chunks = new Dictionary<WorldPos, Chunk>();

    public GameObject chunkPrefab;
    public LoadChunks player;

    public static World singleton;
    public bool isClient = false;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //seed = Random.Range(0, 10000);
        if(!isClient)
        {
            LoadWorld(worldName);
        }
    }

    //Creates a new chunk at given position
    public Chunk CreateChunk(int x, int y, int z)
    {
        WorldPos worldPos = new WorldPos(x, y, z);

        //Instantiate the chunk at the cordinates using chunk prefab
        GameObject newChunkObject = Instantiate(chunkPrefab, new Vector3(x, y, z), Quaternion.Euler(Vector3.zero)) as GameObject;
        Chunk newChunk = newChunkObject.GetComponent<Chunk>();
        newChunk.pos = worldPos;
        newChunk.world = this;
        //Adds it to chunk dictonary
        chunks.Add(worldPos, newChunk);

        //Terrain Generation
        TerrainGen terrainGen = new TerrainGen();
        newChunk = terrainGen.ChunkGen(newChunk);

        //Sets the generated blocks to unmodified and tries to load any modified blocks from the save file
        if (isClient)
        {
            NetworkBlocksClient.RequestChuckData(newChunk.pos);
        }
        else
        {
            newChunk.SetBlocksUnmodified();
            Serialization.Load(newChunk);
        }

        //newChunk.SetBlocksUnmodified();
        //Serialization.Load(newChunk);

        return newChunk;
    }
    
    public void DestroyChunk(int x, int y, int z) //Unloads chunk
    {
        Chunk chunk = null;
        if (chunks.TryGetValue(new WorldPos(x, y, z), out chunk))
        {
            if (!isClient)
            {
                Serialization.SaveChunk(chunk); //Saves chunk to file before unloading
            }
            else
            {
                NetworkBlocksClient.singleton.UnloadChunk(new WorldPos(x, y, z));
            }
            Object.Destroy(chunk.gameObject);
            chunks.Remove(new WorldPos(x, y, z));
        }
    }

    public Chunk GetChunk(int x, int y, int z)
    {
        WorldPos pos = new WorldPos();
        float multiple = Chunk.chunkSize;
        pos.x = Mathf.FloorToInt(x / multiple) * Chunk.chunkSize;
        pos.y = Mathf.FloorToInt(y / multiple) * Chunk.chunkSize;
        pos.z = Mathf.FloorToInt(z / multiple) * Chunk.chunkSize;
        Chunk containerChunk = null;
        chunks.TryGetValue(pos, out containerChunk);

        return containerChunk;
    }

    //Gets the block at a given world position
    public Block GetBlock(int x, int y, int z)
    {
        Chunk containerChunk = GetChunk(x, y, z);
        if (containerChunk != null)
        {
            Block block = containerChunk.GetBlock(x - containerChunk.pos.x, y - containerChunk.pos.y, z - containerChunk.pos.z);
            return block;
        }
        else
        {
            return new BlockAir();
        }
    }
    //Sets block at a given world positon
    public void SetBlock(int x, int y, int z, Block block)
    {
        Chunk chunk = GetChunk(x, y, z);

        if (chunk != null)
        {
            chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
            chunk.update = true;
            //Checks if we need to update bordering chunks
            UpdateIfEqual(x - chunk.pos.x, 0, new WorldPos(x - 1, y, z));
            UpdateIfEqual(x - chunk.pos.x, Chunk.chunkSize - 1, new WorldPos(x + 1, y, z));
            UpdateIfEqual(y - chunk.pos.y, 0, new WorldPos(x, y - 1, z));
            UpdateIfEqual(y - chunk.pos.y, Chunk.chunkSize - 1, new WorldPos(x, y + 1, z));
            UpdateIfEqual(z - chunk.pos.z, 0, new WorldPos(x, y, z - 1));
            UpdateIfEqual(z - chunk.pos.z, Chunk.chunkSize - 1, new WorldPos(x, y, z + 1));
        }
    }

    void UpdateIfEqual(int value1, int value2, WorldPos pos)
    {
        if (value1 == value2)
        {
            Chunk chunk = GetChunk(pos.x,pos.y,pos.z);
            if(chunk !=null)
            {
                chunk.update = true;
            }
        }
    }

    public void SaveAndQuit()
    {
        player.loadChunks = false;
        foreach (KeyValuePair<WorldPos,Chunk> entry in chunks)
        {
            DestroyChunk(entry.Value.pos.x, entry.Value.pos.y, entry.Value.pos.z);
        }
    }

    public void LoadWorld(string worldName)
    {
        WorldConfig config = WorldConfig.LoadConfig(worldName);
        if (config == null)
        {
            Debug.Log("settings.bin for world: " + worldName + " does not exist. Creating one.");
            config = new WorldConfig();
            config.worldName = "World";
            config.seed = Random.Range(0, 10000);
            config.SaveConfig();
        }
        else
        {
            Debug.Log("Loaded settings for world: " + worldName);
            seed = config.seed;
            worldName = config.worldName;
        }
    }
}
