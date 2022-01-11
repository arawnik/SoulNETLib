using System.ComponentModel;
using System.Reflection;

namespace SoulNETLib.Common.Extension
{
    /// <summary>
    /// Define extension methods for <see cref="Enum"/>.
    /// </summary>
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T source) where T : struct, Enum
        {
            var enumMember = source.GetType().GetMember(source.ToString()).FirstOrDefault();
            var descriptionAttribute =
                enumMember == null
                    ? default
                    : enumMember.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
            return
                descriptionAttribute == null
                    ? source.ToString()
                    : descriptionAttribute.Description;
        }
    }
}
