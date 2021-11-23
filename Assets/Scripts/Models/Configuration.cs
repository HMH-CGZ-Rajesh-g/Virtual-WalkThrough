using System.Collections.Generic;

public class MainProjectionArea
{
    public string mediaType { get; set; }
    public string mediaTitle { get; set; }
    public string mediaSource { get; set; }
}

public class SolutionObject
{
    public string mediaType { get; set; }
    public string mediaTitle { get; set; }
    public string mediaSource { get; set; }
    public string description { get; set; }
    public string thumbnail { get; set; }
    public bool isEnabled { get; set; }
}

public class Configuration
{
    public int meetingID { get; set; }
    public string feedbackBucket { get; set; }
    public List<string> clients { get; set; }
    public string date { get; set; }
    public string company { get; set; }
    public string companyLogo { get; set; }
    public List<MainProjectionArea> mainProjectionArea { get; set; }
    public List<MainProjectionArea> prototypeLab { get; set; }
    public List<MainProjectionArea> mindSpace { get; set; }
    public List<SolutionObject> cafe { get; set; }
    public List<SolutionObject> autoGarage { get; set; }
    public List<SolutionObject> agentsOffice { get; set; }
    public List<SolutionObject> cxoCabin { get; set; }
    public List<SolutionObject> warehouse { get; set; }
    public List<SolutionObject> livingRoom { get; set; }
}

