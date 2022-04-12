namespace Api.Data;

public static class Queries
{
    public static string SqlFileExtension = ".sql";

    public static string AdhocFolderPath = "Data\\Sql\\Adhoc";

    public static string SchemeFolderPath = "Data\\Sql\\Scheme";

    #region Schemes

    public static string CreateActivityTable = nameof(CreateActivityTable);

    public static string CreateProfileActivityTable = nameof(CreateProfileActivityTable);

    public static string CreateProfileTable = nameof(CreateProfileTable);

    #endregion Schemes

    #region Queries

    public static string CreateProfileQuery = nameof(CreateProfileQuery);

    public static string GetPasswordQuery = nameof(GetPasswordQuery);

    public static string GetProfileByRefreshTokenQuery = nameof(GetProfileByRefreshTokenQuery);

    public static string GetProfileByUserNameQuery = nameof(GetProfileByUserNameQuery);

    public static string GetProfileByIdQuery = nameof(GetProfileByIdQuery);

    public static string UpdateRefreshTokenQuery = nameof(UpdateRefreshTokenQuery);

    public static string UpdateProfileQuery = nameof(UpdateProfileQuery);

    public static string GetActivityQuery = nameof(GetActivityQuery);

    #endregion Queries
}
