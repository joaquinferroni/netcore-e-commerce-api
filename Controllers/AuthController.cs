using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Xml;
using e_commerce.webapi.Helpers;
using e_commerce.webapi.Models;
using e_commerce.webapi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace e_commerce.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IAuthService authService;
        public AuthController(IOptions<AppSettings> appSettings, IAuthService authService)
        {
            _appSettings = appSettings.Value;
            this.authService = authService;
        }


        [Authorize(Roles = "Admins,User")]
        [HttpGet]
        public ActionResult Get()
        {
            var a = this.User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(a);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public ActionResult Login([FromBody]UserLogin user)
        {
            var result = authService.Authenticate(user.Login, user.Pass);
            if (result != null && result.idCliente != 0)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var me = authService.Me((int)result.idCliente);
                var tokeOptions = new JwtSecurityToken(
                    claims: new List<Claim>(){
                            new Claim(ClaimTypes.Name,user.Login),
                            new Claim("IvaDetailed",me.IvaDetailed.ToString()),
                            new Claim(ClaimTypes.Role, "User"),
                            new Claim(ClaimTypes.Role, "Admin"),
                            new Claim(ClaimTypes.Role,"SuperAdmin")
                    },
                    expires: DateTime.Now.AddDays(5),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                me.Token = tokenString;
                return Ok(me);
            }
            else
            {
                throw new ArgumentException("El usuario o la contraseña son incorrectos");
            }
        }


        [AllowAnonymous]
        [HttpGet("image")]
        public ActionResult GetLoginImage()
        {
            XmlDocument urlXML;
            XmlElement root;
            XmlNodeList nodes;
            try
            {
                urlXML = new XmlDocument();
                urlXML.Load("http://www.bing.com/HPImageArchive.aspx?format=xml&idx=0&n=1&mkt=en-US");
                root = urlXML.DocumentElement;

                // URL de la imagen
                nodes = root.SelectNodes("/images/image/url");
                var Url = string.Format("http://www.bing.com{0}", nodes[0].InnerText.Replace("_1366x768.jpg", "_1920x1080.jpg"));

                // Copyright de la imagen
                nodes = root.SelectNodes("/images/image/copyright");
                var Copyright = nodes[0].InnerText;

                // Link del Copyright de la imagen
                nodes = root.SelectNodes("/images/image/copyrightlink");
                var CopyrightLink = nodes[0].InnerText;

                return Ok(new { Url = Url, Copyright = Copyright, CopyrightLink = CopyrightLink });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [AllowAnonymous]
        [HttpGet("logo")]
        public ActionResult GetLogo(){
            return Ok(this.authService.GetLogo());
        }
        // [HttpGet("CreatePdf")]
        // [AllowAnonymous]
        // public FileStreamResult CreatePdf()
        // {
            
        // } // End Sub WriteTest 
    }
}