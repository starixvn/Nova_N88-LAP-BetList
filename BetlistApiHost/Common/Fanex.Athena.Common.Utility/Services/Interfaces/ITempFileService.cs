namespace Fanex.Athena.Common.Utility.Services.Interfaces
{
    public interface ITempFileService
    {
        void Delete(string path);

        void Save(string path, string content);

        string ReadString(string path);

        byte[] ReadBytes(string path);
    }
}