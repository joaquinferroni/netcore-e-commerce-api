using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.webapi.Models
{
    public class UserLogin{
    
    [Required]
     public string Login { get; set; }   
     [Required]
     public string Pass { get; set; }
    }

    public class ClienteLogin
    {
        [Key]
        public int? idCliente { get; set; }
        public int? IdClienteContacto { get; set; }
        public string RazonSocial { get; set; }
        public string Usuario { get; set; }
    }

    
    public class CustomerDetail
    {
        [Column("IdCliente")]
        public int Id { get; set; }
        [Column("Codigo")]
        public string Code { get; set; }
        [Column("RazonSocial")]
        public string SocialReazon { get; set; }
        [Column("CUIT_CUIL")]
        public string Cuil { get; set; }
        [Column("email_cliente")]
        public string Email { get; set; }
        [Column("IvaDiscriminado")]
        public int IvaDetailed { get; set; }
        [Column("CtaCte")]
        public bool Account { get; set; }
        [Column("idCondicion")]
        public int PaymentConditionId { get; set; }
        [Column("Condicion")]
        public string PaymentConditionDescription { get; set; }
        [Column("idFormaPago")]
        public int PaymentTypeId { get; set; }
        [Column("FormaPago")]
        public string PaymentTypeDescription { get; set; }
        [Column("CreditoMax")]
        public decimal Credit { get; set; }
        [Column("Lista")]
        public string List { get; set; }
        [Column("Vendedor")]
        public string Seller { get; set; }
        [Column("IdVendedor")]
        public int SellerId { get; set; }
        [Column("email_vendedor")]
        public string SellerEmail { get; set; }
        [Column("CotizaDolar")]
        public decimal Dollar { get; set; }
        [Column("IdMoneda")]
        public int MoneyId { get; set; }
        [NotMapped]
        public string Token{ get; set; }
    }
}