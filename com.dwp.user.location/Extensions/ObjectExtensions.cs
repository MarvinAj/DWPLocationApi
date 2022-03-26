
namespace com.dwp.user.location.Extensions
{
    public static class ObjectExtensions
    {
        public static T ThrowIfNull<T>(this T obj, string propertyName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(propertyName);
            }

            return obj;
        }
    }
}
