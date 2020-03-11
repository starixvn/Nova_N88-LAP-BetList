using System.IO;
using Fanex.Athena.Common.Utility.Services.Interfaces;
using Fanex.Athena.Common.Utility.Services.Internal;

namespace Fanex.Athena.Common.Utility.Services
{
    public class TempFileService : ITempFileService
    {
        private readonly IAppSettings _appSettings;

        private readonly IFileSystem _fileSystem;

        public TempFileService(IAppSettings appSetting, IFileSystem fileSystem)
        {
            _appSettings = appSetting ?? new AppSettings();
            _fileSystem = fileSystem ?? new FileSystem();
        }

        public void Delete(string path)
        {
            try
            {
                var realPath = Path.Combine(_appSettings.TempFilePath, path);

                _fileSystem.Delete(realPath);
            }
            catch
            {
                // Ignore it
            }
        }

        public string ReadString(string path)
        {
            var realPath = Path.Combine(_appSettings.TempFilePath, path);
            return _fileSystem.ReadString(realPath);
        }

        public void Save(string path, string content)
        {
            var realPath = Path.Combine(_appSettings.TempFilePath, path);
            _fileSystem.Save(realPath, content);
        }

        public byte[] ReadBytes(string path)
        {
            var realPath = Path.Combine(_appSettings.TempFilePath, path);
            return _fileSystem.ReadAllBytes(realPath);
        }
    }
}