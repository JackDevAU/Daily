using AutoScrum.AzureDevOps;
using Microsoft.AspNetCore.Components;
using OldConfigService = AutoScrum.Services.OldConfigService;


namespace AutoScrum.Pages;

public partial class Settings
{
    private Form<ProjectConfigAzureDevOps> _connectionForm;
    private bool _connectionFormLoading;

    private bool IsPageInitializing { get; set; } = true;

    private ProjectConfigAzureDevOps ConnectionInfo { get; set; } = new();
    private bool ShowConfig { get; set; }
    private ProjectConfigAzureDevOps ConnectionInfoRequest { get; set; } = new();

    [Inject] public IDailyScrumService DailyScrum { get; set; }
    [Inject] public HttpClient HttpClient { get; set; }
    // TODO: Remove when all is updated.
    [Inject] public OldConfigService OldConfigService { get; set; }
    [Inject] public IConfigService ConfigService { get; set; }
    [Inject] public MessageService MessageService { get; set; }
    [Inject] public IClipboardService ClipboardService { get; set; }

    public List<ProjectMetadata> ProjectMetadatas { get; set; } = new(); 
    public List<ProjectConfigAzureDevOps> ProjectData { get; set; } = new();

    public readonly List<string> ProjectLabels = new()
    {
        "The label you want this config to be saved as",
        "The email address used to access the Azure Board",
        "Personal Access Tokens Access via Azure Board > Click your Profile > Security > Personal Access Tokens > + New Token",
        "Grab from the url dev.azure.com/<Org name>/",
        "Grab from the url just after the org name dev.azure.com/<Org name>/<Project name>/"
    };

    public string ProjectNameValue { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            bool showConfig = true;
            AppConfig config = await ConfigService.GetAppConfig();
            if (config != null)
            {
                ProjectMetadatas = await ConfigService.GetProjectsMetadata();

                foreach(var metadata in ProjectMetadatas)
                {
                    ProjectConfig? projectConfig = await ConfigService.GetCurrentProject(int.Parse(metadata.Path));

                    if (projectConfig is ProjectConfigAzureDevOps azureDevOpsProject)
                    {
                        ProjectData.Add(azureDevOpsProject);
                        //ConnectionInfo.Clone()
                    }
                }
            }

            ShowConfig = showConfig;
        }
        catch
        {
            IsPageInitializing = false;
            MessageService.Error("Critical error while loading config");
            throw;
        }

        if (ConnectionInfo is null)
        {
            IsPageInitializing = false;
            return;
        }

        IsPageInitializing = false;
    }
}
