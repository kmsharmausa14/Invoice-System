using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BO
{
    public class UserInfoBO
    {
        #region "Private Variables"

        private string _PrimaryContact;
        private string _AffectedUser;
        private string _ProjectProcess;
        private string _NotifyBy;
        private string _EmailAddress;
        private string _Location;
        private string _SiteBuilding;
        private string _FloorWing;
        private string _SeatNoRoom;

        #endregion "Private Variables"

        #region "Public Properties"

        public string PrimaryContact
        {
            get { return _PrimaryContact; }
            set { _PrimaryContact = value; }
        }

        public string AffectedUser
        {
            get { return _AffectedUser; }
            set { _AffectedUser = value; }
        }
        public string ProjectProcess
        {
            get { return _ProjectProcess; }
            set { _ProjectProcess = value; }
        }
        public string NotifyBy
        {
            get { return _NotifyBy; }
            set { _NotifyBy = value; }
        }

        public string EmailAddress
        {
            get { return _EmailAddress; }
            set { _EmailAddress = value; }
        }
        public string Location
        {
            get { return _Location; }
            set { _Location = value; }
        }
        public string SiteBuilding
        {
            get { return _SiteBuilding; }
            set { _SiteBuilding = value; }
        }
        public string FloorWing
        {
            get { return _FloorWing; }
            set { _FloorWing = value; }
        }
        public string SeatNoRoom
        {
            get { return _SeatNoRoom; }
            set { _SeatNoRoom = value; }
        }

        #endregion "Public Properties"
    }
}
