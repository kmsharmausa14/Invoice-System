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
    public class InvoiceStatusBLL
    {
        public DataSet InvoiceStatusGridview(ListOfDraftInvBO lstOfDrftBo)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@s_code";
            param.DbType = DbType.String;
            param.Value = lstOfDrftBo.SupplierId;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@From_Date";
            param.DbType = DbType.String;
            param.Value = lstOfDrftBo.FromDate;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@To_Date";
            param.DbType = DbType.String;
            param.Value = lstOfDrftBo.ToDate;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@po_no";
            param.DbType = DbType.String;
            param.Value = lstOfDrftBo.PoNumber;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@invcode";
            param.DbType = DbType.String;
            param.Value = lstOfDrftBo.IvoiceNumber;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@status_description";
            param.DbType = DbType.String;
            param.Value = lstOfDrftBo.Status;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@user_id";
            param.DbType = DbType.String;
            param.Value = lstOfDrftBo.UserId;
            lstParam.Add(param);

            DataSet ds;
            bool abc = false;
            ds = new DAL.SqlHelper().SelectDataSet("[dbo].[usp_PopulateInvoiceStatusGridView]", lstParam, abc);
            return ds;
        }
    }
}
