// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
using System.Collections.Generic;

public class BuildingScript
{
    public string name { get; set; }
    public List<int> pos  { get; set; }
    public List<int> rotation { get; set; }
}

public class Demo
{
    public List<BuildingScript> objecsts  { get; set; }
}