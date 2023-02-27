using System.Text.Json.Serialization;

namespace GameWatch.Features.IGameDatabase;

public class Game
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("age_ratings")]
    public long[] AgeRatings { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("aggregated_rating")]
    public double? AggregatedRating { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("aggregated_rating_count")]
    public long? AggregatedRatingCount { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("alternative_names")]
    public long[] AlternativeNames { get; set; }

    [JsonPropertyName("artworks")]
    public long[] Artworks { get; set; }

    [JsonPropertyName("category")]
    public long Category { get; set; }

    [JsonPropertyName("cover")]
    public long Cover { get; set; }

    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    [JsonPropertyName("external_games")]
    public long[] ExternalGames { get; set; }

    [JsonPropertyName("first_release_date")]
    public long FirstReleaseDate { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("follows")]
    public long? Follows { get; set; }

    [JsonPropertyName("game_modes")]
    public long[] GameModes { get; set; }

    [JsonPropertyName("genres")]
    public long[] Genres { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("hypes")]
    public long? Hypes { get; set; }

    [JsonPropertyName("involved_companies")]
    public long[] InvolvedCompanies { get; set; }

    [JsonPropertyName("keywords")]
    public long[] Keywords { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("platforms")]
    public long[] Platforms { get; set; }

    [JsonPropertyName("player_perspectives")]
    public long[] PlayerPerspectives { get; set; }

    [JsonPropertyName("release_dates")]
    public long[] ReleaseDateReleaseDates { get; set; }

    [JsonPropertyName("screenshots")]
    public long[] Screenshots { get; set; }

    [JsonPropertyName("similar_games")]
    public long[] SimilarGames { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("storyline")]
    public string Storyline { get; set; }

    [JsonPropertyName("summary")]
    public string Summary { get; set; }

    [JsonPropertyName("tags")]
    public long[] Tags { get; set; }

    [JsonPropertyName("themes")]
    public long[] Themes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("total_rating")]
    public double? TotalRating { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("total_rating_count")]
    public long? TotalRatingCount { get; set; }

    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; set; }

    [JsonPropertyName("url")]
    public Uri Url { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("videos")]
    public long[] Videos { get; set; }

    [JsonPropertyName("websites")]
    public long[] Websites { get; set; }

    [JsonPropertyName("checksum")]
    public Guid Checksum { get; set; }

    [JsonPropertyName("language_supports")]
    public long[] LanguageSupports { get; set; }

    [JsonPropertyName("game_localizations")]
    public long[] GameLocalizations { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("parent_game")]
    public long? ParentGame { get; set; }
}

public class ReleaseDates
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("category")]
    public long Category { get; set; }

    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    [JsonPropertyName("date")]
    public long Date { get; set; }

    [JsonPropertyName("game")]
    public long Game { get; set; }

    [JsonPropertyName("human")]
    public string Human { get; set; }

    [JsonPropertyName("m")]
    public long M { get; set; }

    [JsonPropertyName("platform")]
    public long Platform { get; set; }

    [JsonPropertyName("region")]
    public long Region { get; set; }

    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; set; }

    [JsonPropertyName("y")]
    public long Y { get; set; }

    [JsonPropertyName("checksum")]
    public Guid Checksum { get; set; }
}
