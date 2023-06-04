using R.Systems.Lexica.Api.Web.Options;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Options.Serilog;

internal class SerilogOptionsData : SerilogOptions, IOptionsData
{
    public SerilogOptionsData()
    {
        StorageAccountData = new SerilogStorageAccountOptionsData();
    }

    public SerilogStorageAccountOptionsData StorageAccountData { get; set; }

    public new SerilogStorageAccountOptions StorageAccount => StorageAccountData;

    public Dictionary<string, string?> ConvertToInMemoryCollection(string? parentPosition = null)
    {
        return StorageAccountData.ConvertToInMemoryCollection(Position);
    }
}

internal class SerilogStorageAccountOptionsData : SerilogStorageAccountOptions, IOptionsData
{
    public SerilogStorageAccountOptionsData()
    {
        ConnectionString = "connection_string";
        ContainerName = "container_name";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection(string? parentPosition = null)
    {
        string position = OptionsPositionsMerger.Merge(parentPosition, Position);

        return new Dictionary<string, string?>
        {
            [$"{position}:{nameof(ConnectionString)}"] = ConnectionString,
            [$"{position}:{nameof(ContainerName)}"] = ContainerName
        };
    }
}
