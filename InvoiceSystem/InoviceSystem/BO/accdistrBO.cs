using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BO
{
    public class accdistrBO
    {
        #region "Private Variables"

        private string _invCode;
        private int _invNo;
        private string _debitcredit;
        private string _generalledger;
        private string _costcenter1;
        private string _costcenter2;
        private string _wbsnumber;
        private string _amount;
        #endregion "Private Variables"

        public accdistrBO()
        {
            _debitcredit=string.Empty;
            _generalledger = string.Empty;
            _costcenter1 = string.Empty;
            _costcenter2 = string.Empty;
            _wbsnumber = string.Empty;
            _amount = string.Empty;
        }

        #region "Public Variables"

        public string InvCode
        {
            get { return _invCode; }
            set { _invCode = value; }
        }

        public int InvNo
        {
            get { return _invNo; }
            set { _invNo = value; }
        }

        public string Debitcredit
        {
            get { return _debitcredit; }
            set { _debitcredit = value; }
        }

        public string Generalledger
        {
            get { return _generalledger; }
            set { _generalledger = value; }
        }

        public string Costcenter1
        {
            get { return _costcenter1; }
            set { _costcenter1 = value; }
        }

        public string Costcenter2
        {
            get { return _costcenter2; }
            set { _costcenter2 = value; }
        }

        public string Wbsnumber
        {
            get { return _wbsnumber; }
            set { _wbsnumber = value; }
        }

        public string Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        #endregion "Public Variables"

    }
}
