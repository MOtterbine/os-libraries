using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace OS.IO
{
    public static class EmailerBase
    {
        /// <summary>
        /// Sends an email according to input parameters. No authentication override of SendEmail(...)
        /// </summary>
        /// <param name="smtpServer">SMTP server addres</param>
        /// <param name="smtpPort">SMTP server port</param>
        /// <param name="fromEmail">sending email address</param>
        /// <param name="toEmail">colon-separated list of email recipients</param>
        /// <param name="subject">email subject line</param>
        /// <param name="body">body text</param>
        /// <param name="logger">ILogger object. Can be null, if so, then function will throw exception containing would-be log information</param>
        /// <returns></returns>
        public static bool SendEmail(string smtpServer, int smtpPort, string fromEmail, string toEmail, string subject, string body, bool useSSL)
        {
            MailMessage mail = new MailMessage();
            SmtpClient client = null;
            try
            {
                string[] emails = toEmail.Split(new char[] { ';' });
                mail.From = new MailAddress(fromEmail);
                foreach (string emailAddress in emails)
                {
                    if (!string.IsNullOrEmpty(emailAddress))
                    {
                        mail.To.Add(emailAddress);
                    }
                }
                if (smtpPort < 1)
                {
                    client = new SmtpClient(smtpServer);
                }
                else
                {
                    client = new SmtpClient(smtpServer, smtpPort);
                }
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                // Not using authentication
                client.UseDefaultCredentials = false;

                // When using authentication
                //client.UseDefaultCredentials = true;
                //System.Net.NetworkCredential creds = new System.Net.NetworkCredential("<Some Username>", "<Some Password>");
                //client.Credentials = creds;

                // Use secure socket layer?
                client.EnableSsl = useSSL;

                mail.Subject = subject;
                mail.Body = body;
                client.Timeout = 7000;
                client.Send(mail);
                return true;
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        Console.WriteLine("Delivery failed - retrying in 5 seconds.");
                        System.Threading.Thread.Sleep(5000);
                        client.Send(mail);
                    }
                    else
                    {
                        Console.WriteLine("Failed to deliver email message to {0}",
                            ex.InnerExceptions[i].FailedRecipient);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed to send email to {0} using server: {1} on port {2} - {3}", toEmail, smtpServer, smtpPort, ex.Message), ex);
            }
            return false;
        }

        /// <summary>
        /// Sends and email where the smtp server requires authentication
        /// </summary>
        /// <param name="smtpServer"></param>
        /// <param name="smtpPort"></param>
        /// <param name="fromEmail"></param>
        /// <param name="toEmail"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool SendEmail(string smtpServer, int smtpPort, string fromEmail, string toEmail, string subject, string body, bool useSSL, string userName, string password)
        {
            MailMessage mail = new MailMessage();
            SmtpClient client = null;
            try
            {
                string[] emails = toEmail.Split(new char[] { ';' });
                mail.From = new MailAddress(fromEmail);
                foreach (string emailAddress in emails)
                {
                    if (!string.IsNullOrEmpty(emailAddress))
                    {
                        mail.To.Add(emailAddress);
                    }
                }
                if (smtpPort < 1)
                {
                    client = new SmtpClient(smtpServer);
                }
                else
                {
                    client = new SmtpClient(smtpServer, smtpPort);
                }
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                // When using authentication
                client.UseDefaultCredentials = true;
                System.Net.NetworkCredential creds = new System.Net.NetworkCredential(userName, password);
                client.Credentials = creds;

                // Use secure socket layer?
                client.EnableSsl = useSSL;

                mail.Subject = subject;
                mail.Body = body;
                client.Timeout = 7000;
                client.Send(mail);
                return true;
            }
            catch (SmtpFailedRecipientsException ex)
            {
                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                {
                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        Console.WriteLine("Delivery failed - retrying in 5 seconds.");
                        System.Threading.Thread.Sleep(5000);
                        client.Send(mail);
                    }
                    else
                    {
                        Console.WriteLine("Failed to deliver email message to {0}",
                            ex.InnerExceptions[i].FailedRecipient);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed to send email to {0} using server: {1} on port {2} - {3}", toEmail, smtpServer, smtpPort, ex.Message), ex);
            }
            return false;
        }

    }
}
