using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] CharacterController CharacterController;
    bool isOpen = false;
    Animator anim;

    // Use this for initialization
    void Start()
    {

        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (!isOpen)
        {
            GetComponent<BoxCollider>().enabled = false;
        }

        anim.SetTrigger("OpenDoor");
    }

    void OnTriggerExit(Collider other)
    {

        GetComponent<BoxCollider>().enabled = true;
    }

    void pauseAnimationEvent()
    {
        isOpen = true;
        //CharacterController.enabled = true;
        anim.enabled = false;
    }


}
