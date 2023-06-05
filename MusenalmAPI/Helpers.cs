public static class ReflectionHelpers {
    
    public static IEnumerable<Type> GetAllTypesThatImplementInterface<T>() {
        return System.Reflection.Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface);
    }

    public static IQueryable<object> Set(Microsoft.EntityFrameworkCore.DbContext context, Type t) {
        return (IQueryable<object>)context.GetType()
            .GetMethod("Set")?
            .MakeGenericMethod(t)
            .Invoke(context, null);

    }
}