namespace Fanex.Athena.Common.Utility.Services.Interfaces
{
    public interface ITemplateBuilder
    {
        string BuildContent(string path, object model);
    }
}