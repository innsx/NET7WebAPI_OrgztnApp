namespace NET7WebAPI_OrgztnApp.Domain.Commons.Utilities
{

    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnNameAttribute : Attribute
    {
        public string NameValue { get; set; } = string.Empty;
        public ColumnNameAttribute( string name)
        {
            NameValue = name;
        }
    }
}
