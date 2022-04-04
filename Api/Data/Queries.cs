namespace Api.Data;

public static class Queries
{
    public static string SqlFileExtension = ".sql";

    public static string AdhocFolderPath = "Data\\Sql\\Adhoc";

    public static string SchemeFolderPath = "Data\\Sql\\Scheme";

    public static string CreateActivityTable = nameof(CreateActivityTable);

    public static string CreateProfileActivityTable = nameof(CreateProfileActivityTable);

    public static string CreateProfileTable = nameof(CreateProfileTable);
}
