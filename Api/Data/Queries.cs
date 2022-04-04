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

    #endregion Queries
}
