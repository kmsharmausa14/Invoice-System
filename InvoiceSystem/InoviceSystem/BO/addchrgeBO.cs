using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BO
{
    public class addchrgeBO
    {
        #region "Private Variables"
        private string _invCode;
        private int _invNo;
        private string _chargenumber;
        private int _chargetype;
        private string _charge;
        private string _chrgeamount;
        private string _chrgedescp;
        private string _gst;
        #endregion "Private Variables"

        public addchrgeBO()
        {
             _chargenumber=string.Empty;
             _chargetype = 0;
             _charge = string.Empty;
             _chrgeamount = string.Empty;
             _chrgedescp = string.Empty;
             _gst = string.Empty;
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
        public string Chargenumber
        {
            get { return _chargenumber; }
            set { _chargenumber = value; }
        }

        public int Chargetype
        {
            get { return _chargetype; }
            set { _chargetype = value; }
        }

        public string Charge
        {
            get { return _charge; }
            set { _charge = value; }
        }

        public string Chrgeamount
        {
            get { return _chrgeamount; }
            set { _chrgeamount = value; }
        }

        public string Chrgedescp
        {
            get { return _chrgedescp; }
            set { _chrgedescp = value; }
        }

        public string Gst
        {
            get { return _gst; }
            set { _gst = value; }
        }

        #endregion "Public Variables"

    }
}
