using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Hcs.Platform.Data;
using Hcs.Platform.User;
using Newtonsoft.Json;

namespace Hcs.Platform.BaseModels
{
    [System.Serializable]
    public class PlatformUser : IPlatformUser, IPlatformEntity
    {
        public long Id { get; set; }
        public string Account { get; set; }

        [NotMapped]
        [JsonProperty(nameof(Password))]
        public string NewPassword { set => Password = value; }
        [JsonIgnore]
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserStatus Status { get; set; }
        public long? CreatedBy { get; set; }
        public long? LastUpdatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? LastUpdatedTime { get; set; }

        IPlatformUser IPlatformEntity.CreatedByUser => CreatedByUser;
        public PlatformUser CreatedByUser { get; set; }

        public PlatformUser LastUpdatedByUser { get; set; }
        IPlatformUser IPlatformEntity.LastUpdatedByUser => LastUpdatedByUser;

        public ICollection<PlatformUserGroup> PlatformUserGroups { get; set; }
    }
}
