using System;
using System.Reflection;

namespace Ademund.Utils.AssemblyExtensions
{
    public static class AssemblyExtensionMethods
    {
        public static string CodebaseUriPath(this Assembly ass)
        {
            var parts = new Uri(ass.CodeBase).AbsolutePath.Split('/');
            return string.Join("/", parts, 0, parts.Length - 1);            
        }
    }
}
