using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkBlocksServer : MonoBehaviour
{
    public int serverPort;

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
        NetworkServer.RegisterHandler(MessaageTypes.RequestChunkData, OnRequestChunkData);
        NetworkServer.RegisterHandler(MessaageTypes.SetBlock, OnSetBlock);

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
        Debug.Log("Client Connected");
    }
    void OnServerDisconnect(NetworkMessage netMsg)
    {
        Debug.Log("Client Disconnected");
    }
    void OnServerError(NetworkMessage netMsg)
    {
        Debug.LogError("ServerError from Server");
    }
    //Application Handlers
    void OnRequestChunkData(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<MessaageTypes.RequestChunkDataMessage>();
        Debug.Log("Client requested block data for chunk: " + msg.x + "," + msg.y + "," + msg.z);
    }
    void OnSetBlock(NetworkMessage netMsg)
    {

    }
}
