using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hcs.Platform.Abstractions.Role;
using Hcs.Platform.Data;
using Hcs.Platform.User;

namespace Hcs.Platform.BaseModels
{
    public class PlatformGroup : IPlatformEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public long? CreatedBy { get; set; }
        public long? LastUpdatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? LastUpdatedTime { get; set; }
        IPlatformUser IPlatformEntity.CreatedByUser => CreatedByUser;
        public PlatformUser CreatedByUser { get; set; }
        public PlatformUser LastUpdatedByUser { get; set; }
        IPlatformUser IPlatformEntity.LastUpdatedByUser => LastUpdatedByUser;
        public ICollection<PlatformUserGroup> PlatformUserGroups { get; set; }
        public ICollection<PlatformGroupRole> PlatformGroupRoles { get; set; }
    }
}
