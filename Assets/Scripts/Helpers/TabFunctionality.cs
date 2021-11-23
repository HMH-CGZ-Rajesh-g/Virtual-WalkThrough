using System.Linq;
using TMPro;
using UnityEngine;

public class TabFunctionality : MonoBehaviour
{
    public TMP_InputField[] inputFields;
    int _count = 1;

    void Update()
    {
        // Check for tab press
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // if count is greater than total number of input fields set it to 0 index 
            if (_count > (inputFields.Count()-1))
            {
                _count = 0;
            }
            // activate the next input field on tab press
            inputFields[_count].ActivateInputField();
            _count++;
        }

        // On enter invoke submit functionality
        if (Input.GetKeyDown(KeyCode.Return))
        {
            transform.parent.GetComponent<CognitoHelper>().OnSignIn();
        }
    }
}
