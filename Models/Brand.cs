using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.webapi.Models
{
    [Table("Marcas", Schema = "Sistema")]
    public class Brand
    {
        [Key]
        [Column("IdMarca")]
        public int Id { get; set; }
        [Column("Marca")]
        public string Text { get; set; }
        [Column("Codigo")]
        public string BrandCode { get; set; }
    }
}