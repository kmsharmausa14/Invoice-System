using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Configuration;

using BO;
using System.Net.Mail;
using System.Net;



namespace Utitlity
{
    public class EmailHelper
    {
        public EmailHelper()
        {
        }

        public void SendEmail(EmailType emailtype, UserEmailBO eobj)
        {
            //UserEmailBO eobj=null;
            if (emailtype == EmailType.InvoiceCreated)
            {
                eobj = PopulateEmailDataToGetApproval(eobj);
                SendEmailToUSer(eobj);

            }
            else if (emailtype == EmailType.InvoiceApprovedBy)
            {
                eobj = PopulateEmailDataAfterApproval(eobj);
                SendEmailToUSer(eobj);
            }
            else if (emailtype == EmailType.InvoiceRejectedBy)
            {
                eobj = PopulateEmailDataAfterReject(eobj);
                SendEmailToUSer(eobj);
            }
            else if (emailtype == EmailType.EmailForNewUser)
            {
                eobj = SendEmailForNewUser(eobj);
                SendEmailToUSer(eobj);
            }

            else if (emailtype == EmailType.EmailForPasswordchange)
            {
                eobj = SendEmailForPasswordChange(eobj);
                SendEmailToUSer(eobj);
            }
           
        }

        
        protected void SendEmailToUSer(UserEmailBO emailbo)
        {
           // var fromAddress = new MailAddress(System.Web.Configuration.WebConfigurationManager.AppSettings["EmailFrom"],
              //   "Global Service Desk");

            //var toAddress = new MailAddress(emailbo.EmailTo, "");
            //string toAddress = new MailAddress(emailbo.EmailTo, "");
            string toAddress = emailbo.EmailTo.ToString();
            string username = System.Web.Configuration.WebConfigurationManager.AppSettings["username"].ToString();
            string Password = System.Web.Configuration.WebConfigurationManager.AppSettings["Password"];

            string subject = emailbo.Subject;
            string body = emailbo.Body;

            var smtp = new SmtpClient
            {
                Host = System.Web.Configuration.WebConfigurationManager.AppSettings["SMTPSERVER"],
                Port = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["Port"]),
                //EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                //Credentials = new NetworkCredential(username, fromPassword)
                Credentials = new NetworkCredential(username, Password)

            };
            using (var message = new MailMessage(username , toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                    smtp.Send(message);
                
            }
        }
        /// <summary>
        /// Send email notification to new user
        /// </summary>
        /// <param name="eobj"></param>
        /// <returns></returns>
        protected UserEmailBO SendEmailForNewUser(UserEmailBO eobj)
        {
            string body = string.Empty;


            body = "Dear " + eobj.UserName;
            body += "</br>";
            body += "You are added as a member of Vendor Portal   </br>";
            body += "your username is : " + eobj.UserName + "</br>";
            body += "your Password is : " + "<fore-color>" + eobj.Password + "<fore-color> " + "</br>";
            body += "</br>";
            body += "Thank You </br> Invoice Systems  ";

            eobj.Subject = "Your are added as a member of Vendor Portal";
            eobj.Body = body;
            return eobj;

        }
        /// <summary>
        /// Send email notification for password change
        /// </summary>
        /// <param name="eobj"></param>
        /// <returns></returns>
        protected UserEmailBO SendEmailForPasswordChange(UserEmailBO eobj)
        {
            string body = string.Empty;


            body = "Dear " + eobj.UserName;
            body += "</br>";
            body += "You are added as a member of Vendor Portal.</br>";
            body += "your username is : " + eobj.UserName + ".</br>";
            body += "your new  Password is : " + eobj.Password + ".</br>";
            body += "</br>";
            body += "Thank You </br> Invoice Systems  ";

            eobj.Subject = "Your new password of Vendor Portal";
            eobj.Body = body;
            return eobj;

        }

        protected UserEmailBO PopulateEmailDataToGetApproval(UserEmailBO emailbo)
        {
            string body = string.Empty;


            body = "Dear " + emailbo.Approver;
            body += "</br>";
            body += "Your Supplier  " + emailbo.Supplier + "has Created invoice  # " + emailbo.Invoicecode + " </br> Please approve.";
            body += "</br>";
            body += "Thank You </br> Invoice Systems  ";

            emailbo.Subject = "Invoice has been created # " + emailbo.Invoicecode + " is pending for approval";
            emailbo.Body = body;
            return emailbo;
        }

        protected UserEmailBO PopulateEmailDataAfterApproval(UserEmailBO emailbo)
        {
            string body = string.Empty;


            body = "Dear Supplier "; //+ emailbo.Supplier;
            body += "</br>";
            body += "Your Approver  has approved invoice  # " + emailbo.Invoicecode;
            body += "</br>";
            body += "Thank You </br> Invoice Systems  ";

            emailbo.Subject = "Invoice has been approved # " + emailbo.Invoicecode;
            emailbo.Body = body;
            return emailbo;
        }

        protected UserEmailBO PopulateEmailDataAfterReject(UserEmailBO emailbo)
        {
            string body = string.Empty;


            body = "Dear Supplier";
            body += "</br>";
            body += "Your Approver  has rejected invoice  # " + emailbo.Invoicecode;
            body += "</br>";
            body += "Thank You </br> Invoice Systems  ";

            emailbo.Subject = "Invoice has been rejected # " + emailbo.Invoicecode;
            emailbo.Body = body;
            return emailbo;
        }


    }
    public enum EmailType
    {
       InvoiceCreated,
        InvoiceApprovedBy,
        InvoiceRejectedBy,
        EmailForNewUser,
        EmailForPasswordchange
    }



}

