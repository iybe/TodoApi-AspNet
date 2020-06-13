using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    [Table("user")]
    public class User
    {
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("username")]
        [Required]
        [MaxLength(40)]
        public string Username { get; set; }

        [Column("password")]
        [Required]
        [MaxLength(20)]
        public string Password { get; set; }

        public List<TodoItem> TodoItems { get; set; }
    }
}
