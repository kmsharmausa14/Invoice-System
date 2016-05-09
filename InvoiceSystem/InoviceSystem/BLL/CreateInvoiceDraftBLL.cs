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

namespace BLL
{
    public class CreateInvoiceDraftBLL
    {
        ArrayList lstParam = new System.Collections.ArrayList();

        public string createInvoiceDraft(CreateInvoiceBO createinvoice)
        {
            lstParam = new System.Collections.ArrayList();
            //string USEID = Session["Userid"];
            SqlParameter param;
            //==========================NKK=============================
            param = new SqlParameter();
            param.ParameterName = "@dft_inv_code";
            param.DbType = DbType.String;
            param.Value = createinvoice.Invoicenumber;            
            lstParam.Add(param);
            //======================================================

            param = new SqlParameter();
            param.ParameterName = "@s_code";
            param.DbType = DbType.String;
            param.Value = createinvoice.Suppliercode;
            lstParam.Add(param);


            param = new SqlParameter();
            param.ParameterName = "@shippedId";
            param.DbType = DbType.Int32;
            param.Value = createinvoice.Shippedto;
            //param.Value = 1;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@invTo";
            param.DbType = DbType.String;
            param.Value = createinvoice.Invoiceto;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@payableId";
            param.DbType = DbType.Int32;
            param.Value = createinvoice.Payableto;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@finalDest";
            param.DbType = DbType.String;
            param.Value = createinvoice.Finaldestination;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@comments";
            param.DbType = DbType.String;
            param.Value = createinvoice.Comments;
            lstParam.Add(param);


            param = new SqlParameter();
            param.ParameterName = "@totalLineAmt";
            param.DbType = DbType.Currency;
            param.Value = createinvoice.totalline;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@totalAddChargeAmt";
            param.DbType = DbType.Currency;
            param.Value = createinvoice.Totalladdcharge;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@totalInvoiceAmt";
            param.DbType = DbType.Currency;
            param.Value = createinvoice.Totalamt;
            lstParam.Add(param);

            //sending supplier code as created by ;
            param = new SqlParameter();
            param.ParameterName = "@createdBy";
            param.DbType = DbType.String;
            param.Value = createinvoice.CreatedBy;
            lstParam.Add(param);

            //sending supplier code as modified by ;
            param = new SqlParameter();
            param.ParameterName = "@modifiedBy";
            param.DbType = DbType.String;
            param.Value = createinvoice.ModifiedBy;
            lstParam.Add(param);

           

            param = new SqlParameter();
            param.ParameterName = "@totalLineAmt_ind";
            param.DbType = DbType.String;
            param.Value = createinvoice.TotalLineInd;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@totalAddChargeAmt_ind";
            param.DbType = DbType.String;
            param.Value = createinvoice.TotalAddChargeInd;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@totalInvoiceAmt_ind";
            param.DbType = DbType.String;
            param.Value = createinvoice.TotalAmountInd;
            lstParam.Add(param);
            string x;
            // x = new DAL.SqlHelper().ReturnValue("sp_Employee_Insert", lstParam);
            x = new DAL.SqlHelper().GetInvoiveCodeAfterInsert("[dbo].[usp_CreateInvoiceDetail_bkp]", lstParam);
            /*if (x == 0)
                return Convert.ToInt32(x);
            else
                return -1;*/
            createinvoice.Invoicenumber = x;
            UpdateAttachmentDetails(createinvoice);
            SaveOrderDetails(createinvoice);
            return x;

        }


        public void UpdateForDraftToInvoice(string Draftno)
        {

            //Add the parameters for the stored procedure here   
            //@dft_inv_code  varchar(10)     //   @dft_inv_code  varchar(10),  
            

            ArrayList lstParam = new ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@dft_inv_code";
            param.DbType = DbType.String;
            param.Value = Draftno;
            lstParam.Add(param);


            //param = new SqlParameter();
            //param.ParameterName = "@user_id";
            //param.DbType = DbType.String;
            //param.Value = createinvoice.CreatedBy;
            //lstParam.Add(param);


            SqlHelper objSqlHelper = new SqlHelper();
            objSqlHelper.Insert("[dbo].[usp_UpdateIsActiveDraftInvoice]", lstParam);
            DeleteAllFilesFromAttachmentBKP(Draftno);

        }

