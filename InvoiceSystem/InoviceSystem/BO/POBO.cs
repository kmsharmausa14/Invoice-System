using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BO
{
    public class POBO
    {
        #region "Private Variables"

        private string _ponumber;
        private string _scode;
        private string _sname;
        private string _orderdate;
        private string _partitemnumber;
        private string _poamendmentnumber;
        private string _releasenumber;
        private string _qtyshipped;
        private string _qtyuom;
        private string _unitprice;
        private string _amount;
        private string _comments;
        private string _packingslip;
        private string _billlading;
        private string  creditOrDebit;
        #endregion "Private Variables"

        #region "Public Variables"

        public string Ponumber
        {
            get { return _ponumber; }
            set { _ponumber = value; }
        }

        public string Scode
        {
            get { return _scode; }
            set { _scode = value; }
        }

        public string Sname
        {
            get { return _sname; }
            set { _sname = value; }
        }

        public string Orderdate
        {
            get { return _orderdate; }
            set { _orderdate = value; }
        }

        public string Partitemnumber
        {
            get { return _partitemnumber; }
            set { _partitemnumber = value; }
        }

        public string Poamendmentnumber
        {
            get { return _poamendmentnumber; }
            set { _poamendmentnumber = value; }
        }

        public string Releasenumber
        {
            get { return _releasenumber; }
            set { _releasenumber = value; }
        }

        public string Qtyshipped
        {
            get { return _qtyshipped; }
            set { _qtyshipped = value; }
        }

        public string Qtyuom
        {
            get { return _qtyuom; }
            set { _qtyuom = value; }
        }

        public string Unitprice
        {
            get { return _unitprice; }
            set { _unitprice = value; }
        }


        public string Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        public string Packingslip
        {
            get { return _packingslip; }
            set { _packingslip = value; }
        }

        public string Billlading
        {
            get { return _billlading; }
            set { _billlading = value; }
        }

        public string CreditOrDebit
        {
            get { return creditOrDebit; }
            set { creditOrDebit = value; }
        }
        #endregion "Public Variables"


    }
}
