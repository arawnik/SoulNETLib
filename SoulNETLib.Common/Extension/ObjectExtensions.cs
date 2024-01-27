using System.Reflection;

namespace SoulNETLib.Common.Extension
{
    /// <summary>
    /// Define extension methods for <see cref="object"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Check if property exists in <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T">Type of object that is checked.</typeparam>
        /// <param name="source">The source from which we check.</param>
        /// <param name="name">Name of the property we check.</param>
        /// <param name="caseSensitive">If true, check case sensitive. Otherwise case insensitive.</param>
        /// <returns>True if property exists, false otherwise.</returns>
        public static bool HasProperty<T>(this T source, string name, bool caseSensitive = true)
        {
            if (source == null)
                return false;
            
            return source.GetType().HasProperty(name, caseSensitive);
        }

        /// <summary>
        /// Check if property exists in <paramref name="source"/> <see cref="Type"/>.
        /// </summary>
        /// <param name="source">The <see cref="Type"/> of class that is checked.</param>
        /// <param name="name">Name of the property that is checked.</param>
        /// <param name="caseSensitive">If true, check case sensitive. Otherwise case insensitive.</param>
        /// <returns>true if property exists, false otherwise.</returns>
        public static bool HasProperty(this Type source, string name, bool caseSensitive = true)
        {
            try
            {
                return source.GetProperty(name, caseSensitive
                    ? BindingFlags.Public | BindingFlags.Instance 
                    : BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
                    ) != null;
            }
            catch (AmbiguousMatchException)
            {
                // There is more than one result
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Copy property values from <paramref name="source"/> into <paramref name="destination"/>.
        /// </summary>
        /// <typeparam name="TDestination">The type of <paramref name="destination"/>.</typeparam>
        /// <typeparam name="TSource">The type of <paramref name="source"/>.</typeparam>
        /// <param name="destination">The destination where we copy the values.</param>
        /// <param name="source">The source from which we copy the values.</param>
        public static void CopyPropertyValuesFrom<TDestination, TSource>(this TDestination destination, TSource source)
        {
            if (source == null || destination == null)
            {
                return;
            }

            var sourceProps = source.GetType().GetProperties().Where(x => x.CanRead).ToList();
            var destProps = destination.GetType().GetProperties()
                .Where(x => x.CanWrite)
                .ToList();

            foreach (var sourceProp in sourceProps)
            {
                if (destProps.Any(x => x.Name == sourceProp.Name))
                {
                    var p = destProps.First(x => x.Name == sourceProp.Name);
                    if (p.CanWrite)
                    { // check if the property can be set or no.
                        p.SetValue(destination, sourceProp.GetValue(source, null), null);
                    }
                }

            }
        }
    }
}
