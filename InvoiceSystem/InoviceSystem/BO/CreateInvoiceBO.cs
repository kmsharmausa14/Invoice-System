using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace BO
{
    public class CreateInvoiceBO
    {

        #region "Private Variables"

        private string _supplliercode;
        private string _invoicenumber;
        private int _shippedto;
        private string _invoiceto;
        private int _payableto;
        private string _supplieraddress;// not sent to sp
        private string _emailaddress;// not sent to sp
        private string _finaldestination;
        private string _currency;// not sent to sp
        private string _comments;
        private string _invoicedate;
        private string _shippeddate;
        private int  _shippedvia;
        private string _attchments;
        private ArrayList _lineitemslist;
        private ArrayList _addchrgelist;
        private ArrayList _accdistrlist;
        private string _totalline;
        private string _totalladdcharge;
        private string _totalamt;
        private string _createdby;
        private string _modifiedby;
        private string draftno;
        private string creditOrdebit;
        private string _totallineind;
        private string _totalladdchargeind;
        private string _totalamtind;
        private string _shippedViaMeans;
        private string _ShippedViaOther;


        #endregion "Private Variables"

        #region "Public Variables"

        public string Suppliercode
        {
            get { return _supplliercode; }
            set { _supplliercode = value; }

        }

        public string Invoicenumber
        {
            get { return _invoicenumber; }
            set { _invoicenumber = value; }
        }

        public int Shippedto
        {
            get { return _shippedto; }
            set { _shippedto = value; }
        }

        public string Invoiceto
        {
            get { return _invoiceto; }
            set { _invoiceto = value; }
        }

        public int Payableto
        {
            get { return _payableto; }
            set { _payableto = value; }
        }

        public string Supplieraddress
        {
            get { return _supplieraddress; }
            set { _supplieraddress = value; }
        }

        public string Emailaddress
        {
            get { return _emailaddress; }
            set { _emailaddress = value; }
        }

        public string Finaldestination
        {
            get { return _finaldestination; }
            set { _finaldestination = value; }
        }

        public string Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }

        public string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        public string Invoicedate
        {
            get { return _invoicedate; }
            set { _invoicedate = value; }
        }

        public string Shippeddate
        {
            get { return _shippeddate; }
            set { _shippeddate = value; }
        }

        public int Shippedvia
        {
            get { return _shippedvia; }
            set { _shippedvia = value; }
        }

        public string ShippedViaMeans
        {
            get { return _shippedViaMeans; }
            set { _shippedViaMeans = value; }
        }

        public string ShippedViaOther
        {
            get { return _ShippedViaOther; }
            set { _ShippedViaOther = value; }
        }

        public string Attchments
        {
            get { return _attchments; }
            set { _attchments = value; }
        }

        public ArrayList LineItemslist
        {
            get { return _lineitemslist; }
            set { _lineitemslist = value; }
        }

        public ArrayList Addchrgelist
        {
            get { return _addchrgelist; }
            set { _addchrgelist = value; }
        }

        public ArrayList Accdistrlist
        {
            get { return _accdistrlist; }
            set { _accdistrlist = value; }
        }

        public string totalline
        {
            get { return _totalline; }
            set { _totalline = value; }
        }

        public string Totalladdcharge
        {
            get { return _totalladdcharge; }
            set { _totalladdcharge = value; }
        }

        public string Totalamt
        {
            get { return _totalamt; }
            set { _totalamt = value; }
        }

        public string CreatedBy
        {
            get { return _createdby; }
            set { _createdby = value; }
        }

        public string ModifiedBy
        {
            get { return _modifiedby; }
            set { _modifiedby = value; }
        }

        public string Draftno
        {
            get { return draftno; }
            set { draftno = value; }
        }

        public string CreditOrDebit
        {
            get { return creditOrdebit; }
            set { creditOrdebit = value; }
        }

        public string TotalLineInd
        {
            get { return _totallineind; }
            set { _totallineind = value; }
        }

        public string TotalAddChargeInd
        {
            get { return _totalladdchargeind; }
            set { _totalladdchargeind = value; }
        }

        public string TotalAmountInd
        {
            get { return _totalamtind; }
            set { _totalamtind = value; }
        }

        #endregion "Public Variables"
       
    }

}
