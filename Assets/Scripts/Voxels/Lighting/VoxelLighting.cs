using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelLighting : MonoBehaviour 
{
    public static Color darkCol = Color.black;
    public static Color lightCol = Color.white;

    static Color GetLightColor(byte lightLevel)
    {
        float level = Mathf.Clamp(0, 1, (lightLevel / 12f));
        return Color.Lerp(darkCol, lightCol, lightLevel);
    }

    public static byte GetLightColorUp(WorldPos pos)
    {
        return World.singleton.GetBlock(pos.x, pos.y, pos.z).lightLevel;
    }
    public static byte GetLightColorDown(WorldPos pos)
    {
        return World.singleton.GetBlock(pos.x, pos.y, pos.z).lightLevel;
    }
    public static byte GetLightColorNorth(WorldPos pos)
    {
        return World.singleton.GetBlock(pos.x, pos.y, pos.z).lightLevel;
    }
    public static byte GetLightColorSouth(WorldPos pos)
    {
        return World.singleton.GetBlock(pos.x, pos.y, pos.z).lightLevel;
    }
    public static byte GetLightColorEast(WorldPos pos)
    {
        return World.singleton.GetBlock(pos.x, pos.y, pos.z).lightLevel;
    }
    public static byte GetLightColorWest(WorldPos pos)
    {
        return World.singleton.GetBlock(pos.x, pos.y, pos.z).lightLevel;
    }
}
