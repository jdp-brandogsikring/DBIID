public static class RoutePaths
{
    public const string Home = "/";
    public const string Weather = "/weather";

    public static class Login
    {
        public const string Base = "/login";
    }

    public static class User
    {
        public const string Base = "/user";
        public const string List = $"{Base}/list";
        public const string Create = $"{Base}/create";
        public static string Details(string userId) => $"{Base}/{userId}";
        //public static string Permissions(string userId) => $"{Details(userId)}/permissions";
    }
}