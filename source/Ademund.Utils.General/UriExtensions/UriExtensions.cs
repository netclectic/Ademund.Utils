using System;
using System.Web;

namespace Ademund.Utils.UriExtensions
{
		/// <summary>
		/// Adds the specified parameter to the Query String.
		/// </summary>
		/// <param name="url"></param>
		/// <param name="paramName">Name of the parameter to add.</param>
		/// <param name="paramValue">Value for the parameter to add.</param>
		/// <returns>Url with added parameter.</returns>
	public static class UriExtensions
	{
		public static Uri UpdateQueryParameter(this Uri url, string paramName, string paramValue)
		{
			var uriBuilder = new UriBuilder(url);
			var query = HttpUtility.ParseQueryString(uriBuilder.Query);
			query[paramName] = paramValue;
			uriBuilder.Query = query.ToString();

			return new Uri(uriBuilder.ToString());
		}

		public static string GetQueryParameter(this Uri url, string paramName)
		{
			var uriBuilder = new UriBuilder(url);
			var query = HttpUtility.ParseQueryString(uriBuilder.Query);
			return query[paramName];
		}
	}
}