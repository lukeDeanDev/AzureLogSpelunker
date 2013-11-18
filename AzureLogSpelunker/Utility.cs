using System.Reflection;

namespace AzureLogSpelunker
{
    public static class Utility
    {
        public static PropertyInfo[] GetProperties<T>()
        {
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

    }
}
