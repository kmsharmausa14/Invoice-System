using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BO
{
    public class SupplierDetailsBO
    {
        #region "Private Variables"

        private string _sCode;
        private string _sName;
        private string _sAdd1;
        private string _sAdd2;
        private string _sAdd3;
        private string _sStreet;
        private string _sCity;
        private string _sState;
        private string _sCountry;
        private string _sPostcode;
        private string _sTele;
        private string _sFax;
        private string _sEmailId;
        private string _sCurrency;

        #endregion "Private Variables"


        #region "Public Properties"
        public string Code
        {
            get { return _sCode; }
            set { _sCode = value; }
        }

        public string Name
        {
            get { return _sName; }
            set { _sName = value; }
        }

        public string Address1
        {
            get { return _sAdd1; }
            set { _sAdd1 = value; }
        }

        public string Address2
        {
            get { return _sAdd2; }
            set { _sAdd2 = value; }
        }

        public string Address3
        {
            get { return _sAdd3; }
            set { _sAdd3 = value; }
        }

        public string Street
        {
            get { return _sStreet; }
            set { _sStreet = value; }
        }

        public string City
        {
            get { return _sCity; }
            set { _sCity = value; }
        }

        public string State
        {
            get { return _sState; }
            set { _sState = value; }
        }

        public string Country
        {
            get { return _sCountry; }
            set { _sCountry = value; }
        }

        public string PostCode
        {
            get { return _sPostcode; }
            set { _sPostcode = value; }
        }

        public string Tele
        {
            get { return _sTele; }
            set { _sTele = value; }
        }

        public string Fax
        {
            get { return _sFax; }
            set { _sFax = value; }
        }

        public string EmailId
        {
            get { return _sEmailId; }
            set { _sEmailId = value; }
        }

        public string Currency
        {
            get { return _sCurrency; }
            set { _sCurrency = value; }
        }

        #endregion "Public Properties"


    }
}
