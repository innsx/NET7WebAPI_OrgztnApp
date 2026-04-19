namespace NET7WebAPI_OrgztnApp.Application.Commons.Utilities
{
    //PageList.cs class that will return a List of Objects based on the
    //PASS-IN QueryParameters such as PageNumber, PageSize
    public class PageList<TEntity>
    {
        //PROPERTIES
        public IEnumerable<TEntity> Items { get; set; } = new List<TEntity>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecordCounts { get; set; }
        public bool HasNextPage => PageNumber * PageSize < TotalRecordCounts;
        public bool HasPreviousPage => PageNumber > 1;


        //PRIVATE CONSTRUCTOR preventing PageList.cs class from
        //being INSTANTIATED/INVOKED from outside of this class
        private PageList(IEnumerable<TEntity> entityItems, int pageNumber, int pageSize, int totalRecordCounts)
        {
            Items = entityItems;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecordCounts = totalRecordCounts;
        }

        //So we add a STATIC Create( ) METHOD that will be ACCESSABLE outside of this class
        // and are passing in THESE "PARAMETERS" 
        // then we use these "PARAMETERS" to use PageList class's CONSTRUCTOR to initialize these "PARAMETERS" 
        // and ASSIGN these "PARAMETERS" to it respect PROPERTIES
        public static PageList<TEntity> Create(IEnumerable<TEntity> entityItems, int pageNumber, int pageSize, int totalRecordCounts)
        {
            return new PageList<TEntity>(entityItems, pageNumber, pageSize, totalRecordCounts);
        }

    }
}
