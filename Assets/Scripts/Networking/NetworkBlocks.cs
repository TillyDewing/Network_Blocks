//William Dewing 2017
using UnityEngine;
using UnityEngine.Networking;

public class MessaageTypes
{
    public enum ServerEvents
    {
        ReceivedChunkDataRequest,
        ReceivedSetBlock,
        ReceivedChunkData,
        ReceivedWorldData
    }

    //----------- Client to Server ---------------
    public const short RequestChunkDataID = 150;
    public const short SetBlockID = 151;
    public const short UnloadChunkID = 152;
    public const short ChatMessageID = 153;
    public const short ClientInfoID = 154;
    public const short UpdatePlayerInfoID = 155;
    //----------- Server to Client ---------------
    public const short ChunkDataID = 160;
    public const short WorldPrefsID = 161;
    public const short OtherPlayersInfoID = 162;
    public const short ClientDisconnectedID = 163;
    //----------- Client to Server Messages ---------------

    public class RequestChunkDataMessage : MessageBase
    {
        public WorldPos pos;
    }

    public class SetBlockMessage : MessageBase
    {
        public WorldPos pos;
        public byte blockID;
    }
    public class UnloadChunkMessage : MessageBase
    {
        public WorldPos pos;
    }

    public class ClientInfoMessage : MessageBase
    {
        public PlayerInfo info;
    }

    public class ChatMessage : MessageBase
    {
        public string username;
        public string message;
    }
    public class UpdatePlayerInfoMessage : MessageBase
    {
        public PlayerInfo info;
    }

    //----------- Server to Client Messages ---------------
    public struct MsgBlock
    {
        public byte x;
        public byte y;
        public byte z;
        public byte blockID;

        public MsgBlock(byte x, byte y, byte z, byte blockID)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.blockID = blockID;
        }
    }
    public class ChunkDataMessage : MessageBase
    {
        public WorldPos chunkPos;
        public MsgBlock[] blocks;
    }
    public class WorldDataMessage : MessageBase
    {
        public int seed;
        public string worldName;
        public Vector3 spawnPos;
    }
    
    public class OtherPlayersInfoMessage : MessageBase
    {
        public PlayerInfo[] players;
    }

    public class ClientDisconectedMessage : MessageBase
    {
        public string username;
    }


}

public struct PlayerInfo
{
    public string username;
    public Vector3 pos;
    public Vector3 rot;
    public byte health;
    public byte hunger;
    //This will store health and hunger
}

