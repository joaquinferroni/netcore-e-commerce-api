using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace e_commerce.webapi.Models
{
    public class Product
    {
        [Column("IdArticulo")]
        public int Id { get; set; }
        [Column("IdSubCategoria")]
        public int SubCategoryId { get; set; }
        [Column("SubCategoria")]
        public string SubCategory { get; set; }
        [Column("IdCategoria")]
        public int CategoryId { get; set; }
        [Column("Categoria")]
        public string Category { get; set; }
        [Column("IdFamilia")]
        public int FamilyId { get; set; }
        [Column("Familia")]
        public string Family { get; set; }
        [Column("IdMarca")]
        public int BrandId { get; set; }
        [Column("Marca")]
        public string Brand { get; set; }
        [Column("Moneda")]
        public string Money { get; set; }
        [Column("TipoAlicuota")]
        public string TipoAlicuota { get; set; }
        [Column("Alicuota")]
        public decimal Alicuota { get; set; }
        [Column("IdAlicuota")]
        public System.Int16 AlicuotaId { get; set; }
        [Column("Iva")]
        public decimal Iva { get; set; }
        [Column("Simbolo")]
        public string Symbol { get; set; }
        [Column("P_Final")]
        public decimal FinalPrice { get; set; }
        [Column("P_Neto")]
        public decimal Price { get; set; }
        [Column("Detalle")]
        public string Detail { get; set; }
        [Column("CodigoBarras")]
        public string Barcode { get; set; }
        [Column("obs")]
        public string Observations { get; set; }
        [NotMapped]
        public IList<ProductImage> images { get; set; }

        [NotMapped]
        public int TotalCart { get; set; }
    }


    public class ProductListDTO
    {
        [Column("IdArticulo")]
        public int Id { get; set; }
        [Column("Codigo")]
        public string Code { get; set; }
        [Column("Detalle")]
        public string Detail { get; set; }
        [Column("Moneda")]
        public string Money { get; set; }
        [Column("Simbolo")]
        public string Symbol { get; set; }
        [Column("TipoAlicuota")]
        public string TipoAlicuota { get; set; }
        [Column("P_Final")]
        public double FinalPrice { get; set; }
        [Column("P_Neto")]
        public double Price { get; set; }
        [Column("imagen")]
        [JsonIgnore]
        public byte[] Image { get; set; }
        [NotMapped]
        public string ImageB64 { get; set; }
        [NotMapped]
        public ProductImage images { get; set; }
    }

    [Table("ArticuloImagen", Schema = "Compras")]
    public class ProductImage
    {
        [Column("IdArticuloImagen")]
        public int Id { get; set; }
        [Column("IdArticulo")]
        public int ProductId { get; set; }
        [Column("Imagen")]
        public byte[] Img { get; set; }
        [NotMapped]
        public string Base64Img { get; set; }
    }


    public class ProductFilter
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? FamilyId { get; set; }
        public int? BrandId { get; set; }
        public string Detail { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; }
        public int Iva { get; set; }

    }

    public class ProductResponse
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public IList<ProductListDTO> Products { get; set; }
    }
}