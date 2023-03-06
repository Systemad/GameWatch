using System.Text.Json.Serialization;

namespace GameWatch.Features.IGameDatabase.Models;

public partial class Game
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("age_ratings")]
    public List<AgeRating>? AgeRatings { get; set; }

    [JsonPropertyName("artworks")]
    public List<Cover>? Artworks { get; set; }

    [JsonPropertyName("category")]
    public GameCategory? Category { get; set; }

    [JsonPropertyName("cover")]
    public Cover? Cover { get; set; }

    [JsonPropertyName("first_release_date")]
    public long FirstReleaseDate { get; set; }

    [JsonPropertyName("game_engines")]
    public List<GameEngine>? GameEngines { get; set; }

    [JsonPropertyName("game_modes")]
    public List<GameMode>? GameModes { get; set; }

    [JsonPropertyName("genres")]
    public List<Genre>? Genres { get; set; }

    [JsonPropertyName("involved_companies")]
    public List<InvolvedCompany>? InvolvedCompanies { get; set; }

    [JsonPropertyName("keywords")]
    public List<Keyword>? Keywords { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("platforms")]
    public List<Platform>? Platforms { get; set; }

    [JsonPropertyName("player_perspectives")]
    public List<PlayerPerspective>? PlayerPerspectives { get; set; }

    [JsonPropertyName("rating")]
    public double Rating { get; set; }

    [JsonPropertyName("release_dates")]
    public List<ReleaseDate>? ReleaseDates { get; set; }

    [JsonPropertyName("screenshots")]
    public List<Screenshot>? Screenshots { get; set; }

    [JsonPropertyName("similar_games")]
    public List<SimilarGame>? SimilarGames { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("storyline")]
    public string? Storyline { get; set; }

    [JsonPropertyName("summary")]
    public string? Summary { get; set; }

    [JsonPropertyName("themes")]
    public List<Theme>? Themes { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("websites")]
    public List<Website>? Websites { get; set; }
}

public partial class AgeRating
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("category")]
    public AgeRatingCategory Category { get; set; }

    [JsonPropertyName("rating")]
    public long Rating { get; set; }
}

public partial class Cover
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("image_id")]
    public string? ImageId { get; set; }
}

public partial class Artwork
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("image_id")]
    public string? ImageId { get; set; }
}

public partial class Screenshot
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("image_id")]
    public string? ImageId { get; set; }
}

public partial class PlayerPerspective
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public partial class GameEngine
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public partial class Company
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public partial class Theme
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public partial class Platform
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public partial class Keyword
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public partial class Genre
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public partial class GameMode
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public partial class InvolvedCompany
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("company")]
    public Company? Company { get; set; }

    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    [JsonPropertyName("developer")]
    public bool Developer { get; set; }

    [JsonPropertyName("game")]
    public long Game { get; set; }

    [JsonPropertyName("porting")]
    public bool Porting { get; set; }

    [JsonPropertyName("publisher")]
    public bool Publisher { get; set; }

    [JsonPropertyName("supporting")]
    public bool Supporting { get; set; }

    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; set; }

    [JsonPropertyName("checksum")]
    public Guid Checksum { get; set; }
}

public partial class ReleaseDate
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("category")]
    public ReleaseDateCategory Category { get; set; }

    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    [JsonPropertyName("date")]
    public long Date { get; set; }

    [JsonPropertyName("game")]
    public long Game { get; set; }

    [JsonPropertyName("human")]
    public string? Human { get; set; }

    [JsonPropertyName("m")]
    public long M { get; set; }

    [JsonPropertyName("platform")]
    public Platform? Platform { get; set; }

    [JsonPropertyName("region")]
    public ReleaseDateRegion Region { get; set; }

    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; set; }

    [JsonPropertyName("y")]
    public long Y { get; set; }

    [JsonPropertyName("checksum")]
    public Guid Checksum { get; set; }
}

public partial class SimilarGame
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("cover")]
    public Cover? Cover { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public partial class Website
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("category")]
    public WebsiteCategory Category { get; set; }

    [JsonPropertyName("game")]
    public long Game { get; set; }

    [JsonPropertyName("trusted")]
    public bool Trusted { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("checksum")]
    public Guid Checksum { get; set; }
}

public enum GameStatus
{
    Released = 0,
    Alpha = 2,
    Beta = 3,
    EarlyAccess = 4,
    Offline = 5,
    Cancelled = 6,
    Rumored = 7,
    Delisted = 8,
}

public enum GameCategory
{
    MainGame = 0,
    DlcAddon = 1,
    Expansion = 2,
    Bundle = 3,
    StandaloneExpansion = 4,
    Mod = 5,
    Episode = 6,
    Season = 7,
    Remake = 8,
    Remaster = 9,
    ExpandedGame = 10,
    Port = 11,
    Fork = 12,
    Pack = 13,
    Update = 14,
}

public enum AgeRatingCategory
{
    ESRB = 1,
    PEGI = 2,
    CERO = 3,
    USK = 4,
    GRAC = 5,
    CLASS_IND = 6,
    ACB = 7
}

public enum AgeRatingTitle
{
    Three = 1,
    Seven = 2,
    Twelve = 3,
    Sixteen = 4,
    Eighteen = 5,
    RP = 6,
    EC = 7,
    E = 8,
    E10 = 9,
    T = 10,
    M = 11,
    AO = 12,
    CERO_A = 13,
    CERO_B = 14,
    CERO_C = 15,
    CERO_D = 16,
    CERO_Z = 17,
    USK_0 = 18,
    USK_6 = 19,
    USK_12 = 20,
    USK_18 = 21,
    GRAC_All = 22,
    GRAC_Twelve = 23,
    GRAC_Fifteen = 24,
    GRAC_Eighteen = 25,
    GRAC_Testing = 26,
    CLASS_IND_L = 27,
    CLASS_IND_Ten = 28,
    CLASS_IND_Twelve = 29,
    CLASS_IND_Fourteen = 30,
    CLASS_IND_Sixteen = 31,
    CLASS_IND_Eighteen = 32,
    ACB_G = 33,
    ACB_PG = 34,
    ACB_M = 35,
    ACB_MA15 = 36,
    ACB_R18 = 37,
    ACB_RC = 38,
}

public enum ReleaseDateCategory
{
    YYYYMMMMDD = 0,
    YYYYMMMM = 1,
    YYYY = 2,
    YYYYQ1 = 3,
    YYYYQ2 = 4,
    YYYYQ3 = 5,
    YYYYQ4 = 6,
    TBD = 7
}

public enum ReleaseDateRegion
{
    Europe = 1,
    NorthAmerica = 2,
    Australia = 3,
    NewZealand = 4,
    Japan = 5,
    China = 6,
    Asia = 7,
    Worldwide = 8,
    Korea = 9,
    Brazil = 10
}

public enum WebsiteCategory
{
    Official = 1,
    Wikia = 2,
    Wikipedia = 3,
    Facebook = 4,
    Twitter = 5,
    Twitch = 6,
    Instagram = 8,
    YouTube = 9,
    iPhone = 10,
    iPad = 11,
    Android = 12,
    Steam = 13,
    Reddit = 14,
    Itch = 15,
    EpicGames = 16,
    GOG = 17,
    Discord = 18
}

public class CountResponse
{
    [JsonPropertyName("count")]
    public long Count { get; set; }
}
