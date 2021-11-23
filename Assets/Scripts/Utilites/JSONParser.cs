using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONParser : MonoBehaviour
{
    void ReadJSON(string json)
    {
        // MiniJSON read data
        // .Net JSON
        Data data = new Data();
        /*MiniJSON*/ 

         // data["ProjectionArea area"][0]["mediasource"]
        // data.projectionArea[0].mediaSource [""] 

    }
    /*
    "{
    "name" : "Manasa"
}"
    */

}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
public class EntryDoor
{
    public string mediaType { get; set; }
    public string mediaTitle { get; set; }
    public string mediaSource { get; set; }

}

public class SeatingArea
{
    public string mediaType { get; set; }
    public string mediaTitle { get; set; }
    public string mediaSource { get; set; }
    public string thumbnail { get; set; }

}

public class ProjectionArea
{
    public string mediaType { get; set; }
    public string mediaTitle { get; set; }
    public string mediaSource { get; set; }
    public string thumbnail { get; set; }

}

public class Data
{
    public int meetingID { get; set; }
    public List<string> clients { get; set; }
    public string company { get; set; }
    public string companyLogo { get; set; }
    public List<EntryDoor> entryDoor { get; set; }
    public List<SeatingArea> seatingArea { get; set; }
    public List<ProjectionArea> projectionArea { get; set; }

}
