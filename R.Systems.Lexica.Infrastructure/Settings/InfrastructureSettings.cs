namespace R.Systems.Lexica.Infrastructure.Settings;

public class InfrastructureSettings
{
    public const string PropertyName = "Lexica";

    private string _setFilesDirPath = "";
    public string SetFilesDirPath
    {
        get
        {
            return _setFilesDirPath;
        }
        init
        {
            if (!Directory.Exists(value))
            {
                throw new DirectoryNotFoundException($"Set files directory doesn't exist: {value}");
            }
            _setFilesDirPath = value;
        }
    }
}
