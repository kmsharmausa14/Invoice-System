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
    public class CreateInvoiceBLL
    {
       

        public string createInvoice(CreateInvoiceBO createinvoice)
        {
            //string USEID = Session["Userid"];
             ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;

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
            param.Value = createinvoice.totalline;//createinvoice.totalline;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@totalAddChargeAmt";
            param.DbType = DbType.Currency;
            param.Value = createinvoice.Totalladdcharge;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@totalInvoiceAmt";
            param.DbType = DbType.Currency;
            param.Value =  createinvoice.Totalamt;
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

             ////@totalLineAmt_ind varchar(2),  
             ////@totalAddChargeAmt_ind varchar(2),  
             ////@totalInvoiceAmt_ind varchar(2),

            //int totallineamt = 0;
            //int totalAddcharge=0;
            //int invAmt = 0;

            //totallineamt = Convert.ToInt32(Convert.ToDecimal((createinvoice.totalline.ToString())));
            //totalAddcharge = Convert.ToInt32(Convert.ToDecimal((createinvoice.Totalladdcharge.ToString())));
            //invAmt = Convert.ToInt32(Convert.ToDecimal((createinvoice.Totalamt.ToString()))); 

            //string totalLineAmt_ind = string.Empty;
            //string totalAddChargeAmt_ind = string.Empty;
            //string totalInvoiceAmt_ind = string.Empty;

            //if (totallineamt >= 0)
            //{
            //    totalLineAmt_ind = "Dr";
            //}
            //else
            //{
            //    totalLineAmt_ind = "Cr";
            //}

            //if (totalAddcharge >= 0)
            //{
            //    totalAddChargeAmt_ind = "Dr";
            //}
            //else
            //{
            //    totalAddChargeAmt_ind = "Cr";
            //}

            //if (invAmt >= 0)
            //{
            //    totalInvoiceAmt_ind = "Dr";
            //}
            //else
            //{
            //    totalInvoiceAmt_ind = "Cr";
            //}


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

            string  x;
            // x = new DAL.SqlHelper().ReturnValue("sp_Employee_Insert", lstParam);
            x = new DAL.SqlHelper().GetInvoiveCodeAfterInsert("[dbo].[usp_CreateInvoiceDetail]", lstParam);
            /*if (x == 0)
                return Convert.ToInt32(x);
            else
                return -1;*/
            createinvoice.Invoicenumber = x;
            SaveOrderDetails(createinvoice);           
            SaveAttachments(createinvoice);
            return x;
            
        }

        protected void SaveAttachments(CreateInvoiceBO createinvoice)
        {
            // ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;
            DataSet ds;

           // @invcode varchar(10),
	       // @user_id varchar(15)

            ArrayList lstParam = new System.Collections.ArrayList();

            param = new SqlParameter();
            param.ParameterName = "@invcode";
            param.DbType = DbType.String;
            param.Value = string.Empty;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@user_id";
            param.DbType = DbType.String;
            param.Value = createinvoice.CreatedBy;
            lstParam.Add(param);

          //  string x;
            // x = new DAL.SqlHelper().ReturnValue("sp_Employee_Insert", lstParam);
            bool flag = false;
            ds = new DAL.SqlHelper().SelectDataSet("[dbo].[usp_PopulateAttachmentDetails]", lstParam, flag);

            UpdateAttachments(createinvoice, ds);
            


        }

        protected void UpdateAttachments(CreateInvoiceBO createinvoice,DataSet ds)
        {
            // ArrayList lstParam = new System.Collections.ArrayList();
            SqlParameter param;
           // DataSet ds;
            string invcode = string.Empty;
            string userid = string.Empty;
            string filename = string.Empty;
           //  @filename varchar(50),
            //  @invcode varchar(10),
             //    @user_id varchar(15)   
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                filename = ds.Tables[0].Rows[i][1].ToString();
                


                ArrayList lstParam = new System.Collections.ArrayList();

                param = new SqlParameter();
                param.ParameterName = "@filename";
                param.DbType = DbType.String;
                param.Value = filename;
                lstParam.Add(param);

                param = new SqlParameter();
                param.ParameterName = "@invcode";
                param.DbType = DbType.String;
                param.Value = createinvoice.Invoicenumber;
                lstParam.Add(param);

                param = new SqlParameter();
                param.ParameterName = "@user_id";
                param.DbType = DbType.String;
                param.Value = createinvoice.CreatedBy;
                lstParam.Add(param);

                param = new SqlParameter();
                param.ParameterName = "@dft_inv_code";
                param.DbType = DbType.String;

                if (!string.IsNullOrEmpty(createinvoice.Draftno))
                {
                    param.Value = createinvoice.Draftno;
                }
                else
                {
                    param.Value = string.Empty;
                }
               
                lstParam.Add(param);
                            
                new DAL.SqlHelper().Insert("[dbo].[usp_SaveAttachmentDetails]", lstParam);

            }

        }

        protected void SaveOrderDetails(CreateInvoiceBO createinvoice)
        {

            ArrayList lstParam = new System.Collections.ArrayList();
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

            //COmmented by sachin 25 feb
            param = new SqlParameter();
            param.ParameterName = "@shippedId1";
            param.DbType = DbType.Int32;
            param.Value = createinvoice.Shippedvia;
            lstParam.Add(param);


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

            new DAL.SqlHelper().Insert("[dbo].[usp_CreateInvoiceOrderDetail]", lstParam);
           
        }

           

        public void inslineitemdetils(lineitemsBO lineitemdtls)
        {
            SqlParameter param;
             ArrayList lstParam = new System.Collections.ArrayList();
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

            /*param = new SqlParameter();
            param.ParameterName = "@QuantityShipped";
            param.DbType = DbType.Int32;
            param.Value = lineitemdtls.Qtyshipped;
            lstParam.Add(param);*/

            //----------------------------------
            param = new SqlParameter();
            param.ParameterName = "@QuantityShipped";
            int QuantityShpd = 0;
            if (string.IsNullOrEmpty(lineitemdtls.Qtyshipped))
            {              
                QuantityShpd = 0;
            }
            else
            {
                QuantityShpd = Convert.ToInt32(lineitemdtls.Qtyshipped.ToString()); ;
            }            
            param.DbType = DbType.Int32;            
            param.Value = QuantityShpd;
            lstParam.Add(param);
            //----------------------------------------

            param = new SqlParameter();
            param.ParameterName = "@QtyUnitOfmeasureId";
            param.DbType = DbType.Int32;
            param.Value = lineitemdtls.Qtyuom;
            lstParam.Add(param);

            param = new SqlParameter();
            param.ParameterName = "@Amount";
            param.DbType = DbType.Currency;
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
            param.Value = Convert.ToInt32(lineitemdtls.Unitprice);
            lstParam.Add(param);

           

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
            param.ParameterName = "@unit_price_ind ";
            param.DbType = DbType.String;
            param.Value = lineitemdtls.UnitPriceInd;
            lstParam.Add(param);

            //int x;
            SqlHelper abc = new SqlHelper();
            abc.Insert("[dbo].[usp_SaveLineItemDetails]", lstParam);
            // x = new DAL.SqlHelper().ReturnValue("sp_Employee_Insert", lstParam);
           // x = new DAL.SqlHelper().ReturnValue("[dbo].[usp_SaveLineItemDetails]", lstParam);
            /*if (x == 0)
             * SqlHelper abc = new SqlHelper();
            abc.Insert("[dbo].[usp_SaveLineItemDetails_bkp]", lstParam);
                return Convert.ToInt32(x);
            else
                return -1;*/
            //return x;
        }

        public void insaddchrgedtls(addchrgeBO addcgedtl)
        {
            SqlParameter param;
            ArrayList lstParam = new System.Collections.ArrayList();
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
            param.DbType = DbType.Currency;
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
            objSqlHelper.Insert("[dbo].[usp_SaveAdditionalChargeDetails]", lstParam);
            //int x;
            //// x = new DAL.SqlHelper().ReturnValue("sp_Employee_Insert", lstParam);
            //x = new DAL.SqlHelper().ReturnValue("[dbo].[usp_SaveAdditionalChargeDetails]", lstParam);
            ///*if (x == 0)
            //    return Convert.ToInt32(x);
            //else
            //    return -1;*/
            //return x;
        }

        public void insAccDistredtls(accdistrBO accdistdtl)
        {
            SqlParameter param;
            ArrayList lstParam = new System.Collections.ArrayList();
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
            param.DbType = DbType.Currency;
            param.Value = accdistdtl.Amount;
            lstParam.Add(param);

            SqlHelper objSqlHelpr = new SqlHelper();
            objSqlHelpr.Insert("[dbo].[usp_SaveAccountDistributionDetails]", lstParam);

            //int x;
            //// x = new DAL.SqlHelper().ReturnValue("sp_Employee_Insert", lstParam);
            //x = new DAL.SqlHelper().ReturnValue("[dbo].[usp_SaveAccountDistributionDetails]", lstParam);
            ///*if (x == 0)
            //    return Convert.ToInt32(x);
            //else
            //    return -1;*/
            //return x;

        }

        //public DataSet PopulateGridView(string gridViewNmae)
        //{
        //    ArrayList lstParam = new System.Collections.ArrayList();
        //    SqlParameter param;

        //    param = new SqlParameter();
        //    param.ParameterName = "@gridView";
        //    param.DbType = DbType.String;
        //    param.Value = gridViewNmae;
        //    lstParam.Add(param);


        //    DataSet ds;
        //    bool abc = false;
        //    ds = new DAL.SqlHelper().SelectDataSet("[dbo].[usp_PopulateGridViews]", lstParam, abc);  //SP called to fill values in all dropdownlist

        //    return ds;

        //}

        public int getExactQty(lineitemsBO lineItem , string Suppcode)
        {
            SqlParameter param;
            ArrayList lstParam = new System.Collections.ArrayList();
            param = new SqlParameter();
            param.ParameterName = "@po_no";
            param.DbType = DbType.String;
            param.Value = lineItem.Ponumber;
            lstParam.Add(param);

            //SqlParameter param;@po_no				        varchar(15),
	//@part_item_id		        int,
	//@s_code	

            param = new SqlParameter();
            param.ParameterName = "@part_item_id";
            param.DbType = DbType.String;
            param.Value = lineItem.Partitemnumber;
            lstParam.Add(param);



            param = new SqlParameter();
            param.ParameterName = "@s_code";
            param.DbType = DbType.String;
            param.Value = Suppcode;
            lstParam.Add(param);

            int x;
            bool abc= false;
            x = Convert.ToInt32(new DAL.SqlHelper().SelectValue("[usp_ValidateInvoicePoQtyshipped]", lstParam, abc));

            return x;
        }


    }
}
