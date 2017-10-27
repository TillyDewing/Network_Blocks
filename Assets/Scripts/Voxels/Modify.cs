using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//Test script for modifying terrain
public class Modify : MonoBehaviour
{

    public NetworkWorldManager worldManager;
    Vector2 rot;

    private void Start()
    {
        worldManager = GameObject.FindGameObjectWithTag("World").GetComponent<NetworkWorldManager>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            {
                if (World.singleton.isClient)
                {
                    NetworkBlocksClient.SetBlock(hit, new BlockAir());
                }
                else
                {
                    EditTerrain.SetBlock(hit, new BlockAir());
                }
            }
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
