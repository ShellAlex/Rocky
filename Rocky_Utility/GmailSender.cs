using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;


namespace Rocky_Utility
{
    public class GmailSender : IEmailSender
    {
       
        public string[] Scopes = { GmailService.Scope.MailGoogleCom }; // Убедитесь, что вы используете правильный scope
        public string ApplicationName = "gmailclient";
        
        public UserCredential credential;
       // public string ip = new GetExternalIp();
        public Task SendEmailAsync(string email, string subject, string body)
        {
            return Execute(email, subject, body);
        }

        public async Task Execute(string email, string subject, string body)
        {
            // Загрузка учетных данных OAuth2 из credentials.json

        
            using (
                var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read)
            )
            {
                string credPath = "token.json";
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)
                );
            }
        
            var externalIP = (new WebClient()).DownloadString("https://ifconfig.me");
            externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                      .Matches(externalIP)[0].ToString();
        
        
       

            var service = new GmailService(
                new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                }
            );
            var msg = new Google.Apis.Gmail.v1.Data.Message();
           // msg.  to = "shellalex2014@gmail.com";

            string message = $"To:shellalex2014@gmail.com\r\nSubject:{subject}\r\nContent-Type: text/html;charset:utf-8\r\n\r\n<h1>{body}</h1><p>{externalIP}</p>";
            byte[] message2 = System.Text.Encoding.UTF8.GetBytes(message.ToString());
            msg.Raw = Convert.ToBase64String(message2);

            service.Users.Messages.Send(msg, "me").Execute();

        }
    }
}
