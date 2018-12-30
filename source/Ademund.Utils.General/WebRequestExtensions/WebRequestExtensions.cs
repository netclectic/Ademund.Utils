using System;
using System.Net;
using System.IO;

namespace Ademund.Utils.WebRequestExtensions
{
	/// <summary>
	/// http://www.thomaslevesque.com/2014/01/14/tackling-timeout-issues-when-uploading-large-files-with-httpwebrequest/
	/// </summary>
	public static class WebRequestExtensions
	{
		public static Stream GetRequestStreamWithTimeout(this WebRequest request, int? millisecondsTimeout = null)
		{
			return AsyncToSyncWithTimeout(
				request.BeginGetRequestStream,
				request.EndGetRequestStream,
				millisecondsTimeout ?? request.Timeout);
		}

		public static WebResponse GetResponseWithTimeout(this HttpWebRequest request, int? millisecondsTimeout = null)
		{
			return AsyncToSyncWithTimeout(
				request.BeginGetResponse,
				request.EndGetResponse,
				millisecondsTimeout ?? request.Timeout);
		}

		private static T AsyncToSyncWithTimeout<T>(Func<AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, T> end, int millisecondsTimeout)
		{
			var iar = begin(null, null);
			if (!iar.AsyncWaitHandle.WaitOne(millisecondsTimeout))
			{
				var ex = new TimeoutException();
				throw new WebException(ex.Message, ex, WebExceptionStatus.Timeout, null);
			}
			return end(iar);
		}
	}
}