using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using DAL;
using BO;

namespace BLL
{
    public class AttachmentBLLcs
    {

        public DataSet GetAttachmentInfo(AttachmentBO attachBO)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@invcode";
            param.DbType = DbType.String;
            if (string.IsNullOrEmpty(attachBO.Inv_Code))
            {
                //attachBO.Inv_Code = string.Empty;
                attachBO.Inv_Code = "";
            }
            
            param.Value = attachBO.Inv_Code;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@user_id";
            param.DbType = DbType.String;
            param.Value = attachBO.Userid;
            lstParam.Add(param);

            bool abc = false;
            DataSet ds;
            ds = new DAL.SqlHelper().SelectDataSet("[usp_PopulateAttachmentDetails]", lstParam, abc);
            return ds;
        }

        // Get All files of Invoice created from [tbl_AttachmentDetails] table
        public DataSet GetAllAttachmentFileDetailsByInvoiceID(AttachmentBO attachBO)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@Invoiceno";
            param.DbType = DbType.String;
            param.Value = attachBO.Inv_Code;
            lstParam.Add(param);

            ////param = new SqlParameter();
            ////param.ParameterName = "@user_id";
            ////param.DbType = DbType.String;
            ////param.Value = attachBO.Userid;
            ////lstParam.Add(param);

            bool abc = false;
            DataSet ds;
            ds = new DAL.SqlHelper().SelectDataSet("[usp_getAttachmentDetailsByInvoiceID]", lstParam, abc);
            return ds;
        }

        // Get All files of Draft created from [tbl_AttachmentDetails_bkp] table
        public DataSet GetAllAttachmentFileDetailsByDraftID(AttachmentBO attachBO)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@Invoiceno";
            param.DbType = DbType.String;
            param.Value = attachBO.Inv_Code;
            lstParam.Add(param);

            ////param = new SqlParameter();
            ////param.ParameterName = "@user_id";
            ////param.DbType = DbType.String;
            ////param.Value = attachBO.Userid;
            ////lstParam.Add(param);

            bool abc = false;
            DataSet ds;
            ds = new DAL.SqlHelper().SelectDataSet("[usp_getAttachmentDetailsOfDraftByDraftID]", lstParam, abc);
            return ds;
        }
        public DataSet SendDeletedAttachmentInfo(AttachmentBO attachBO)
        {
            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@attachmentId";
            param.DbType = DbType.String;
            param.Value = attachBO.AttachmentId;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@user_id";
            param.DbType = DbType.String;
            param.Value = attachBO.Userid;
            lstParam.Add(param);

            bool abc = false;
            DataSet ds;
            ds = new DAL.SqlHelper().SelectDataSet("[usp_DeleteAttachmentDetails]", lstParam, abc);
            return ds;
        }


        public int InsertFileInfo(AttachmentBO attachBO)
        {

            //-- Add the parameters for the stored procedure here	
            //     @filename  varchar(50),
            //     @user_id varchar(15)      

            ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@filename";
            param.DbType = DbType.String;
            param.Value = attachBO.Filename;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@user_id";
            param.DbType = DbType.String;
            param.Value = attachBO.Userid;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@Inv_Code";
            param.DbType = DbType.String;
            if (string.IsNullOrEmpty(attachBO.Inv_Code))
            {
                attachBO.Inv_Code = "";
            }

            param.Value = attachBO.Inv_Code;
            lstParam.Add(param);


            int attachmentId;
            //attachmentId = new DAL.SqlHelper().FileInfo("[usp_AttachmentDetails]", lstParam);
            attachmentId = new DAL.SqlHelper().ReturnAttachmentIdAfterInsert("[usp_InsertAttachmentDetails]", lstParam);

            return attachmentId;


        }
        public void UpdateAttachments(string invcode,string draftcode, string userid , DataSet ds)
        {
            // ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;
            // DataSet ds;
            //string invcode = string.Empty;
            //string userid = string.Empty;
            string filename = string.Empty;
            //  @filename varchar(50),
            //  @invcode varchar(10),
            //    @user_id varchar(15)    
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                filename = ds.Tables[0].Rows[i][0].ToString();



                ArrayList lstParam = new System.Collections.ArrayList();

                param = new SqlParameter();
                param.ParameterName = "@filename";
                param.DbType = DbType.String;
                param.Value = filename;
                lstParam.Add(param);

                param = new SqlParameter();
                param.ParameterName = "@invcode";
                param.DbType = DbType.String;
                param.Value = invcode;
                lstParam.Add(param);

                param = new SqlParameter();
                param.ParameterName = "@user_id";
                param.DbType = DbType.String;
                param.Value = userid;
                lstParam.Add(param);

                param = new SqlParameter();
                param.ParameterName = "@dft_inv_code";
                param.DbType = DbType.String;

                if (!string.IsNullOrEmpty(draftcode))
                {
                    param.Value = draftcode;
                }
                else
                {
                    param.Value = string.Empty;
                }

                lstParam.Add(param);

                //  string x;
                // x = new DAL.SqlHelper().ReturnValue("sp_Employee_Insert", lstParam);
                // bool flag = false;
                new DAL.SqlHelper().Insert("[dbo].[usp_SaveAttachmentDetails]", lstParam);

            }

        }
    }
}
