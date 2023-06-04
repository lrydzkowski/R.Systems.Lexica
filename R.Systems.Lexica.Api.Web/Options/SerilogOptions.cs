namespace R.Systems.Lexica.Api.Web.Options;

public class SerilogOptions
{
    public const string Position = "Serilog";

    public SerilogStorageAccountOptions StorageAccount { get; init; } = new();
}

public class SerilogStorageAccountOptions
{
    public const string Position = "StorageAccount";

    public string? ConnectionString { get; init; }

    public string? ContainerName { get; init; }
}
