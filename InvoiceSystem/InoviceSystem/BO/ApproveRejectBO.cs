using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace BO
{
    public class ApproveRejectBO
    {
        #region "Private Variables"
        private string _invoiceCode;

        private string _approveRejectComments;

        #endregion "Private Variables"

        #region "Public Variables"

        public string InvoiceCode
        {
            get { return _invoiceCode; }
            set { _invoiceCode = value; }

        }

        public string ApproveRejectComments
        {
            get { return _approveRejectComments; }
            set { _approveRejectComments = value; }

        }
        #endregion "Public Variables"
    }
}
