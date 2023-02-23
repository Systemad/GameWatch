namespace GameWatch.Features.IGameDatabase.Helpers;

public static class PlatformHelper
{
    public static string GetPlatformFullname(this int platformInt)
    {
        return platformInt switch
        {
            48 => "Playstation 4",
            1 => "console",
            3 => "xbox",
            _ => "Unknown"
        };
    }

    public static int PlatformFullnameToCode(this string platformName)
    {
        return platformName switch
        {
            "Playstation 4" => 48,
            "PC(Windows)" => 6,
            "PS2" => 8,
            _ => 0
        };
    }
}
