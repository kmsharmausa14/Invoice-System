using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BO
{
    public class lineitemsBO
    {
        #region "Private Variables"

        private string _invCode;
        private int _invNo;
        private string _partitemnumber;
        private string __ponumber;
        private string _poamendmentnumber;
        private string _releasenumber;
        private string _qtyshipped;
        private string _unitprice;
        private string _amount;
        private int _qtyuom;
        private string _packingslip;
        private string _billlading;
        private string _comments;
        private string _qty_ship_ind;
        private string _unit_price_ind;
        private string _amount_ind;
        #endregion "Private Variables"

        public lineitemsBO()
        {
            _partitemnumber=string.Empty;
            __ponumber = string.Empty;
            _poamendmentnumber = string.Empty;
            _releasenumber = string.Empty;
            _qtyshipped = string.Empty;
            _unitprice = string.Empty;
            _amount = string.Empty;
            _qtyuom = 0;
            _packingslip = string.Empty;
            _billlading = string.Empty;
            _comments = string.Empty;
            _qty_ship_ind = string.Empty;
            _unit_price_ind = string.Empty;
            _amount_ind = string.Empty;
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

        public string Partitemnumber
        {
            get { return _partitemnumber; }
            set { _partitemnumber = value; }
        }

        public string Ponumber
        {
            get { return __ponumber; }
            set { __ponumber = value; }
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

        public int Qtyuom
        {
            get { return _qtyuom; }
            set { _qtyuom = value; }
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

        public string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        public string QunatityShipInd
        {
            get { return _qty_ship_ind; }
            set { _qty_ship_ind = value; }
        }

        public string UnitPriceInd
        {
            get { return _unit_price_ind; }
            set { _unit_price_ind = value; }
        }

        public string AmountInd
        {
            get { return _amount_ind; }
            set { _amount_ind = value; }
        }
        #endregion "Public Variables"

    }
}