        public void DeleteAllFilesFromAttachmentBKP(string Draftno)
        {

     //       -- Add the parameters for the stored procedure here    
     //@dft_inv_code  varchar(10)   


            ArrayList lstParam = new ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@dft_inv_code";
            param.DbType = DbType.String;
            param.Value = Draftno;
            lstParam.Add(param);


            //param = new SqlParameter();
            //param.ParameterName = "@user_id";
            //param.DbType = DbType.String;
            //param.Value = createinvoice.CreatedBy;
            //lstParam.Add(param);


            SqlHelper objSqlHelper = new SqlHelper();
            objSqlHelper.Insert("[dbo].[usp_DeleteAttachmentDetails_bkp]", lstParam);
           

        }
        protected void UpdateAttachmentDetails(CreateInvoiceBO createinvoice)
        {

             //Add the parameters for the stored procedure here   
             //   @dft_inv_code  varchar(10),  
             //    @user_id       varchar(15)

            ArrayList lstParam = new ArrayList();
            SqlParameter param;

            param = new SqlParameter();
            param.ParameterName = "@dft_inv_code";
            param.DbType = DbType.String;
            param.Value = createinvoice.Invoicenumber;
            lstParam.Add(param);


            param = new SqlParameter();
            param.ParameterName = "@user_id";
            param.DbType = DbType.String;
            param.Value = createinvoice.CreatedBy;
            lstParam.Add(param);


            SqlHelper objSqlHelper = new SqlHelper();
            objSqlHelper.Insert("[dbo].[usp_UpdateAttachmentDetails]", lstParam);

        }
        protected void SaveOrderDetails(CreateInvoiceBO createinvoice)
        {
            lstParam = new System.Collections.ArrayList();
            SqlParameter param;



            param = new SqlParameter();
            param.ParameterName = "@invcode";
            param.DbType = DbType.String;
            param.Value = createinvoice.Invoicenumber;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@orderDate";
            param.DbType = DbType.Date;
            param.Value = createinvoice.Invoicedate;
            lstParam.Add(param);


            param = new SqlParameter();
            param.ParameterName = "@shippedDate";
            param.DbType = DbType.Date;
            param.Value = createinvoice.Shippeddate;
            lstParam.Add(param);


            param = new SqlParameter();
            param.ParameterName = "@shippedId1";
            param.DbType = DbType.Int32;
            param.Value = createinvoice.Shippedvia;
            lstParam.Add(param);

         //Commented by sachin 25 feb
            //param = new SqlParameter();
            //param.ParameterName = "@shippedVia";
            //param.DbType = DbType.String;
            //param.Value = createinvoice.ShippedViaMeans;
            //lstParam.Add(param);

            //added by sachin 25 feb if else part
            if (createinvoice.ShippedViaOther == null)
            {
                param = new SqlParameter();
                param.ParameterName = "@shippedVia";
                param.DbType = DbType.String;
                param.Value = createinvoice.ShippedViaMeans;
                lstParam.Add(param);
            }
            else
            {
                param = new SqlParameter();
                param.ParameterName = "@shippedVia";
                param.DbType = DbType.String;
                param.Value = createinvoice.ShippedViaOther;
                lstParam.Add(param);
            }
            new DAL.SqlHelper().Insert("[dbo].[usp_CreateInvoiceOrderDetail_bkp]", lstParam);
        }

