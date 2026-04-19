namespace NET7WebAPI_OrgztnApp.Domain.Commons.Utilities
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttribute : Attribute
    {
        public string NameValue { get; set; } = string.Empty;

        public TableNameAttribute(string nameValue)
        {
            NameValue = nameValue;
        }
    }
}
