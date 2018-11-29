using System;
using System.Collections.Generic;
using Hcs.Platform.Abstractions.Role;
using Hcs.Platform.Data;
using Hcs.Platform.User;

namespace Hcs.Platform.BaseModels
{
    public class PlatformGroupRole : IPlatformEntity
    {
        public long Id { get; set; }
        public long PlatformGroupId { get; set; }
        public PlatformGroup PlatformGroup { get; set; }
        public PermissionStatus Permission { get; set; }
        public string FunctionCode { get; set; }
        public string FunctionRoleCode { get; set; }
        public long? CreatedBy { get; set; }
        public long? LastUpdatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? LastUpdatedTime { get; set; }
        IPlatformUser IPlatformEntity.CreatedByUser => CreatedByUser;
        public PlatformUser CreatedByUser { get; set; }

        public PlatformUser LastUpdatedByUser { get; set; }
        IPlatformUser IPlatformEntity.LastUpdatedByUser => LastUpdatedByUser;
    }
}
