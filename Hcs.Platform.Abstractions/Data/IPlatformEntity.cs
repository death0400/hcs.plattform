using System;
namespace Hcs.Platform.Data
{
    public interface IPlatformEntity
    {
        long? CreatedBy { get; set; }

        User.IPlatformUser CreatedByUser { get; }

        long? LastUpdatedBy { get; set; }
        User.IPlatformUser LastUpdatedByUser { get; }
        DateTime? CreatedTime { get; set; }

        DateTime? LastUpdatedTime { get; set; }
    }
}