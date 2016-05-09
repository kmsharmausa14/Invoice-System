using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Configuration;
using DAL;
using BO;
using System.Data.SqlClient;

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//   ApproveRejectInvoiceBLL is written to Send Approval or rejection Comments while the 
//   updation of Invoice Status.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace BLL
{
    public class ApproveRejectInvoiceBLL
    {
       

        public void ApproveInvoice(ApproveRejectBO approveReject,string userid)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            //sending Invoice Code
            param = new SqlParameter();
            param.ParameterName = "@invcode";
            param.DbType = DbType.String;
            param.Value = approveReject.InvoiceCode;            
            lstParam.Add(param);

            //sending Approval or Rejection Comments
            param = new SqlParameter();
            param.ParameterName = "@app_rej_comments";
            param.DbType = DbType.String;
            param.Value = approveReject.ApproveRejectComments;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@user_id";
            param.DbType = DbType.String;
            param.Value = userid;
            lstParam.Add(param);

            SqlHelper objSqlHelper = new SqlHelper();
            objSqlHelper.Insert("[dbo].[usp_UpdateApproveInvoiceStatus]", lstParam);           
          
        }

        public void RejectInvoice(ApproveRejectBO apvReject, string userid)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            //sending Invoice Code
            param = new SqlParameter();
            param.ParameterName = "@invcode";
            param.DbType = DbType.String;
            param.Value = apvReject.InvoiceCode;
            lstParam.Add(param);

            //sending Approval or Rejection Comments
            param = new SqlParameter();
            param.ParameterName = "@app_rej_comments";
            param.DbType = DbType.String;
            param.Value = apvReject.ApproveRejectComments;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@user_id";
            param.DbType = DbType.String;
            param.Value = userid;
            lstParam.Add(param);

            SqlHelper objSqlHelper = new SqlHelper();
            objSqlHelper.Insert("[dbo].[usp_UpdateRejectInvoiceStatus]", lstParam);

        }

        public DataSet GetInvoiceStatusDetails(string invoiceNumber)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            //sending Invoice Code
            param = new SqlParameter();
            param.ParameterName = "@invcode";
            param.DbType = DbType.String;
            param.Value = invoiceNumber;
            lstParam.Add(param);

            DataSet ds;
            bool abc = false;
            ds = new DAL.SqlHelper().SelectDataSet("[dbo].[usp_GetInvoiceStatusdetails]", lstParam, abc);
            return ds;

        }

    }
}
