﻿using System;
using System.Collections.Generic;

namespace dirtbike.api.Models;

public partial class Park
{
    public int ParkId { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Region { get; set; }

    public double? TrailLengthMiles { get; set; }

    public string? Difficulty { get; set; }

    public string? Description { get; set; }

    public double? DayPassPriceUsd { get; set; }

    public decimal? Longitude { get; set; }

    public decimal? Latitude { get; set; }

    public string? Trailmapurl { get; set; }

    public string? Parklogourl { get; set; }

    public int? Maxvisitors { get; set; }

    public int? Currentvisitors { get; set; }

    public int? Currentvisitorschildren { get; set; }

    public int? Currentvisitorsadults { get; set; }

    public int? Maxcampsites { get; set; }

    public int? Columns { get; set; }

    public string? State { get; set; }

    public string? Pic1url { get; set; }

    public string? Pic2url { get; set; }

    public string? Pic3url { get; set; }

    public string? Pic4url { get; set; }

    public string? Pic5url { get; set; }

    public string? Pic6url { get; set; }

    public string? Pic7url { get; set; }

    public string? Pic8url { get; set; }

    public string? Pic9url { get; set; }

    public string? Isnationalpark { get; set; }

    public string? Isstatepark { get; set; }

    public string? Hqbranchid { get; set; }

    public int? Mountainbikes { get; set; }

    public int? Camping { get; set; }

    public int? Rafting { get; set; }

    public int? Canoeing { get; set; }

    public int? Frisbee { get; set; }

    public int? Iscanadian { get; set; }

    public int? Ismexican { get; set; }
}
