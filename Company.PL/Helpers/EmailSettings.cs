using System.Net;
using System.Net.Mail;

namespace Company.PL.Helpers
{
    public class EmailSettings
    {
        public static bool SendEmail(Email email)
        {
            // Mail Server : Gmail
            // Protocol : SMTP
            // Port : 587
            // SSL : true

            try{
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("moatef702@gmail.com", "zudjiszlmlznmqbl\r\n"); // Sender
                client.Send("moatef702@gmail.com", email.To, email.Subject, email.Body);
               
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }
    }
}
