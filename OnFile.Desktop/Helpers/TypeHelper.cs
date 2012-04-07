using System.ComponentModel;

namespace OnFile.Desktop.Helpers
{
    internal class TypeHelper
    {
        public static T PropertyValue<T>(object item, string propertyName)
        {
            var prop = TypeDescriptor.GetProperties(item).Find(propertyName, true);
            return (T)prop.GetValue(item);
        }
    }
}