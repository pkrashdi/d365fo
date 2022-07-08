using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKR_ResortServiceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string pass = "";
            ConsoleKeyInfo key;

            PKR_ResortAuthenticateContract authContract = new PKR_ResortAuthenticateContract();
     
            authContract.ApplicationId = "412e5b80-3567-4108-ad50-962510130dda";
            authContract.Resource =  "https://usnconeboxax1aos.cloud.onebox.dynamics.com/";
            Console.WriteLine("Use your Microsoft Dynamics 365 Finance account");

            Console.Write("O365 Username: ");
            authContract.UserName = Console.ReadLine();

            Console.Write("O365 Password: ");

            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    Console.Write("\b");
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);

            authContract.Password = pass;

            Console.WriteLine("\nThis is your tenant, such as usnconeboxax1aos.cloud.onebox.dynamics.com");

            string defaultDomain = authContract.UserName.Split('@').Last<string>();

            Console.WriteLine("O365 Domain: ");
            Console.Write("(" + defaultDomain + ") :");

            authContract.Domain = Console.ReadLine();

            if (authContract.Domain == "")
            {
                authContract.Domain = defaultDomain;
            }


            PKR_ResortUpdateResortGroup update = new PKR_ResortUpdateResortGroup();

            PKR_ResortServiceTest.PKR_ResortResortGroupChangeContract change = new PKR_ResortServiceTest.PKR_ResortResortGroupChangeContract();

            change.parmResortID = "RES-000000002";
            change.parmResortGroupID = "Central";
            PKR_ResortServiceTest.PKR_ResortMessageContract message;
            message = update.updateSOAP(authContract, change);

            if (message.success)
            {
                Console.WriteLine("Successfully updated, Resort: " 
                                    + change.parmResortID + ", Resort Group: " + change.parmResortGroupID);
            }
            else
            {
                Console.WriteLine(message.message);
            }

            Console.ReadLine();
        }
    }
}
