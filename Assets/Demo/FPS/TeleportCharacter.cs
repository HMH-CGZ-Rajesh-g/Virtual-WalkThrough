using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCharacter : MonoBehaviour
{
    [SerializeField]
    GameObject teleportationRig;
    [SerializeField]
    GameObject fps, camera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TeleportPlayer()
    {
        fps.transform.position = new Vector3(teleportationRig.transform.position.x, 1.5f, teleportationRig.transform.position.z);
        fps.transform.rotation = teleportationRig.transform.rotation;
        camera.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
