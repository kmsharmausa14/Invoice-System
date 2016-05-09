using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BO
{
    public class ListOfDraftInvBO
    {
        #region "Private Variables"

        private string _supplierId;        
        private string _fromDate;
        private string _toDate;
        private string _poNumber;
        private string _invoiceNumber;
        private string _status;
        private string _userId;

        #endregion "Private Variables"

        #region "Public Variables"
        

        public string SupplierId
        {
            get { return _supplierId; }
            set { _supplierId = value; }
        }
       
        public string FromDate
        {
            get { return _fromDate; }
            set { _fromDate = value; }
        }
        
        public string ToDate
        {
            get { return _toDate; }
            set { _toDate = value; }
        }

        public string PoNumber
        {
            get { return _poNumber; }
            set { _poNumber = value; }
        }

        public string IvoiceNumber
        {
            get { return _invoiceNumber; }
            set { _invoiceNumber = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
        #endregion "Public Variables"
    }
}
