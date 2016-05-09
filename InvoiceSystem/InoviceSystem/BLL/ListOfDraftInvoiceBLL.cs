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
    public class ListOfDraftInvoiceBLL
    {
        public DataSet ListOfDraftInv(ListOfDraftInvBO lstOfDrftBo)
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
            param.ParameterName = "@user_id";
            param.DbType = DbType.String;
            param.Value = lstOfDrftBo.UserId;
            lstParam.Add(param);

            DataSet ds;
            bool abc = false;
            ds = new DAL.SqlHelper().SelectDataSet("[dbo].[usp_PopulateListofDraftInvoicesGridView]", lstParam, abc);
            return ds;
        }

        public DataSet PopulateCreateInvoiceHeader(string invCode)
        {

            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@invcode";
            param.DbType = DbType.String;
            param.Value = invCode;
            lstParam.Add(param);

            DataSet ds;
            bool abc = false;
            ds = new DAL.SqlHelper().SelectDataSet("[dbo].[usp_PopulateInvoiceOrderDetails_bkp]", lstParam, abc);
            return ds;
        }

        public DataSet PopulateCreateInvoiceGridview(string GridView, string invCode)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@gridView";
            param.DbType = DbType.String;
            param.Value = GridView;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@invcode";
            param.DbType = DbType.String;
            param.Value = invCode;
            lstParam.Add(param);

            DataSet ds;
            bool abc = false;
            ds = new DAL.SqlHelper().SelectDataSet("[dbo].[usp_PopulateGridViews_bkp]", lstParam, abc);
            return ds;

        }
        public DataSet Getidsforshippedandpayable(string invCode)
        {

            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@invcode";
            param.DbType = DbType.String;
            param.Value = invCode;
            lstParam.Add(param);

            DataSet ds;
            bool abc = false;
            ds = new DAL.SqlHelper().SelectDataSet("[dbo].[usp_GetShipandPayableDetailsByDraftId_bkp]", lstParam, abc);
            return ds;
        }
     
    }
}
