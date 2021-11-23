using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cdb.virtualwalkthrough;
public static class Utility 
{
    /// <summary>
    /// GetCubical positions
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="width"></param>
    /// <param name="length"></param>
    /// <param name="numberofsteps"></param>
    /// <returns></returns>
    public static List<Vector3>  GetCubicalPositions(Vector3 pos, float width,float length, int numberofsteps)
    {

        List<Vector3> odcPosition= new List<Vector3>();
        Vector3 refPos = pos;
        for (int i = 0; i < numberofsteps; i++)
        {
            refPos = pos + Vector3.back * i*length;
            odcPosition.Add(refPos);
            odcPosition.Add(refPos + width * Vector3.left);
            odcPosition.Add(refPos + width * Vector3.right);
        }



        return  odcPosition;
    }


    public static string GetMediaPath()
    {
#if UNITY_EDITOR
       // return GlobalConstants.LOCAL_MEDIA_SERVER_URL + "Webgl/";
       return GlobalConstants.LOCAL_MEDIA_SERVER_URL+"/OSX/";
#elif UNITY_WEBGL
        return GlobalConstants.MEDIA_SERVER_URL+"Webgl/";
#endif
        return GlobalConstants.LOCAL_MEDIA_SERVER_URL + "/OSX/";
    }

    public static string GetStreaminAssetsPath()
    {
#if UNITY_EDITOR
        //return GlobalConstants.MEDIA_SERVER_URL + "Webgl/";
        return GlobalConstants.LOCAL_MEDIA_SERVER_URL + "/StreamingAssets/";
#elif UNITY_WEBGL
        return GlobalConstants.MEDIA_SERVER_URL+"StreamingAssets/";
#endif
        return GlobalConstants.LOCAL_MEDIA_SERVER_URL + "/StreamingAssets/";

    }
}


public struct Details
{
    public Dictionary<string, object> details { get; set; }

}

