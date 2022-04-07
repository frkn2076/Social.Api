namespace Api.Infra;

public class ServiceResponse<T> : ServiceResponse
{
    public T Response { get; set; }
}

public class ServiceResponse
{
    public bool IsSuccessful { get; set; }

    public string Error { get; set; }
}
