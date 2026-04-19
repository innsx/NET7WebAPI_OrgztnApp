namespace NET7WebAPI_OrgztnApp.Application.Commons.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {        
        // we ADD these PROPERTIES as REPOSITORIES as REFERENCES
        public ICompanyRepository Companies { get; set; }
        public IEmployeeRepository Employees { get; set; }


        //Transactions
        void Opens_DbConnection_BeginTransaction();
        void Commits_Transaction_N_Invoke_Disposed();
        void Commits_Transaction_N_Close_DbConnection_InvokeDispose();
        void Rollback_Transaction_N_Dispose();
    }
}
