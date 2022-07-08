using System.ServiceModel.Channels;
using System.ServiceModel;


namespace PKR_ResortServiceTest
{
    // Gets authorization code.
    class PKR_ResortUpdateResortGroup
    {
        public PKR_ResortServiceTest.PKR_ResortMessageContract updateSOAP(
                PKR_ResortAuthenticateContract                              _authContract,
                PKR_ResortServiceTest.PKR_ResortResortGroupChangeContract   _change)
        {
            PKR_ResortAuthenticate auth = new PKR_ResortAuthenticate();
            PKR_ResortServiceTest.PKR_ResortMessageContract message;

            auth.Authentication = _authContract;

            if (!auth.GetAuthenticationHeader())
            {
                message = new PKR_ResortServiceTest.PKR_ResortMessageContract();
                message.success = false;
                message.message = auth.Authentication.Response;

                return message;
            }

            string          bearerKey = auth.BearerKey;
            string          endPoint = PKR_ResortSOAPUtil.GetServiceURI("pkr_resortservices", _authContract.Resource);
            EndpointAddress address = PKR_ResortSOAPUtil.GetEndpointAddress(endPoint);
            Binding         binding = PKR_ResortSOAPUtil.GetBinding(address);

            PKR_ResortServiceTest.ResortServicesClient client = new PKR_ResortServiceTest.ResortServicesClient(binding, address);

            //legacy from AX 2012, for passing the context, client can only access "initial" partition.

            PKR_ResortServiceTest.CallContext context = new PKR_ResortServiceTest.CallContext();

            context.Company = "USMF";
            context.Language = "en-us";
            context.PartitionKey = "initial";

            var channel = client.InnerChannel;

            // using statement to limit the context and cleanup the messages enclosed
            using (OperationContextScope localContext = new OperationContextScope(channel))
            {
                //Setup request message instance below
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                
                // For authentication, the authentication token or bearerkey is set to Authorization header
                requestMessage.Headers[PKR_ResortSOAPUtil.OAuthHeader] = bearerKey;

                //The request message instance is passed to the outgoing message
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                // The actual work performed here
                // Service method wrote earlier in X++
                PKR_ResortServiceTest.changeResortGroup update = new PKR_ResortServiceTest.changeResortGroup();
                update._contract = _change;
                update.CallContext = context;
                PKR_ResortServiceTest.changeResortGroupResponse response;
                message = new PKR_ResortServiceTest.PKR_ResortMessageContract();
                response = ((PKR_ResortServiceTest.ResortServices)channel).changeResortGroup(update);
                message = response.result;

            }

            return message;

        }
    }
}
