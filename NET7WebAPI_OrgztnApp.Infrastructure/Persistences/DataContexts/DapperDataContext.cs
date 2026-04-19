using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace NET7WebAPI_OrgztnApp.Infrastructure.Persistences.DataContexts
{
    //this class references IConfigurtion to access ConnectionString
    //this class references IDbConnection to access to DataSource(SQL Server)
    public sealed class DapperDataContext
    {
        //READONLY fields
        private readonly IConfiguration _configuration;
        private readonly string? _dbConnStrng;

        //fields
        private IDbConnection? _dbConnection;
        private IDbTransaction? _dbTransaction;

        public DapperDataContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnStrng = _configuration.GetConnectionString("DbConnStrg");
        }


        public IDbConnection? DbConnection //getting DbConnection property value
        {
            get 
            {
                if (_dbConnection == null || _dbConnection.State != ConnectionState.Open) 
                {
                    _dbConnection = new SqlConnection(_dbConnStrng);
                }
                
                return _dbConnection; 
            }
        }

        public IDbTransaction? DbTransaction //Getting & Setting DbTransaction property
        {
            get 
            { 
                return _dbTransaction; 
            }

            set
            {
                _dbTransaction = value;
            }
        }
    }
}
