using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.webapi.Models
{
   [Table("NotaDeVentasDetalle", Schema = "Venta")]
    public class OrderDetail{
            [Column("IdNotaDeVentasDetalle")]
            [Key]
            public long DetailOrderId { get; set; }
            [Column("IdNotaDeVenta")]
            public int OrderId { get; set; }
            [Column("IdArticulo")]
            public int ProductId { get; set; }
            [Column("ArtDetalle")]
            public string ProductDetail { get; set; }
            [Column("Cantidad")]
            public decimal Count { get; set; }
            [Column("PrecioUnitario_A")]
            public decimal Price_A { get; set; }
            [Column("PrecioUnitario_B")]
            public decimal Price_B { get; set; }
            [Column("SubTotal")]
            public decimal SubTotal { get; set; }

    }
}