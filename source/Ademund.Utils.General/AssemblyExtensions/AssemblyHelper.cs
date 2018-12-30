using System;
using System.Reflection;

namespace Ademund.Utils.AssemblyExtensions
{
    public interface IAssemblyHelper
    {
        string GetLocationOfExecutingAssembly();
    }

    public class AssemblyHelper: IAssemblyHelper
    {
        public string GetLocationOfExecutingAssembly()
        {
            return System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
	}

	public static class AssemblyHelperMethods
	{
		public static string GetLocationOfExecutingAssembly()
		{
			return System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		}
	}
}
