using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BO
{
    public class UserEmailBO
    {
        string emailFrom;
        string emailTo;
        string subject;
        string body;
        string supplier;
        string approver;
        string invoicecode;
        string username;
        string password;

        public string UserName
        {
            get { return username; }
            set { username = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string Invoicecode
        {
            get { return invoicecode; }
            set { invoicecode = value; }
        }

        public string EmailFrom
        {
            get { return emailFrom; }
            set { emailFrom = value; }
        }


        public string EmailTo
        {
            get { return emailTo; }
            set { emailTo = value; }
        }


        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }


        public string Body
        {
            get { return body; }
            set { body = value; }
        }
        public string Supplier
        {
            get { return supplier; }
            set { supplier = value; }
        }
        public string Approver
        {
            get { return approver; }
            set { approver = value; }
        }
    }
}
