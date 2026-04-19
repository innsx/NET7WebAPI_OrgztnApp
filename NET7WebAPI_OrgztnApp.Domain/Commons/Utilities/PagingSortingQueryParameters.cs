namespace NET7WebAPI_OrgztnApp.Domain.Commons.Utilities
{
    public class PagingSortingQueryParameters
    {        
        //FIELDS with default values
        public int _maxPageSize = 50; //default values for TESTING only
        public int _pageSize = 10;     //default values for TESTING only

        //PROPERTIES 
        public int PageNumber { get; set; } = 1; //default value is 1

        public int PageSize
        {
            get
            {
                return _pageSize;
            }

            set
            {
                //returns the SMALLER integer number of "_maxPageSize or value"
                _pageSize = Math.Min(_maxPageSize, value);
            }
        }


        //for DEMO ONLY: hard-coded "asc" as DEFAULT value if SortOrder value is NOT SPECIFIED
        //OPTIONS: we can use ENUM or create a Database TABLE to hold "asc" & "desc"
        //then get value for sorting by selected sort order
        private string _sortOrder = "asc";

        // SortBy Property assigned DEFAULT "PagingOrder" value
        public string SortBy { get; set; } = "PagingOrder";
        public string SortOrder
        {
            get
            {
                return _sortOrder;
            }

            set
            {
                if (value == "asc" || value == "desc")
                {
                    _sortOrder = value;
                }
            }
        }
    }
}
