using System.Reflection;

namespace NET7WebAPI_OrgztnApp.Domain.Commons.Utilities
{    
    public class AssociatedType 
    {
        public Type? Type { get; }
        public PropertyInfo? ForeignKeyProperty { get;  }

        public AssociatedType(Type type, PropertyInfo foreignPropertyInfo)
        {
            Type = type;
            ForeignKeyProperty = foreignPropertyInfo;
        }
    }
}
