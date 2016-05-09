using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using BO;
using DAL;

namespace BLL
{
    public class LoginDetailsBLL
    {
        
        public int CheckLoginDetails(LoginBO loginBo)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@userid";
            param.DbType = DbType.String;
            param.Value = loginBo.UserId;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@password";
            param.DbType = DbType.String;
            param.Value = loginBo.Password;
            lstParam.Add(param);

            DataSet ds;
            bool abc = false;
            int usercount = new DAL.SqlHelper().ReturnValuefromLogin("[dbo].[usp_CheckLoginDetails]", lstParam);
            return usercount;
        }

        public string GetSupplierCode(LoginBO loginBo)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@userid";
            param.DbType = DbType.String;
            param.Value = loginBo.UserId;
            lstParam.Add(param);      

            DataSet ds;
            bool abc = false;          
            string supplierCode = new DAL.SqlHelper().GetSupplierCode("[dbo].[usp_getSupplierCode]", lstParam);
            return supplierCode;
        }
        
        public int GetRoleIdByUserId(string Uid)
        {           

            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@userid";
            param.DbType = DbType.String;
            param.Value = Uid;
            lstParam.Add(param);
           
            bool abc = false;
            int roleId = new DAL.SqlHelper().ReturnRoleIdByUserId("[dbo].[usp_GetRoleByUserId]", lstParam);
            return roleId;
        }

        public int CheckUserFromResetPasswordTable(LoginBO loginBo)
        {

            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@userid";
            param.DbType = DbType.String;
            param.Value = loginBo.UserId;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@password";
            param.DbType = DbType.String;
            param.Value = loginBo.Password;
            lstParam.Add(param);

            //DataSet ds;
            bool abc = false;
            int userCountfromResetPasswordTable = new DAL.SqlHelper().ReturnValuefromLogin("[dbo].[usp_CheckResetPasswordDetails]", lstParam);
            return userCountfromResetPasswordTable;
        }

        public void ChangeResetedPassword(LoginBO userCredentialsBO)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@userId";
            param.DbType = DbType.String;
            param.Value = userCredentialsBO.UserId;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@pword";
            param.DbType = DbType.String;
            param.Value = userCredentialsBO.Password;
            lstParam.Add(param);

            int x;
            SqlHelper sqlObj = new SqlHelper();
            //bool abc = false;
            sqlObj.Insert("[dbo].[usp_changeResetedPassword]", lstParam);

        }

        public void Forgotpassword(LoginBO userCredentialsBO)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@user_id";
            param.DbType = DbType.String;
            param.Value = userCredentialsBO.UserId;
            lstParam.Add(param);

            int x;
            SqlHelper sqlObj = new SqlHelper();
            //bool abc = false;
            sqlObj.Insert("[dbo].[sp_forgotpassword]", lstParam);

        }


        public string getEmaild(string userid)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@userId";
            param.DbType = DbType.String;
            param.Value = userid;
            lstParam.Add(param);
            bool abc = true;
            string emaild = (string)new DAL.SqlHelper().SelectValue("SELECT [Email] FROM [InvoiceSystem].[dbo].[tbl_user] WHERE [user_id]= @userId", lstParam, abc);
            //string password = (string)new DAL.SqlHelper().SelectValue("SELECT  [password] FROM [InvoiceSystem].[dbo].[tbl_user] WHERE [user_id]= @userId", lstParam, abc);
            return emaild;
        }

        public string getpassword(string userid)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@userId";
            param.DbType = DbType.String;
            param.Value = userid;
            lstParam.Add(param);
            bool abc = true;
            //string emaild = (string)new DAL.SqlHelper().SelectValue("SELECT [Email] FROM [InvoiceSystem].[dbo].[tbl_user] WHERE [user_id]= @userId", lstParam, abc);
            string password = (string)new DAL.SqlHelper().SelectValue("SELECT  [password] FROM [InvoiceSystem].[dbo].[tbl_user] WHERE [user_id]= @userId", lstParam, abc);
            return password;
        }

        public int checkuser(string userid)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@user_id";
            param.DbType = DbType.String;
            param.Value = userid;
            lstParam.Add(param);
            //bool abc = true;
            int check = (int)new DAL.SqlHelper().ReturnValue("[dbo].[sp_checkuser]", lstParam);
            return check;
        }
    }
}
