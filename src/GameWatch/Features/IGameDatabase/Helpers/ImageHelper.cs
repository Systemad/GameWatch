namespace GameWatch.Features.IGameDatabase.Helpers;

// Credits: https://github.com/kamranayub/igdb-dotnet/blob/master/IGDB/ImageHelper.cs
public static class ImageHelper
{
    private const string IGDB_IMAGE_TEMPLATE =
        "//images.igdb.com/igdb/image/upload/t_{size}/{hash}.jpg";

    /// <returns></returns>
    public static IDictionary<ImageSize, string> ImageSizeMap = new Dictionary<ImageSize, string>()
    {
        { ImageSize.CoverSmall, "cover_small" },
        { ImageSize.CoverBig, "cover_big" },
        { ImageSize.ScreenshotMed, "screenshot_med" },
        { ImageSize.ScreenshotBig, "screenshot_big" },
        { ImageSize.ScreenshotHuge, "screenshot_huge" },
        { ImageSize.LogoMed, "logo_med" },
        { ImageSize.Thumb, "thumb" },
        { ImageSize.Micro, "micro" },
        { ImageSize.HD720, "720p" },
        { ImageSize.HD1080, "1080p" },
    };

    public static string GetImageUrl(
        string imageId,
        ImageSize size = ImageSize.Thumb,
        bool retina = false
    )
    {
        if (ImageSizeMap.ContainsKey(size))
        {
            return IGDB_IMAGE_TEMPLATE
                .Replace("{hash}", imageId)
                .Replace("{size}", ImageSizeMap[size] + (retina ? "_2x" : ""));
        }
        else
        {
            throw new ArgumentException("ImageSize unknown", nameof(size));
        }
    }

    public enum ImageSize
    {
        CoverSmall = 0,
        ScreenshotMed = 1,
        CoverBig = 2,
        LogoMed = 3,
        ScreenshotBig = 4,
        ScreenshotHuge = 5,
        Thumb = 6,
        Micro = 7,
        HD720 = 8,
        HD1080 = 9
    }
}
