using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using e_commerce.webapi.Helpers;
using e_commerce.webapi.Infrastructure;
using e_commerce.webapi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace e_commerce.webapi.Services
{
    public interface IOrdersService
    {
        
        Order SaveOrder(Order order);
        IList<OrderModel> GetAll(int user, DateTime? dateFrom, DateTime? dateTo, int? status);
        OrderModel GetById(int id);
        IList<Status> GetStatus();
        IList<OrderDetail> GetDetailById(int id);
        
    }
    public class OrdersService : IOrdersService
    {
         private readonly IConfiguration configuration;
        private readonly IHostingEnvironment environment;
        private readonly IAuthService authService;
         private AppDbContext _context;
        
        public OrdersService(AppDbContext _context, IConfiguration conf, IHostingEnvironment env, IAuthService authService){
            this._context = _context;
            this.configuration = conf;
            this.environment = env;
            this.authService = authService;
        }
        public IList<OrderModel> GetAll(int userId, DateTime? dateFrom, DateTime? dateTo, int? status){
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@IdNotaDeVenta", DBNull.Value));
            parameters.Add(new SqlParameter("@IdCliente", userId));
            if(dateFrom.HasValue){
                parameters.Add(new SqlParameter("@fechaDesde", dateFrom));
            }else{
                parameters.Add(new SqlParameter("@fechaDesde", DBNull.Value));
            }
            if(dateTo.HasValue){
                parameters.Add(new SqlParameter("@fechaHasta", dateTo));
            }else{
                parameters.Add(new SqlParameter("@fechaHasta", DBNull.Value));
            }
            parameters.Add(new SqlParameter("@pv", DBNull.Value));
            parameters.Add(new SqlParameter("@numero", DBNull.Value));
            parameters.Add(new SqlParameter("@idSucursal", DBNull.Value));
            if(status.HasValue && status != 0){
                parameters.Add(new SqlParameter("@estado", status));
            }else{
                parameters.Add(new SqlParameter("@estado", DBNull.Value));
            }
            var orders = _context.Orders.FromSql("Venta.NotaDeVentasGet" 
                                                        +" @IdNotaDeVenta "
                                                        +",@IdCliente " 
                                                        +",@fechaDesde  "
                                                        +",@fechaHasta  "
                                                        +",@pv  "
                                                        +",@numero  "
                                                        +",@idSucursal  "
                                                        +",@estado  "
                                            , parameters.ToArray())
                                .ToList();
            return orders;
        }

        public OrderModel GetById(int id){
            return _context.Orders.Where(o => o.OrderId == id).FirstOrDefault();
        }

        public IList<Status> GetStatus(){
            return _context.Status.FromSql("Venta.NotaDeVentaEstadoGet ").ToList();
        }

        public IList<OrderDetail> GetDetailById(int id){
            return _context.OrderDetail.Where(o => o.OrderId == id).ToList();
        }

        public Order SaveOrder(Order order)
        {
            var user = authService.Me(order.UserId);   
            using (var transaction = _context.Database.BeginTransaction())
            {
                try{
                    var HeaderId = SaveHeader(order, user.SellerId, user.MoneyId);
                    _context.SaveChanges();

                    var ivaDetail = order.Products
                                    .GroupBy(i => i.AlicuotaId)
                                    .Select( p => new {
                                        AlicuotaId =  p.Key, 
                                        Amount = p.Sum(s => s.Price * s.Alicuota * s.TotalCart),
                                        BaseImponible = p.Sum( s=> s.FinalPrice * s.TotalCart),
                                    }).ToList();

                    foreach( var iva in ivaDetail){
                        this.SaveIvaDetail(iva.AlicuotaId, HeaderId,iva.Amount,iva.Amount != 0 ? iva.BaseImponible : 0,iva.Amount == 0 ? iva.BaseImponible: 0, order.DollarValue, user.MoneyId);
                        _context.SaveChanges();
                    }

                    foreach( var item in order.Products){
                        this.SaveDetail(item, HeaderId, order.DollarValue, user.MoneyId);
                        _context.SaveChanges();
                    }
                    transaction.Commit();
                }catch(Exception e){
                    transaction.Rollback();
                    throw e;
                }
            }
            Task.Run(()=>{
                SendEmail(order, user.Email, user.SellerEmail);
            });
            return order;
        }

        private long SaveHeader(Order order, int SellerId, int MoneyId){
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@IdVendedor", SellerId));
            parameters.Add(new SqlParameter("@IdCliente", order.CustomerId));
            parameters.Add(new SqlParameter("@NetoGrabado", order.Neto));
            parameters.Add(new SqlParameter("@Iva", order.Iva));
            parameters.Add(new SqlParameter("@Total", order.Total));
            parameters.Add(new SqlParameter("@IdUsuario", 4));
            parameters.Add(new SqlParameter("@IdMoneda", MoneyId));
            parameters.Add(new SqlParameter("@Nota", order.Observation));

            var product = _context.IdentityResult.FromSql("Web.NotaDeVentasWebInsert" 
                                                        +" @IdVendedor  "
                                                        +",@IdCliente  "
                                                        +",@NetoGrabado  "
                                                        +",@Iva  "
                                                        +",@Total  "
                                                        +",@IdUsuario  "
                                                        +",@IdMoneda  "
                                                        +",@Nota  "
                                            , parameters.ToArray())
                                            .FirstOrDefault();
            return product.Id;
        }

        private long SaveDetail(Product item, long HeaderId, float DollarValue, int MoneyId){
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@IdNotaDeVenta",HeaderId));
            parameters.Add(new SqlParameter("@IdArticulo",item.Id));
            parameters.Add(new SqlParameter("@CodBarra",item.Barcode));
            parameters.Add(new SqlParameter("@ArtDetalle"  ,item.Detail));
            parameters.Add(new SqlParameter("@Cantidad"  ,item.TotalCart));
            parameters.Add(new SqlParameter("@Lista_A"  ,item.Price));
            parameters.Add(new SqlParameter("@Lista_B"  ,item.FinalPrice));
            parameters.Add(new SqlParameter("@SubTotal"  ,item.Price*item.TotalCart));
            parameters.Add(new SqlParameter("@IdAlicuota"  ,item.AlicuotaId));
            parameters.Add(new SqlParameter("@AlicuotaPorciento"  ,item.Alicuota));
            parameters.Add(new SqlParameter("@AlicuotaValor",(double)((item.Price)*(item.Alicuota)*item.TotalCart)));//falta la cantidad
            parameters.Add(new SqlParameter("@TotalConIva"  ,(double)(item.FinalPrice*item.TotalCart)));
            parameters.Add(new SqlParameter("@Idmoneda"  ,MoneyId));

            var product = _context.Database.ExecuteSqlCommand("Web.NotaDeVentasDetalleWebInsert" 
                                                        +" @IdNotaDeVenta  "
                                                        +",@IdArticulo  "
                                                        +",@CodBarra  "
                                                        +",@ArtDetalle  "
                                                        +",@Cantidad  "
                                                        +",@Lista_A  "
                                                        +",@Lista_B  "
                                                        +",@SubTotal  "
                                                        +",@IdAlicuota  "
                                                        +",@AlicuotaPorciento  "
                                                        +",@AlicuotaValor  "
                                                        +",@TotalConIva  "
                                                        +",@Idmoneda  "
                                            , parameters.ToArray());//.FirstOrDefault();
            return product;
        }

        private long SaveIvaDetail(int AlicuotaId, long HeaderId, decimal Amount, decimal BaseImponible, decimal NetoNoGrabado, float DollarValue, int MoneyId){
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@IdNotaDeVenta",HeaderId));
            parameters.Add(new SqlParameter("@IdAlicuota",AlicuotaId));
            parameters.Add(new SqlParameter("@Monto" ,Amount));
            parameters.Add(new SqlParameter("@BaseImponible", BaseImponible));
            parameters.Add(new SqlParameter("@NetoNoGrabado", NetoNoGrabado));
            parameters.Add(new SqlParameter("@Idmoneda"  ,MoneyId));
            var product = _context.Database.ExecuteSqlCommand("Web.NotaDeVentasDetalleIvaWebInsert" 
                                                        +" @IdNotaDeVenta  "
                                                        +",@IdAlicuota  "
                                                        +",@Monto  "
                                                        +",@BaseImponible  "
                                                        +",@NetoNoGrabado  "
                                                        +",@Idmoneda  "
                                            , parameters.ToArray());//.FirstOrDefault();
            return product;
        }

         private void SendEmail(Order order, string to, string hiddenCC ){
            IEmailBase email = new RequestDetailEmail(configuration, environment, order);
            email.Subject= "Pedido realizado";
            email.CCEmail = hiddenCC;
            email.ToEmail = to;
            EmailHelper.Send(this.configuration, email);
        }
    }
}