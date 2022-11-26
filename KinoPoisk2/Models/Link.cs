using System.Collections.Generic;

namespace KinoPoisk2.Models;

public class Link
{
    public Link(string? type, string? url, string? suggested_link_text)
    {
        this.type = type;
        this.url = url;
        this.suggested_link_text = suggested_link_text;
    }
    public string? type { get; set; }
    public string? url { get; set; }
    public string? suggested_link_text { get; set; }
}