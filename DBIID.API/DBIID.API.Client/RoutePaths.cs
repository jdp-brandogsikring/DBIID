public static class RoutePaths
{
    public const string Home = "/";
    public const string Counter = "/counter";
    public const string Weather = "/weather";

    public static class User
    {
        public const string Base = "/user";
        public const string List = $"{Base}/list";
        public const string Create = $"{Base}/create";
        public static string Details(int userId) => $"{Base}/{userId}";
        public static string Permissions(int userId) => $"{Details(userId)}/permissions";
    }


}