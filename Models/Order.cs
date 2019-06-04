using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System;
namespace e_commerce.webapi.Models
{
    public class Order
    {

        public int PaymentMethod { get; set; }
        public string PaymentMethodDescription { get; set; }
        public float Total { get; set; }
        public float Iva { get; set; }
        public float Neto { get; set; }
        public int CustomerId { get; set; }
        public int SellerId { get; set; }
        public int UserId { get; set; }
        public string Observation { get; set; }
        public float DollarValue { get; set; }
        public IList<Product> Products { get; set; }
    }

    [Table("NotaDeVentas", Schema = "Venta")]
    public class OrderModel{
        [Column("IdNotaDeVenta")]
        [Key]
        public int OrderId { get; set; }
        [Column("Nro")]
        public string Number { get; set; }
        [Column("Fecha")]
        public DateTime OrderDate { get; set; }
        [Column("NetoGrabado")]
        public decimal Neto { get; set; }
        [Column("Iva")]
        public decimal Iva { get; set; }
        [Column("Total")]
        public decimal Total { get; set; }
        [Column("CotizaDolar")]
        public decimal Dollar { get; set; }
        [Column("IdCliente")]
        public int ClientId { get; set; }
        [Column("Nota")]
        public string Observation { get; set; }
        [Column("Nombre")]
        public string SellerName { get; set; }
        [Column("IdVendedor")]
        public int SellerId { get; set; }
        [Column("IdMoneda")]
        public Int16 MoneyId { get; set; }
        [Column("Moneda")]
        public string Money { get; set; }
        [Column("IdEstado")]
        public Int16 StatusId { get; set; }
        [Column("descripcion")]
        public string StatusName { get; set; }
        [Column("TotalEnMoneda")]
        public decimal TotalInMoney { get; set; }
        [Column("TotalNetoEnMoneda")]
        public decimal NetoInMoney { get; set; }
        [Column("Comprobante")]
        public string Ticket { get; set; }

    }
}