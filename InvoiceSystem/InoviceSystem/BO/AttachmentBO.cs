using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BO
{
    public class AttachmentBO
    {

        #region "Private Variables"
        private string _filename;
        private string _filepath;
        private string userid;
        private string inv_Code;
        private int attachmentID;

        #endregion "Private Variables"


        #region "Public Variables"

        public string Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        public string Filepath
        {
            get { return _filepath; }
            set { _filepath = value; }
        }

        public string Userid
        {
            get { return userid; }
            set { userid = value; }
        }

        public string Inv_Code
        {
            get { return inv_Code; }
            set { inv_Code = value; }
        }

        public int AttachmentId
        {
            get { return attachmentID; }
            set { attachmentID = value; }
        }

        #endregion "Public Variables"
    }
}
