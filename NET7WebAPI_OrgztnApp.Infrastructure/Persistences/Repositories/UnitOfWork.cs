using NET7WebAPI_OrgztnApp.Application.Commons.Interfaces;
using NET7WebAPI_OrgztnApp.Infrastructure.Persistences.DataContexts;

namespace NET7WebAPI_OrgztnApp.Infrastructure.Persistences.Repositories
{
    //this UnitOfWork class interacts with Database Context so we INJECTED correspondent Context class
    public class UnitOfWork : IUnitOfWork
    {
        //declare PROPERTIES and specify thier Respected Type ICompanyRepository & IEmployeeRepository
        //so we can INTERACT with the database ENTITES
        public ICompanyRepository Companies { get; set; }
        public IEmployeeRepository Employees { get; set; }
        
        //declare fields
        private bool _disposed;  //be default _disposed set to "false"
        private readonly DapperDataContext _dapperDataContext;


        public UnitOfWork(DapperDataContext dapperDataContext)
        {
            _dapperDataContext = dapperDataContext;

            Init();
        }

        public void Init()
        {
            //here we INITIALIZE properties by assigning thier respected repository class
            Companies = new CompanyRepository(_dapperDataContext);
            Employees = new EmployeeRepository(_dapperDataContext);
            //Users = new UserRepository(_dapperDataContext);
        }

        public void Commits_Transaction_N_Close_DbConnection_InvokeDispose()
        {
            _dapperDataContext.DbTransaction?.Commit();
            _dapperDataContext.DbTransaction?.Dispose();
            _dapperDataContext.DbTransaction = null;

            _dapperDataContext.DbConnection?.Close();
            _dapperDataContext.DbConnection?.Dispose();
        }

        public void Commits_Transaction_N_Invoke_Disposed()
        {
            _dapperDataContext.DbTransaction?.Commit();
            _dapperDataContext.DbTransaction?.Dispose();
            _dapperDataContext.DbTransaction = null;

        }

        // Custom cleanup logic using Dispose Pattern 
        public void Dispose()
        {
            Dispose(true); //passing an argument to Dispose(bool disposing) 
            GC.SuppressFinalize(this); // Telling the GC not to call the finalizer

        }

        protected virtual void Dispose(bool disposing) //bool disposing parameter
        {
            if (_disposed == false)
            {
                if (disposing)
                {
                    _dapperDataContext.DbTransaction?.Dispose();
                    _dapperDataContext.DbConnection?.Dispose();
                }

                _disposed = true;
            }
        }


        public void Opens_DbConnection_BeginTransaction()
        {
            _dapperDataContext.DbConnection?.Open();
            _dapperDataContext.DbTransaction = _dapperDataContext.DbConnection?.BeginTransaction();
        }

        public void Rollback_Transaction_N_Dispose()
        {
            _dapperDataContext.DbTransaction?.Rollback();
            _dapperDataContext.DbTransaction?.Dispose();
            _dapperDataContext.DbTransaction = null;
        }
    }
}
