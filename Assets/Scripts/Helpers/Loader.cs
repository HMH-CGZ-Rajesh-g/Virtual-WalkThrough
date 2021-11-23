using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    [Range(0, 1)] public float Progress = 0f;
    [SerializeField] TextMeshProUGUI progressTxt;
    [SerializeField] Image fill;
    // Start is called before the first frame update
    // Update is called once per frame
    public void UpdateProgress(float counter)
    {
        Progress = counter;
        fill.fillAmount = Progress;
        progressTxt.text = Math.Floor(Progress * 100).ToString() + "%";
    }
}
