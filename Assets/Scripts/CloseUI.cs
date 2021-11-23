using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUI : MonoBehaviour
{
    public GameObject gameObject;
    bool active;

    public void OpenAndClose()
    {
        if(active == true)
        {
            gameObject.transform.gameObject.SetActive(true);
            active = true;

        }
        else
        {
            gameObject.transform.gameObject.SetActive(false);
            active = false;

        }
    }
}
