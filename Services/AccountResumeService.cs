using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using e_commerce.webapi.Infrastructure;
using e_commerce.webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.webapi.Services
{
    public interface IAccountResumeService
    {
        IList<AccountResume> Get(DateTime? from, DateTime? to, int clientId);
    }

    public class AccountResumeService : IAccountResumeService
    {
        private AppDbContext _context;

        public AccountResumeService(AppDbContext context)
        {
            _context = context;
        }

        public IList<AccountResume> Get(DateTime? from, DateTime? to, int clientId)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@IdCliente", clientId));
             if (from.HasValue) {
                parameters.Add(new SqlParameter("@fechaDesde", from));
            } else {
                parameters.Add(new SqlParameter("@fechaDesde", DBNull.Value));
            };
            if (to.HasValue) {
                parameters.Add(new SqlParameter("@fechaHasta", to));
            } else {
                parameters.Add(new SqlParameter("@fechaHasta", DBNull.Value));
            };
            parameters.Add(new SqlParameter("@Opcion", 1));
            parameters.Add(new SqlParameter("@OpcionCotiza", 2));

            var resumes = _context.AccountResumes.FromSql("Venta.ResumenDeMovimientosDeClientes" 
                                                        +" @IdCliente  "
                                                        +",@fechaDesde  "
                                                        +",@fechaHasta  "
                                                        +",@Opcion  "
                                                        +",@OpcionCotiza  "
                                            , parameters.ToArray())
                                            .ToList();
            return resumes;
        }
    }
}