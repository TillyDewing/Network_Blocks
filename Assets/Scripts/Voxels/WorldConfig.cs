using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

[Serializable]
public class WorldConfig
{
    public int seed;
    public string worldName;


    public void SaveConfig()
    {
        string saveLoc = Serialization.SaveLocation(worldName) + "Settings.bin";

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(saveLoc, FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, this);
        stream.Close();
    }

    public static WorldConfig LoadConfig(string worldName)
    {
        string saveFile = Serialization.SaveLocation(worldName) + "Settings.bin";
        if (!File.Exists(saveFile))
        {
            return null;
        }

        IFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFile, FileMode.Open);

        WorldConfig save = (WorldConfig)formatter.Deserialize(stream);
        stream.Close();
        return save;
    }

    
}
