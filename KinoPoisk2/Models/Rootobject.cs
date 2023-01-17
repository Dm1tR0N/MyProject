using System.Collections.Generic;

namespace KinoPoisk2.Models;

public class Rootobject
 {
     public string? status { get; set; }
     public string? copyright { get; set; }
     public bool? has_more { get; set; }
     public int? num_results { get; set; }
     public Result[]? results { get; set; }
 }