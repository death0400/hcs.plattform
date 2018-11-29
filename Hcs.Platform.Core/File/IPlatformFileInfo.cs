namespace Hcs.Platform.File
{
    public interface IPlatformFileInfo
    {
        string Dir { get; }
        string Name { get; }
        string MimeType { get; }
        string ETag { get; }
        long Length { get; }
    }
}