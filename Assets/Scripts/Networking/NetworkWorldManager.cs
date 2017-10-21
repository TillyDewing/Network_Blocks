using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkWorldManager : NetworkBehaviour
{
    public static World world;
    
    public override void OnStartClient()
    {
        if (isServer)
        {
            RpcGetWorldInfo(world.worldName, World.seed);
        }
    }

    [ClientRpc]
    void RpcGetWorldInfo(string worldName, int seed)
    {
        Debug.Log("AM CLIENT");
        world.worldName = worldName;
        World.seed = seed;
    }
    [Command]
    public void CmdSetBlockHit(Vector3 position, Vector3 forward, byte blockID, bool adjacent)
    {
            RaycastHit hit;
            if (Physics.Raycast(position, forward, out hit, 100))
            {
                EditTerrain.SetBlock(hit,BlockIDManager.GetBlock(blockID), adjacent);
            }
        RpcClientSetBlockHit(transform.position, transform.forward, blockID, adjacent);
    }

    [ClientRpc]
    void RpcClientSetBlockHit(Vector3 position, Vector3 forward, byte blockID, bool adjacent)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, forward, out hit, 100))
        {
            EditTerrain.SetBlock(hit, BlockIDManager.GetBlock(blockID), adjacent);
        }
    }

    
}
