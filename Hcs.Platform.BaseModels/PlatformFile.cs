using System.ComponentModel.DataAnnotations;
using System;
namespace Hcs.Platform.BaseModels
{
    public class PlatformFile
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Key { get; set; }

        [StringLength(300)]
        public string Dir { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public long Length { get; set; }

        [Required]
        [StringLength(100)]
        public string MimeType { get; set; }

        public DateTime Date { get; set; }

        public bool Confirmed { get; set; }
    }
}