using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hcs.Platform.BaseModels;
using Hcs.Platform.Data;
using Hcs.Platform.User;
using Newtonsoft.Json;

namespace Hcs.PlatformModule.Test.Model.Entities
{
    public class Customer : IPlatformEntity
    {
        public long Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }

        public IEnumerable<Order> Orders { get; set; }

        public long? CreatedBy { get; set; }
        public PlatformUser CreatedByUser { get; set; }
        public long? LastUpdatedBy { get; set; }
        public PlatformUser LastUpdatedByUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? LastUpdatedTime { get; set; }
        IPlatformUser IPlatformEntity.CreatedByUser => CreatedByUser;
        IPlatformUser IPlatformEntity.LastUpdatedByUser => LastUpdatedByUser;
    }
}
