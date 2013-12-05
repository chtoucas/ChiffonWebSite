namespace Mvc.Mailer
{
    using System.Net.Mail;
    using System.Threading.Tasks;

    public class MvcMailMessage : MailMessage
    {
        public string ViewName { get; set; }
        public string MasterName { get; set; }

        /// <summary>
        /// Sends a MailMessage using smtpClient
        /// </summary>
        /// <param name="smtpClient">leave null to use default System.Net.Mail.SmtpClient</param>
        public void Send(ISmtpClient smtpClient = null)
        {
            smtpClient = smtpClient ?? GetSmtpClient();
            using (smtpClient) {
                smtpClient.Send(this);
            }
        }

        /// <summary>
        /// Asynchronously Sends a MailMessage using smtpClient
        /// </summary>
        /// <param name="userState">The userState</param>
        /// <param name="smtpClient">leave null to use default System.Net.Mail.SmtpClient</param>
        public async Task SendAsync(object userState = null, ISmtpClient smtpClient = null)
        {
            await Task.Run(() => {
                smtpClient = smtpClient ?? GetSmtpClient();
                smtpClient.SendAsync(this, userState);
            });
        }

        public ISmtpClient GetSmtpClient()
        {
            return new SmtpClientWrapper();
        }
    }
}
