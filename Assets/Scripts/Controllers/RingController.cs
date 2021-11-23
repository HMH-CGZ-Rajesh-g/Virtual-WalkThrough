using UnityEngine;

public class RingController : MonoBehaviour
{
    bool isFirst = true;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<CharacterMovement>() != null && isFirst)
        {
            isFirst = false;
//            other.gameObject.GetComponent<CharacterMovement>().SwapPosition(gameObject.transform);
        }
    }

    public void DisableSnap()
    {
        GetComponent<CapsuleCollider>().enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        isFirst = true;
    }
}
