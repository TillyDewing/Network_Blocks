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
    //----------- Server to Client ---------------
    public const short ChunkDataID = 160;
    public const short WorldPrefsID = 161;

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
    }
}

