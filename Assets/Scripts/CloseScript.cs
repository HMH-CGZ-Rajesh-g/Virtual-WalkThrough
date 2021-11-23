using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CloseScript : MonoBehaviour
{
   public GameObject Panel;
   public void hidePanel()
   {
       Panel.gameObject.SetActive(false);
   }
}
