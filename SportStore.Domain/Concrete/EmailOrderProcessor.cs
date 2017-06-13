using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
	

namespace SportStore.Domain.Concrete
{
    public class Emailsettings
    {
        public string MailToAddress = "saspee@gmail.com";
        public string MailFromAddress = "saspee@gmail.com";
        public bool UseSsl = true;
        public string Username = "saspee";
        public string Password = "8ntd121dell26k";
        public string ServerName = "smtp.gmail.com";
        public int ServerPort = 465;
        public bool WriteAsFile = true;// Guarda el correo como un archivo de texto .eml
        public string FileLocation = @"c:\sport_store_email";
    }

    public class EmailOrderProcessor:IOrderProcessor
    {
        private Emailsettings emailSettings;
        public EmailOrderProcessor(Emailsettings settings)
        {
            emailSettings = settings;
             
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            using (var smtpClient =new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                                    .AppendLine("A new order has been submitted")
                                    .AppendLine("---")
                                    .AppendLine("Items:");

                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (SubTotal: {2:c}", line.Quantity, line.Product.Price, subtotal);
                }

                body.AppendFormat("Total Order Value: {o:c}", cart.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("Ship to:")
                    .AppendLine(shippingInfo.Name)
                    .AppendLine(shippingInfo.Line1)
                    .AppendLine(shippingInfo.Line2 ?? "")
                    .AppendLine(shippingInfo.Line3 ?? "")
                    .AppendLine(shippingInfo.City)
                    .AppendLine(shippingInfo.State ?? "")
                    .AppendLine(shippingInfo.Country)
                    .AppendLine(shippingInfo.Zip)
                    .AppendLine("---")
                    .AppendFormat("Gift Wrap {0}", shippingInfo.GiftWrap ? "Yes" : "No");

                MailMessage mailMessage = new MailMessage(emailSettings.MailFromAddress,
                                                        emailSettings.MailToAddress,
                                                        "New Order Submitted",
                                                        body.ToString());
                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }

                smtpClient.Send(mailMessage);
            }
        }
    }
}
