using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.SqlClient;

namespace BLL
{
     public class POGRdetailsBLL
    {
         public DataSet GetPOdetails(string Ponumber , string Scode)
         {
             ArrayList lstParam = new System.Collections.ArrayList();
             SqlParameter param;

             param = new SqlParameter();
             param.ParameterName = "@POnumber";
             param.DbType = DbType.String;
             param.Value = Ponumber;
             lstParam.Add(param);

             param = new SqlParameter();
             param.ParameterName = "@scode";
             param.DbType = DbType.String;
             param.Value = Scode;
             lstParam.Add(param);
             //param = new SqlParameter();
             //param.ParameterName = "@partitmnumber";
             //param.DbType = DbType.String;
             //param.Value = Partitmnmber;
             //lstParam.Add(param);

             DataSet ds;
             bool abc = false;
             ds = new DAL.SqlHelper().SelectDataSet("[dbo].[usp_getPODetails]", lstParam, abc);
             return ds;
         }

        public DataSet GetGRdetails(string Scode, string Ponumber)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@scode";
            param.DbType = DbType.String;
            param.Value = Scode;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@POnumber";
            param.DbType = DbType.String;
            param.Value = Ponumber;
            lstParam.Add(param);

            //param = new SqlParameter();
            //param.ParameterName = "@item";
            //param.DbType = DbType.String;
            //param.Value = Partitemnumber;
            //lstParam.Add(param);

            DataSet ds;
            bool abc = false;
            ds = new DAL.SqlHelper().SelectDataSet("[usp_getGRDetails]", lstParam, abc);
            return ds;
        }

    }
}
