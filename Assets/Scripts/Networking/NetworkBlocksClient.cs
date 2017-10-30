//William Dewing 2017
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class NetworkBlocksClient : MonoBehaviour
{
    public string serverIp = "127.0.0.1";
    public int serverPort = 25560;
    public NetworkClient client = null;
    public static NetworkBlocksClient singleton;
    public GameObject playerPrefab;
    public GameObject player;
    public NetworkBlocksPlayer otherPlayerPrefab;
    public float playerUpdateRate = .1f;
    public Dictionary<string, NetworkBlocksPlayer> otherPlayers = new Dictionary<string, NetworkBlocksPlayer>();
    public string username = "Player";

    float timer = 0;
    bool updateClientInfo = false;
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

    private void Update()
    {
        //if (client != null && client.isConnected)
        //{

        //    timer += Time.deltaTime;

        //    if (timer >= playerUpdateRate)
        //    {
        //        timer = 0;
        //        UpdatePlayerData();
        //    }
        //}
    }

    private void FixedUpdate()
    {
        if(updateClientInfo)
        {
            UpdatePlayerData();
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
        client.RegisterHandler(MessaageTypes.ChatMessageID, OnReceiveChatMessage);
        client.RegisterHandler(MessaageTypes.OtherPlayersInfoID, OnReceivePlayersInfo);
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
        updateClientInfo = false;
    }

    void OnClientConnect(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server: " + serverIp + " on port: " + serverPort);

        //Connected to server send client info
        var response = new MessaageTypes.ClientInfoMessage();
        response.info.username = username;
        netMsg.conn.Send(MessaageTypes.ClientInfoID, response);
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
        player = Instantiate(playerPrefab, msg.spawnPos, Quaternion.identity) as GameObject;
        NetworkChunkLoader.singleton.loadChunks = true;
        updateClientInfo = true;
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

    void OnReceiveChatMessage(NetworkMessage netMsg)
    {
        Debug.Log("ReceviedChatMessage");
        //Send message to chat display
    }

    void OnReceivePlayersInfo(NetworkMessage netMsg)
    {
        //Debug.Log("ReceviedPlayerInfo");

        var msg = netMsg.ReadMessage<MessaageTypes.OtherPlayersInfoMessage>();

        foreach (PlayerInfo info in msg.players)
        {
            //Debug.Log("U: " + info.username);
            if (info.username != username)
            {
                if (otherPlayers.ContainsKey(info.username))
                {
                    otherPlayers[info.username].UpdatePlayer(info);
                    
                }
                else
                {
                    NetworkBlocksPlayer newPlayer = Instantiate(otherPlayerPrefab, info.pos, Quaternion.Euler(info.rot)) as NetworkBlocksPlayer;
                    newPlayer.info = info;
                    otherPlayers.Add(info.username, newPlayer);
                }
            }
        }
    }

    public bool isConnected
    {
        get
        {
            if (client == null)
            {
                return false;
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

    public void UpdatePlayerData()
    {
        if (!client.isConnected)
            return;

        var msg = new MessaageTypes.UpdatePlayerInfoMessage();
        msg.info.username = username;
        msg.info.pos = player.transform.position;
        msg.info.rot = player.transform.rotation.eulerAngles;
        client.Send(MessaageTypes.UpdatePlayerInfoID, msg);
        //Debug.Log("SentPlayerInfo");

    }
}


