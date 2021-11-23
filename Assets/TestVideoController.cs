using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TestVideoController : MonoBehaviour
{
    public VideoPlayer player;
    public string url = "https://virtualwalkthrough.s3.us-east-2.amazonaws.com/WaterDamageScenario.mp4";
    public RenderTexture tex;
    bool playClicked=false;
    // Start is called before the first frame update
    void Awake()
    {
        player.prepareCompleted += Player_seekCompleted;
    }
    private void Start()
    {
    }
    private void OnDestroy()
    {
        player.prepareCompleted -= Player_seekCompleted;


    }

    private void Player_seekCompleted(VideoPlayer source)
    {
        Debug.Log("Completed");
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.P))
        {
            player.Play();
            playClicked = true;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            player.renderMode = VideoRenderMode.RenderTexture;
            player.targetTexture = tex;

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            player.renderMode = VideoRenderMode.MaterialOverride;
            player.targetMaterialRenderer = this.GetComponent<MeshRenderer>();
        }

        if (!player.isPrepared&& playClicked)
        {
            Debug.Log("preparing");
        }
    }
}
