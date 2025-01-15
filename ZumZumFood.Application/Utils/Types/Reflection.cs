using System;
using System.Reflection;
/// <summary>
/// A static class for reflection type functions
/// </summary>
public static class Reflection
{
    
    public static void CopyProperties(this object source, object destination)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source), "Source object cannot be null.");
        if (destination == null)
            throw new ArgumentNullException(nameof(destination), "Destination object cannot be null.");

        Type typeDest = destination.GetType();
        Type typeSrc = source.GetType();

        var targetProperties = typeDest.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        .ToDictionary(p => p.Name);

        foreach (PropertyInfo srcProp in typeSrc.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!srcProp.CanRead) continue;

            if (!targetProperties.TryGetValue(srcProp.Name, out PropertyInfo targetProperty)) continue;

            if (!targetProperty.CanWrite || targetProperty.GetSetMethod(true)?.IsPrivate == true) continue;

            Type targetType = Nullable.GetUnderlyingType(targetProperty.PropertyType) ?? targetProperty.PropertyType;
            Type sourceType = Nullable.GetUnderlyingType(srcProp.PropertyType) ?? srcProp.PropertyType;

            if (!targetType.IsAssignableFrom(sourceType)) continue;

            try
            {
                targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to copy property {srcProp.Name}: {ex.Message}");
            }
        }
    }
}