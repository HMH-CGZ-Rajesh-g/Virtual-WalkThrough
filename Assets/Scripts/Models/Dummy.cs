/*
    Author: BhomitB, 18July2020
    Revision:
    For tutorial purpose only rmeove in production.    
 */
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
using System.Collections.Generic;

public class Hit
{
    public int id { get; set; }
    public string pageURL { get; set; }
    public string type { get; set; }
    public string tags { get; set; }
    public string previewURL { get; set; }
    public int previewWidth { get; set; }
    public int previewHeight { get; set; }
    public string webformatURL { get; set; }
    public int webformatWidth { get; set; }
    public int webformatHeight { get; set; }
    public string largeImageURL { get; set; }
    public int imageWidth { get; set; }
    public int imageHeight { get; set; }
    public int imageSize { get; set; }
    public int views { get; set; }
    public int downloads { get; set; }
    public int favorites { get; set; }
    public int likes { get; set; }
    public int comments { get; set; }
    public int user_id { get; set; }
    public string user { get; set; }
    public string userImageURL { get; set; }

}

public class Pixabay
{
    public int total { get; set; }
    public int totalHits { get; set; }
    public List<Hit> hits { get; set; }

}

