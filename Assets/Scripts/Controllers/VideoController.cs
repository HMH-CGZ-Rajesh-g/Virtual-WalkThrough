using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    private VideoPlayer _vPlayer;
    private GameObject playerButton;
    private Vector3 originalPos;
    int count = 0;
    private void Start()
    {
        _vPlayer = GetComponent<VideoPlayer>();;
        //playerButton = gameObject.transform.GetChild(2).gameObject;
        //originalPos = playerButton.transform.localPosition;
    }

    private void OnMouseDown()
    {
        if (count == 0)
        {
            playerButton.transform.localPosition = new Vector3(-1.26f, -0.84f, originalPos.z);
            _vPlayer.Play();
            count++;
        }
        else
        {
            count = 0;
            _vPlayer.Pause();
            playerButton.transform.localPosition = new Vector3(originalPos.x, -originalPos.y, originalPos.z);
        }
    }


    public void VideoProgress()
    {

    }

    public void CloseScreen()
    {

    }

    public void PlayVideo(string url, RenderTexture texture)
    {
        _vPlayer.url = url;
        _vPlayer.targetTexture = texture;
        _vPlayer.Play();
    }
}
