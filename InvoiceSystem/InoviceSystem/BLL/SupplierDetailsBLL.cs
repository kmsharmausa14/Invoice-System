using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using BO;

namespace BLL
{
    public class SupplierDetailsBLL
    {
      
        public DataSet GetSupplierDetailsById(string SCode)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@SupplierCode";
            param.DbType = DbType.String;
            param.Value = SCode;
            lstParam.Add(param);
         
            DataSet ds;
            bool abc = false;                       
            ds = new DAL.SqlHelper().SelectDataSet("[dbo].[usp_getSupplierDetails]", lstParam, abc);
            return ds;
        }


        public DataSet GetAllSupplier()
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            /*SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@SupplierCode";
            param.DbType = DbType.String;
            param.Value = SCode;
            lstParam.Add(param);*/

            DataSet ds;
            bool abc = false;
            ds = new DAL.SqlHelper().SelectDataSet("[dbo].[usp_SupplierLookUp]", lstParam, abc);
            return ds;
        }
    }
}
