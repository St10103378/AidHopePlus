
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Net;

namespace AidHopePlus.Services
{
    public class EmailService
    {
        private const string SenderEmail = "thatomotha40@gmail.com";

        private const string AppPassword = "nlfd cldg dfoq jxnc";

        public Task<bool> IsConfiguredAsync()
        {
            return Task.FromResult(
                !string.IsNullOrWhiteSpace(SenderEmail) &&
                !string.IsNullOrWhiteSpace(AppPassword));
        }

        public async Task<bool> SendWelcomeEmailAsync(
            string recipientEmail,
            string fullName,
            string username,
            string password)
        {
            if (string.IsNullOrWhiteSpace(recipientEmail))
                throw new ArgumentException("Recipient email is required.");

            string safeName = WebUtility.HtmlEncode(fullName);
            string safeUsername = WebUtility.HtmlEncode(username);
            string safePassword = WebUtility.HtmlEncode(password);

            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress("AidHopePlus", SenderEmail));

            message.To.Add(
                new MailboxAddress(fullName, recipientEmail));

            message.Subject = "Welcome to AidHopePlus!";

            var body = new BodyBuilder
            {
                HtmlBody = $@"
                <html>
                <body style='font-family:Arial,sans-serif;
                             color:#333;
                             line-height:1.6;'>

                    <div style='max-width:600px;
                                margin:auto;
                                padding:25px;
                                border:1px solid #ddd;
                                border-radius:10px;'>

                        <h2 style='color:#246B4A;'>
                            Welcome to AidHopePlus!
                        </h2>

                        <p>Hello {safeName},</p>

                        <p>
                            Your AidHopePlus account has been
                            successfully created.
                        </p>

                        <p><strong>Your account details:</strong></p>

                        <p>
                            Username: <strong>{safeUsername}</strong>
                        </p>

                        <p>
                            Password: <strong>{safePassword}</strong>
                        </p>

                        <p>
                            You can now sign in to the
                            AidHopePlus Management System.
                        </p>

                        <p>
                            If you need assistance, please contact
                            your system administrator.
                        </p>

                        <br/>

                        <p>
                            Kind regards,<br/>
                            <strong>AidHopePlus Administration</strong>
                        </p>

                    </div>
                </body>
                </html>",

                TextBody = $@"
Welcome to AidHopePlus!

Hello {fullName},

Your AidHopePlus account has been successfully created.

Username: {username}
Password: {password}

Kind regards,
AidHopePlus Administration"
            };

            message.Body = body.ToMessageBody();

            using var smtp = new SmtpClient();

            try
            {
                // Workaround for mobile devices unable to reach
                // the certificate revocation (OCSP/CRL) service.
                smtp.CheckCertificateRevocation = false;

                await smtp.ConnectAsync(
                    "smtp.gmail.com",
                    587,
                    SecureSocketOptions.StartTls);

                await smtp.AuthenticateAsync(
                    SenderEmail,
                    AppPassword.Replace(" ", ""));

                await smtp.SendAsync(message);

                await smtp.DisconnectAsync(true);

                return true;
            }
            catch
            {
                if (smtp.IsConnected)
                    await smtp.DisconnectAsync(true);

                throw;
            }
        }
    }
}
