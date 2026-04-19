namespace NET7WebAPI_OrgztnApp.Domain.Commons.Utilities
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NavigationAttribute : Attribute
    {
        public Type? AssociatedType { get; }
        public string AssociatedProperty { get; set; } = string.Empty;

        public NavigationAttribute(Type associatedType, string associatedProperty) 
        { 
            this.AssociatedType = associatedType;
            this.AssociatedProperty = associatedProperty;
        }
    }
}