        // This method detete records from tbl_Invlineitems_details_bkp, tbl_Invadditionalcharge_details_bkp, and 
        // tbl_InvAccountDistribution_details_bkp tables for particular draft number before inserting
        public void DeleteLineItemChargeAccountDetailsBkp(string invCd)
        {
            lstParam = new System.Collections.ArrayList();
            SqlParameter param;


            param = new SqlParameter();
            param.ParameterName = "@Inv_Code";
            param.DbType = DbType.String;
            param.Value = invCd;
            lstParam.Add(param);

            SqlHelper objSqlHelper = new SqlHelper();
            objSqlHelper.Insert("[dbo].[usp_DeleteLineItemChargeAccountDetails_bkp]", lstParam);
        }
        public void inslineitemdetils(lineitemsBO lineitemdtls)
        {
            SqlParameter param;
            lstParam = new System.Collections.ArrayList();
            param = new SqlParameter();
            param.ParameterName = "@Inv_Code";
            param.DbType = DbType.String;
            param.Value = lineitemdtls.InvCode;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@InvNo";
            param.DbType = DbType.Int32;
            param.Value = lineitemdtls.InvNo;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@PartItemNo";
            //param.DbType = DbType.Int32;
            param.DbType = DbType.String;
            param.Value = lineitemdtls.Partitemnumber;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@PoNumber";
            param.DbType = DbType.String;
            param.Value = lineitemdtls.Ponumber;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@PoAmendmentNo";
            param.DbType = DbType.String;
            param.Value = lineitemdtls.Poamendmentnumber;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@ReleaseNo";
            param.DbType = DbType.String;
            param.Value = lineitemdtls.Releasenumber;
            lstParam.Add(param);




            param = new SqlParameter();
            param.ParameterName = "@QuantityShipped";

            int QuantityShpd = 0;
            if (string.IsNullOrEmpty(lineitemdtls.Qtyshipped))
            {
                // lineitemdtls.Qtyshipped = 0;
                QuantityShpd = 0;
            }
            else
            {
                QuantityShpd = Convert.ToInt32(lineitemdtls.Qtyshipped.ToString());;
            }


            //param.DbType = DbType.Double;//===================================
            param.DbType = DbType.Int32;            
           // param.Value = lineitemdtls.Qtyshipped;
            param.Value = QuantityShpd;
            lstParam.Add(param);




            param = new SqlParameter();
            param.ParameterName = "@QtyUnitOfmeasureId";
            param.DbType = DbType.Int32;
            param.Value = lineitemdtls.Qtyuom;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@Amount";
            //param.DbType = DbType.Currency;
            param.DbType = DbType.String;
            param.Value = lineitemdtls.Amount;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@PackingSlip";
            param.DbType = DbType.String;
            param.Value = lineitemdtls.Packingslip;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@BillLading";
            param.DbType = DbType.String;
            param.Value = lineitemdtls.Billlading;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@Comments";
            param.DbType = DbType.String;
            param.Value = lineitemdtls.Comments;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@unit_price";
            param.DbType = DbType.Currency;
            if (!string.IsNullOrEmpty(lineitemdtls.Unitprice))
            {
                //param.Value = Convert.ToInt32(lineitemdtls.Unitprice.ToString()); //*********************
                param.Value = Convert.ToInt32(Convert.ToDecimal((lineitemdtls.Unitprice.ToString())));              
            }
            else
            {
                param.Value = 0;
            }
            lstParam.Add(param);

            ////@qty_ship_ind varchar(2), 
            ////@unit_price_ind varchar(2)
            //amount_ind varchar

            string qty_ship_ind = string.Empty;
            string unit_price_ind = string.Empty;
            int qtyship = 0;
            int unit_price = 0;

            string amount_ind = string.Empty;
            int amount = 0;

            //qtyship = Convert.ToInt32(lineitemdtls.Qtyshipped.ToString());
            
            

            //if (!string.IsNullOrEmpty(lineitemdtls.Qtyshipped))
            //{
            //    //qtyship = Convert.ToInt32(lineitemdtls.Qtyshipped.ToString());//**************************                
            //    qtyship = Convert.ToInt32(Convert.ToDecimal((lineitemdtls.Qtyshipped.ToString())));
            //    if (qtyship >= 0)
            //    {
            //        qty_ship_ind = "Dr";
            //    }
            //    else
            //    {
            //        qty_ship_ind = "Cr";
            //    }
            //}

            //if (!string.IsNullOrEmpty(lineitemdtls.Unitprice))
            //{

            //    //unit_price = Convert.ToInt32(lineitemdtls.Unitprice.ToString());//**************************
            //    unit_price = Convert.ToInt32(Convert.ToDecimal((lineitemdtls.Unitprice.ToString())));
            //    if (unit_price >= 0)
            //    {
            //        unit_price_ind = "Dr";
            //    }
            //    else
            //    {
            //        unit_price_ind = "Cr";
            //    }
            //}

            //if (!string.IsNullOrEmpty(lineitemdtls.Amount))
            //{
            //    //amount = Convert.ToInt32(lineitemdtls.Amount.ToString());//**************************
            //    amount = Convert.ToInt32(Convert.ToDecimal((lineitemdtls.Amount.ToString())));
            //    if (amount >= 0)
            //    {
            //        amount_ind = "Dr";
            //    }
            //    else
            //    {
            //        amount_ind = "Cr";
            //    }
            //}
            param = new SqlParameter();
            param.ParameterName = "@amount_ind";
            param.DbType = DbType.String;
            param.Value = lineitemdtls.AmountInd;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@qty_ship_ind";
            param.DbType = DbType.String;
            param.Value = lineitemdtls.QunatityShipInd;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@unit_price_ind"; //****
            param.DbType = DbType.String;
            param.Value = lineitemdtls.UnitPriceInd;
            lstParam.Add(param);
            //Insert

            //int x;
            // x = new DAL.SqlHelper().ReturnValue("sp_Employee_Insert", lstParam);


            SqlHelper abc = new SqlHelper();
            abc.Insert("[dbo].[usp_SaveLineItemDetails_bkp]", lstParam);
           //DAL.SqlHelper().ReturnValue("[dbo].[usp_SaveLineItemDetails]", lstParam);
            /*if (x == 0)
                return Convert.ToInt32(x);
            else
                return -1;*/
            //return x;
        }

