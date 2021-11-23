// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
using System;
using System.Collections.Generic;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
public class MeetingRoom
{
    public string WallDisplay1_MeetingRoom { get; set; }
    public string WallWallpaper_MeetingRoom { get; set; }
    public string MeetingRoomName_MeetingRoom { get; set; }
    public string InputImage_MeetingRoom { get; set; }
    public string AgendaSpeakers_MeetingRoom { get; set; }
    public string MeetingRoomType_MeetingRoom { get; set; }
    public string AgendaTitle_MeetingRoom { get; set; }
    public string AgendaDetails_MeetingRoom { get; set; }
    public string MeetingRoomNumber_MeetingRoom { get; set; }
}

public class Team
{
    public string AgendaSpeakers_Team { get; set; }
    public string TeamName_Team { get; set; }
    public string WallWallpaper_Team { get; set; }
    public bool AglieTeam_Team { get; set; }
    public string AgendaTitle_Team { get; set; }
    public string AgendaDetails_Team { get; set; }
    public string InputImage_Team { get; set; }
    public string TeamSize_Team { get; set; }
    public string WallDisplay1_Team { get; set; }
}

public class TeamArea
{
    public string TeamBackWallpaper { get; set; }
    public List<Team> users { get; set; }
}

public class ManagerCabin
{
    public string AgendaSpeakers_ManagerCabin { get; set; }
    public string InputImage_ManagerCabin { get; set; }
    public string WallWallpaper_ManagerCabin { get; set; }
    public string AgendaDetails_ManagerCabin { get; set; }
    public string WallDisplay1_ManagerCabin { get; set; }
    public string AgendaTitle_ManagerCabin { get; set; }
}

public class OpenSpacesCafe
{
    public string AgendaTitle_Cafe { get; set; }
    public string AgendaDetails_Cafe { get; set; }
    public string InputImage_Cafe { get; set; }
    public string WallDisplay1_Cafe { get; set; }
    public string AgendaSpeakers_Cafe { get; set; }
    public string WhiteBoard_Cafe { get; set; }
    public string WallWallpaper_Cafe { get; set; }
}

public class TrophyDetail
{
    public string trophyNumber { get; set; }
    public string trophyName { get; set; }
    public string trophyDetails { get; set; }
    public string TrophyImage_TrophyRoom { get; set; }
}

public class TrophyRoom
{
    public List<TrophyDetail> trophyDetails { get; set; }
    public string AgendaDetails_TrophyRoom { get; set; }
    public string AgendaTitle_TrophyRoom { get; set; }
    public string AgendaSpeakers_TrophyRoom { get; set; }
    public string WallDisplay1_TrophyRoom { get; set; }
    public string WallWallpaper_TrophyRoom { get; set; }
}

public class WallDisplayStagingArea
{
    public string InputImage_StagingArea { get; set; }
    public string WallDisplay_StagingArea { get; set; }
    public string InputTitle_StagingArea { get; set; }
}

public class StagingArea
{
    public string WallLogo_StagingArea { get; set; }
    public List<WallDisplayStagingArea> WallDisplay_StagingArea { get; set; }
    public string Display2_StagingArea { get; set; }
    public string AgendaSpeakers_StagingArea { get; set; }
    public string AgendaTitle_StagingArea { get; set; }
    public string AgendaDetails_StagingArea { get; set; }
    public string ClientOrganisation_StagingArea { get; set; }
    public string ClientNames_StagingArea { get; set; }
    public string WallWallpaper_StagingArea { get; set; }
    public bool helpDesk { get; set; }
}

public class ODCData
{
    public string customerName { get; set; }
    public List<MeetingRoom> Meeting_Room { get; set; }
    public TeamArea Team_Area { get; set; }
    public DateTime updatedAt { get; set; }
    public ManagerCabin Manager_Cabin { get; set; }
    public DateTime createdAt { get; set; }
    public OpenSpacesCafe OpenSpaces_Cafe { get; set; }
    public List<string> emailPOC { get; set; }
    public string id { get; set; }
    public TrophyRoom Trophy_Room { get; set; }
    public StagingArea Staging_Area { get; set; }
    public string customerID { get; set; }
}

public class ODCRoot
{
    public List<ODCData> Items { get; set; }
    public int Count { get; set; }
    public int ScannedCount { get; set; }
}







// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
public class Asset
{
    public string name { get; set; }
    public string direction { get; set; }
    public int length { get; set; }
}

public class AssetData
{
    public List<Asset> asset { get; set; }
}

