using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace E_commerce.Helpers.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var member = value.GetType().GetMember(value.ToString()).FirstOrDefault();

            if (member is null)
            {
                return value.ToString();
            }

            var attribute = member.GetCustomAttribute<DisplayAttribute>();

            return attribute?.Name ?? value.ToString();
        }
    }
}
