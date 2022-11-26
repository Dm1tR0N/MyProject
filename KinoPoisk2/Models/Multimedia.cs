using System;
using System.Drawing;

namespace KinoPoisk2.Models;

public class Multimedia
{
    public Multimedia(string? type, string? src, int width, int height, Image image)
    {
        this.type = type;
        this.src = src;
        this.height = height;
        this.width = width;
        this.image = image;
    }
    public string? type { get; set; }
    public string? src { get; set; }
    public int height { get; set; }
    public int width { get; set; }
    
    public Image image { get; set; }
}