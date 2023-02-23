namespace GameWatch.Features.IGameDatabase.Helpers;

public static class CategoryHelper
{
    public static string GetCategoryFullname(this int categoryInt)
    {
        return categoryInt switch
        {
            0 => "main_game",
            1 => "dlc_addon",
            2 => "expansion",
            3 => "bundle",
            4 => "standalone_expansion",
            5 => "mod",
            6 => "episode",
            7 => "season",
            8 => "remake",
            9 => "remaster",
            10 => "expanded_game",
            11 => "port",
            12 => "fork",
            13 => "pack",
            14 => "update",
            _ => "main_game"
        };
    }

    public static int CategoryFullnameToCodeInt(this string categoryFullname)
    {
        return categoryFullname switch
        {
            "main_game" => 0,
            "dlc_addon" => 1,
            "expansion" => 2,
            "bundle" => 3,
            "standalone_expansion" => 4,
            "mod" => 5,
            "episode" => 6,
            "season" => 7,
            "remake" => 8,
            "remaster" => 9,
            "expanded_game" => 10,
            "port" => 11,
            "fork" => 12,
            "pack" => 13,
            "update" => 14,
            _ => 0
        };
    }
}
