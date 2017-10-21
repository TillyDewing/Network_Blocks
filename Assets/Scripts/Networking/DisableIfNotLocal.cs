using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DisableIfNotLocal : NetworkBehaviour
{
    public GameObject[] objects;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            foreach (GameObject obj in objects)
            {
                obj.SetActive(false);
            }
        }
    }
}
