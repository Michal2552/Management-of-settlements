using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Settlement_pm.Models
{
    public class Settlement
    {
        public int Id { get; set; }



        [Required]
        [StringLength(255)]
        public string SettlementName { get; set; } 
    }

}
