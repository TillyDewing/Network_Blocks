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
    public const short RequestChunkData = 150;
    public const short SetBlock = 151;
    //----------- Server to Client ---------------
    public const short ChunkData = 160;
    public const short WorldPrefs = 151;

    //----------- Client to Server Messages ---------------

    public class RequestChunkDataMessage : MessageBase
    {
        public byte x;
        public byte y;
        public byte z;
    }

    public class SetBlockMessage : MessageBase
    {
        public WorldPos pos;
        public byte blockID;
    }

    //----------- Server to Client Messages ---------------
    public struct MsgBlock
    {
        public byte x;
        public byte y;
        public byte z;
        public byte blockID;
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
    }
}

