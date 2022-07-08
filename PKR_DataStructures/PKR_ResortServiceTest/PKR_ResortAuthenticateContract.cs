using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace PKR_ResortServiceTest
{
    //The contract class passes data to authenticate class.
    class PKR_ResortAuthenticateContract
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public string ApplicationId { get; set; }
        public string Resource { get; set; }
        public string Response { get; set; }
        
    }
}
