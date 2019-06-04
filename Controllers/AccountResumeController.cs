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
    public class AccountResumeController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IAccountResumeService accountResumeService;
        public AccountResumeController(IOptions<AppSettings> appSettings, IAccountResumeService accountResumeService)
        {
            _appSettings = appSettings.Value;
            this.accountResumeService = accountResumeService;
        }


        [Authorize]
        [HttpGet]
        public ActionResult Get(DateTime? from, DateTime? to)
        {
            var user = int.Parse(User.Identity.Name);
            return Ok(this.accountResumeService.Get(from,to,user));
        }
    }
        
}