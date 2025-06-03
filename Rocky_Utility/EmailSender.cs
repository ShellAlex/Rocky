//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text.RegularExpressions;
//using MailKit.Net.Smtp;

namespace Rocky_Utility;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;
    public MailTrapSettings _mailTrapSettings{get;set;}

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return Execute(email,subject,htmlMessage);
    }

    public async Task Execute(string email, string subject, string body){
        _mailTrapSettings = _configuration.GetSection("MailTrap").Get<MailTrapSettings>();
        

        
        var client = new System.Net.Mail.SmtpClient("live.smtp.mailtrap.io", 587)
        {
            Credentials = new NetworkCredential("api", _mailTrapSettings.ApiKey),
            EnableSsl = true
        };

    
    string ip = getExternalIp();

    client.Send("hello@demomailtrap.com", "shellalex2014@gmail.com", subject, $"{ip} {body}");
   // System.Console.WriteLine("Sent");

}

    

    public static string getExternalIp()
{
    try
    {
        string externalIP;
        externalIP = (new WebClient()).DownloadString("https://ifconfig.me");
        externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                      .Matches(externalIP)[0].ToString();
        return externalIP;
    }
    catch { return null; }
}


}


