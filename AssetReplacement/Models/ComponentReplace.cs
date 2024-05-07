using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetReplacement.Models
{
    public class ComponentReplace
    {
        [Key]
        public int Id { get; set; }
        public int AssetRequestId { get; set; }
        [ForeignKey("AssetRequestId")]
        [ValidateNever]
        public AssetRequest AssetRequest { get; set; }
        [Required] 
        public string Name {  get; set; }
        public string Action {  get; set; }
        public DateTime? ComponentReplaceDate { get; set; }
    }
}
