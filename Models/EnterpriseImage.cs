using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace e_commerce.webapi.Models
{
    public class EnterpriseImage
    {
        [Column("Imagen")]
        [JsonIgnore]
        public byte[] Image { get; set; }
        [Column("IdEmpresaImagene")]
        public int EnterpriseImageId { get; set; }
        [Column("Principal")]
        public bool Principal { get; set; }
        [NotMapped]
        public string ImageB64 { get; set; }
    }
}