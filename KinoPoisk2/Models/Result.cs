
using System;
using System.ComponentModel;
using Avalonia.Controls;
using Newtonsoft.Json;
using Image = System.Drawing.Image;

namespace KinoPoisk2.Models;

public class Result
{
    public Result(string? itemTitleFilm, string? itemDopTitle, string? itemDiscriptionFilm, string? itemAuthor, string itemRatingFilm, string? itemDatePublic, string? itemDateOut, string? itemDateUpdatePost, Multimedia itemMultimedia, Link itemLink)
    {
        this.TitleFilm = itemTitleFilm;
        this.DopTitle = itemDopTitle;
        this.DiscriptionFilm = itemDiscriptionFilm;
        this.Author = itemAuthor;
        this.RatingFilm = itemRatingFilm;
        this.DatePublic = itemDatePublic;
        this.DateOut = itemDateOut;
        this.DateUpdatePost = itemDateUpdatePost;
        this.multimedia = itemMultimedia;
        this.link = itemLink;
    }

    [JsonProperty("display_title")]
    public string? TitleFilm { get; set; }
    [JsonProperty("mpaa_rating")]
    public string? RatingFilm { get; set; }
    [JsonProperty("critics_pick")]
    public int? Pick { get; set; }
    [JsonProperty("byline")]
    public string? Author { get; set; }
    [JsonProperty("headline")]
    public string? DopTitle { get; set; }
    [JsonProperty("summary_short")]
    public string? DiscriptionFilm { get; set; }
    [JsonProperty("publication_date")]
    public string? DatePublic { get; set; }
    [JsonProperty("opening_date")]
    public string? DateOut { get; set; }
    [JsonProperty("date_updated")]
    public string? DateUpdatePost { get; set; }
    public Link? link { get; set; }
    public Multimedia? multimedia { get; set; }
}
