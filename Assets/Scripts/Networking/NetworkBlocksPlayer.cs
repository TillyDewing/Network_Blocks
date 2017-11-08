using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkBlocksPlayer : MonoBehaviour
{
    public PlayerInfo info;

    public Text usernameBox;
    public Transform head;

    public void UpdatePlayer(PlayerInfo info)
    {
        this.info = info;

        transform.position = info.pos;
        transform.eulerAngles = new Vector3(0,0,info.rot.z);
        head.eulerAngles = info.rot;
        if (usernameBox != null)
        {
            usernameBox.text = info.username;
        }
    }
}
