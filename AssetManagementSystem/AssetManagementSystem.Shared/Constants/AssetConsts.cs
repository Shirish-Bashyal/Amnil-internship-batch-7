namespace AssetManagementSystem.Shared.Constants;

public static class AssetConsts
{
    public static class Name
    {
        public const int MaxLength = 100;
    }

    public static class Description
    {
        public const int MaxLength = 1000;
    }

    public static class SerialNumber
    {
        public const int MaxLength = 100;
    }

    public static class Image
    {
        public const int FileSize = 1 * 1024 * 1024; //file size in bytes

        public const string JpegFormat = "image/jpeg";
        public const string PngFormat = "image/png";

        public static readonly string[] AllowedFormats = { JpegFormat, PngFormat };
    }
}
