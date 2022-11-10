namespace Api.Data;

public static class Constants
{
    public static string SqlFileExtension = ".sql";

    public static string AdhocFolderPath = Path.Combine("Data", "Sql", "Adhoc");

    public static string SchemeFolderPath = Path.Combine("Data", "Sql", "Scheme");

    public static class Schemes
    {
        public static string CreateActivityTable = nameof(CreateActivityTable);

        public static string CreateProfileActivityTable = nameof(CreateProfileActivityTable);

        public static string CreateProfileTable = nameof(CreateProfileTable);
    }

    public static class Queries
    {
        public static string CreateProfileQuery = nameof(CreateProfileQuery);

        public static string CreateProfileActivityQuery = nameof(CreateProfileActivityQuery);

        public static string CreateActivityQuery = nameof(CreateActivityQuery);

        public static string GetPasswordQuery = nameof(GetPasswordQuery);

        public static string GetProfileByRefreshTokenQuery = nameof(GetProfileByRefreshTokenQuery);

        public static string GetProfileByUserNameQuery = nameof(GetProfileByUserNameQuery);

        public static string GetProfileByIdQuery = nameof(GetProfileByIdQuery);

        public static string UpdateRefreshTokenQuery = nameof(UpdateRefreshTokenQuery);

        public static string UpdateProfileQuery = nameof(UpdateProfileQuery);

        public static string GetActivityQuery = nameof(GetActivityQuery);

        public static string GetUserActivityQuery = nameof(GetUserActivityQuery);

        public static string GetActivityByIdQuery = nameof(GetActivityByIdQuery);

        public static string GetUsersByActivityIdQuery = nameof(GetUsersByActivityIdQuery);
    }
}
