using System;
using System.Net;

namespace CIS580_Final
{
    internal static class WebHandler
    {
        private const string Uri = "https://blockchain.info/tobtc?currency=USD&value=1";

        internal static double GetConversionFactor()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebClient client = new WebClient { UseDefaultCredentials = true };
                string data = client.DownloadString(Uri);

                return Convert.ToDouble(data);
            }
            catch (WebException exception)
            {
                Console.WriteLine(exception.Message);
                return 0.00012;
            }
        }
    }
}
