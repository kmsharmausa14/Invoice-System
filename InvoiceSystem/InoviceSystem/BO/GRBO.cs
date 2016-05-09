using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BO
{
    public class GRBO
    {
        #region "Private Variables"

        private string _scode;
        private string _goodsrecievedid;
        private string _ponumber;
        private string _status;
        private string _location;
        private string _partitemnumber;
        private string _batch;
        private string _expirydate;
        private string _packsize;
        private string _quantity;
        private string creditOrDebit;
        #endregion "Private Variables"

        #region "Public Variables"

        public string Scode
        {
            get { return _scode; }
            set { _scode = value; }
        }

        public string Goodsrecievedid
        {
            get { return _goodsrecievedid; }
            set { _goodsrecievedid = value; }
        }

        public string Ponumber
        {
            get { return _ponumber; }
            set { _ponumber = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public string Partitemnumber
        {
            get { return _partitemnumber; }
            set { _partitemnumber = value; }
        }

        public string Batch
        {
            get { return _batch; }
            set { _batch = value; }
        }

        public string Expirydate
        {
            get { return _expirydate; }
            set { _expirydate = value; }
        }

        public string Packsize
        {
            get { return _packsize; }
            set { _packsize = value; }
        }

        public string Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public string CreditOrDebit
        {
            get { return creditOrDebit; }
            set { creditOrDebit = value; }
        }
        #endregion "Public Variables"
    }
}
