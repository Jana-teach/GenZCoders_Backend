using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenZCoders.Models
{
    [Table("tbl_media")]
    public class Media
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [Column("TableName")]
        public string TableName { get; set; } = null!;

        [Column("Table_ID")]
        public long TableId { get; set; }

        [Required]
        [Column("FilePath")]
        public string FilePath { get; set; } = null!;
    }
}
