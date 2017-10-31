using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientJoinUI : MonoBehaviour
{
    public InputField usernameBox;
    public InputField ipBox;
    public GameObject joinUI;

    public void JoinServer()
    {
        NetworkBlocksClient.singleton.username = usernameBox.text;
        NetworkBlocksClient.singleton.serverIp = ipBox.text;
        NetworkBlocksClient.singleton.InitializeClient();
    }

    private void Update()
    {
        if (NetworkBlocksClient.singleton.isConnected)
        {
            joinUI.SetActive(false);
        }
        else
        {
            joinUI.SetActive(true);
        }
    }
}
