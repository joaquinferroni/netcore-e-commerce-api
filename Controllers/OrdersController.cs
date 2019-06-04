using System;
using System.Security.Claims;
using e_commerce.webapi.Models;
using e_commerce.webapi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace e_commerce.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController: ControllerBase
    {
        
        public readonly IOrdersService _ordersService;
        public OrdersController (IOrdersService ordersService){
                this._ordersService = ordersService;
        }

        [HttpPost]
        public ActionResult Orders(Order order){
            if(order ==null|| order.Products.Count == 0)
                throw new ArgumentException("El pedido que se encuentra solicitando no se puede generar. Por favor intente nuevamente");
            
            var user = int.Parse(User.Identity.Name);
            order.UserId = user;
            order.CustomerId = user;
            return Ok(this._ordersService.SaveOrder(order));
        }

        [HttpGet]
        public ActionResult Orders( DateTime? from, DateTime? to, int? status){
            var user = int.Parse(User.Identity.Name);
            return Ok(this._ordersService.GetAll(user,from,to,status));
        }

        [HttpGet("status")]
        public ActionResult GetStatus(){
            return Ok(this._ordersService.GetStatus());
        }

        [HttpGet("{id}/Details")]
        public ActionResult Orders(int id){
            if(id == 0)
                throw new ArgumentException("No se encuentra la orden solicitada. Por favor intente nuevamente");
            
            var user = int.Parse(User.Identity.Name);
            return Ok(this._ordersService.GetDetailById(id));
        }
    }
}