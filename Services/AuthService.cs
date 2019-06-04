using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using e_commerce.webapi.Infrastructure;
using e_commerce.webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.webapi.Services
{
    public interface IAuthService
    {
        ClienteLogin Authenticate(string username, string password);
        CustomerDetail Me(int userID);
        IEnumerable<UserLogin> GetAll();
        UserLogin GetById(int id);
        EnterpriseImage GetLogo();
    }

    public class AuthService : IAuthService
    {
        private AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public ClienteLogin Authenticate(string username, string password)
        {
            if (isValid(new UserLogin() { Login = username, Pass = password }))
            {
                try
                {

                    var parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter("Login", username));
                    parameters.Add(new SqlParameter("Pass", password));


                    var user = _context.Users.FromSql("Venta.ClienteLogin @Login, @Pass", parameters.ToArray()).FirstOrDefault();
                    // check if username exists
                    if (user == null)
                        throw new ArgumentException("Usuario o contraseña incorrectos");
                    return user;
                }catch( Exception e){
                    throw new ArgumentException("Usuario o contraseña incorrectos");
                }
            }
            return null;
        }


        public CustomerDetail Me(int userID)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@IdCliente", userID));


            var user = _context.Me.FromSql("Web.ClienteDetalleWebGet @IdCliente", parameters.ToArray()).FirstOrDefault();
            // check if username exists
            return user;
        }


        public EnterpriseImage GetLogo()
        {
            var empImg = _context.EmpresaImagen.FromSql("Sistema.EmpresaImagenesGet" ).FirstOrDefault();
            empImg.ImageB64 = System.Convert.ToBase64String(empImg.Image);
            empImg.Image = null;
            // check if username exists
            return empImg;
        }

        public IEnumerable<UserLogin> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public UserLogin GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        private bool isValid(UserLogin user)
        {
            var isValid = true;
            if (string.IsNullOrWhiteSpace(user.Pass) || string.IsNullOrWhiteSpace(user.Login))
                throw new ArgumentNullException("El usuario y contraseña son campos requeridos");

            if (user.Pass.Length < 5)
                throw new ArgumentException("La contraseña sebe tener mas de 5 caracteres");

            return isValid;
        }
    }
}