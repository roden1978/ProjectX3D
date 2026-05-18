using System;

public static class Settings
{
    public const int ScreenWidth = 1980;
    public const int ScreenHeight = 1200;
    public const string GameName = "PX3D";
    public static string PlayerName => Environment.UserName;
    public const int LeaderboardCapacity = 5;
}