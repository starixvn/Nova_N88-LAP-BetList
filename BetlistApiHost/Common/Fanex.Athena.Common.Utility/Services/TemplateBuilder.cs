using Fanex.Athena.Common.Utility.Services.Interfaces;
using Fanex.Athena.Common.Utility.Services.Internal;
using RazorEngine;
using RazorEngine.Templating;

namespace Fanex.Athena.Common.Utility.Services
{
    public class TemplateBuilder : ITemplateBuilder
    {
        private readonly IFileSystem _fileSystem;

        public TemplateBuilder(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public string BuildContent(string path, object model)
        {
            var templateContent = _fileSystem.ReadString(path);

            return Engine.Razor.RunCompile(templateContent, path, model: model);
        }
    }
}