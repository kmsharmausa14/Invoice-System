using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.SqlClient;

namespace BLL
{
    public class LoadValuesinDropDownlistBLL
    {
        public DataSet LoadValuesinDropDownList(string tableName)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@tablename";
            param.DbType = DbType.String;
            param.Value = tableName;
            lstParam.Add(param);

          
            DataSet ds;
            bool abc = false;
            ds = new DAL.SqlHelper().SelectDataSet("[dbo].[usp_LoadValuesinDropDownlist]", lstParam, abc);  //SP called to fill values in all dropdownlist

            return ds;

        }
    }
}
