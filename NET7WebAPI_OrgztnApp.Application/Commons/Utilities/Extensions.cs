using NET7WebAPI_OrgztnApp.Domain.Commons.Utilities;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NET7WebAPI_OrgztnApp.Application.Commons.Utilities
{
    public static class Extensions
    {
        public static string GetDbTableName(this Type type)
        {
            return type.GetCustomAttribute<TableNameAttribute>()?.NameValue ?? string.Empty;
        }

        public static string GetDbTableColumnNames(this Type type, string[] selectedProperties)
        {
            if (selectedProperties.Length < 1)
            {
                return string.Join(",", 
                                    type.GetProperties()
                                        .Select(p => p.GetDbColumnName())
                                  
                ).TrimEnd(',');
            }
            else
            {
                return string.Join(",",
                                   type.GetProperties()
                                       .Where(p => selectedProperties
                                            .ToLowerInvariant()
                                            .Contains(p.Name.ToLowerInvariant())
                                       )
                                        .Select(p => p.GetDbColumnName())
                                              
                ).TrimEnd(',');
            }
        }

        public static string GetColumnValuesForInsert<T>(this Type type, T obj)
        {
            return string.Join(",", type.GetColumnProperties().Select(p => $"'{p.GetValue(obj)}'"));
        }

        public static string GetColumnValuesForUpdate<T>(this Type type, T obj)
        {
            return string.Join(",", 
                                type.GetNonPrimaryKeyColumnProperties()
                                    .Select(p => $"{p.GetDbColumnName()} = '{p.GetValue(obj)}'"));
        }

        public static string GetDbColumnName(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<ColumnNameAttribute>()?.NameValue ?? string.Empty;
        }

        public static IEnumerable<string> ToLowerInvariant(this string[] source)
        {
            foreach (var item in source)
            {
                yield return item.ToLowerInvariant();
            }
        }

        public static IEnumerable<PropertyInfo> GetColumnProperties(this Type type)
        {
            return type.GetProperties().Where(p => p.GetCustomAttribute<NavigationAttribute>() is null);
        }

        public static IEnumerable<PropertyInfo> GetNonPrimaryKeyColumnProperties(this Type type)
        {
            return type.GetProperties()
                        .Where(p => p.GetCustomAttribute<PrimaryKeyAttribute>() is null 
                                 && p.GetCustomAttribute<NavigationAttribute>() is null
                              );
        }


        public static IEnumerable<AssociatedType> GetAssociatedTypes(this Type type)
        {
            var attributes = type.GetProperties()
                                    .Where(p => p.GetCustomAttribute<NavigationAttribute>() is not null)
                                    .Select(p => p.GetCustomAttribute<NavigationAttribute>());

            foreach (var item in attributes)
            {
                yield return new AssociatedType(item!.AssociatedType!,
                                                item.AssociatedType!.GetProperty(item.AssociatedProperty)!
                );
            }
        }

        public static string GetDistinguishingUniqueKeyName(this Type type)
        {
            return type.GetProperties()
                        .Where(p => p.GetCustomAttribute<DistinguishingUniqueKeyAttribute>() is not null)
                        .FirstOrDefault()!.Name;
        }


        //we add a OrderByCustom() method & use EXPRESSION TREE with REFLECTION to achieve SORTING
        public static IQueryable<IDbEntity> OrderByCustom<IDbEntity>(this IQueryable<IDbEntity> items, string sortBy, string sortOrder)
        {
            //is used for reflection to obtain metadata about a specific class or structure
            var type = typeof(IDbEntity);

            //is used in Expression Trees (typically found in System.Linq.Expressions)
            //to create a ParameterExpression node
            //Purpose: It creates a named parameter to be used within
            //a dynamically constructed lambda expression
            //(e.g., in a Where clause or when dynamically accessing properties)
            var parameterExpression = Expression.Parameter(type, "t");

            //Purpose: It searches for a public property with the name matching the string variable sortBy
            var property = type.GetProperty(sortBy);

            if (property != null)
            {
                //This line of code is used in C# Expression Trees to
                //programmatically create a MemberExpression that accesses a specific property or
                //field on an object
                //It is commonly used when building dynamic queries (e.g., in LINQ to Entities or EF Core)
                //where the property name is not known until runtime.
                var memberExpression = Expression.MakeMemberAccess(parameterExpression, property!);

                // uses the Expression.Lambda Method from the System.Linq.Expressions namespace
                // to create a LambdaExpression at runtime.
                //Expression.Lambda: A factory method used to create a LambdaExpression node
                //in an expression tree.
                var lambdaExpression = Expression.Lambda(memberExpression, parameterExpression);

                //Purpose: It generates a node in an expression tree representing a method call,
                //such as object.Method() or StaticMethod().
                //Usage: It is heavily used in building dynamic LINQ expressions
                //where the method to call is only known at runtime, such as calling
                //.Where() or .ToString() dynamically.
                var methodCallExpression = Expression.Call
                (
                    //typeof(Queryable) refers to the System.Type object representing the System.Linq.Queryable static class.
                    //used to obtain the System.Type object for the static System.Linq.Queryable class
                    typeof(Queryable), 
                    
                    sortOrder == "desc" ? "OrderByDescending" : "OrderBy",

                    // is a C# expression used to create a new array of Type objects
                    // containing exactly two elements.
                    // This is a common pattern in Reflection for identifying specific methods
                    // or constructors that require these types as parameters.
                    //type: Refers to a variable or parameter of type System.Type already defined in your scope.
                    //property: A PropertyInfo object representing a property of a class.
                    new Type[] { type, property!.PropertyType },

                    //These expressions enable dynamic data retrieval, arithmetic calculations,
                    //and conditional logic, often allowing for the automation of tasks,
                    //such as creating derived values, formatting dates, or updating databases.
                    //Power Automate item(): Used within loops ("apply to each") to reference
                    //and act upon specific properties of the current item being processed.
                    items.Expression,

                    //It acts as a "quote" operator, telling a LINQ provider to treat a nested lambda
                    //as a data structure (expression tree) rather than as executable code
                    //or a constant, enabling dynamic LINQ queries, especially with Invoke
                    //Purpose: It is essential when constructing expression trees that
                    //contain other lambda expressions (e.g., Invoke, Select, Where)
                    //to ensure they are interpreted correctly by providers like Entity Framework.
                    Expression.Quote(lambdaExpression)
                );

                return items.Provider.CreateQuery<IDbEntity>(methodCallExpression);
            }

            return items;
        }

        public static bool IsValidEmail(this string email)
        {
            string emailExpression = @"^\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b$";
            var emailRegex = new Regex(emailExpression);

            return emailRegex.IsMatch(email);
        }

        public static bool IsValidPassword(this string password)
        {
            var passwordExpression = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*()-+])(?=\S+$).{8,}$";
            var passwordRegex = new Regex(passwordExpression);

            return passwordRegex.IsMatch(password);
        }

    }
}



//var firstCharToUpper = char.ToUpper(sortBy[0]) + sortBy.Substring(1);
//var property = type.GetProperty(firstCharToUpper);

//PropertyInfo[] properties = typeof(Employee).GetProperties();

//foreach (PropertyInfo propertyInfo in properties)
//{
//    if (propertyInfo.Name.ToLower() == sortBy.ToLower())
//    {
//        property = propertyInfo;
//    }
//}