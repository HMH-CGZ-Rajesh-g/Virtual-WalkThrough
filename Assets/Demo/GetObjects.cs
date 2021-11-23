using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using cdb.virtualwalkthrough;
public class GetObjects : MonoBehaviour
{
    public BundleDownloader bundleDownloader;
    public List<GameObject> objects= new List<GameObject>();
    public string urllink = "";

    public GameObject fpsPrefeb,fps;
    public Demo demo;
    // Start is called before the first frame update

    void Start()
    {
        BundleDownloader.AllBundleDownloadFinished += BundleDownloader_AllBundleDownloadFinished;
        StartCoroutine(LoadJson(urllink));
    }

    private void Update()
    {



    }

    private void BundleDownloader_AllBundleDownloadFinished()
    {
        Debug.Log("Called Assets");
      Instantiate(  bundleDownloader.GetAssetBundle("manager_room_v01").LoadAsset<GameObject>("manager_room_v01"));
     GameObject stagingArea =  Instantiate(bundleDownloader.GetAssetBundle("reception_room_v01").LoadAsset<GameObject>("reception_room_v01"));


        Instantiate(fpsPrefeb, new Vector3(0,1,-3), Quaternion.identity);

    }

    IEnumerator LoadJson(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null)
        {
            Processjson(www.text);
        }
        else
        {
            Debug.Log("ERROR: " + www.error);
        }


    }
    void Processjson(string json)
    {
         demo = JsonConvert.DeserializeObject<Demo>(json);
            //JsonUtility.FromJson<Demo>(json);// textAsset.ToString());
        Debug.Log(demo.objecsts.Count);

        List<string> paths = new List<string>();
        List<SpawanObject> spawanObjects= new List<SpawanObject>();

        for (int i=0; i<objects.Count;i++)
        {
            SpawanObject obj = new SpawanObject(objects[i]);
            obj.pos =  new Vector3( demo.objecsts[i].pos[0], demo.objecsts[i].pos[1], demo.objecsts[i].pos[2]);
            obj.rotation =  new Vector3(demo.objecsts[i].rotation[0], demo.objecsts[i].rotation[1], demo.objecsts[i].rotation[2]);
            spawanObjects.Add(obj);
        }

        VirtualwalkThrough.FireInstantiateObject(spawanObjects);

        paths.Add(Utility.GetMediaPath()  + "manager_room_v01");
        paths.Add(Utility.GetMediaPath () + "reception_room_v01");

        StartCoroutine( bundleDownloader.DownloadAllBundles(paths));

    }


    private void OnDestroy()
    {
        BundleDownloader.AllBundleDownloadFinished -= BundleDownloader_AllBundleDownloadFinished;

    }


}
