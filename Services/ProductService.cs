using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using e_commerce.webapi.Helpers;
using e_commerce.webapi.Infrastructure;
using e_commerce.webapi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace e_commerce.webapi.Services
{

    public interface IProductService
    {
        IList<Category> GetCategories();
        IList<SubCategory> GetSubCategories();
        IList<Brand> GetBrands();
        ProductResponse GetProducts(ProductFilter filter);
        Product Get(int id, int userId,bool ivaDetailed);
        
    }
    public class ProductService : IProductService
    {
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment environment;
        private readonly IAuthService authService;
         private AppDbContext _context;
         public ProductService (AppDbContext context, IConfiguration conf, IHostingEnvironment env, IAuthService authService)
        {
            _context = context;
            this.environment = env;
            this.configuration = conf;
            this.authService = authService;
        }
        public IList<Category> GetCategories()
        {
            var categories = _context.Categories.FromSql("Compra.CategoriasGet ").ToList();
            return categories;
        }

        public IList<Brand> GetBrands()
        {
            var brands = _context.Brands.ToList();
            return brands;
        }

        public IList<SubCategory> GetSubCategories()
        {
            var subcategories = _context.SubCategories.FromSql("Compra.CategoriasSubArticulosGet ").ToList();
            return subcategories;
        }

        public ProductResponse GetProducts(ProductFilter filter)
        {
            var user = authService.Me(filter.UserId);
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@IdCliente", filter.UserId));
            if (!string.IsNullOrWhiteSpace(filter.Detail)) {
                parameters.Add(new SqlParameter("@Detalle", filter.Detail));
            } else {
                parameters.Add(new SqlParameter("@Detalle", DBNull.Value));
            };
            
            if (filter.CategoryId != 0) {
                parameters.Add(new SqlParameter("@IdCategoria", filter.CategoryId));
            } else {
                parameters.Add(new SqlParameter("@IdCategoria", DBNull.Value));
            };
            if (filter.SubCategoryId != 0) {
                parameters.Add(new SqlParameter("@IdSubCategoria", filter.SubCategoryId));
            }else {
                parameters.Add(new SqlParameter("@IdSubCategoria", DBNull.Value));
            };

            if (filter.FamilyId != null && filter.FamilyId != 0) {
                parameters.Add(new SqlParameter("@IdFamilia", filter.FamilyId));
            }else {
                parameters.Add(new SqlParameter("@IdFamilia", DBNull.Value));
            };

            if (filter.BrandId != null && filter.BrandId != 0) {
                parameters.Add(new SqlParameter("@IdMarca", filter.BrandId));
            }else {
                parameters.Add(new SqlParameter("@IdMarca", DBNull.Value));
            };
            parameters.Add(new SqlParameter("@PageNum", filter.Page));
            parameters.Add(new SqlParameter("@PageSize", filter.PageSize));
            var pTotal = new SqlParameter() { ParameterName = "@TotalRows", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            parameters.Add(pTotal);
            parameters.Add(new SqlParameter("@IdTipoMoneda", user.MoneyId));
            parameters.Add(new SqlParameter("@IvaDiscriminado", filter.Iva==1));
            if (!string.IsNullOrWhiteSpace(filter.Code)) {
                parameters.Add(new SqlParameter("@tnCodigo", filter.Code));
            }else {
                parameters.Add(new SqlParameter("@tnCodigo", DBNull.Value));
            };
            var products = _context.ProductsListDTO.FromSql("Web.ArticulosWebGet" 
                                                        +" @IdCliente " 
                                                        +",@Detalle "
                                                        +",@IdCategoria "
                                                        +",@IdSubCategoria "
                                                        +",@IdFamilia "
                                                        +",@IdMarca "
                                                        +",@PageNum "
                                                        +",@PageSize "
                                                        +",@TotalRows OUT  "
                                                        +",@IdTipoMoneda  "
                                                        +",@IvaDiscriminado  "
                                                        +",@tnCodigo"
                                            , parameters.ToArray())
                                .ToList();
            products.ForEach( p => {
                if (p.Image != null)
                {
                    p.ImageB64 = System.Convert.ToBase64String(p.Image); 
                }
            });

            ProductResponse presponse = new ProductResponse();
            presponse.Total = (int)(pTotal.Value ?? 0);
            presponse.Products = products;               
           
            return presponse;
        }

        public Product Get(int id, int UserId, bool ivaDetailed)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@IdArticulo", id));
            parameters.Add(new SqlParameter("@IdCliente", UserId));
            parameters.Add(new SqlParameter("@IdTipoMoneda", DBNull.Value));
            parameters.Add(new SqlParameter("@IvaDiscriminado", ivaDetailed));
            var product = _context.Products.FromSql("Web.ArticuloDetalleWebGet" 
                                                        +" @IdArticulo  "
                                                        +",@IdCliente  "
                                                        +",@IdTipoMoneda  "
                                                        +",@IvaDiscriminado  "
                                            , parameters.ToArray())
                                .FirstOrDefault();

            if(product == null){
                throw new NotFoundException("El articulo que intenta buscar no esta disponible");
            }
            product.images = GetImages(product.Id);
            return product;
        }

        

        private IList<ProductImage> GetImages(int productId){
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@IdArticuloImagen", DBNull.Value));
            parameters.Add(new SqlParameter("@IdArticulo", productId));
            var images = _context.ProductImages.FromSql("Compra.ArticuloImagenGet @IdArticuloImagen, @IdArticulo " , parameters.ToArray()).ToList();
            images.ForEach( p => {
                if (p.Img != null)
                {
                    p.Base64Img = System.Convert.ToBase64String(p.Img); 
                    p.Img = null;
                }
            });
            return images;
        }
       
    }
}