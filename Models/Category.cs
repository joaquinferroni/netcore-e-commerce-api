using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.webapi.Models
{
    public class Category
    {
        [Column("IdCategoria")]
        public int Id { get; set; }
        [Column("Categoria")]
        public string Text { get; set; }

    }

    public class SubCategory
    {
        [Column("IdSubCategoria")]
        public int Id { get; set; }
        [Column("SubCategoria")]
        public string Text { get; set; }
        [Column("IdCategoria")]
        public int CategoryId { get; set; }
    }

}