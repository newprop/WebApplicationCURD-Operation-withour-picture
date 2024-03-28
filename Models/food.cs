using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class food
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID {  get; set; }
        [Required]
        [StringLength(100)]
        public string foodName { get; set; }
        public string foodDescription { get; set; } 
        public string foodCode { get; set; } 
        public int foodType { get; set;}
        public List<foodItem> Items { get; set; } = new List<foodItem>();
    }
}
