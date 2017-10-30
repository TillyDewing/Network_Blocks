using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientJoinUI : MonoBehaviour
{
    public InputField usernameBox;
    public InputField ipBox;

    public void JoinServer()
    {
        NetworkBlocksClient.singleton.username = usernameBox.text;
        NetworkBlocksClient.singleton.serverIp = ipBox.text;
        NetworkBlocksClient.singleton.InitializeClient();
    }
}
