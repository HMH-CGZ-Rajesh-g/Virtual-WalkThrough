using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using cdb.virtualwalkthrough;
using UnityEngine.Networking;
using System.Text;

public class DataManager : MonoBehaviour
{
    public GameObject fps, videocontent, videocontroller;
    public static ODCData odcData;
    public static ODCRoot odcRoot;
    public static AssetData assetData;

    public RuntimeAnimatorController singleDoor, doubleDoor, meetingSingleDoor, meetingDoubleDoor;
    public BundleDownloader bundleDownloader;

    public List<(string, object)> downloadObjects = new List<(string, object)>();
    public Vector3 leftPos, rightPos, middlePos, frontPos, backPos, lobbyPos;

    private List<string> cubicals = new List<string>();
    private GameObject frontWall, backWall, leftWall, rightWall, leftdisplayWall;

    List<Vector3> occupideplaces = new List<Vector3>();
    List<GameObject> occupideObjectes = new List<GameObject>();
    int videoControllerCount = 0;
    bool assetsDone, imagesDone, videosDone;

    List<(GameObject, string, object)> dataassign = new List<(GameObject, string, object)>();
    List<GameObject> odcScreens = new List<GameObject>();

    public GameObject loadingObject;

    List<string> paths = new List<string>();
    List<string> imagepaths = new List<string>();
    Dictionary<string, Type> jsonData = new Dictionary<string, Type>();

    Dictionary<string, Action> actionsData = new Dictionary<string, Action>();

    public static Camera playerCamera;

    public List<string> videoLinks = new List<string>();

    bool isHelpDesk = false;
    public static DataManager instance;
    public RenderTexture renderTexture;
    private bool isMobileDevice = true;


