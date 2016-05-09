using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace BLL
{
    public class AdminBLL
    {
        public DataSet GetUserDetails(string parameter, int criteria)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;
            DataSet ds = new DataSet();
            if (criteria == 1)
            {
                param = new SqlParameter();
                param.ParameterName = "@user_id";
                param.DbType = DbType.String;
                param.Value = parameter;
                lstParam.Add(param);

                bool abc = false;
                ds = new DAL.SqlHelper().SelectDataSet("[dbo].[sp_searchbyuserid]", lstParam, abc);
            }
            if (criteria == 2)
            {
                param = new SqlParameter();
                param.ParameterName = "@Descrip";
                param.DbType = DbType.String;
                param.Value = parameter;
                lstParam.Add(param);

                bool abc = false;
                ds = new DAL.SqlHelper().SelectDataSet("[dbo].[sp_searchbyroledescription]", lstParam, abc);
            }
            if (criteria == 0)
            {
                bool abc = false;
                ds = new DAL.SqlHelper().SelectDataSet("[dbo].[sp_searchbyshowall]", null, abc);
            }



            return ds;
        }

        public bool DeleteUser(string UserId)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@user_id";
            param.DbType = DbType.String;
            param.Value = UserId;
            lstParam.Add(param);

            bool abc = false;
            bool status;
            status = new DAL.SqlHelper().UpdateData("[dbo].[sp_Deleteuserid]", lstParam, abc);

            return status;
        }

        public int IfExists(string UserID)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@UserID";
            param.DbType = DbType.String;
            param.Value = UserID;
            lstParam.Add(param);

            bool abc = false;
            int i = Convert.ToInt32(new DAL.SqlHelper().SelectValue("[dbo].[sp_Deleteuserid]", lstParam, abc));

            return i;
        }

        public DataSet Gets_code()
        {
            DataSet i = new DAL.SqlHelper().SelectDataSet("Select s_code from tbl_supplier ", null, true);

            return i;
        }

        public bool UpdateUser(ArrayList userdtls)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@UserID";
            param.DbType = DbType.String;
            param.Value = userdtls[0].ToString();
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@Roleid";
            param.DbType = DbType.Int32;
            param.Value = Convert.ToInt32(userdtls[1].ToString());
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@password";
            param.DbType = DbType.String;
            param.Value = userdtls[2].ToString();
            lstParam.Add(param);

            bool abc = false;
            bool status = new DAL.SqlHelper().UpdateData("[dbo].[sp_IFUserExists]", lstParam, abc);

            return status;

        }

        public int AddUser(string userid, int roleid, string email, string name, string contact)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@user_id";
            param.DbType = DbType.String;
            param.Value = userid;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@name";
            param.DbType = DbType.String;
            param.Value = name;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@role_id";
            param.DbType = DbType.Int32;
            param.Value = roleid;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@email";
            param.DbType = DbType.String;
            param.Value = email;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@contact";
            param.DbType = DbType.String;
            param.Value = contact;
            lstParam.Add(param);

            int success = new DAL.SqlHelper().ReturnValue("[dbo].[sp_addnewuser]", lstParam);

            return success;
        }

        public int AddUserSupplier(string userid, int roleid,string s_code)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@user_id";
            param.DbType = DbType.String;
            param.Value = userid;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@role_id";
            param.DbType = DbType.Int32;
            param.Value = roleid;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@s_code";
            param.DbType = DbType.String;
            param.Value = s_code;
            lstParam.Add(param);

            int success = new DAL.SqlHelper().ReturnValue("[dbo].[sp_addnewusersupplier]", lstParam);

            return success;
        }
    }
}
