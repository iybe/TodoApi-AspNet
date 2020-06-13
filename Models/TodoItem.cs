using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    [Table("todo")]
    public class TodoItem
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("name")]
        [Required]
        [MaxLength(60)]
        public string Name { get; set; }

        [Column("isComplete")]
        [Required]
        public bool IsComplete { get; set; }

        [Column("user_id")]
        [Required]
        public int User_Id { get; set; }
        [ForeignKey("user_id")]
        public User User { get; set; }
    }
}
