using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//Test script for modifying terrain
public class Modify : NetworkBehaviour
{

    public NetworkWorldManager worldManager;
    Vector2 rot;

    private void Start()
    {
        worldManager = GameObject.FindGameObjectWithTag("World").GetComponent<NetworkWorldManager>();
        if(!isLocalPlayer)
        {
            GetComponent<Camera>().enabled = false;
            GetComponent<AudioListener>().enabled = false;
        }
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            worldManager.CmdSetBlockHit(transform.position, transform.forward, 0, false);
        }

        rot = new Vector2(
            rot.x + Input.GetAxis("Mouse X") * 2,
            rot.y + Input.GetAxis("Mouse Y") * 2);

        transform.localRotation = Quaternion.AngleAxis(rot.x, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rot.y, Vector3.left);

        transform.position += transform.forward * 2 * Input.GetAxis("Vertical");
        transform.position += transform.right * 2 * Input.GetAxis("Horizontal");
    }


}
