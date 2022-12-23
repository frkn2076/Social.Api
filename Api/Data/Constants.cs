namespace Api.Data;

public static class Constants
{
    public static string SqlFileExtension = ".sql";

    public static string AdhocFolderPath = Path.Combine("Data", "Sql", "Adhoc");

    public static string SchemeFolderPath = Path.Combine("Data", "Sql", "Scheme");

    public static class Queries
    {
        public static string CreateProfileQuery = nameof(CreateProfileQuery);

        public static string CreateProfileActivityQuery = nameof(CreateProfileActivityQuery);

        public static string CreateActivityQuery = nameof(CreateActivityQuery);

        public static string GetProfileByUserNameQuery = nameof(GetProfileByUserNameQuery);

        public static string GetProfileByIdQuery = nameof(GetProfileByIdQuery);

        public static string UpdateProfileQuery = nameof(UpdateProfileQuery);

        public static string GetActivityPaginationFilterQuery = nameof(GetActivityPaginationFilterQuery);
        
        public static string GetActivityRandomlyQuery = nameof(GetActivityRandomlyQuery);

        public static string GetActivityRandomlyByKeyQuery = nameof(GetActivityRandomlyByKeyQuery);

        public static string GetActivityRandomlyByFilterQuery = nameof(GetActivityRandomlyByFilterQuery);

        public static string GetUserActivityQuery = nameof(GetUserActivityQuery);

        public static string GetOwnerActivityQuery = nameof(GetOwnerActivityQuery);

        public static string GetActivityByIdQuery = nameof(GetActivityByIdQuery);

        public static string GetUsersByActivityIdQuery = nameof(GetUsersByActivityIdQuery);

        public static string CreateChatMessageQuery = nameof(CreateChatMessageQuery);

        public static string GetChatMessagesByActivityIdQuery = nameof(GetChatMessagesByActivityIdQuery);
    }
}
