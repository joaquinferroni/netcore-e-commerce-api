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
    public class ProductController : ControllerBase
    {
        public readonly IProductService _productService;

        public ProductController (IProductService productService){
                this._productService = productService;
        }

        [HttpGet]
        public ActionResult Get(string filter)
        {
            var user = User.Identity.Name;
            
           ProductFilter pf = JsonConvert.DeserializeObject<ProductFilter>(filter);
           pf.UserId = int.Parse(user);
            return Ok(this._productService.GetProducts(pf));
        }

        [HttpGet ("{id}")]
        public ActionResult Get(int id,int ivaDetailed)
        {
            if(id == 0)
                throw new ArgumentException("El elemento solicitado no se encuentra disponible");

            var user = User.Identity.Name;
            return Ok(this._productService.Get(id,int.Parse(user),ivaDetailed==1));
        }

        [HttpGet("categories")]
        public ActionResult GetCategories()
        {
            return Ok(this._productService.GetCategories());
        }

        [HttpGet("subcategories")]
        public ActionResult GetSubCategories()
        {
            return Ok(this._productService.GetSubCategories());
        }

        [HttpGet("brands")]
        public ActionResult GetBrands()
        {
            return Ok(this._productService.GetBrands());
        }

        
        
    }
}