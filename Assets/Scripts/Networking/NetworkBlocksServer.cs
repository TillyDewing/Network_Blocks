//William Dewing 2017
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkBlocksServer : MonoBehaviour
{
    public int serverPort = 25560;
    public Vector3 startPosistion = new Vector3(0, 50, 0);
    public GameObject playerPrefab;

    public Dictionary<int, PlayerInfo> clients = new Dictionary<int, PlayerInfo>(); //List of all connected clients

    static NetworkBlocksServer singleton;
    
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


    public void InitializeServer()
    {
        
        Debug.Log("Starting server on port: " + serverPort);
        if (NetworkServer.active)
        {
            Debug.LogError("Server already running");
        }

        NetworkServer.Listen(serverPort);

        //System msgs
        NetworkServer.RegisterHandler(MsgType.Connect, OnServerConnect);
        NetworkServer.RegisterHandler(MsgType.Disconnect, OnServerDisconnect);
        NetworkServer.RegisterHandler(MsgType.Error, OnServerError);

        //Application msgs
        NetworkServer.RegisterHandler(MessaageTypes.RequestChunkDataID, OnRequestChunkData);
        NetworkServer.RegisterHandler(MessaageTypes.SetBlockID, OnSetBlock);
        NetworkServer.RegisterHandler(MessaageTypes.ChatMessageID, OnReceiveChatMessage);
        NetworkServer.RegisterHandler(MessaageTypes.ClientInfoID, OnRecivePlayerInfo);
        NetworkServer.RegisterHandler(MessaageTypes.UnloadChunkID, OnUnloadChunk);
        NetworkServer.RegisterHandler(MessaageTypes.UpdatePlayerInfoID, OnUpdatePlayerInfo);

        DontDestroyOnLoad(gameObject);
        Debug.Log("Server Started");
        
    }



    public void ShutDownServer()
    {
        Debug.Log("Shuting Down Server");
        NetworkServer.Shutdown();
    }
    //System Handlers
    void OnServerConnect(NetworkMessage netMsg)
    {
        Debug.Log("Client Joining Ip: " + netMsg.conn.address);
    }
    void OnServerDisconnect(NetworkMessage netMsg)
    {
        
        Debug.Log("Client: " + clients[netMsg.conn.connectionId].username +  " Disconnected");
        UnregisterClient(netMsg.conn.connectionId);
    }
    void OnServerError(NetworkMessage netMsg)
    {
        Debug.LogError("ServerError from Server");
    }

    //Application Handlers
    void OnRequestChunkData(NetworkMessage netMsg)
    {
        int blocksPerResponse = 200;

        var msg = netMsg.ReadMessage<MessaageTypes.RequestChunkDataMessage>();
        //Debug.Log("Client requested block data for chunk: " + msg.pos.x + "," + msg.pos.y + "," + msg.pos.z);

        Chunk chunk = LoadChunk(msg.pos); //Load chunk
        //Find number of messages chunk data needs to be split into (Sending a complete chunk will require about 20 msgs)
        int numMsgs = Mathf.FloorToInt(chunk.modifiedBlocks.Count / 200f);
        List<KeyValuePair<WorldPos, Block>> blocks = chunk.modifiedBlocks.ToList();

        for (int msgNum = 0; msgNum <= numMsgs; msgNum++)
        {
            List<MessaageTypes.MsgBlock> msgBlocks = new List<MessaageTypes.MsgBlock>();
            for (int i = numMsgs * blocksPerResponse; numMsgs < (numMsgs + 1) * blocksPerResponse; i++)
            {
                if (i >= blocks.Count)
                {
                    break;
                }

                msgBlocks.Add(new MessaageTypes.MsgBlock((byte)blocks[i].Key.x, (byte)blocks[i].Key.y, (byte)blocks[i].Key.z, BlockIDManager.GetID(blocks[i].Value)));
            }

            var response = new MessaageTypes.ChunkDataMessage();
            response.blocks = msgBlocks.ToArray();
            response.chunkPos = msg.pos;
            netMsg.conn.Send(MessaageTypes.ChunkDataID, response);
            chunk.numClientsLoaded++;
        }
        
    }
    void OnSetBlock(NetworkMessage netMsg)
    {

        var msg = netMsg.ReadMessage<MessaageTypes.SetBlockMessage>();
        //Debug.Log("Client Set block");

        //Update block on server
        Chunk chunk = LoadChunk(msg.pos);
        WorldPos pos = GetBlockPosInChunk(msg.pos);
        chunk.SetBlock(pos.x, pos.y, pos.z, BlockIDManager.GetBlock(msg.blockID));
        
        //Send data to all clients to update clients chunk
        var response = new MessaageTypes.ChunkDataMessage();
        response.blocks = new MessaageTypes.MsgBlock[1];
        response.blocks[0] = new MessaageTypes.MsgBlock((byte)pos.x,(byte)pos.y,(byte)pos.z,msg.blockID);
        response.chunkPos = chunk.pos;

        //netMsg.conn.Send(MessaageTypes.ChunkDataID, response);

        NetworkServer.SendToAll(MessaageTypes.ChunkDataID, response);
    }

    void OnUnloadChunk(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<MessaageTypes.UnloadChunkMessage>();
        WorldPos pos = msg.pos;
        Chunk chunk = World.singleton.GetChunk(pos.x, pos.y, pos.z);

        if (chunk != null)
        {
            chunk.numClientsLoaded--;

            if (chunk.numClientsLoaded <= 0)
            {
                Debug.Log("Chunk no longer needed saving and deleting");
                World.singleton.DestroyChunk(pos.x, pos.y, pos.z);
            }
        }
    }

    void OnReceiveChatMessage(NetworkMessage netMsg)
    {
        Debug.Log("ReceviedChatMessage");
    }

    void OnRecivePlayerInfo(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<MessaageTypes.ClientInfoMessage>();
        if(!RegisterClient(netMsg.conn.connectionId, msg.info))
        {
            //If Clients username is the same as anouther disconnect client.
            netMsg.conn.Disconnect();
            return;
        }

        var response = new MessaageTypes.WorldDataMessage();
        response.seed = World.seed;
        response.worldName = World.singleton.worldName;
        
        netMsg.conn.Send(MessaageTypes.WorldPrefsID, response);
    }

    private void OnUpdatePlayerInfo(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<MessaageTypes.UpdatePlayerInfoMessage>();
        clients[netMsg.conn.connectionId] = msg.info;
        var response = new MessaageTypes.OtherPlayersInfoMessage();
        response.players = clients.Values.ToArray();
        netMsg.conn.Send(MessaageTypes.OtherPlayersInfoID, response);
    }


    public static Chunk LoadChunk(WorldPos pos)
    {
        Chunk chunk;
        float multiple = Chunk.chunkSize;
        pos.x = Mathf.FloorToInt(pos.x / multiple) * Chunk.chunkSize;
        pos.y = Mathf.FloorToInt(pos.y / multiple) * Chunk.chunkSize;
        pos.z = Mathf.FloorToInt(pos.z / multiple) * Chunk.chunkSize;
        
        if (!World.singleton.chunks.ContainsKey(pos))
        {
            chunk = World.singleton.CreateChunk(pos.x, pos.y, pos.z);
        }
        else
        {
            chunk = World.singleton.GetChunk(pos.x, pos.y, pos.z);
        }
        
        return chunk;
    }

    public static WorldPos GetBlockPosInChunk(WorldPos pos)
    {
        float multiple = Chunk.chunkSize;
        pos.x = Mathf.Abs(pos.x - (Mathf.FloorToInt(pos.x / multiple) * Chunk.chunkSize));
        pos.y = Mathf.Abs(pos.y - (Mathf.FloorToInt(pos.y / multiple) * Chunk.chunkSize));
        pos.z = Mathf.Abs(pos.z - (Mathf.FloorToInt(pos.z / multiple) * Chunk.chunkSize));
        return pos;
    }

    public bool RegisterClient(int connectionID, PlayerInfo info)
    {
        if (!clients.ContainsKey(connectionID))
        {
            Debug.Log(info.username  + "joined the server.");
            clients.Add(connectionID, info);
            return true;
        }
        return false;
    }
    public void UnregisterClient(int connectionID)
    {
        if (clients.ContainsKey(connectionID))
        {
            Debug.Log("UnRegistered Client");

            clients.Remove(connectionID);
        }
    }

    public int GetConnectionID(string username)
    {
        foreach (KeyValuePair<int, PlayerInfo> info in clients)
        {
            if (info.Value.username == username)
            {
                return info.Key;
            }
        }
        return -1;
    }

    
}

