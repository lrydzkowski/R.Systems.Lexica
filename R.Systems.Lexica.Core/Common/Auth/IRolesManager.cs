namespace R.Systems.Lexica.Core.Common.Auth;

public interface IRolesManager
{
    IReadOnlyCollection<string> Roles { get; }
}
