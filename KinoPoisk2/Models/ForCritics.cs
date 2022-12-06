namespace KinoPoisk2.Models;

public class RootobjectCritic
{
    public string status { get; set; }
    public string copyright { get; set; }
    public int num_results { get; set; }
    public ResultCritic[] results { get; set; }
}
public class ResultCritic
{
    public ResultCritic(string? display_name, string? sort_name, string? status, string? bio)
    {
        this.display_name = display_name;
        this.sort_name = sort_name;
        this.status = status;
        this.bio = bio;
    }
    public string display_name { get; set; }
    public string sort_name { get; set; }
    public string status { get; set; }
    public string bio { get; set; }
    public string seo_name { get; set; }
    public MultimediaCritic multimedia { get; set; }
}

public class ResourceCritic
{
    public string type { get; set; }
    public string src { get; set; }
    public int height { get; set; }
    public int width { get; set; }
    public string credit { get; set; }
}

public class MultimediaCritic
{
    public ResourceCritic resource { get; set; }
}
