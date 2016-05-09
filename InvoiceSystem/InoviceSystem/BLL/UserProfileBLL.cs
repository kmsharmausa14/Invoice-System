using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
using System.Configuration;
using DAL;
using BO;

namespace BLL
{
    public class UserProfileBLL
    {
        //View User Profile      @empid
        public DataSet UserProfile(int empId)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@empid";
            param.DbType = DbType.Int32;
            param.Value = empId;
            lstParam.Add(param);


            DataSet ds;
            bool abc = false;
            //new DAL.SqlHelper().ReturnValue("sp_Employee_Insert", lstParam);
            ds = new DAL.SqlHelper().SelectDataSet("[dbo].[sp_SubmitReqGetUserInfo]", lstParam, abc);  //[dbo].[sp_SubmitReqGetUserInfo]
            //ds=  [dbo].[sp_ticketDetails]

            return ds;

        }

        public DataSet Report1()
        {
            DataSet ds;
            bool abc = true;
            
            ds = new DAL.SqlHelper().SelectDataSet("select convert(varchar(50),Date) as [Date],[NoofInvoices] as [Total Invoices],[InvoicesApproved] as [Approved Invoices] ,[InvoicesRejected] as [Rejected Invoices],[Invoicespendingforapproval] as [Pending for Approval],[Cumulativepending] as [Cumulative Pending] from DayWiseReport", null, abc); 

            return ds;

        }

        public DataSet Report2()
        {
            DataSet ds;
            bool abc = true;

            ds = new DAL.SqlHelper().SelectDataSet("select [Supplier],[NumberOfInvoices] as [Total Invoices],[InvoicesApproved] as [Approved Invoices],[InvoicesRejected] as [Rejected Invoices],[Invoicespendingforapproval] as [Pending for Approval],[Cumulativepending] as [Cumulative Pending]  from SupplierWiseReport", null, abc);

            return ds;

        }

        public DataSet Report3()
        {
            DataSet ds;
            bool abc = true;

            ds = new DAL.SqlHelper().SelectDataSet("select  convert(varchar(50),Date) as [Date],[ApproverName] as [Approver Name],[NumberOfInvoices] as [Total Invoices],[AvgApprovalTime] as [Average Approval Time],[<1Day],[1-2Day],[2-5Day],[>5Day] from TATReport", null, abc);

            return ds;

        }

    }
}
