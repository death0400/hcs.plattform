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
    public class Order : IPlatformEntity
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public Customer Customer { get; set; }
        [StringLength(100)]
        public string Item { get; set; }
        public decimal Quantity { get; set; }
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