    GameObject commonCube, agileCube, commonTV;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        BundleDownloader.AllBundleDownloadFinished += BundleDownloader_AllBundleDownloadFinished;
        BundleDownloader.AllImagesFinished += BundleDownloader_AllImagesFinished;
        InputManager.CUSTOMERINPUT += InpuManager_CUSTOMERINPUT;
        // VirtualwalkThrough.UpdateFeedBack += VirtualwalkThrough_UpdateFeedBack;

#if UNITY_WEBGL && !UNITY_EDITOR
isMobileDevice = false;     
#endif


    }


    private void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown)
        {
            VirtualwalkThrough.FireKeyActon(e.keyCode);
            //            Debug.Log(e.keyCode);
        }
    }
    private void InpuManager_CUSTOMERINPUT(string obj)
    {
        //jsonData.Add(Utility.GetMediaPath() + "MCD1.json", typeof(ODCData));//81613
        //jsonData.Add("https://virtual-walkthrough-admin.s3.ap-south-1.amazonaws.com/" + obj + "/undefined".ToString(), typeof(ODCData));//, InputDataValidation);

        //phase2
        LoadJsonFile loadJsonFile = GetComponent<LoadJsonFile>();

        loadJsonFile.LoadWithCustomerID(obj, odcRoot, InputDataValidation);

        //StartCoroutine(LoadJson(jsonData, InputDataValidation));

    }
    void InputDataValidation(bool validation, ODCRoot root)
    {


        InputManager.FireValidation(validation);
        if (validation)
        {
            odcRoot = root;
            odcData = odcRoot.Items[0];
            jsonData.Clear();

            LoadData();
        }
    }
    public void LoadData()
    {
        jsonData.Add(GlobalConstants.MEDIA_SERVER_URL + "Webgl/" + "AssetDetails.json", typeof(AssetData));
        jsonData.Add(GlobalConstants.MEDIA_SERVER_URL  + "Feedback.json", typeof(List<Feedback>));

        LoadODCJson(jsonData);
    }


    private void BundleDownloader_AllImagesFinished(bool obj)
    {
        imagesDone = true;
        StartExperience();
    }

    private void BundleDownloader_AllBundleDownloadFinished()
    {

        //AssigningWalls Waals
        frontWall = bundleDownloader.GetAssetBundle("m_wall3").LoadAsset<GameObject>("m_wall3");
        leftWall = bundleDownloader.GetAssetBundle("m_wall2").LoadAsset<GameObject>("m_wall2");

        rightWall = bundleDownloader.GetAssetBundle("m_wall6").LoadAsset<GameObject>("m_wall6");
        backWall = bundleDownloader.GetAssetBundle("m_wall4").LoadAsset<GameObject>("m_wall4");
        leftdisplayWall = bundleDownloader.GetAssetBundle("m_wall8").LoadAsset<GameObject>("m_wall8");


        if (isHelpDesk)
        {

            PlaceHelpDesk(bundleDownloader.GetAssetBundle("m_cube_2").LoadAsset<GameObject>("m_cube_2"), bundleDownloader.GetAssetBundle("m_odc_small").LoadAsset<GameObject>("m_odc_small"));

        }

        if (paths.Contains(Utility.GetMediaPath() + "m_cube_1"))
        {
            commonCube = bundleDownloader.GetAssetBundle("m_cube_1").LoadAsset<GameObject>("m_cube_1");
        }
        if (paths.Contains(Utility.GetMediaPath() + "m_cube_4"))
        {
            agileCube = bundleDownloader.GetAssetBundle("m_cube_4").LoadAsset<GameObject>("m_cube_4");
        }
        commonTV = bundleDownloader.GetAssetBundle("m_display_screen2").LoadAsset<GameObject>("m_display_screen2");

        // PlaceCubesBase(cubicals, bundleDownloader.GetAssetBundle("m_cube_1").LoadAsset<GameObject>("m_cube_1"), bundleDownloader.GetAssetBundle("m_display_screen2").LoadAsset<GameObject>("m_display_screen2"));





        foreach ((string, object) item in downloadObjects)
        {
            Asset asset2 = new Asset();
            foreach (Asset asset1 in assetData.asset)
            {
                if (asset1.name == item.Item1)
                {
                    asset2 = asset1;
                }
            }

            GameObject instatiated = Instantiate(bundleDownloader.GetAssetBundle(item.Item1).LoadAsset<GameObject>(item.Item1));


            PlaceObject(instatiated, asset2);
            // dataassign.Add((instatiated, item.Item1,item.Item2));
            AssignData((instatiated, item.Item1, item.Item2));


        }

        //for (int i = 0; i < occupideplaces.Count; i++)
        //{
        //    Debug.Log(occupideplaces[i]);
        //    GameObject display = Instantiate(bundleDownloader.GetAssetBundle("m_wall8").LoadAsset<GameObject>("m_wall8"));
        //    display.transform.position = occupideplaces[i];
        //    occupideObjectes.Add(display.transform.GetChild(0).gameObject);
        //}


        while (frontPos.x > -18)
        {
            Instantiate(frontWall, frontPos, frontWall.transform.rotation);
            frontPos += Vector3.left * 6;

        }

        int leftvalue = 0;
        while (leftvalue > middlePos.z)
        {
            Vector3 currentleft = new Vector3(-18, 0, leftvalue);
            if (!occupideplaces.Contains(currentleft))
            {
                Instantiate(leftWall, currentleft, leftWall.transform.rotation);
            }
            leftvalue -= 6;

        }
        while (leftvalue > lobbyPos.z)
        {
            Vector3 currentleft = new Vector3(-6, 0, leftvalue);
            if (!occupideplaces.Contains(currentleft))
            {
                Instantiate(leftWall, currentleft, leftWall.transform.rotation);
            }
            leftvalue -= 6;

        }
        while (rightPos.z > lobbyPos.z)
        {
            Instantiate(rightWall, rightPos, rightWall.transform.rotation);
            rightPos += Vector3.back * 6;

        }
        backPos += Vector3.forward * middlePos.z;
        while (backPos.x < 0)
        {
            if (backPos.x == -12)
            {
                GameObject display = Instantiate(bundleDownloader.GetAssetBundle("m_wall1").LoadAsset<GameObject>("m_wall1"));
                display.transform.position = backPos;
                occupideObjectes.Add(display.transform.GetChild(0).gameObject);
            }
            else if (backPos.x == -6)
            {
                Instantiate(backWall, backPos + (Vector3.forward * (lobbyPos.z - middlePos.z)), backWall.transform.rotation);
            }
            else
            {
                Instantiate(backWall, backPos, backWall.transform.rotation);
            }
            backPos += Vector3.right * 6;

        }



        LoadImageORVideo(odcData.Team_Area.TeamBackWallpaper, occupideObjectes[occupideObjectes.Count - 1].gameObject);


        assetsDone = true;
        StartExperience();

    }



    IEnumerator LoadFromResources()
    {

        yield return new WaitUntil(() => imagesDone);

        //Generating Waals
        frontWall = Resources.Load<GameObject>("m_wall3");
        leftWall = Resources.Load<GameObject>("m_wall2");

        rightWall = Resources.Load<GameObject>("m_wall6");
        backWall = Resources.Load<GameObject>("m_wall4");
        leftdisplayWall = Resources.Load<GameObject>("m_wall8");


        if (isHelpDesk)
        {
            PlaceHelpDesk(Resources.Load<GameObject>("m_cube_2"), Resources.Load<GameObject>("m_odc_small"));
            // PlaceHelpDesk(bundleDownloader.GetAssetBundle("m_cube_2").LoadAsset<GameObject>("m_cube_2"));

        }
        // PlaceCubesBase(cubicals, Resources.Load<GameObject>("m_cube_1"), Resources.Load<GameObject>("m_display_screen2"));

        commonCube = Resources.Load<GameObject>("m_cube_1");

        agileCube = Resources.Load<GameObject>("m_cube_4");

        commonTV = Resources.Load<GameObject>("m_display_screen2");



        foreach ((string, object) item in downloadObjects)
        {
            Asset asset2 = new Asset();
            foreach (Asset asset1 in assetData.asset)
            {
                if (asset1.name == item.Item1)
                {
                    asset2 = asset1;
                }
            }

            GameObject instatiated = Instantiate(Resources.Load<GameObject>(item.Item1));


            PlaceObject(instatiated, asset2);
            //dataassign.Add(
            AssignData((instatiated, item.Item1, item.Item2));

        }


        while (frontPos.x > -18)
        {
            Instantiate(frontWall, frontPos, frontWall.transform.rotation);
            frontPos += Vector3.left * 6;

        }

        int leftvalue = 0;
        while (leftvalue > middlePos.z)
        {
            Vector3 currentleft = new Vector3(-18, 0, leftvalue);
            if (!occupideplaces.Contains(currentleft))
            {
                Instantiate(leftWall, currentleft, leftWall.transform.rotation);
            }
            leftvalue -= 6;

        }
        while (leftvalue > lobbyPos.z)
        {
            Vector3 currentleft = new Vector3(-6, 0, leftvalue);
            if (!occupideplaces.Contains(currentleft))
            {
                Instantiate(leftWall, currentleft, leftWall.transform.rotation);
            }
            leftvalue -= 6;

        }
        while (rightPos.z > lobbyPos.z)
        {
            Instantiate(rightWall, rightPos, rightWall.transform.rotation);
            rightPos += Vector3.back * 6;

        }
        backPos += Vector3.forward * middlePos.z;
        while (backPos.x < 0)
        {
            if (backPos.x == -12)
            {
                GameObject display = Instantiate(Resources.Load<GameObject>("m_wall1"));
                display.transform.position = backPos;
                occupideObjectes.Add(display.transform.GetChild(0).gameObject);
            }
            else if (backPos.x == -6)
            {
                Instantiate(backWall, backPos + (Vector3.forward * (lobbyPos.z - middlePos.z)), backWall.transform.rotation);
            }
            else
            {
                Instantiate(backWall, backPos, backWall.transform.rotation);
            }
            backPos += Vector3.right * 6;

        }



        LoadImageORVideo(odcData.Team_Area.TeamBackWallpaper, occupideObjectes[occupideObjectes.Count - 1].gameObject);


        assetsDone = true;
        StartExperience();

    }


    void StartExperience()
    {
        Debug.Log("=====> sHere");
        if (assetsDone && imagesDone)
        {
            Connection.instance.playerobj = Instantiate(fps, new Vector3(-11.4f, 1, 2.5f), Quaternion.Euler(0, 90, 0));
            playerCamera = Connection.instance.playerobj.transform.GetChild(0).GetComponent<Camera>();
            //AssignData(dataassign);
            VirtualwalkThrough.FireAreaActions(actionsData);
            VideosLoaded();
            loadingObject.SetActive(false);

        }

    }
    void VideosLoaded()
    {
        loadingObject.SetActive(false);

    }


    void LoadODCJson(Dictionary<string, Type> urls)
    {
        StartCoroutine(LoadJson(urls, GetODCData));

    }





    void GetODCData(bool odcstatus)
    {


        string assetsPath = Utility.GetMediaPath();


        paths.Add(assetsPath + "m_wall6");
        paths.Add(assetsPath + "m_wall2");
        paths.Add(assetsPath + "m_wall3");
        paths.Add(assetsPath + "m_wall4");
        paths.Add(assetsPath + "m_wall8");
        paths.Add(assetsPath + "m_wall1");
        paths.Add(assetsPath + "m_small_lobby");

        isHelpDesk = odcData.Staging_Area.helpDesk;

        if (odcData.Staging_Area != null)
        {
            paths.Add(assetsPath + "m_staging_room");
            downloadObjects.Add(("m_staging_room", odcData.Staging_Area));
            AddImagePath(odcData.Staging_Area.WallLogo_StagingArea);
            AddImagePath(odcData.Staging_Area.WallWallpaper_StagingArea);
            AddImagePath(odcData.Staging_Area.Display2_StagingArea);


            for (int i = 0; i < odcData.Staging_Area.WallDisplay_StagingArea.Count; i++)
            {
                AddImagePath(odcData.Staging_Area.WallDisplay_StagingArea[i].InputImage_StagingArea);
                AddImagePath(odcData.Staging_Area.WallDisplay_StagingArea[i].WallDisplay_StagingArea);
            }


        }





        if (isHelpDesk)
        {
            paths.Add(assetsPath + GetODCName("Small"));
        }
        //ODC Team Area
        if (odcData.Team_Area != null)
        {
            AddImagePath(odcData.Team_Area.TeamBackWallpaper);
            Debug.Log(odcData.Team_Area.TeamBackWallpaper);

            foreach (Team team in odcData.Team_Area.users)
            {
                if (!paths.Contains(assetsPath + GetODCName(team.TeamSize_Team)))
                {
                    paths.Add(assetsPath + GetODCName(team.TeamSize_Team));
                }
                if(!paths.Contains(assetsPath + "m_cube_1")&& !team.AglieTeam_Team)
                {
                    paths.Add(assetsPath + "m_cube_1");
                }
                else if (!paths.Contains(assetsPath + "m_cube_4") && team.AglieTeam_Team)
                {
                    paths.Add(assetsPath + "m_cube_4");
                }
                AddImagePath(team.WallWallpaper_Team);

                AddImagePath(team.WallDisplay1_Team);
                cubicals.Add(team.TeamSize_Team);
                downloadObjects.Add((GetODCName(team.TeamSize_Team), team));

            }
        }


        if (odcData.Meeting_Room != null)
        {
            List<string> meetingRooms = new List<string>();
            for (int i = 0; i < odcData.Meeting_Room.Count; i++)
            {
                if (!meetingRooms.Contains(odcData.Meeting_Room[i].MeetingRoomType_MeetingRoom))
                {
                    meetingRooms.Add(odcData.Meeting_Room[i].MeetingRoomType_MeetingRoom);
                    paths.Add(assetsPath + GetMeetingRoomName(odcData.Meeting_Room[i].MeetingRoomType_MeetingRoom));


                }

                AddImagePath(odcData.Meeting_Room[i].WallWallpaper_MeetingRoom);
                AddImagePath(odcData.Meeting_Room[i].WallDisplay1_MeetingRoom);
                //AddImagePath(odcData.Meeting_Room[i].WallDisplay2_MeetingRoom);


                downloadObjects.Add((GetMeetingRoomName(odcData.Meeting_Room[i].MeetingRoomType_MeetingRoom), odcData.Meeting_Room[i]));

            }
        }



        if (odcData.Manager_Cabin != null)
        {
            paths.Add(assetsPath + "m_manager_room");
            downloadObjects.Add(("m_manager_room", odcData.Manager_Cabin));
            AddImagePath(odcData.Manager_Cabin.WallWallpaper_ManagerCabin);
            AddImagePath(odcData.Manager_Cabin.WallDisplay1_ManagerCabin);

        }
        if (odcData.OpenSpaces_Cafe != null)
        {
            paths.Add(assetsPath + "m_cafe");
            downloadObjects.Add(("m_cafe", odcData.OpenSpaces_Cafe));
            AddImagePath(odcData.OpenSpaces_Cafe.WallWallpaper_Cafe);
            AddImagePath(odcData.OpenSpaces_Cafe.WallDisplay1_Cafe);
            AddImagePath(odcData.OpenSpaces_Cafe.WhiteBoard_Cafe);


        }
        if (odcData.Trophy_Room != null)
        {
            paths.Add(assetsPath + "m_trophy_room");
            downloadObjects.Add(("m_trophy_room", odcData.Trophy_Room));
            AddImagePath(odcData.Trophy_Room.WallWallpaper_TrophyRoom);
            AddImagePath(odcData.Trophy_Room.WallDisplay1_TrophyRoom);


        }


        //paths.Add(assetsPath + "m_cube_1");
        //paths.Add(assetsPath + "m_cube_4");

        paths.Add(assetsPath + "m_cube_2");

        paths.Add(assetsPath + "m_display_screen2");

        for (int i = 0; i < imagepaths.Count; i++)
        {
            //  imagepaths[i] = imagepaths[i].Replace(" ", "%20");
        }

        StartCoroutine(bundleDownloader.LoadImagesFromFilePath(imagepaths, false));

        if (isMobileDevice)
            StartCoroutine(LoadFromResources());
        else
            StartCoroutine(bundleDownloader.DownloadAllBundles(paths));

        VirtualwalkThrough.FireNumberofAssetsToLoad(imagepaths.Count + paths.Count);
        for (int i = 0; i < videoLinks.Count; i++)
        {

            // StartCoroutine(videoLinks[i])
        }

    }

    /// <summary>
    /// loading JsonFiles
    /// </summary>
    /// <param name="urls"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    IEnumerator LoadJson(Dictionary<string, Type> urls, Action<bool> action)
    {
        bool callback = false;
        //foreach (KeyValuePair<string, Type> url in urls)
        for (int i = 0; i < urls.Count; i++)
        {
            KeyValuePair<string, Type> url = urls.ElementAt(i);
            Debug.Log(url.Key);
            WWW www = new WWW(url.Key);
            yield return www;
            if (www.error == null)
            {
                // Processjson(www.text);
                //action?.Invoke(www.text);
                callback = true;
                if (url.Value == typeof(ODCData))
                {
                    odcData = JsonConvert.DeserializeObject<ODCData>(www.text);
                }
                if (url.Value == typeof(AssetData))
                {

                    assetData = JsonConvert.DeserializeObject<AssetData>(www.text);
                }
                if (url.Value == typeof(List<Feedback>))
                {
                    Debug.Log(www.text);
                    VirtualwalkThrough.FireLoadFeedback(JsonConvert.DeserializeObject<List<Feedback>>(www.text));
                }

            }
            else
            {
                Debug.Log("ERROR: " + www.error);
            }
        }
        action?.Invoke(callback);

    }



    string GetMeetingRoomName(string type)
    {
        string meetingRoom = "m_meeting_room1";
        if (type == "Type-B")
        {
            meetingRoom = "m_meeting_room2";

        }
        if (type == "Type-C")
            meetingRoom = "m_meeting_room3";

        return meetingRoom;
    }

    string GetODCName(string type)
    {
        string meetingRoom = "m_odc_small";
        if (type == "Medium")
        {
            meetingRoom = "m_odc_medium";

        }
        if (type == "Large")
            meetingRoom = "m_odc_high";

        return meetingRoom;
    }

    void PlaceObject(GameObject _gameObject, Asset asset)
    {
        if (asset.direction == "left")
        {
            int leftvalue = 0;
            Vector3 compareVector = leftPos;

            int asetlength = asset.length;
            int next = 0;
            while (asetlength > 0)
            {

                if (middlePos.z > compareVector.z - asset.length && middlePos.z < leftvalue)
                {

                    leftvalue -= 6;
                }
                else
                {
                    asetlength -= 6;




                    if (middlePos.z < leftvalue)
                    {
                        compareVector = leftPos + (Vector3.forward * leftvalue);
                    }
                    else
                    {
                        compareVector = leftPos + (Vector3.forward * leftvalue) + Vector3.right * 12;
                    }
                    if (occupideplaces.Contains(compareVector - (Vector3.forward * 6 * next)))// - Vector3.forward * 6*next))
                    {

                        leftvalue -= 6 * (next + 1);
                        asetlength = asset.length;
                        next = 0;
                        continue;
                    }
                    next++;
                }
            }



            if (middlePos.z < leftvalue)
            {
                compareVector = leftPos + (Vector3.forward * leftvalue);
            }
            else
            {
                compareVector = leftPos + (Vector3.forward * leftvalue) + Vector3.right * 12;
            }

            _gameObject.transform.position = compareVector;// leftPos + (Vector3.forward * leftvalue) + Vector3.right*12;


            asetlength = asset.length;
            Debug.Log("Length " + asetlength);

            next = 0;
            while (asetlength > 0)
            {
                asetlength -= 6;



                occupideplaces.Add(compareVector - (Vector3.forward * next * 6));

                if ((compareVector - (Vector3.forward * next * 6)).z <= lobbyPos.z)
                {

                    Instantiate(bundleDownloader.GetAssetBundle("m_small_lobby").LoadAsset<GameObject>("m_small_lobby"), lobbyPos, Quaternion.identity);
                    lobbyPos -= Vector3.forward * 6;
                }
                //Debug.Log("Adding " + (compareVector - (Vector3.forward * next * 6)));

                next++;
            }
            // leftPos -= Vector3.forward * asset.length;


        }
        if (asset.direction == "right")
        {
            _gameObject.transform.position = rightPos;


            rightPos -= Vector3.forward * asset.length;

            if (rightPos.z < middlePos.z && rightPos.z < lobbyPos.z)
            {
                while (rightPos.z < lobbyPos.z)
                {
                    Instantiate(Instantiate(bundleDownloader.GetAssetBundle("m_small_lobby").LoadAsset<GameObject>("m_small_lobby"), lobbyPos, Quaternion.identity));
                    lobbyPos -= Vector3.forward * 6;
                }
            }
        }
        if (asset.direction == "middle")
        {
            _gameObject.transform.position = middlePos;
            middlePos -= Vector3.forward * asset.length;
            lobbyPos -= Vector3.forward * asset.length;
        }
        if (asset.direction == "front")
        {
            _gameObject.transform.position = frontPos;
            frontPos += Vector3.left * asset.length;
        }

    }

    void GenerateWalls()
    {
        //Wall Generation
        frontWall = bundleDownloader.GetAssetBundle("m_wall3").LoadAsset<GameObject>("m_wall3");
        leftWall = bundleDownloader.GetAssetBundle("m_wall2").LoadAsset<GameObject>("m_wall2");

        rightWall = bundleDownloader.GetAssetBundle("m_wall6").LoadAsset<GameObject>("m_wall6");
        backWall = bundleDownloader.GetAssetBundle("m_wall4").LoadAsset<GameObject>("m_wall4");


    }



    void PlaceCubesBase(string ODCType, GameObject _cube, GameObject _display, int startPosition)
    {
        int distance = startPosition;
        int nuberofchairs = 0;


        GameObject cube = Instantiate(_cube, new Vector3(-7, 0, distance - 9), Quaternion.identity);

        GameObject display = Instantiate(_display, new Vector3(-8, 0, distance - 4), Quaternion.identity);
        odcScreens.Add(GetChildWithname("TV_screen_", display.transform).gameObject);
        if (ODCType == "Small")
        {
            occupideplaces.Add(leftPos + Vector3.forward * (distance - 6));
            nuberofchairs = 3;
            // distance -= 18;
        }
        if (ODCType == "Medium")
        {
            occupideplaces.Add(leftPos + Vector3.forward * (distance - 6));

            nuberofchairs = 4;
            //distance -= 24;

        }
        if (ODCType == "Large")
        {
            occupideplaces.Add(leftPos + Vector3.forward * (distance - 12));

            nuberofchairs = 5;
            //distance -= 30;

        }
        PlaceDisplayWall(leftdisplayWall, occupideplaces[occupideplaces.Count - 1]);
        PlaceCubes(cube, cube.transform.position, nuberofchairs);

    }

    void PlaceDisplayWall(GameObject _gameObject, Vector3 _position)
    {
        GameObject display = Instantiate(_gameObject);
        display.transform.position = _position;
        occupideObjectes.Add(display.transform.GetChild(0).gameObject);
    }

    /// <summary>
    /// placing cubicals
    /// </summary>
    /// <param name="cube"></param>
    /// <param name="middlePossition"></param>
    void PlaceCubes(GameObject cube, Vector3 middlePossition, int count)
    {
        List<Vector3> positions = Utility.GetCubicalPositions(middlePossition, 4f, 4f, count);

        for (int i = 0; i < positions.Count; i++)
        {
            ActiaveCharacters(Instantiate(cube, positions[i], Quaternion.identity), 4);
        }
    }

    /// <summary>
    /// placing cubicals
    /// </summary>
    /// <param name="cube"></param>
    void PlaceHelpDesk(GameObject cube, GameObject odc_helpDesk)
    {
        Asset asset = assetData.asset[1];
        GameObject helpDesk = Instantiate(odc_helpDesk);
        PlaceObject(helpDesk, asset);

        TextMeshPro[] nameboardscafe = helpDesk.transform.GetComponentsInChildren<TextMeshPro>();

        for (int j = 0; j < nameboardscafe.Length; j++)
        {
            nameboardscafe[j].text = "HelpDesk";
        }
        Vector3 baseposition = new Vector3(-10, 0, -5);

        for (int i = 0; i < 6; i++)
        {
            ActiaveCharacters(Instantiate(cube, baseposition + Vector3.left * 4 - Vector3.forward * i * 2, cube.transform.rotation), 2);
            ActiaveCharacters(Instantiate(cube, baseposition - Vector3.forward * i * 2, cube.transform.rotation), 2);
            ActiaveCharacters(Instantiate(cube, baseposition + Vector3.right * 4 - Vector3.forward * i * 2, cube.transform.rotation), 2);


        }
    }

    void ActiaveCharacters(GameObject cube, int numberofchairs)
    {
        for (int cubeNumber = 1; cubeNumber < numberofchairs + 1; cubeNumber++)
        {
            GameObject charactedParent = cube.transform.Find("ChairOffice0" + cubeNumber).gameObject;

            int randomNumber = UnityEngine.Random.Range(0, 6);

            if (randomNumber < 3)
            {
                charactedParent.transform.GetChild(0).GetChild(randomNumber).gameObject.SetActive(true);
            }
        }
    }
    /// <summary>
    /// AssigningData
    /// </summary>
    /// <param name="instatiated"> instantiated object</param>
    /// <param name="item"> item1: name of bundle/object item2: type of object
    /// we are  using item1 in this method to get gameobject from assetbundle </param>
    private void AssignData((GameObject, string, object) assigningData)
    {
        //for (int i = 0; i < assigningData.Count; i++)
        //{
        GameObject instatiated = assigningData.Item1;

        switch (assigningData.Item3)
        {
            case StagingArea stagingArea:
                StagingAreaDataAssigner stagingAreaDataAssigner = instatiated.AddComponent<StagingAreaDataAssigner>();
                stagingAreaDataAssigner.AssigningStagingArea(instatiated, stagingArea);

                break;

            case ManagerCabin managerCabin:

                ManagerCabinDataAssigner managerCabinDataAssigner = instatiated.AddComponent<ManagerCabinDataAssigner>();
                managerCabinDataAssigner.AssignData(instatiated, managerCabin);

                break;

            case MeetingRoom meetingroom:

                int meetingNumber = odcData.Meeting_Room.IndexOf(meetingroom);

                // instatiated.transform.GetComponentInChildren<TextMeshPro>().text = meetingroom.InputTitle_MeetingRoom;

                LoadImageORVideo(meetingroom.WallWallpaper_MeetingRoom, instatiated.transform.Find(nameof(meetingroom.WallWallpaper_MeetingRoom)).gameObject);
                LoadImageORVideo(meetingroom.WallDisplay1_MeetingRoom, instatiated.transform.Find("TV_screen_1").gameObject);

                AgendaDetails agenda_MeetingRoom = new AgendaDetails();

                string meetingRoomName = "MeetingRoom " + (1 + meetingNumber);
                if (meetingroom.MeetingRoomName_MeetingRoom != null && !actionsData.ContainsKey(meetingroom.MeetingRoomName_MeetingRoom))
                {
                    meetingRoomName = meetingroom.MeetingRoomName_MeetingRoom;
                }
                instatiated.transform.GetComponentInChildren<TextMeshPro>().text = meetingRoomName;

                switch (meetingroom.MeetingRoomType_MeetingRoom)
                {
                    case "Type-A":

                        instatiated.transform.GetComponentInChildren<TextMeshPro>().text = meetingRoomName;

                        instatiated.transform.Find("Floor_Meeting_room1").name = "Floor_Meeting_room1" + meetingNumber.ToString();
                        agenda_MeetingRoom.AgendaSpeakers = meetingroom.AgendaSpeakers_MeetingRoom;
                        agenda_MeetingRoom.AgendaTitle = meetingroom.AgendaTitle_MeetingRoom;
                        agenda_MeetingRoom.ClientNames = "";
                        agenda_MeetingRoom.Details = meetingroom.AgendaDetails_MeetingRoom;


                        GlobalConstants.Agendas.Add("Floor_Meeting_room1" + meetingNumber.ToString(), agenda_MeetingRoom);

                        GameObject meetingRoomA = instatiated.transform.Find("Door_small").GetChild(0).gameObject;
                        meetingRoomA.name = "meetingRoom_" + meetingNumber.ToString();
                        meetingRoomA.AddComponent<Animator>().runtimeAnimatorController = meetingSingleDoor;
                        GlobalConstants.AppActions.Add("meetingRoom_" + meetingNumber.ToString(), () => meetingRoomA.GetComponent<Animator>().SetTrigger("open"));

                        //instatiated.transform.Find("Door_small").GetChild(0).gameObject.AddComponent<Animator>().runtimeAnimatorController = meetingSingleDoor;
                        actionsData.Add(meetingRoomName, () => playerCamera.transform.parent.GetComponent<CharacterMovement>().SetPoision(instatiated.transform.position + new Vector3(1, 1, -1.5f), new Vector3(0, 130, 0)));
                        GlobalConstants.AppActions.Add(meetingRoomName, () => playerCamera.transform.parent.GetComponent<CharacterMovement>().SetPoision(instatiated.transform.position + new Vector3(1, 1, -1.5f), new Vector3(0, 130, 0)));


                        break;
                    case "Type-B":

                        //instatiated.transform.GetComponentInChildren<TextMeshPro>().text = "MeetingRoom";

                        instatiated.transform.Find("Floor_Meeting_room2").name = "Floor_Meeting_room1" + meetingNumber.ToString();
                        agenda_MeetingRoom.AgendaSpeakers = meetingroom.AgendaSpeakers_MeetingRoom;
                        agenda_MeetingRoom.AgendaTitle = meetingroom.AgendaTitle_MeetingRoom;
                        agenda_MeetingRoom.ClientNames = "";
                        agenda_MeetingRoom.Details = meetingroom.AgendaDetails_MeetingRoom;


                        GlobalConstants.Agendas.Add("Floor_Meeting_room1" + meetingNumber.ToString(), agenda_MeetingRoom);

                        GameObject meetingRoomB = instatiated.transform.Find("DoorGlassDecor1").gameObject;
                        meetingRoomB.name = "meetingRoom_" + meetingNumber.ToString();
                        meetingRoomB.AddComponent<Animator>().runtimeAnimatorController = meetingDoubleDoor;
                        GlobalConstants.AppActions.Add("meetingRoom_" + meetingNumber.ToString(), () => meetingRoomB.GetComponent<Animator>().SetTrigger("open"));

                        //instatiated.transform.Find("DoorGlassDecor1").gameObject.AddComponent<Animator>().runtimeAnimatorController = meetingDoubleDoor;
                        actionsData.Add(meetingRoomName, () => playerCamera.transform.parent.GetComponent<CharacterMovement>().SetPoision(instatiated.transform.position + new Vector3(-3, 1, -11), new Vector3(0, 0, 0)));
                        GlobalConstants.AppActions.Add(meetingRoomName, () => playerCamera.transform.parent.GetComponent<CharacterMovement>().SetPoision(instatiated.transform.position + new Vector3(-3, 1, -11), new Vector3(0, 0, 0)));


                        break;
                    case "Type-C":
                        //instatiated.transform.GetComponentInChildren<TextMeshPro>().text = "MeetingRoom";

                        instatiated.transform.Find("Floor_Meeting_room3").name = "Floor_Meeting_room1" + meetingNumber.ToString();
                        agenda_MeetingRoom.AgendaSpeakers = meetingroom.AgendaSpeakers_MeetingRoom;
                        agenda_MeetingRoom.AgendaTitle = meetingroom.AgendaTitle_MeetingRoom;
                        agenda_MeetingRoom.ClientNames = "";
                        agenda_MeetingRoom.Details = meetingroom.AgendaDetails_MeetingRoom;


                        GlobalConstants.Agendas.Add("Floor_Meeting_room1" + meetingNumber.ToString(), agenda_MeetingRoom);

                        GameObject meetingRoomC = instatiated.transform.Find("Door_small").GetChild(0).gameObject;
                        meetingRoomC.name = "meetingRoom_" + meetingNumber.ToString();
                        meetingRoomC.AddComponent<Animator>().runtimeAnimatorController = meetingSingleDoor;
                        GlobalConstants.AppActions.Add("meetingRoom_" + meetingNumber.ToString(), () => meetingRoomC.GetComponent<Animator>().SetTrigger("open"));

                        // instatiated.transform.Find("Door_small").GetChild(0).gameObject.AddComponent<Animator>().runtimeAnimatorController = meetingSingleDoor;
                        actionsData.Add(meetingRoomName, () => playerCamera.transform.parent.GetComponent<CharacterMovement>().SetPoision(instatiated.transform.position + new Vector3(1, 1, -1.5f), new Vector3(0, 130, 0)));
                        GlobalConstants.AppActions.Add(meetingRoomName, () => playerCamera.transform.parent.GetComponent<CharacterMovement>().SetPoision(instatiated.transform.position + new Vector3(1, 1, -1.5f), new Vector3(0, 130, 0)));

                        break;

                }

                break;
            case Team team:


                //placing Cubicals
                if(team.AglieTeam_Team)
                    PlaceCubesBase(team.TeamSize_Team, agileCube, commonTV, (int)instatiated.transform.position.z);
                    else
                PlaceCubesBase(team.TeamSize_Team, commonCube, commonTV, (int)instatiated.transform.position.z);



                int number = odcData.Team_Area.users.IndexOf(team);
                string teamName = "Team " + 1 + number;
                if (team.TeamName_Team != null && !actionsData.ContainsKey(team.TeamName_Team))
                {
                    teamName = team.TeamName_Team;
                }

                LoadImageORVideo(team.WallWallpaper_Team, occupideObjectes[number].gameObject);
                LoadImageORVideo(team.WallDisplay1_Team, odcScreens[number].gameObject);

                if (number == odcData.Team_Area.users.Count - 1)
                {
                    //Removing for Temporarily
                    //LoadImageORVideo(teamArea.WallWallpaper_Team, occupideObjectes[odcData.Team_Area.Count].gameObject);

                }

                TextMeshPro[] nameboards = instatiated.transform.GetComponentsInChildren<TextMeshPro>();

                for (int j = 0; j < nameboards.Length; j++)
                {
                    nameboards[j].text = team.TeamName_Team;
                }


                actionsData.Add(teamName, () => playerCamera.transform.parent.GetComponent<CharacterMovement>().SetPoision(instatiated.transform.position + new Vector3(-7.5f, 1, -0.35f), new Vector3(0, 180, 0)));
                GlobalConstants.AppActions.Add(teamName, () => playerCamera.transform.parent.GetComponent<CharacterMovement>().SetPoision(instatiated.transform.position + new Vector3(-7.5f, 1, -0.35f), new Vector3(0, 180, 0)));

                AgendaDetails agenda_ODC = new AgendaDetails();

                string floorName = "";
                if (team.TeamSize_Team == "Large")
                {
                    floorName = "Floor_odc_high";
                }
                if (team.TeamSize_Team == "Medium")
                {
                    floorName = "Floor_odc_medium";
                }
                if (team.TeamSize_Team == "Small")
                {
                    floorName = "Floor_odc_small";
                }

                instatiated.transform.Find(floorName).name = "Floor_odc_" + number.ToString();
                agenda_ODC.AgendaSpeakers = team.AgendaSpeakers_Team;
                agenda_ODC.AgendaTitle = team.AgendaTitle_Team;
                agenda_ODC.ClientNames = "";
                agenda_ODC.Details = team.AgendaDetails_Team;

                GlobalConstants.Agendas.Add("Floor_odc_" + number.ToString(), agenda_ODC);


                break;

            case OpenSpacesCafe openSpacesCafe:

                TextMeshPro[] nameboardscafe = instatiated.transform.GetComponentsInChildren<TextMeshPro>();
                //if(openSpacesCafe.InputTitle_Cafe== string.Empty)
                //{
                //    openSpacesCafe.InputTitle_Cafe = "Cafe Area";
                //}
                for (int j = 0; j < nameboardscafe.Length; j++)
                {
                    nameboardscafe[j].text = "Cafe Area";//openSpacesCafe.InputTitle_Cafe;
                }

                //instatiated.transform.GetComponentInChildren<TextMeshPro>().text = openSpacesCafe.InputTitle_Cafe;

                LoadImageORVideo(openSpacesCafe.WallWallpaper_Cafe, instatiated.transform.Find(nameof(openSpacesCafe.WallWallpaper_Cafe)).gameObject);
                LoadImageORVideo(openSpacesCafe.WallDisplay1_Cafe, instatiated.transform.Find("TV_screen_2").gameObject);

                LoadImageORVideo(openSpacesCafe.WhiteBoard_Cafe, instatiated.transform.Find("Projecter_Screen").gameObject);


                actionsData.Add("Cafe", () => playerCamera.transform.parent.GetComponent<CharacterMovement>().SetPoision(instatiated.transform.position + new Vector3(0, 1, -4.5f), new Vector3(0, 90, 0)));
                GlobalConstants.AppActions.Add("Cafe", () => playerCamera.transform.parent.GetComponent<CharacterMovement>().SetPoision(instatiated.transform.position + new Vector3(0, 1, -4.5f), new Vector3(0, 90, 0)));


                AgendaDetails agenda_Cafe = new AgendaDetails();



                // instatiated.transform.Find("Floor_Cafe").name = "Floor_odc_" + number.ToString();
                agenda_Cafe.AgendaSpeakers = openSpacesCafe.AgendaSpeakers_Cafe;
                agenda_Cafe.AgendaTitle = openSpacesCafe.AgendaTitle_Cafe;
                agenda_Cafe.ClientNames = "";
                agenda_Cafe.Details = openSpacesCafe.AgendaDetails_Cafe;

                GlobalConstants.Agendas.Add("Floor_Cafe", agenda_Cafe);

                break;
            case TrophyRoom trophyRoom:

                instatiated.transform.GetComponentInChildren<TextMeshPro>().text = "TrophyRoom";

                LoadImageORVideo(trophyRoom.WallWallpaper_TrophyRoom, instatiated.transform.Find("Poster_B").gameObject);
                LoadImageORVideo(trophyRoom.WallDisplay1_TrophyRoom, GetChildWithname("TV_screen_", instatiated.transform.Find("TV_for_wall (1)")).gameObject);


                actionsData.Add("TrophyRoom", () => playerCamera.transform.parent.GetComponent<CharacterMovement>().SetPoision(instatiated.transform.position + new Vector3(-1, 1, -2), new Vector3(0, 120, 0)));
                GlobalConstants.AppActions.Add("TrophyRoom", () => playerCamera.transform.parent.GetComponent<CharacterMovement>().SetPoision(instatiated.transform.position + new Vector3(-1, 1, -2), new Vector3(0, 120, 0)));

                AgendaDetails agenda_Trophy = new AgendaDetails();



                // instatiated.transform.Find("Floor_Cafe").name = "Floor_odc_" + number.ToString();
                agenda_Trophy.AgendaSpeakers = trophyRoom.AgendaSpeakers_TrophyRoom;
                agenda_Trophy.AgendaTitle = trophyRoom.AgendaTitle_TrophyRoom;
                agenda_Trophy.ClientNames = "";
                agenda_Trophy.Details = trophyRoom.AgendaDetails_TrophyRoom;

                GlobalConstants.Agendas.Add("Floor_Trophy_room", agenda_Trophy);
                break;
        }

        //}

    }


    (string, string) GetImageName(string url)
    {
        string _name = url.Substring(url.LastIndexOf('/') + 1);
        string extention = _name.Substring(_name.LastIndexOf('.') + 1);
        _name = _name.Substring(0, _name.IndexOf(".") + 0);

        return (_name, extention);
    }

    public void AddCameraActions(string name, Vector3 pos, Vector3 rot)
    {
        actionsData.Add(name, () => playerCamera.transform.parent.GetComponent<CharacterMovement>().SetPoision(pos, rot));
        GlobalConstants.AppActions.Add(name, () => playerCamera.transform.parent.GetComponent<CharacterMovement>().SetPoision(pos, rot));


    }
    IEnumerator LoadVideo(string url, VideoPlayer myVideoPlayer)
    {

        myVideoPlayer.playOnAwake = false;
        myVideoPlayer.source = UnityEngine.Video.VideoSource.Url;
        myVideoPlayer.aspectRatio = VideoAspectRatio.FitInside;
        myVideoPlayer.url = url;

        yield return 0;

    }

    private void OnDestroy()
    {
        BundleDownloader.AllImagesFinished -= BundleDownloader_AllImagesFinished;
        BundleDownloader.AllBundleDownloadFinished -= BundleDownloader_AllBundleDownloadFinished;
        InputManager.CUSTOMERINPUT -= InpuManager_CUSTOMERINPUT;
        // VirtualwalkThrough.UpdateFeedBack -= VirtualwalkThrough_UpdateFeedBack;




    }


    void AddImagePath(string currentpath)
    {
        if (currentpath != null && currentpath != "" && !currentpath.Contains(".mp4"))
        {
            if (!imagepaths.Contains(currentpath))
            {
                imagepaths.Add(currentpath);
            }
        }
        else if (currentpath != null && currentpath != "" && currentpath.Contains(".mp4"))
        {
            videoLinks.Add(currentpath);

        }
    }

    public void LoadVideos(List<WallDisplayStagingArea> url, GameObject instatiated)
    {
        if (url != null)
        {
            //if (url.Count > 1)
            //{
            GameObject screen = GetChildWithname("TV_screen_", instatiated.transform.Find("Stand_TV")).gameObject;
            GameObject container1 = Instantiate(videocontroller, screen.transform);
            container1.name = "VideoController" + videoControllerCount;
            videoControllerCount++;
            GameObject container = Instantiate(videocontent, screen.transform);

            container1.GetComponent<Canvas>().worldCamera = playerCamera;
            container.GetComponent<Canvas>().worldCamera = playerCamera;
            container1.SetActive(false);

            for (int i = 0; i < url.Count; i++)
            {
                GameObject presenter = container.transform.GetChild(0).GetChild(i).gameObject;
                presenter.SetActive(true);
                //Debug.Log(presenter.name);
                //Debug.Log(url[i].InputImage_StagingArea);
                if (url[i].InputImage_StagingArea != null && bundleDownloader.GetImage(url[i].InputImage_StagingArea) != null)
                    presenter.transform.GetChild(0).GetComponent<RawImage>().texture = bundleDownloader.GetImage(url[i].InputImage_StagingArea);
                presenter.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = url[i].InputTitle_StagingArea;

                if (url[i].WallDisplay_StagingArea.Contains(".mp4"))
                {
                    VideoPlayer currentPlayer = screen.AddComponent<VideoPlayer>();
                    StartCoroutine(LoadVideo(url[i].WallDisplay_StagingArea, currentPlayer));
                    //  presenter.GetComponent<VideoPlayer>().targetMaterialRenderer = screen.GetComponent<MeshRenderer>();
                    GlobalConstants.AppActions.Add(("stagingArea_Video" + i).ToString(), () =>


                          {
                              Debug.Log("Calling Action");

                              currentPlayer.Play();
                              container1.SetActive(true);
                              container1.GetComponent<VideoController1>().Setpurpose(currentPlayer);
                              container.SetActive(false);
                          }
                            );
                    string action = "stagingArea_Video" + i.ToString();
                    presenter.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                    {
                        VirtualwalkThrough.FireAppAction(action);
                        Debug.Log("ButtonClicked");

                        for (int j = 0; j < GlobalConstants.AppActions.Count; j++)
                        {
                            Debug.Log(GlobalConstants.AppActions.ElementAt(j).Key);
                        }
                    });
                }
                else
                {
                    //        presenter.GetComponent<MeshRenderer>().material.mainTexture =
                    //bundleDownloader.GetImage(url[i].WallDisplay_StagingArea);

                    if (url.Count > 1)
                    {
                        if (url[i].WallDisplay_StagingArea != null && bundleDownloader.GetImage(url[i].WallDisplay_StagingArea) != null)
                        {
                            Texture2D texture2D = bundleDownloader.GetImage(url[i].WallDisplay_StagingArea);
                            GlobalConstants.AppActions.Add("stagingArea_Image" + i.ToString(), () =>

                            {
                                screen.GetComponent<MeshRenderer>().material.mainTexture = texture2D;
                                container1.SetActive(true);

                                container1.GetComponent<VideoController1>().Setpurpose(texture2D);

                                container.SetActive(false);
                                Debug.Log("Clicked");
                            }
                            );

                            presenter.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(
                            () => VirtualwalkThrough.FireAppAction("stagingArea_Image" + i));

                        }
                        else
                        {
                            // Debug.Log("it is null");
                        }
                    }
                    else
                    {
                        container.SetActive(false);

                        LoadImageORVideo(url[0].WallDisplay_StagingArea, GetChildWithname("TV_screen_", instatiated.transform.Find("Stand_TV")).gameObject);

                    }
                }
            }

            //}
            //else
            //{
            //    LoadImageORVideo(url[0].WallDisplay_StagingArea, GetChildWithname("TV_screen_", instatiated.transform.Find("Stand_TV")).gameObject);

            //}

        }
    }
    public void LoadImageORVideo(string url, GameObject placeholder)
    {
        if (url != null && url != "")
        {
            if (url.Contains(".mp4"))
            {
                StartCoroutine(LoadVideo(url, placeholder.AddComponent<VideoPlayer>()));
                GameObject container1 = Instantiate(videocontroller, placeholder.transform);
                container1.name = "VideoController" + videoControllerCount;
                container1.SetActive(true);
                videoControllerCount++;
                container1.GetComponent<Canvas>().worldCamera = playerCamera;
                container1.GetComponent<VideoController1>().Setpurpose(placeholder.GetComponent<VideoPlayer>());


            }
            else
            {
                placeholder.GetComponent<MeshRenderer>().material.mainTexture =
        bundleDownloader.GetImage(url);
            }



        }
    }

    //private void VirtualwalkThrough_UpdateFeedBack(List<Feedback> feedbacks)
    //{
    //    string jsonStringTrial = JsonConvert.SerializeObject(feedbacks);

    //    StartCoroutine(SendFeedBack(jsonStringTrial));

    //}

    IEnumerator SendFeedBack(string jsonStringTrial)
    {
        Debug.Log("Uploading");



        byte[] bytes = Encoding.UTF8.GetBytes(jsonStringTrial);

        using (UnityWebRequest www = UnityWebRequest.Put(Utility.GetMediaPath() + "FeedBack", bytes))
        {
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.Send();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }


            //        UnityWebRequest www = UnityWebRequest.Put(Utility.GetMediaPath()+"FeedBack.json" , jsonStringTrial);
            //www.SetRequestHeader("Content-Type", "application/json");
            //yield return www.Send();

            //if(www.isNetworkError|| www.isHttpError)
            //{
            //    Debug.Log(www.error);
            //}else
            //{
            //    Debug.Log(www.downloadHandler.text);
        }
    }

    Transform GetChildWithname(string _name, Transform parent)
    {
        foreach (Transform eachChild in parent)
        {

            if (eachChild.name.Contains(_name))
            {
                return eachChild;
            }
            if (eachChild.childCount > 0)
            {
                return GetChildWithname(_name, eachChild);
            }
        }

        return null;
    }
}




