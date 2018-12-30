using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ademund.Utils.Net
{
    public class IPUtils
    {
        private static readonly string[] _checkIPUrls =
            {
                "https://api.ipify.org",
                "https://bot.whatismyipaddress.com/",
                "https://checkip.amazonaws.com/",
                "https://icanhazip.com",
                "https://ifconfig.me/ip",
                "https://ipecho.net/plain",
                "https://ipinfo.io/ip",
                "https://wtfismyip.com/text",
            };

        /// <summary>
        /// Synchronous wrapper around <c>GetExternalIPAddressAsync</c>, usage: 
        /// <para><code>
        ///     using Ademund.Utils.Net;
        ///     string myIp = IPUtils.GetExternalIPAddress();
        /// </code></para>
        /// </summary>
        /// <param name="proxyAddress">Optional proxy address, e.g. 127.0.0.1:8888</param>
        /// <returns>A string representation of an IP Address, or null if no valid responses are received</returns>
        public static string GetExternalIPAddress(string proxyAddress = null, string[] checkIPUrls = null, int failDelay = 3000)
        {
            return GetExternalIPAddressAsync(proxyAddress, checkIPUrls, failDelay).GetAwaiter().GetResult();
        }

        /// <summary>
        /// In response to this SO question: <see cref="https://stackoverflow.com/questions/3253701/get-public-external-ip-address"/>
        /// <para>fires off parallel requests to each url defined in checkIPUrls, returns the fist valid response, usage:</para> 
        /// <para><code>
        ///     using Ademund.Utils.Net;
        ///     string myIp = await IPUtils.GetExternalIPAddressAsync();
        /// </code></para>
        /// </summary>
        /// <param name="proxyAddress">Optional proxy address, e.g. 127.0.0.1:8888</param>
        /// <returns>A string representation of an IP Address, or null if no valid responses are received</returns>
        public static async Task<string> GetExternalIPAddressAsync(string proxyAddress = null, string[] checkIPUrls = null, int failDelay = 3000)
        {
            var cts = new CancellationTokenSource();
            var tasks = new List<Task<string>>();

            IWebProxy proxy = string.IsNullOrWhiteSpace(proxyAddress) ? null : new WebProxy(proxyAddress);
            using (var client = new HttpClient(new HttpClientHandler() { Proxy = proxy, UseProxy = (proxy != null) }))
            {
                foreach (var url in checkIPUrls ?? _checkIPUrls)
                {
                    tasks.Add(CheckIP(client, url, cts.Token, failDelay));
                }

                var result = await await Task.WhenAny(tasks).ConfigureAwait(false);

                // once we get a valid response, trigger the cancellation token and cancel any pending http client requests
                cts.Cancel();
                client.CancelPendingRequests();

                return result;
            }
        }

        private static async Task<string> CheckIP(HttpClient client, string url, CancellationToken ct, int failDelay = 3000)
        {
            try
            {
                var result = await client.GetStringAsync(url).ConfigureAwait(false);

                if (ct.IsCancellationRequested)
                    return null;

                // check if we got a valid ip address
                if (IPAddress.TryParse(result.Replace("\n", "").Trim(), out IPAddress ip))
                    return ip.ToString();
            }
            catch
            {
            }

            try
            {
                // if we didn't get a valid response then wait to return to allow the other requests a chance to finish
                await Task.Delay(failDelay, ct).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
            }

            return null;
        }
    }
}