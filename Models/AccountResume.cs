using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.webapi.Models
{
    public class AccountResume
    {
        [Key]
        [Column("IdMovimiento")]
       public int MovementId { get; set; }
       [Column("fecha")]
       public DateTime Date { get; set; }
       [Column("FchVtoPago")]
       public DateTime? EndOfPayment { get; set; }
       [Column("Estado")]
       public string Status { get; set; }
       [Column("Comprobante")]
       public string Receipt { get; set; }
       [Column("pendiente")]
       public decimal Pending { get; set; }
       [Column("debe")]
       public decimal Debit { get; set; }
       [Column("haber")]
       public decimal Credit { get; set; }
       [Column("Acumulado")]
       public decimal Sum { get; set; }
    }
}