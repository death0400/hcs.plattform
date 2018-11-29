namespace Hcs.Platform.File
{
    class PlatformFileInfo : IPlatformFileInfo
    {
        public string Name { get; set; }
        public string Dir { get; set; }
        public string ETag { get; set; }
        public string MimeType { get; set; }
        public long Length { get; set; }
    }
}