using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.webapi.Models
{
    public class Status
    {
        [Column("IdEstado")]
        public int Id { get; set; }
        [Column("Descripcion")]
        public string Text { get; set; }
    }
}