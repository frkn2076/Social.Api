namespace Api.Utils;

public static class FileResourceUtility
{
    public static string LoadResource(string path, string file)
    {
        var filePath = Path.Combine(path, file);
        return File.ReadAllText(filePath);
    }
}
