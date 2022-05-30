﻿using System.Drawing;

namespace FireCloud.WebClient.PrimeService.Service.Helper;

public class AppSettings
{
    public App App { get; set; }
    /// <summary>
    /// Application Version to be displayed in the Main title.
    /// </summary>
    public string? Version { get; set; }
    public string? Build { get; set; }
    public bool IsDev { get; set; }
   
}


public class App
{
    public string AuthUrl { get; set; }
    public string GoogleAuth { get; set; }
}