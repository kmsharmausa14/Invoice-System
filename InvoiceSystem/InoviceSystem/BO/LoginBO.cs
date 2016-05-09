using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BO
{
     public class LoginBO
    {
        #region "Private Variables"

        private string _UserId;
        private string _PWD;

        #endregion "Private Variables"


        #region "Public Properties"

        public string UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        public string Password
        {
            get { return _PWD; }
            set { _PWD = value; }
        }
        #endregion "Public Properties"
    }
}
