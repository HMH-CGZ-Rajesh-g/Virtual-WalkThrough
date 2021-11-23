using UnityEngine;

public class JourneyController : MonoBehaviour
{
    public GameObject otherObject;

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            var selection = hitInfo.transform;
            if (Input.GetKey(KeyCode.Mouse0) && gameObject.name == hitInfo.transform.name)
            {
                gameObject.SetActive(false);
                otherObject.SetActive(true);
            };
        }
    }
}

