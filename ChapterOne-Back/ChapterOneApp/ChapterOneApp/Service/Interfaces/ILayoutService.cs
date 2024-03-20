namespace ChapterOneApp.Service.Interfaces
{
    public interface ILayoutService
    {
        Dictionary<string, string> GetSettingsData();
        Dictionary<string, string> GetHeaderBackgroundData();
    }
}
