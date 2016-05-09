using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BO;
using System.Collections;
using System.Data.SqlClient;

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  ApproverWorkqueueBLL is written to Populate the Approver Workqueue Grid with Data based upon the Parameters
//  sent to the Stored Procedure [dbo].[usp_PopulateApproverWorkQueueGridView].
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace BLL
{
    public class ApproverWorkqueueBLL
    {
        public DataSet ApproverWorkqueueGridview(ListOfDraftInvBO lstOfDrftBo)
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
            ds = new DAL.SqlHelper().SelectDataSet("[dbo].[usp_PopulateApproverWorkQueueGridView]", lstParam, abc);
            return ds;
        }

        public DataSet PopulateApproverScreenHeader(string invCode)
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
            ds = new DAL.SqlHelper().SelectDataSet("[dbo].[usp_PopulateInvoiceOrderDetails]", lstParam, abc);
            return ds;
        }

        public DataSet PopulateApproverScreenGridview(string GridView, string invCode)
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
            ds = new DAL.SqlHelper().SelectDataSet("[dbo].[usp_PopulateGridViews]", lstParam, abc);
            return ds;

        }

    }
}