        public void insaddchrgedtls(addchrgeBO addcgedtl)
        {
            SqlParameter param;
            lstParam = new System.Collections.ArrayList();

            param = new SqlParameter();
            param.ParameterName = "@Inv_Code";
            param.DbType = DbType.String;
            param.Value = addcgedtl.InvCode;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@InvNo";
            param.DbType = DbType.Int32;
            param.Value = addcgedtl.InvNo;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@ChargeNo";
            param.DbType = DbType.String;
            param.Value = addcgedtl.Chargenumber;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@ChargeId";
            param.DbType = DbType.Int32;
            param.Value = addcgedtl.Chargetype;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@Charge";
            param.DbType = DbType.String;
            param.Value = addcgedtl.Charge;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@Amount";
            //param.DbType = DbType.Currency;
            param.DbType = DbType.String;
            param.Value = addcgedtl.Chrgeamount;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@Description";
            param.DbType = DbType.String;
            param.Value = addcgedtl.Chrgedescp;
            lstParam.Add(param);


            param = new SqlParameter();
            param.ParameterName = "@GSTNo";
            param.DbType = DbType.String;
            param.Value = addcgedtl.Gst;
            lstParam.Add(param);          


            SqlHelper objSqlHelper = new SqlHelper();
            objSqlHelper.Insert("[dbo].[usp_SaveAdditionalChargeDetails_bkp]", lstParam);
        }

        public void insAccDistredtls(accdistrBO accdistdtl)
        {
            SqlParameter param;
            lstParam = new System.Collections.ArrayList();
            param = new SqlParameter();
            param.ParameterName = "@Inv_Code";
            param.DbType = DbType.String;
            param.Value = accdistdtl.InvCode;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@InvNo";
            param.DbType = DbType.Int32;
            param.Value = accdistdtl.InvNo;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@DebitCredit";
            param.DbType = DbType.String;
            param.Value = accdistdtl.Debitcredit;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@GeneralLedgerAccount";
            param.DbType = DbType.String;
            param.Value = accdistdtl.Generalledger;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@Costcenter1";
            param.DbType = DbType.String;
            param.Value = accdistdtl.Costcenter1;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@Costcenter2";
            param.DbType = DbType.String;
            param.Value = accdistdtl.Costcenter2;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@WBSNo";
            param.DbType = DbType.String;
            param.Value = accdistdtl.Wbsnumber;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@Amount";
            //param.DbType = DbType.Currency;
            param.DbType = DbType.String;
            param.Value = accdistdtl.Amount;
            lstParam.Add(param);


            SqlHelper objSqlHelpr = new SqlHelper();
            objSqlHelpr.Insert("[dbo].[usp_SaveAccountDistributionDetails_bkp]", lstParam);          

        }
    }
}
