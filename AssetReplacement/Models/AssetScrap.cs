﻿using System.ComponentModel.DataAnnotations;

namespace AssetReplacement.Models
{
    public class AssetScrap
    {
        [Key]
        public int Id { get; set; }
        [Required]

        public string Type { get; set; }
        public string SerialNumber { get; set; }
        public string Location { get; set; }
        public DateTime? DateInput { get; set; }
        public bool? Status { get; set; }
    }
}
