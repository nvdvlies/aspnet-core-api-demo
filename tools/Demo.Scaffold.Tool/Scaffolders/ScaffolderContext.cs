using Demo.Scaffold.Tool.Models;

namespace Demo.Scaffold.Tool.Scaffolders;

internal class ScaffolderContext
{
    public ScaffolderContext(AppSettings appSettings)
    {
        AppSettings = appSettings;
        Variables = new ScaffolderVariables();
    }

    public AppSettings AppSettings { get; set; }
    public ScaffolderTypes ScaffolderType { get; set; }
    public ScaffolderVariables Variables { get; set; }
}
