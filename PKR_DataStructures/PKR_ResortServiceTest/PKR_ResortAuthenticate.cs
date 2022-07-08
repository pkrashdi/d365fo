using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace PKR_ResortServiceTest
{
    // The class responsible to get authentication token (brearer key) used for requests made.
    class PKR_ResortAuthenticate
    {
        public const string OAuthHeader = "Authorization";
        string bearerkey;
        public PKR_ResortAuthenticateContract Authentication { get; set; }
        public string BearerKey
        {
            get { return bearerkey; }
        }

        public string GetSecurityURI()
        {

            UriBuilder uri = new UriBuilder("https://login.microsoftonline.com/" + Authentication.Domain);

            return uri.ToString();
        }

        public UserPasswordCredential GetCredential()
        {
            string uri = this.GetSecurityURI();
            UserPasswordCredential credential = new UserPasswordCredential(Authentication.UserName, Authentication.Password);

            return credential;
        }

        private AuthenticationResult GetAuthorization()
        {
            UserPasswordCredential credential = GetCredential();

            AuthenticationContext context = new AuthenticationContext(GetSecurityURI());

            Task<AuthenticationResult> task = context.AcquireTokenAsync(Authentication.Resource.TrimEnd('/'),
                                                                        Authentication.ApplicationId,
                                                                        credential);
            task.Wait();

            return task.Result;
        }

        public Boolean GetAuthenticationHeader()
        {
            AuthenticationResult result;

            try
            {
                result = GetAuthorization();
                bearerkey = result.CreateAuthorizationHeader();

            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    Authentication.Response = "Authentication failed: " + e.InnerException.Message;
                }
                else
                {
                    Authentication.Response = "Authentication failed: " + e.Message;
                }

                return false;
            }

            Authentication.Response = "OK";

            return true;
        }

    }
}
