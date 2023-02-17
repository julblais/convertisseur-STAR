namespace webApp
{
    public static class Version
    {
        public const int Major = 0;
        public const int Minor = 8;
        public const int Patch = 0;

        public static string AsString = $"{Major}.{Minor}.{Patch}";
        public static string Core = STAR.Version.AsString;
    }
}
