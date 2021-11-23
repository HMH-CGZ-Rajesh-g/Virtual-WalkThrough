using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public InputField inputField;
    public Button playButton;
    public GameObject Loading,validText;
    public static event Action<string> CUSTOMERINPUT;
    public static event Action<bool> CUSTOMERINPUTVALIDATION;
    public Image progressbar;

    int numberofAssets, currentNumber;
    string currentID;
    public void Awake()
    {
        CUSTOMERINPUTVALIDATION += InpuManager_CUSTOMERINPUTVALIDATION;
        VirtualwalkThrough.NumberofAssetsToLoad += VirtualwalkThrough_NumberofAssetsToLoad;
        VirtualwalkThrough.AssetDownloaded += VirtualwalkThrough_AssetDownloaded;
    }

    private void VirtualwalkThrough_AssetDownloaded()
    {
        currentNumber--;
        progressbar.fillAmount = (float)(numberofAssets - currentNumber) / (float)numberofAssets;  }

    private void VirtualwalkThrough_NumberofAssetsToLoad(int obj)
    {
        numberofAssets = obj;
        currentNumber = obj;
    }

    private void InpuManager_CUSTOMERINPUTVALIDATION(bool obj)
    {
        if (obj)
        {
            playButton.transform.parent.gameObject.SetActive(false);
            //inputField.gameObject.SetActive(false);
            //Connection.id = currentID;
            VirtualwalkThrough.FireID(currentID);
            Loading.SetActive(true);
        }
        else
        {
            Loading.SetActive(false);

            EnableValid();
            playButton.transform.parent.gameObject.SetActive(true);

            playButton.interactable = true;

        }


    }

    public void GiveInput()
    {
        Debug.Log(inputField.text);
        if (inputField.text != null && inputField.text!="")
        {
            currentID =inputField.text;
            CUSTOMERINPUT?.Invoke(currentID);
            playButton.interactable = false;
        }
        else
        {
            EnableValid();
        }


    }
    public void OnDestroy()
    {
        CUSTOMERINPUTVALIDATION -= InpuManager_CUSTOMERINPUTVALIDATION;
        VirtualwalkThrough.NumberofAssetsToLoad -= VirtualwalkThrough_NumberofAssetsToLoad;
        VirtualwalkThrough.AssetDownloaded -= VirtualwalkThrough_AssetDownloaded;



    }

    public static void FireValidation(bool validation)
    {
        CUSTOMERINPUTVALIDATION?.Invoke(validation);
    }
    private void EnableValid(string str="")
    {
        validText.SetActive(true);
        Invoke("OFFText", 2);
    }
    void OFFText()
    {
        validText.SetActive(false);
    }

}
