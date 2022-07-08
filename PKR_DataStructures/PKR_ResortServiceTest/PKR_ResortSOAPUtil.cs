using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace PKR_ResortServiceTest
{
    class PKR_ResortSOAPUtil
    {
        public const string OAuthHeader = "Authorization";

        public static string GetServiceURI( string _service, string _d365OURI)
        {
            string serviceName = _service.Split('.').Last();

            if (serviceName == "")
            {
                serviceName = _service;
            }

            return _d365OURI.TrimEnd('/') + "/soap/services/" + serviceName;
        }

        public static EndpointAddress GetEndpointAddress(string _uri)
        {
            EndpointAddress address = new EndpointAddress(_uri);
            return address;
        }

        public static Binding GetBinding(EndpointAddress _address)
        {
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
            binding.ReceiveTimeout = TimeSpan.MaxValue;
            binding.SendTimeout = TimeSpan.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;

            var httpsBindingElement = binding.CreateBindingElements().OfType<HttpsTransportBindingElement>().FirstOrDefault();

            if (httpsBindingElement != null)
            {
                httpsBindingElement.MaxPendingAccepts = 10000;
            }

            return binding;
        }
    }
}
