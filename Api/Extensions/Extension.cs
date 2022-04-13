namespace Api.Extensions;

public static class Extension
{
    public static T GetOptions<T>(this IConfiguration configuration) where T : new()
    {
        var t = new T();
        configuration.GetSection(typeof(T).Name).Bind(t);
        return t;
    }
}
