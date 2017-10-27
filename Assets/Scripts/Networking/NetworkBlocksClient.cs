using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class NetworkBlocksClient : MonoBehaviour
{
    public string serverIp = "127.0.0.1";
    public int serverPort = 25560;
    public NetworkClient client = null;
    static NetworkBlocksClient singleton;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializeClient()
    {
        
        if (client != null)
        {
            Debug.LogError("Already Connected");
            return;
        }
        Debug.Log("Trying to connect");
        client = new NetworkClient();
        client.Connect(serverIp, serverPort);
        
        //System msgs
        client.RegisterHandler(MsgType.Connect, OnClientConnect);
        client.RegisterHandler(MsgType.Disconnect, OnClientDisconnect);
        client.RegisterHandler(MsgType.Error, OnClientError);
        //Application msgs
        client.RegisterHandler(MessaageTypes.WorldPrefsID, OnReceiveWorldPrefs);
        client.RegisterHandler(MessaageTypes.ChunkDataID, OnReceiveChunkData);
        DontDestroyOnLoad(gameObject);
    }

    public void ResetClient()
    {
        if (client == null)
        {
            return;
        }

        client.Disconnect();
        client = null;
    }

    void OnClientConnect(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server: " + serverIp + " on port: " + serverPort);
    }
    void OnClientDisconnect(NetworkMessage netMsg)
    {
        Debug.Log("Disconected from server");
        NetworkChunkLoader.singleton.loadChunks = true;
    }
    void OnClientError(NetworkMessage netMsg)
    {
        Debug.Log("ServerError from Client");
    }

    void OnReceiveWorldPrefs(NetworkMessage netMsg)
    {
        Debug.Log("Revieved World Prefs from server");
        var msg = netMsg.ReadMessage<MessaageTypes.WorldDataMessage>();
        World.singleton.worldName = msg.worldName;
        World.seed = msg.seed;
        World.singleton.isClient = true;
        NetworkChunkLoader.singleton.loadChunks = true;
    }

    void OnReceiveChunkData(NetworkMessage netMsg)
    {
        //Debug.Log("Recevied Chunk Data");
        var msg = netMsg.ReadMessage<MessaageTypes.ChunkDataMessage>();
        
        Chunk chunk = EditTerrain.GetChunk(msg.chunkPos);
        if (chunk == null)
        {
            Debug.Log("Chunk not loaded");
            return;
        }
        foreach (MessaageTypes.MsgBlock msgBlock in msg.blocks)
        {
            Debug.Log(msg.chunkPos.x + "," + msg.chunkPos.y + "," + msg.chunkPos.z);
            Debug.Log(msgBlock.x + "," + msgBlock.y + "," + msgBlock.y);
            World.singleton.SetBlock(msgBlock.x + msg.chunkPos.x, msgBlock.y + msg.chunkPos.y, msgBlock.z + msg.chunkPos.z, BlockIDManager.GetBlock(msgBlock.blockID));
        }
        //If chunk is already rendered update and rerender it.
        if (chunk.rendered)
        {
            chunk.update = true;
        }
    }

    public bool isConnected
    {
        get
        {
            if (client == null)
            {
                return true;
            }

            return client.isConnected;

        }
    }

    public static void RequestChuckData(WorldPos pos)
    {
        if (singleton.isConnected)
        {
            var msg = new MessaageTypes.RequestChunkDataMessage();
       
            msg.pos = pos;
            singleton.client.Send(MessaageTypes.RequestChunkDataID, msg);
        }
        else
        {
            Debug.LogError("Cannont request chunk data client is not connected to server.");
        }
    }

    public static void SetBlock(RaycastHit hit, Block block, bool adjacent = false)
    {
        if (!singleton.isConnected)
        {
            Debug.LogError("Cannont set block client is not connected");
            return;
        }
        WorldPos pos = EditTerrain.GetBlockPos(hit, adjacent);
        var msg = new MessaageTypes.SetBlockMessage();
        msg.pos = pos;
        msg.blockID = BlockIDManager.GetID(block);
        singleton.client.Send(MessaageTypes.SetBlockID, msg);
    }

    public void TestChunkLoading()
    {
        RequestChuckData(new WorldPos(0, 0, 0));
    }
}
