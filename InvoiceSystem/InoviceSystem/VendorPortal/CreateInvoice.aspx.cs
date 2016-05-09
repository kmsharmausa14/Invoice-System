using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using BLL;
using BO;
using System.Net;
using System.IO;
using System.Configuration;

namespace VendorPortal
{
    public partial class CreateInvoice : System.Web.UI.Page
    {
        CreateInvoiceBO objsub = new CreateInvoiceBO();
        lineitemsBO objline = new lineitemsBO();
        addchrgeBO objadd = new addchrgeBO();
        accdistrBO objacc = new accdistrBO();
        POBO objpodtls = new POBO();
        GRBO objgrdtls = new GRBO();
        long totalLineAmount = 0;
        long totalAdditionalChargesAmount = 0;
        long totalLineAmountAfterDelete = 0;
        long totalAdditionalChargesAmountAfterDelete = 0;
        string SC;
        string errormessg;
        bool done;
        bool IsSaveasDraft = false;
        DropDownList qtyuom = new DropDownList();

        protected void Page_Load(object sender, EventArgs e)
        {
            // getLineItemdetails();
            
            if (Session["Userid"] == null)
            {
                Response.Redirect("LoginPage.aspx");
            }
           
            errmessage.Text = string.Empty;
            hidIsFromDraft.Value = string.Empty;
            hidcount.Value = "false";
            if (!IsPostBack)
            {
                string SourcePath = Server.MapPath("~") + @"\Attachments\" + Session["Userid"].ToString() + @"\";
                if (Directory.Exists(SourcePath))
                {
                    DirectoryInfo dr = new DirectoryInfo(SourcePath);
                    dr.Delete(true);
                   
                }
                txtsubmit.Visible = false;
                this.ExportWord.Visible = false;
                PopulateDropDownlist();
                //PopulateSupplier();
                SetInitialRow();
                SetNewRowAdditionalCharge();
                SetInitialRowAccDist();

                //By default select todays date
                CalendarExtenderInvoiceDate.SelectedDate = DateTime.Now.Date;
                CalendarExtenderShippedDate.SelectedDate = DateTime.Now.Date;

                CalendarExtenderInvoiceDate.EndDate = DateTime.Now.Date;
                CalendarExtenderShippedDate.EndDate = DateTime.Now.Date;

                //PopulateCreateInvoiceFromListofDraft();
                //PopulateCreateInvoiceScreenFromInvoiceStatus(); 

                BindAttachmentGridviewData();
                PopulateAllDates();

                //Calender will not allow future dates
                // CalendarExtenderInvoiceDate.EndDate = DateTime.Now.Date;
                // CalendarExtenderShippedDate.EndDate = DateTime.Now.Date;

                //Validation for Invoice and shipped Date
                txtInvoiceDate.Attributes.Add("onchange", "ValidateShippedDateGreaterThanInvoiceDate()");
                txtShippedDate.Attributes.Add("onchange", "ValidateShippedDateGreaterThanInvoiceDate()");

                if (Request.QueryString["QS_Scode"] == null)
                {
                    PopulateSupplier();
                }
                else if (Request.QueryString["QS_Scode"] != null)
                {
                    SC = Convert.ToString(Request.QueryString["QS_Scode"]);
                    Session["SupDetailsfromLookup"] = SC.ToString();
                    PopulateSupplierfromLookUp();
                }

                PopulateCreateInvoiceFromListofDraft();
                PopulateCreateInvoiceScreenFromInvoiceStatus();
                PopulateTextboxes();
                Updatetnfoelne.Visible = false;
                Cancelbtnforlne.Visible = false;
                               
            }
            else
            {
                PopulateAllDates();
            }
            
            this.txtFinalDestination.Attributes.Add("onkeypress", "return ValidateLength('" + this.txtFinalDestination.ClientID + "')");
            this.txtComments.Attributes.Add("onkeypress", "return ValidateLengthComments('" + this.txtComments.ClientID + "')");//
            this.txtFinalDestination.Attributes.Add(" onChange", "return ValidateLengthAfterTextChange('" + this.txtFinalDestination.ClientID + "')");//
            this.txtComments.Attributes.Add(" onChange", "return ValidateLengthAfterTextChange('" + this.txtComments.ClientID + "')");//
            this.txtdescp.Attributes.Add("onkeypress", "return ValidateLengthComments('" + this.txtdescp.ClientID + "')");//
            this.txtdescp.Attributes.Add(" onChange", "return ValidateLengthAfterTextChange('" + this.txtdescp.ClientID + "')");//
            txtqtyshipped.Attributes.Add("onChange", "calAmtLineDetails()");
            txtunitprice.Attributes.Add("onChange", "calAmtLineDetails()");
            btnadd.Attributes.Add("onclick", " ValidateTextboxOfLineItemGrid()");
            btnaddchrge.Attributes.Add("onclick", " ValidateTextboxOfAdditionalChargeGrid()");
            btnaddacc.Attributes.Add("onclick", "ValidateTextboxOfAccountDistributionGrid()");
            btnValidateInvoice.Attributes.Add("onclick", "  ValidateTextbox(" + this.rowcounthidden.ClientID + ")");
            this.txtComm.Attributes.Add("onkeypress", "return ValidateLengthComments('" + this.txtComm.ClientID + "')");
            this.txtComm.Attributes.Add(" onChange", "return ValidateLengthAfterTextChange('" + this.txtComm.ClientID + "')");
            txtqtyshipped.Attributes.Add("onblur", "calAmtLineDetails()");
            txtunitprice.Attributes.Add("onblur", "calAmtLineDetails()");
            //txtunitprice.Attributes.Add("onblur", "return validatedecimalpoint(" + txtunitprice.ClientID + ")");
            ShowGvColumn();
            DataSet ds = new BLL.SupplierDetailsBLL().GetAllSupplier();
            if (ds.Tables[0].Rows.Count != 0)
            {
                grd_ViewSupplierCodeLookUp.DataSource = ds;
                grd_ViewSupplierCodeLookUp.DataBind();
            }
           

           
        }
        //Added supplier code changes
        private void ShowGvColumn()
        {
            DataSet ds = new BLL.SupplierDetailsBLL().GetAllSupplier();
            if (ds.Tables[0].Rows.Count != 0)
            {
                grd_ViewSupplierCodeLookUp.DataSource = ds;
                grd_ViewSupplierCodeLookUp.DataBind();
            }
            grd_ViewSupplierCodeLookUp.Columns[0].Visible = true;
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            
                Response.Redirect("~/CreateInvoice.aspx");
            
         }
        //End supplier code


        #region User coming from Invoice status page

        //Getting InvoiceCode which is coming from Approver_WrkQue page
        private string GetInvCode()
        {
            string InvoiceCode = Convert.ToString(Request.QueryString["QS_Inv_Code1"]);
            ViewState["InvCode"] = InvoiceCode;
            return InvoiceCode;
        }
        private void PopulateCreateInvoiceScreenFromInvoiceStatus()
        {

            string strInvCode = GetInvCode();
            ViewState["InvCodeFromInvoiceStatusScreen"] = strInvCode;

            //This code will executed if user is directing from Invoice status page to Create Invoice page
            if (string.IsNullOrEmpty(strInvCode) != true)
            {
                lbnSupplierCodeLookup.Enabled = false;
                DataSet dsPopulateApproverScn = new BLL.ApproverWorkqueueBLL().PopulateApproverScreenHeader(strInvCode);

                if (dsPopulateApproverScn.Tables[0].Rows.Count > 0)
                {

                    txtSupplierCode.Text = dsPopulateApproverScn.Tables[0].Rows[0][0].ToString();
                    txtSupplierCode.Enabled = false;
                    //ddlShippedTo.SelectedValue = dsPopulateApproverScn.Tables[0].Rows[0][1].ToString();
                    ddlShippedTo.SelectedItem.Text = Convert.ToString(dsPopulateApproverScn.Tables[0].Rows[0][1].ToString());
                    ddlShippedTo.Enabled = false;
                    //ddlPayableTo.SelectedValue = dsPopulateApproverScn.Tables[0].Rows[0][3].ToString();

                    ddlPayableTo.SelectedItem.Text = Convert.ToString(dsPopulateApproverScn.Tables[0].Rows[0][3].ToString());
                    ddlPayableTo.Enabled = false;

                    txtEmailAddress.Text = dsPopulateApproverScn.Tables[0].Rows[0][4].ToString();
                    txtEmailAddress.Enabled = false;
                    txtCurrency.Text = dsPopulateApproverScn.Tables[0].Rows[0][5].ToString();
                    txtCurrency.Enabled = false;
                    txtInvoiceNumber.Text = dsPopulateApproverScn.Tables[0].Rows[0][6].ToString();
                    txtInvoiceNumber.Enabled = false;
                    txtInvoiceTo.Text = dsPopulateApproverScn.Tables[0].Rows[0][7].ToString();
                    txtInvoiceTo.Enabled = false;
                    txtSupplierAddress.Text = dsPopulateApproverScn.Tables[0].Rows[0][8].ToString();
                    txtSupplierAddress.Enabled = false;
                    txtFinalDestination.Text = dsPopulateApproverScn.Tables[0].Rows[0][9].ToString();
                    txtFinalDestination.Enabled = false;
                    txtComments.Text = dsPopulateApproverScn.Tables[0].Rows[0][10].ToString();
                    txtComments.Enabled = false;

                    // DisableAJAXExtenders(this);
                    //date 
                    hidIsFromDraft.Value = "inv";
                    string shipdate = dsPopulateApproverScn.Tables[0].Rows[0][12].ToString();
                    DateTime shippedDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(shipdate))
                    {
                        shippedDate = Convert.ToDateTime(dsPopulateApproverScn.Tables[0].Rows[0][12].ToString());
                        hidshippeddate.Value = shippedDate.ToShortDateString();
                    }


                    txtShippedDate.Text = shippedDate.ToShortDateString();
                    // txtShippedDate.Enabled = false;

                    string invoicedate = dsPopulateApproverScn.Tables[0].Rows[0][11].ToString();
                    DateTime invoiceDate = DateTime.Now;
                    hidinvoicedate.Value = DateTime.Now.ToShortDateString();
                    if (!string.IsNullOrEmpty(invoicedate))
                    {
                        invoiceDate = Convert.ToDateTime(dsPopulateApproverScn.Tables[0].Rows[0][11].ToString());
                        hidinvoicedate.Value = invoiceDate.ToShortDateString();
                    }

                    txtInvoiceDate.Text = invoiceDate.ToShortDateString();
                    //txtInvoiceDate.Enabled = false;


                    // end date
                    //ddlShippedVia.SelectedValue = dsPopulateApproverScn.Tables[0].Rows[0][13].ToString();

                    ddlShippedVia.SelectedItem.Text = Convert.ToString(dsPopulateApproverScn.Tables[0].Rows[0][13].ToString());
                    ddlShippedVia.Enabled = false;
                    txtOther.Text = Convert.ToString(dsPopulateApproverScn.Tables[0].Rows[0][13].ToString());
                    txtOther.Enabled = false;
                    txtTotalLineAmount.Text = dsPopulateApproverScn.Tables[0].Rows[0][14].ToString();
                    txtTotalLineAmount.Enabled = false;
                    txtTotalAdditionalCharges.Text = dsPopulateApproverScn.Tables[0].Rows[0][15].ToString();
                    txtTotalAdditionalCharges.Enabled = false;
                    txtTotalInvoiceAmount.Text = dsPopulateApproverScn.Tables[0].Rows[0][16].ToString();
                    txtTotalInvoiceAmount.Enabled = false;
                    linitemtxtboxes.Visible = false;
                    addchargetxtboxes.Visible = false;
                    accdistrtxtboxes.Visible = false;
                }

                DataSet dsPopulateLineItemsGridview = new BLL.ApproverWorkqueueBLL().PopulateApproverScreenGridview("gvLineItemDetails", strInvCode);
                if (dsPopulateLineItemsGridview.Tables[0].Rows.Count != 0)
                {
                    ViewState["datafromInvoicelistForLineitemDropdown"] = dsPopulateLineItemsGridview.Tables[0];
                    DataTable dtCurrentTable = (DataTable)ViewState["PopulatedTableLineItemDetails"];
                    DataRow drCurrentRow;
                    //foreach (DataRow dr in dsPopulateLineItemsGridview.Tables[0].Rows)
                    //{
                    for (int i = 0; i < dsPopulateLineItemsGridview.Tables[0].Rows.Count; i++)
                    {
                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["part_item_no"] = dsPopulateLineItemsGridview.Tables[0].Rows[i][1].ToString();
                        drCurrentRow["po_no"] = dsPopulateLineItemsGridview.Tables[0].Rows[i][2].ToString();
                        drCurrentRow["po_amendment_no"] = dsPopulateLineItemsGridview.Tables[0].Rows[i][3].ToString();
                        drCurrentRow["release_no"] = dsPopulateLineItemsGridview.Tables[0].Rows[i][4].ToString();
                        drCurrentRow["qty_shipped"] = dsPopulateLineItemsGridview.Tables[0].Rows[i][5].ToString();
                        drCurrentRow["unit_price"] = dsPopulateLineItemsGridview.Tables[0].Rows[i][7].ToString();
                        drCurrentRow["amount"] = dsPopulateLineItemsGridview.Tables[0].Rows[i][8].ToString(); //Convert.ToInt32(txtqtyshipped.Text.ToString()) * Convert.ToInt32(txtunitprice.Text.ToString());
                        drCurrentRow["qty_unitofmeasure_id"] = dsPopulateLineItemsGridview.Tables[0].Rows[i][6].ToString();// dsPopulateLineItemsGridview.Tables[0].Rows[0][0].ToString();
                        drCurrentRow["packing_slip"] = dsPopulateLineItemsGridview.Tables[0].Rows[i][9].ToString();
                        drCurrentRow["bill_lading"] = dsPopulateLineItemsGridview.Tables[0].Rows[i][10].ToString();
                        drCurrentRow["Comments"] = dsPopulateLineItemsGridview.Tables[0].Rows[i][11].ToString();
                        dtCurrentTable.Rows.Add(drCurrentRow);
                    }
                    ViewState["PopulatedTableLineItemDetails"] = dtCurrentTable;
                    gvLineItemDetails.DataSource = dtCurrentTable;
                    gvLineItemDetails.DataBind();
                    gvLineItemDetails.Enabled = false;


                }
                gvLineItemDetails.Enabled = false;

                DataSet dsPopulateAdditionalChargeDetailsGridview = new BLL.ApproverWorkqueueBLL().PopulateApproverScreenGridview("gvAdditionalChargeDetails", strInvCode);
                if (dsPopulateAdditionalChargeDetailsGridview.Tables[0].Rows.Count != 0)
                {
                    ViewState["PopulateAdditionchrgFromInvoiceScreen"] = dsPopulateAdditionalChargeDetailsGridview.Tables[0];
                    DataTable Addchargesfromgrid = (DataTable)ViewState["PopulateTableAdditionalCharge"];
                    for (int i = 0; i < dsPopulateAdditionalChargeDetailsGridview.Tables[0].Rows.Count; i++)
                    {
                        DataRow drCurrentRow = Addchargesfromgrid.NewRow();
                        drCurrentRow["charge_no"] = dsPopulateAdditionalChargeDetailsGridview.Tables[0].Rows[i][1].ToString();
                        //int selectvalue = Convert.ToInt32(dsPopulateAdditionalChargeDetailsGridview.Tables[0].Rows[0][6].ToString());
                        drCurrentRow["Charge_id"] = dsPopulateAdditionalChargeDetailsGridview.Tables[0].Rows[i][6].ToString();

                        drCurrentRow["charge"] = dsPopulateAdditionalChargeDetailsGridview.Tables[0].Rows[i][2].ToString();
                        drCurrentRow["amount"] = dsPopulateAdditionalChargeDetailsGridview.Tables[0].Rows[i][3].ToString();
                        drCurrentRow["description"] = dsPopulateAdditionalChargeDetailsGridview.Tables[0].Rows[i][4].ToString();
                        drCurrentRow["Gst_no"] = dsPopulateAdditionalChargeDetailsGridview.Tables[0].Rows[i][5].ToString();
                        Addchargesfromgrid.Rows.Add(drCurrentRow);
                    }
                    ViewState["PopulateTableAdditionalCharge"] = Addchargesfromgrid;
                    gvAdditionalChargeDetails.DataSource = Addchargesfromgrid;
                    //  gvAdditionalChargeDetails.DataSource = dsPopulateAdditionalChargeDetailsGridview.Tables[0];
                    gvAdditionalChargeDetails.DataBind();
                    gvAdditionalChargeDetails.Enabled = false;


                }
                else
                {
                    DisplayAdditionalChargeGridviewWhenEmpty();
                }

                gvAdditionalChargeDetails.Enabled = false;
                DataSet dsPopulateAccountDistributionDetailsGridview = new BLL.ApproverWorkqueueBLL().PopulateApproverScreenGridview("gvAccountDistributionDetails", strInvCode);
                if (dsPopulateAccountDistributionDetailsGridview.Tables[0].Rows.Count != 0)
                {
                    gvAccountDistributionDetails.DataSource = dsPopulateAccountDistributionDetailsGridview.Tables[0];
                    gvAccountDistributionDetails.DataBind();
                    gvAccountDistributionDetails.Enabled = false;

                    //Changed By devendrakumar....dataset added to viewstate for exporting
                    ViewState["PopulateAccountDistDetails"] = dsPopulateAccountDistributionDetailsGridview.Tables[0];
                }
                else
                {

                    DisplayAccountChargeGridviewWhenEmpty();
                }

                gvAccountDistributionDetails.Enabled = false;
                //Hide all buttons
                txtsubmit.Visible = false;
                //commented by sachin to show save as draft after ckicking
                //txtsave.Visible = false;
                btnValidateInvoice.Visible = false;
                txtclr.Visible = false;
                AttachButton.Enabled = false;
                FileUpload1.Enabled = false;
                this.ExportWord.Visible = true;
                this.txtcancel.Text = "Back";

                string flageFrmInvStatus = Convert.ToString(Request.QueryString["FlageFromInvStatus"]);
                if (!string.IsNullOrEmpty(flageFrmInvStatus))
                {
                    gvDetails.Enabled = false;
                }
                else if (string.IsNullOrEmpty(flageFrmInvStatus))
                {
                    gvDetails.Enabled = true;
                }

                DataSet dsGetdetailsInvoicestatus = new BLL.ApproveRejectInvoiceBLL().GetInvoiceStatusDetails(strInvCode);
                if (dsGetdetailsInvoicestatus.Tables.Count > 0)
                {
                    if (dsGetdetailsInvoicestatus.Tables[0].Rows.Count > 0)
                    {
                        if (dsGetdetailsInvoicestatus.Tables[0].Rows[0][1].ToString() != string.Empty)
                        {
                            lblCommentsforstatus.Visible = true;
                            txtCommentsforstatus.Visible = true;
                            txtCommentsforstatus.Text = dsGetdetailsInvoicestatus.Tables[0].Rows[0][1].ToString();
                        }

                        if (dsGetdetailsInvoicestatus.Tables[0].Rows[0][0].ToString() == "4")
                        {
                            txtstatusTimestamp.Text = "Approved on " + dsGetdetailsInvoicestatus.Tables[0].Rows[0][3];
                          
                        }
                        else if (dsGetdetailsInvoicestatus.Tables[0].Rows[0][0].ToString() == "5")
                        {
                            txtstatusTimestamp.Text = "Rejected on " + dsGetdetailsInvoicestatus.Tables[0].Rows[0][3];
                            txtstatusTimestamp.ForeColor = System.Drawing.Color.Red;
                        }

                    }
                    else
                    {
                        lblCommentsforstatus.Visible = false;
                        txtCommentsforstatus.Visible = false;
                    }
                }
                else
                {
                    lblCommentsforstatus.Visible = false;
                    txtCommentsforstatus.Visible = false;
                }

            }
            //BindAttachmentGridviewData1(strInvCode);

            BindAttachmentGridviewData();



        }

        //-------------------
        private void BindAttachmentGridviewData1(string invoiceno)
        {
            string userId = Session["Userid"].ToString();
            BLL.AttachmentBLLcs obj1 = new AttachmentBLLcs();
            AttachmentBO attachbo = new AttachmentBO();
            attachbo.Userid = userId;
            attachbo.Inv_Code = invoiceno;
            DataSet dsLoadPayableTo = obj1.GetAllAttachmentFileDetailsByInvoiceID(attachbo);

            if (dsLoadPayableTo.Tables.Count > 0)
            {
                gvDetails.DataSource = dsLoadPayableTo;
                gvDetails.DataBind();

            }
        }


        #endregion

        #region Populate all textbox from List of Draft Gridview

        private void PopulateCreateInvoiceFromListofDraft()
        {
            string strInvCode = Request.QueryString["Inv_Code"];


            //This code will executed if user is directing from List of Draft page to Create Invoice page
            if (string.IsNullOrEmpty(strInvCode) != true)
            {
                //lbnSupplierCodeLookup.Enabled = false;
                ViewState["draftnofromlist"] = strInvCode;
                ViewState["draftnofromlistForlineitemGrid"] = strInvCode;
                ViewState["draftnofromlistForAdditemGrid"] = strInvCode;
                ViewState["draftnofromlistForAccitemGrid"] = strInvCode;
                lbnSupplierCodeLookup.Enabled = false;
                DataSet dsPopulateCreateInvoice = new BLL.ListOfDraftInvoiceBLL().PopulateCreateInvoiceHeader(strInvCode);
                DataSet dsgetisforshippednadpayble = new BLL.ListOfDraftInvoiceBLL().Getidsforshippedandpayable(strInvCode);
                if (dsPopulateCreateInvoice.Tables[0].Rows.Count > 0)
                {
                    txtSupplierCode.Text = dsPopulateCreateInvoice.Tables[0].Rows[0][0].ToString();

                    if (dsgetisforshippednadpayble.Tables[0].Rows.Count > 0)
                    {
                        ddlShippedTo.SelectedValue = dsgetisforshippednadpayble.Tables[0].Rows[0][0].ToString();
                        //if (dsPopulateCreateInvoice.Tables[0].Rows[0][1].ToString().Length > 0)
                        //{
                        //    int shiptoindex = Convert.ToInt32(dsPopulateCreateInvoice.Tables[0].Rows[0][1].ToString());
                        //    ddlShippedTo.SelectedIndex = shiptoindex - 1;
                        //}
                        ddlPayableTo.SelectedValue = dsgetisforshippednadpayble.Tables[0].Rows[0][1].ToString();
                        //if (dsPopulateCreateInvoice.Tables[0].Rows[0][2].ToString().Length > 0)
                        //{
                        //    int payableindex = Convert.ToInt32(dsPopulateCreateInvoice.Tables[0].Rows[0][2].ToString());
                        //    ddlPayableTo.SelectedIndex = payableindex-1;
                        //}
                    }
                    txtEmailAddress.Text = dsPopulateCreateInvoice.Tables[0].Rows[0][3].ToString();

                    txtCurrency.Text = dsPopulateCreateInvoice.Tables[0].Rows[0][4].ToString();
                    txtInvoiceNumber.Text = dsPopulateCreateInvoice.Tables[0].Rows[0][5].ToString();
                    txtInvoiceTo.Text = dsPopulateCreateInvoice.Tables[0].Rows[0][6].ToString();
                    txtSupplierAddress.Text = dsPopulateCreateInvoice.Tables[0].Rows[0][7].ToString();
                    txtFinalDestination.Text = dsPopulateCreateInvoice.Tables[0].Rows[0][8].ToString();
                    txtComments.Text = dsPopulateCreateInvoice.Tables[0].Rows[0][9].ToString();

                    hidIsFromDraft.Value = "draft";
                    string invoicedate = dsPopulateCreateInvoice.Tables[0].Rows[0][10].ToString();
                    DateTime invoiceDate = DateTime.Now;
                    hidinvoicedate.Value = DateTime.Now.ToShortDateString();
                    if (!string.IsNullOrEmpty(invoicedate))
                    {
                        invoiceDate = Convert.ToDateTime(dsPopulateCreateInvoice.Tables[0].Rows[0][10].ToString());
                        hidinvoicedate.Value = invoiceDate.ToShortDateString();
                    }

                    string shipdate = dsPopulateCreateInvoice.Tables[0].Rows[0][11].ToString();
                    DateTime shippedDate = DateTime.Now;
                    hidshippeddate.Value = DateTime.Now.ToShortDateString();
                    if (!string.IsNullOrEmpty(shipdate))
                    {
                        shippedDate = Convert.ToDateTime(dsPopulateCreateInvoice.Tables[0].Rows[0][11].ToString());
                        hidshippeddate.Value = shippedDate.ToShortDateString();
                    }

                    txtInvoiceDate.Text = invoiceDate.Date.ToShortDateString();
                    txtShippedDate.Text = shippedDate.Date.ToShortDateString();
                    //txtInvoiceDate.Text = dsPopulateCreateInvoice.Tables[0].Rows[0][10].ToString();
                    //txtShippedDate.Text = dsPopulateCreateInvoice.Tables[0].Rows[0][11].ToString();
                    //CalendarExtenderInvoiceDate.SelectedDate = Convert.ToDateTime(txtInvoiceDate.Text).Date;
                    //CalendarExtenderShippedDate.SelectedDate = Convert.ToDateTime(txtShippedDate.Text).Date;
                    if (dsgetisforshippednadpayble.Tables[0].Rows.Count > 0)
                    {
                        ddlShippedVia.SelectedValue = dsgetisforshippednadpayble.Tables[0].Rows[0][2].ToString();
                    }
                    txtOther.Text = ddlShippedVia.SelectedItem.Text;//Convert.ToString(dsPopulateApproverScn.Tables[0].Rows[0][13].ToString());
                    txtOther.Enabled = false;
                    // = dsPopulateCreateInvoice.Tables[0].Rows[0][13].ToString();
                    txtTotalLineAmount.Text = dsPopulateCreateInvoice.Tables[0].Rows[0][13].ToString();
                    ViewState["totallineamount"] = txtTotalLineAmount.Text;
                    txtTotalAdditionalCharges.Text = dsPopulateCreateInvoice.Tables[0].Rows[0][14].ToString();
                    ViewState["totalAdditionalAmount"] = txtTotalAdditionalCharges.Text.ToString();
                    txtTotalInvoiceAmount.Text = dsPopulateCreateInvoice.Tables[0].Rows[0][15].ToString();
                }

                DataSet dsPopulateLineItemsGridview = new BLL.ListOfDraftInvoiceBLL().PopulateCreateInvoiceGridview("gvLineItemDetails", strInvCode);
                if (dsPopulateLineItemsGridview.Tables[0].Rows.Count != 0)
                {
                    ViewState["datafromDRaftlistForLineitemDropdown"] = dsPopulateLineItemsGridview.Tables[0];
                    DataTable dtCurrentTable = (DataTable)ViewState["PopulatedTableLineItemDetails"];
                    foreach (DataRow dr in dsPopulateLineItemsGridview.Tables[0].Rows)
                    {
                        DataRow drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["part_item_no"] = dr[1].ToString();
                        drCurrentRow["po_no"] = dr[2].ToString();
                        drCurrentRow["po_amendment_no"] = dr[3].ToString();
                        drCurrentRow["release_no"] = dr[4].ToString();
                        drCurrentRow["qty_shipped"] = dr[5].ToString();
                        drCurrentRow["unit_price"] = dr[7].ToString();
                        drCurrentRow["amount"] = dr[8].ToString(); //Convert.ToInt32(txtqtyshipped.Text.ToString()) * Convert.ToInt32(txtunitprice.Text.ToString());
                        drCurrentRow["qty_unitofmeasure_id"] = dr[6].ToString();// dsPopulateLineItemsGridview.Tables[0].Rows[0][0].ToString();
                        drCurrentRow["packing_slip"] = dr[9].ToString();
                        drCurrentRow["bill_lading"] = dr[10].ToString();
                        drCurrentRow["Comments"] = dr[11].ToString();
                        dtCurrentTable.Rows.Add(drCurrentRow);
                    }
                    ViewState["PopulatedTableLineItemDetails"] = dtCurrentTable;
                    gvLineItemDetails.DataSource = dtCurrentTable;
                    gvLineItemDetails.DataBind();
                   
                    rowcounthidden.Value = gvLineItemDetails.Rows.Count.ToString();


                }

                DataSet dsPopulateAdditionalChargeDetailsGridview = new BLL.ListOfDraftInvoiceBLL().PopulateCreateInvoiceGridview("gvAdditionalChargeDetails", strInvCode);
                if (dsPopulateAdditionalChargeDetailsGridview.Tables[0].Rows.Count != 0)
                {
                    ViewState["PopulateAdditionchrgFromDraftScreen"] = dsPopulateAdditionalChargeDetailsGridview.Tables[0];
                    DataTable Addchargesfromgrid = (DataTable)ViewState["PopulateTableAdditionalCharge"];
                    foreach (DataRow dr in dsPopulateAdditionalChargeDetailsGridview.Tables[0].Rows)
                    {
                        DataRow drCurrentRow = Addchargesfromgrid.NewRow();
                        drCurrentRow["charge_no"] = dr[1].ToString();
                        int selectvalue = Convert.ToInt32(dr[6].ToString());
                        if (selectvalue <= 5)
                        {
                            drCurrentRow["Charge_id"] = ddlchtgetype.Items[selectvalue - 1].Text;
                        }
                        else if (selectvalue > 5)
                        {
                            drCurrentRow["Charge_id"] = ddlchtgetype.Items[selectvalue - 2].Text;
                        }
                        drCurrentRow["charge"] = dr[2].ToString();
                        drCurrentRow["amount"] = dr[3].ToString();
                        drCurrentRow["description"] = dr[4].ToString();
                        drCurrentRow["Gst_no"] = dr[5].ToString();
                        Addchargesfromgrid.Rows.Add(drCurrentRow);
                    }
                    ViewState["PopulateTableAdditionalCharge"] = Addchargesfromgrid;
                    gvAdditionalChargeDetails.DataSource = Addchargesfromgrid;
                    //  gvAdditionalChargeDetails.DataSource = dsPopulateAdditionalChargeDetailsGridview.Tables[0];
                    gvAdditionalChargeDetails.DataBind();

                    this.ExportWord.Visible = true;

                }


                DataSet dsPopulateAccountDistributionDetailsGridview = new BLL.ListOfDraftInvoiceBLL().PopulateCreateInvoiceGridview("gvAccountDistributionDetails", strInvCode);
                if (dsPopulateAccountDistributionDetailsGridview.Tables[0].Rows.Count != 0)
                {
                    gvAccountDistributionDetails.DataSource = dsPopulateAccountDistributionDetailsGridview.Tables[0];
                    gvAccountDistributionDetails.DataBind();

                }

                ViewState["PopulateAccountDistDetails"] = dsPopulateAccountDistributionDetailsGridview.Tables[0];
                BindAttachmentGridviewData();

            }
        }

        private void PopulateAllDates()
        {
            string strInvCodeFromListOfDraft = Request.QueryString["Inv_Code"];


            //If user in not redirecting from 'List of Draft' page to Create Invoice page
            if (string.IsNullOrEmpty(strInvCodeFromListOfDraft) == true)
            {
                if (txtInvoiceDate.Text.Trim().Length == 0)
                {
                    CalendarExtenderInvoiceDate.SelectedDate = DateTime.Now.Date;
                }
                else
                {
                    CalendarExtenderInvoiceDate.SelectedDate = Convert.ToDateTime(txtInvoiceDate.Text);
                }
                if (txtShippedDate.Text.Trim().Length == 0)
                {
                    CalendarExtenderShippedDate.SelectedDate = DateTime.Now.Date;
                }
                else
                {
                    CalendarExtenderShippedDate.SelectedDate = Convert.ToDateTime(txtShippedDate.Text);
                }

                //CalendarExtenderInvoiceDate.SelectedDate = DateTime.Now.Date;
                //CalendarExtenderShippedDate.SelectedDate = DateTime.Now.Date;
            }
            //If user in redirecting from 'List of Draft' page to Create Invoice page
            else
            {
                if (!string.IsNullOrEmpty(txtInvoiceDate.Text))
                {
                    CalendarExtenderInvoiceDate.SelectedDate = Convert.ToDateTime(txtInvoiceDate.Text);
                }
                if (!string.IsNullOrEmpty(txtShippedDate.Text))
                {
                    CalendarExtenderShippedDate.SelectedDate = Convert.ToDateTime(txtShippedDate.Text);
                }


            }

        }

        #endregion

        #region Populate data on PageLoad

        public void PopulateDropDownlist()
        {
            //Populate Payable to DropDownlist
            DataSet dsLoadPayableTo = new BLL.LoadValuesinDropDownlistBLL().LoadValuesinDropDownList("tbl_payable_to");
            if (dsLoadPayableTo.Tables[0].Rows.Count != 0)
            {
                ddlPayableTo.DataSource = dsLoadPayableTo.Tables[0];
                ddlPayableTo.DataTextField = "description";
                ddlPayableTo.DataValueField = "payable_id";
                ddlPayableTo.DataBind();
            }

            //Populate Shipped Via DropDownlist
            DataSet dsLoadShippedVia = new BLL.LoadValuesinDropDownlistBLL().LoadValuesinDropDownList("tbl_shippedvia");
            if (dsLoadShippedVia.Tables[0].Rows.Count != 0)
            {
                ddlShippedVia.DataSource = dsLoadShippedVia.Tables[0];
                ddlShippedVia.DataTextField = "description";
                ddlShippedVia.DataValueField = "shipped_id";
                ddlShippedVia.DataBind();
            }

            //Populate Shipped To DropDownlist
            DataSet dsLoadShippedTo = new BLL.LoadValuesinDropDownlistBLL().LoadValuesinDropDownList("tbl_Shipped_to");
            if (dsLoadShippedTo.Tables[0].Rows.Count != 0)
            {
                ddlShippedTo.DataSource = dsLoadShippedTo.Tables[0];
                ddlShippedTo.DataTextField = "description";
                ddlShippedTo.DataValueField = "Shipped_id";
                ddlShippedTo.DataBind();
            }

            DataSet dsLoadQtyUnitofMeasure = new BLL.LoadValuesinDropDownlistBLL().LoadValuesinDropDownList("tbl_qty_unitmeasure");
            if (dsLoadQtyUnitofMeasure.Tables[0].Rows.Count != 0)
            {
                ddlQtyUnitofMeasure.DataSource = dsLoadQtyUnitofMeasure.Tables[0];
                ddlQtyUnitofMeasure.DataTextField = "description";
                ddlQtyUnitofMeasure.DataValueField = "qty_unitofmeasure_id";
                ddlQtyUnitofMeasure.DataBind();
                qtyuom.DataSource = dsLoadQtyUnitofMeasure.Tables[0];
                qtyuom.DataTextField = "description";
                qtyuom.DataValueField = "qty_unitofmeasure_id";
                qtyuom.DataBind();
            }
            Hashtable hasuom = new Hashtable();
            foreach (DataRow dritem in dsLoadQtyUnitofMeasure.Tables[0].Rows)
            {
                hasuom.Add(dritem["qty_unitofmeasure_id"], dritem["description"]);
            }
            ViewState["HashtableChargeType"] = hasuom;
            DataSet dsLoadChargedType = new BLL.LoadValuesinDropDownlistBLL().LoadValuesinDropDownList("tbl_ChargeDetails");
            Hashtable hashcharge = new Hashtable();
            if (dsLoadChargedType.Tables[0].Rows.Count != 0)
            {
                ddlchtgetype.DataSource = dsLoadChargedType.Tables[0];
                ddlchtgetype.DataTextField = "Charge_type";
                ddlchtgetype.DataValueField = "Charge_id";
                ddlchtgetype.DataBind();

                foreach (DataRow dritem in dsLoadChargedType.Tables[0].Rows)
                {
                    hashcharge.Add(dritem["Charge_id"], dritem["Charge_type"]);

                }
                ViewState["HashtableCharge"] = hashcharge;
                //}
            }
        }

        //public void PopulateGridview()
        //{
        //    DataSet dsLineItemDetails = new BLL.CreateInvoiceBLL().PopulateGridView("gvLineItemDetails");
        //    if (dsLineItemDetails.Tables[0].Rows.Count != 0)
        //    {
        //        gvLineItemDetails.DataSource = dsLineItemDetails.Tables[0];
        //        gvLineItemDetails.DataBind();
        //        ViewState["TempTableLineItem"] = dsLineItemDetails.Tables[0];
        //    }

        //    DataSet dsAdditionalChargeDetails = new BLL.CreateInvoiceBLL().PopulateGridView("gvAdditionalChargeDetails");
        //    {
        //        gvAdditionalChargeDetails.DataSource = dsAdditionalChargeDetails.Tables[0];
        //        gvAdditionalChargeDetails.DataBind();
        //    }

        //    DataSet dsAccountDistributionDetails = new BLL.CreateInvoiceBLL().PopulateGridView("gvAccountDistributionDetails");
        //    {
        //        gvAccountDistributionDetails.DataSource = dsAccountDistributionDetails.Tables[0];
        //        gvAccountDistributionDetails.DataBind();
        //        ViewState["TempTableAccDist"] = dsAccountDistributionDetails.Tables[0];
        //    }

        //    //Populate footer dropdownlist Charge Type in Additional Charge Details Gridview
        //    DropDownList ddlfootChargeType = (DropDownList)gvAdditionalChargeDetails.FooterRow.FindControl("ddlfootChargeType");
        //    DataSet dsLoadChargedType = new BLL.LoadValuesinDropDownlistBLL().LoadValuesinDropDownList("tbl_ChargeDetails");
        //    if (dsLoadChargedType.Tables[0].Rows.Count != 0)
        //    {
        //        ddlfootChargeType.DataSource = dsLoadChargedType.Tables[0];
        //        ddlfootChargeType.DataTextField = "Charge_type";
        //        ddlfootChargeType.DataValueField = "Charge_id";
        //        ddlfootChargeType.DataBind();
        //    }

        //    //Populate footer dropdownlist Qty Unit of Measure in Line Items Details Gridview
        //    DropDownList ddlfootQty = (DropDownList)gvLineItemDetails.FooterRow.FindControl("ddlfootQtyUnitofMeasure");
        //    DataSet dsLoadQtyUnitofMeasure = new BLL.LoadValuesinDropDownlistBLL().LoadValuesinDropDownList("tbl_qty_unitmeasure");
        //    if (dsLoadQtyUnitofMeasure.Tables[0].Rows.Count != 0)
        //    {
        //        ddlfootQty.DataSource = dsLoadQtyUnitofMeasure.Tables[0];
        //        ddlfootQty.DataTextField = "description";
        //        ddlfootQty.DataValueField = "qty_unitofmeasure_id";
        //        ddlfootQty.DataBind();
        //    }

        //}

        public void PopulateTextboxes()
        {
            if (txtTotalAdditionalCharges.Text.Trim().Length == 0)
            {
                this.txtTotalAdditionalCharges.Text = "0";
            }

            if (txtTotalInvoiceAmount.Text.Trim().Length == 0)
            {
                this.txtTotalInvoiceAmount.Text = "0";
            }

            if (txtTotalLineAmount.Text.Trim().Length == 0)
            {
                this.txtTotalLineAmount.Text = "0";
            }
        }

        #endregion

        private void BindAttachmentGridviewData()
        {
            string userId = Session["Userid"].ToString();
            BLL.AttachmentBLLcs obj1 = new AttachmentBLLcs();
            AttachmentBO attachbo = new AttachmentBO();
            attachbo.Userid = userId;
            DataSet dsLoadPayableTo = new DataSet();
            if (ViewState["draftnofromlist"] != null)
            {
                attachbo.Inv_Code = ViewState["draftnofromlist"].ToString();
                dsLoadPayableTo = obj1.GetAllAttachmentFileDetailsByDraftID(attachbo);
            }
            else if (ViewState["InvCodeFromInvoiceStatusScreen"] != null)
            {
                attachbo.Inv_Code = ViewState["InvCodeFromInvoiceStatusScreen"].ToString();
                dsLoadPayableTo = obj1.GetAllAttachmentFileDetailsByInvoiceID(attachbo);
            }
            else
            {
                dsLoadPayableTo = obj1.GetAttachmentInfo(attachbo);
            }

            if (dsLoadPayableTo.Tables.Count > 0)
            {
                gvDetails.DataSource = dsLoadPayableTo;
                gvDetails.DataBind();
                if (dsLoadPayableTo.Tables[0].Rows.Count >= 4)
                {
                    AttachButton.Visible = false;
                    UploadStatusLabel.Text = "4 Attachments Limit Reached";
                }
                //else if (dsLoadPayableTo.Tables[0].Rows.Count == 0)
                //{
                //    AttachButton.Visible = false;
                //}
                else
                    AttachButton.Visible = true;
                ViewState["TempTableFileInfo"] = dsLoadPayableTo.Tables[0];
            }
        }

        private void PopulateSupplier()
        {
            SupplierDetailsBO objSupDetail = new SupplierDetailsBO();
            objSupDetail = (SupplierDetailsBO)Session["SupDetails"];
            if (objSupDetail != null)
            {
                txtSupplierCode.Text = objSupDetail.Code;
                txtEmailAddress.Text = objSupDetail.EmailId;
                txtCurrency.Text = objSupDetail.Currency;
                string fullAddress = objSupDetail.Address1 + "," + objSupDetail.Address2 + "," + objSupDetail.Address3 + "," + objSupDetail.Street + "," + objSupDetail.City + "," + objSupDetail.State + "," + objSupDetail.Country + "," + objSupDetail.PostCode;
                txtSupplierAddress.Text = fullAddress;
            }

        }

        private void PopulateSupplierfromLookUp()
        {

            string SCodel = Session["SupDetailsfromLookup"].ToString();

            DataSet dsSupplierDtl1 = new BLL.SupplierDetailsBLL().GetSupplierDetailsById(SCodel);

            if (dsSupplierDtl1.Tables[0].Rows.Count != 0)
            {
                txtSupplierCode.Text = SCodel;
                txtEmailAddress.Text = dsSupplierDtl1.Tables[0].Rows[0][12].ToString(); ;
                txtCurrency.Text = dsSupplierDtl1.Tables[0].Rows[0][13].ToString();
                string ad1 = dsSupplierDtl1.Tables[0].Rows[0][2].ToString();
                string ad2 = dsSupplierDtl1.Tables[0].Rows[0][3].ToString();
                string ad3 = dsSupplierDtl1.Tables[0].Rows[0][4].ToString();
                string street = dsSupplierDtl1.Tables[0].Rows[0][5].ToString();
                string city = dsSupplierDtl1.Tables[0].Rows[0][6].ToString();
                string state = dsSupplierDtl1.Tables[0].Rows[0][7].ToString();
                string country = dsSupplierDtl1.Tables[0].Rows[0][8].ToString();
                string postcode = dsSupplierDtl1.Tables[0].Rows[0][9].ToString();
                string fullAddress1 = ad1 + "," + ad2 + "," + ad3 + "," + street + "," + city + "," + state + "," + country + "," + postcode;
                txtSupplierAddress.Text = fullAddress1;
            }
        }

        #region attachment Gridview

        protected void btnsave_Click(object sender, EventArgs e)
        {

            string savePath = Server.MapPath("~") + @"\Attachments\" + Session["Userid"].ToString() + @"\";

            if (FileUpload1.HasFile)
            {

                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                
                string fileName = FileUpload1.FileName;
                string pathToCheck = savePath + fileName;
                // Call a helper method routine to save the file.


                // Check to see if a file already exists with the
                // same name as the file to upload.        
                if (System.IO.File.Exists(pathToCheck))
                {

                    // Notify the user that the file name was changed.
                    UploadStatusLabel.Text = "A file with the same name already exists." +
                        "<br />Please remove the file before continuing ";
                    return;
                }
                else
                {
                    // Notify the user that the file was saved successfully.
                    //UploadStatusLabel.Text = "Your file was uploaded successfully.";
                }

                // Append the name of the file to upload to the path.
                savePath += fileName;

                //code to filter extension and size

                if (this.FileUpload1.PostedFile.ContentLength > 1048576 * 2)
                {
                    UploadStatusLabel.Text = "Uploaded file is exceeding size limit. Please upload file less than 2MB size";
                    return;
                }

                // code end
                // Call the SaveAs method to save the uploaded
                // file to the specified directory.

                //throw new Exception("File size exceeds the limit of 100kb");
                string fileName1 = FileUpload1.PostedFile.FileName;
                string ext = Path.GetExtension(fileName1).ToLower();
                if (ext.Equals(".doc") || ext.Equals(".docx") || ext.Equals(".xls") || ext.Equals(".xlsx") || ext.Equals(".jpg") || ext.Equals(".jpeg"))
                {
                    FileUpload1.SaveAs(savePath);
                    ViewState["path"] = savePath;
                    UploadStatusLabel.Text = "Your file was uploaded successfully.";
                }
                else
                {
                    UploadStatusLabel.Text = " Uploaded file type is not allowed. Please upload files like doc,excel and jpg";
                    return;

                }


                AttachmentBO attachbo = new AttachmentBO();
                attachbo.Filename = fileName;
                attachbo.Userid = Session["Userid"].ToString();
                if (ViewState["draftnofromlist"] != null)
                {
                    attachbo.Inv_Code = ViewState["draftnofromlist"].ToString();
                    //dsLoadPayableTo = obj1.GetAllAttachmentFileDetailsByDraftID(attachbo);
                }

                int attachmentId = new BLL.AttachmentBLLcs().InsertFileInfo(attachbo);
                BindAttachmentGridviewData();

            }
            //SaveFile(FileUpload1.PostedFile);
            else
                // Notify the user that a file was not uploaded.
                UploadStatusLabel.Text = "You did not specify a file to upload.";

        }


        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Click")
            {


                // LinkButton lnk1 = gvDetails//.FindControl("Link1");



                int rowIndex = int.Parse(e.CommandArgument.ToString());
                //LinkButton lnk1 = (LinkButton)gvDetails.Rows[rowIndex].Cells[1].FindControl("link1");

                string val = (string)this.gvDetails.DataKeys[rowIndex]["File_name"];

                string strURL = string.Empty;
                if (ViewState["draftnofromlist"] != null)
                {
                    strURL = @"attachments\\" + ViewState["draftnofromlist"] + @"\\" + val;
                    //ClientScript.RegisterClientScriptBlock(typeof(object), "", "<script>alert('" + strURL.ToString() + "')</script>");
                    string fileindraft = Server.MapPath("~") + @"\attachments\" + ViewState["draftnofromlist"] + @"\" + val;
                    //ClientScript.RegisterClientScriptBlock(typeof(object), "", "<script>alert('" + fileindraft.ToString() + "')</script>");
                    if (!File.Exists(fileindraft))
                    {
                        strURL = @"attachments\\" + val;
                    }
                }
                else if (ViewState["InvCodeFromInvoiceStatusScreen"] != null)
                {
                    strURL = @"attachments\\" + ViewState["InvCodeFromInvoiceStatusScreen"] + @"\\" + val;
                    //ClientScript.RegisterClientScriptBlock(typeof(object), "", "<script>alert(' str1" + strURL.ToString() + "')</script>");

                    string fileindraft = Server.MapPath("~") + @"\attachments\" + ViewState["InvCodeFromInvoiceStatusScreen"] + @"\" + val;

                    //ClientScript.RegisterClientScriptBlock(typeof(object), "", "<script>alert(' file draft:" + fileindraft.ToString() + "')</script>");
                    if (!File.Exists(fileindraft))
                    {
                        strURL = @"attachments\\" + val;
                    }
                }
                else
                {
                    strURL = @"attachments\\" + Session["Userid"].ToString() + @"\\" + val;
                }

                string fullpath1 = string.Empty;
                fullpath1 = Server.MapPath("~") + @"\" + strURL;
                // ClientScript.RegisterClientScriptBlock(typeof(object), "", "<script>alert('fullpath1 :" + fullpath1.ToString() + "')</script>");
                if (File.Exists(fullpath1))
                {
                    //string strURL = @"attachments\" + val;
                    WebClient req = new WebClient();
                    HttpResponse response = HttpContext.Current.Response;
                    response.Clear();
                    response.ClearContent();
                    response.ClearHeaders();
                    response.Buffer = true;
                    string fullpath = Server.MapPath(strURL);
                    response.AddHeader("Content-Disposition", "attachment;filename=\"" + Server.MapPath(strURL) + "\"");
                    byte[] data = req.DownloadData(Server.MapPath(strURL));
                    response.BinaryWrite(data);
                    //ScriptManager.GetCurrent(this).RegisterPostBackControl(lnk1);
                    response.End();
                    //Updatepanel1.Update();
                    //ClientScript.RegisterClientScriptBlock(typeof(object), "", "<script>alert('" + fullpath1.ToString() + "')</script>");

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(typeof(object), "", "<script>alert('Files does not exists')</script>");
                    // ClientScript.RegisterClientScriptBlock(typeof(object), "", "<script>alert('" + fullpath1.ToString() +  "')</script>");
                }

            }

        }

        protected void gvDetails_RowCreated(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowIndex > -1)
            {

                int rowIndex = Convert.ToInt32(e.Row.RowIndex.ToString());
                string val = (string)this.gvDetails.DataKeys[rowIndex]["File_name"];
                LinkButton lnk = (LinkButton)e.Row.FindControl("link1");
                lnk.Text = val;
                string savePath = Server.MapPath("~") + @"\Attachments\" + val;
                HiddenField1.Value = savePath;

                if (ViewState["InvCodeFromInvoiceStatusScreen"] != null)
                {

                    LinkButton Deletelnk = (LinkButton)e.Row.FindControl("lnkFileDelete");
                    this.gvDetails.Enabled = true;
                    Deletelnk.Enabled = false;
                    lnk.Enabled = true;
                }

            }

        }

        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            BLL.AttachmentBLLcs obj1 = new AttachmentBLLcs();
            AttachmentBO attachbo = new AttachmentBO();
            string userId = Session["Userid"].ToString();
            attachbo.Userid = userId;
            //*************************************************
            //ViewState["draftnofromlist"] = strInvCode;
            string draftCode = Convert.ToString(ViewState["draftnofromlist"]);
            attachbo.Inv_Code = draftCode;
            //***************************************************
            DataSet dt = obj1.GetAttachmentInfo(attachbo);
            gvDetails.DataSource = dt;
            gvDetails.DataBind();
            int a = e.RowIndex;
            //string str = dt.Rows[a][0].ToString(); 
            DataTable AttachmentTable = new DataTable();
            AttachmentTable = dt.Tables[0];
            attachbo.AttachmentId = Convert.ToInt32(AttachmentTable.Rows[a][0].ToString());
            string attachment = AttachmentTable.Rows[a][1].ToString();

            DataSet dsLoadPayableTo = obj1.SendDeletedAttachmentInfo(attachbo);
            //dt.Rows.RemoveAt(e.RowIndex);
            gvDetails.DataSource = dsLoadPayableTo;
            gvDetails.DataBind();
            string deletepath=string.Empty;
            if (ViewState["draftnofromlist"] != null)
            {
                deletepath = Server.MapPath("~") + @"\Attachments\" + ViewState["draftnofromlist"] + @"\"+ attachment;
            }
            else
            {
                deletepath = Server.MapPath("~") + @"\Attachments\" + Session["Userid"].ToString() + @"\" + attachment;
            }
            

            File.Delete(deletepath);
            UploadStatusLabel.Text = "";
            DataSet dt1 = obj1.GetAttachmentInfo(attachbo);
            gvDetails.DataSource = dt1;
            gvDetails.DataBind();
            if (dt1.Tables[0].Rows.Count >= 4)
            {
                AttachButton.Visible = false;
                UploadStatusLabel.Text = "4 Attachments Limit Reached";
            }
            //else if (dsLoadPayableTo.Tables[0].Rows.Count == 0)
            //{
            //    AttachButton.Visible = false;
            //}
            else
                AttachButton.Visible = true;
        }


        #endregion

        private void SetInitialRow()
        {

            DataTable dt = new DataTable();
            //DataRow dr = null;
            // dt.Columns.Add(new DataColumn("Inv_No", typeof(string)));
            dt.Columns.Add(new DataColumn("part_item_no", typeof(string)));
            dt.Columns.Add(new DataColumn("po_no", typeof(string)));
            dt.Columns.Add(new DataColumn("po_amendment_no", typeof(string)));
            dt.Columns.Add(new DataColumn("release_no", typeof(string)));
            dt.Columns.Add(new DataColumn("qty_shipped", typeof(string)));
            dt.Columns.Add(new DataColumn("unit_price", typeof(string)));
            dt.Columns.Add(new DataColumn("amount", typeof(string)));
            dt.Columns.Add(new DataColumn("qty_unitofmeasure_id", typeof(string)));
            dt.Columns.Add(new DataColumn("packing_slip", typeof(string)));
            dt.Columns.Add(new DataColumn("bill_lading", typeof(string)));
            dt.Columns.Add(new DataColumn("Comments", typeof(string)));
            // DataRow dr = dt.NewRow();
            //dt.Rows.Add(dr);
            //ViewState["TempTableLineItemDetails"] = dt;
            ViewState["PopulatedTableLineItemDetails"] = dt;
            // ViewState["PopulateTableAdditionalChargeDetails"] = dt;


        }

        protected void gvLineItemDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = (DataTable)ViewState["PopulatedTableLineItemDetails"];


            #region Calculation of Line Item amounts and Total Invoice amount after deletion

            int rowIndex = 0;
            int totalAdditionalChargesAmount2 = 0;
            int totalAdditionalChargesAmountAfterDelete = 0;

            //int LineAmount = Convert.ToInt32(e.RowIndex..Cells[4].Text);
            long totalLineAmountdel = 0;
            if (ViewState["totallineamount"] != null)
            {
                if (ViewState["totallineamount"].ToString().Contains('('))
                {
                    string[] totallineamt = ViewState["totallineamount"].ToString().Split(new[] { '(', ')' });
                    totalLineAmountdel = -Convert.ToInt64(Convert.ToDecimal(totallineamt[1]));
                }
                else
                {
                    totalLineAmountdel = Convert.ToInt64(Convert.ToDecimal(ViewState["totallineamount"]));
                }

                //totalLineAmount = Convert.ToInt32(Convert.ToDecimal(ViewState["totallineamount"]));
            }

            long AmountFromAdditional = 0;
            if (ViewState["totalAdditionalAmount"] != null)
            {
                AmountFromAdditional = Convert.ToInt64(Convert.ToDecimal(ViewState["totalAdditionalAmount"]));
            }
            int rowind = Convert.ToInt32(e.RowIndex.ToString());
            long LineAmount = 0;

            //int totalineamt = 0;
            //if (ViewState["totallineamount"] != null)
            //{
            //    totalineamt = Convert.ToInt32(Convert.ToDecimal(ViewState["totallineamount"]));
            //}


            if (txtTotalInvoiceAmount.Text.Contains('('))
            {
                string[] res = txtTotalInvoiceAmount.Text.Split(new[] { '(', ')' });
                txtTotalInvoiceAmount.Text = "-" + res[1];
            }

            if (!string.IsNullOrEmpty(txtTotalInvoiceAmount.Text))
            {
                if (dt.Rows[rowind].ItemArray[6].ToString().Contains('('))
                {
                    string[] res = (dt.Rows[rowind].ItemArray[6].ToString()).Split(new[] { '(', ')' });
                    LineAmount = Convert.ToInt64(Convert.ToDecimal(res[1]));
                    long totallineamtafterdelete = totalLineAmountdel + LineAmount;
                    ViewState["totallineamount"] = totallineamtafterdelete;
                    txtTotalLineAmount.Text = totallineamtafterdelete.ToString();
                    txtTotalInvoiceAmount.Text = (Convert.ToInt32(Convert.ToDecimal(txtTotalInvoiceAmount.Text)) + LineAmount).ToString();
                }
                else
                {
                    LineAmount = Convert.ToInt64(Convert.ToDecimal(dt.Rows[rowind].ItemArray[6].ToString()));
                    long totallineamtafterdelete = totalLineAmountdel - LineAmount;
                    ViewState["totallineamount"] = totallineamtafterdelete;
                    txtTotalLineAmount.Text = totallineamtafterdelete.ToString();
                    txtTotalInvoiceAmount.Text = (Convert.ToInt64(Convert.ToDecimal(txtTotalInvoiceAmount.Text)) - LineAmount).ToString();
                }

                if (Convert.ToInt32(txtTotalLineAmount.Text) < 0)
                {
                    string[] res = txtTotalLineAmount.Text.Split(new[] { '-' });
                    txtTotalLineAmount.Text = "(" + res[1] + ")";

                }
                if (Convert.ToInt32(txtTotalInvoiceAmount.Text) < 0)
                {
                    string[] res = txtTotalInvoiceAmount.Text.Split(new[] { '-' });
                    txtTotalInvoiceAmount.Text = "(" + res[1] + ")";

                }
            }
            #endregion


            dt.Rows.RemoveAt(e.RowIndex);
            gvLineItemDetails.DataSource = dt;
            gvLineItemDetails.DataBind();

        }

        protected void gvLineItemDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            ViewState["lineitemrowindex"] = Convert.ToInt32(e.NewEditIndex);
            DataTable lintmdata = (DataTable)ViewState["PopulatedTableLineItemDetails"];
            txtprtnumbr.Text = lintmdata.Rows[e.NewEditIndex].ItemArray[0].ToString();
            txtponumbr.Text = lintmdata.Rows[e.NewEditIndex].ItemArray[1].ToString();
            txtpoamend.Text = lintmdata.Rows[e.NewEditIndex].ItemArray[2].ToString();
            txtrelease.Text = lintmdata.Rows[e.NewEditIndex].ItemArray[3].ToString();
            txtqtyshipped.Text = lintmdata.Rows[e.NewEditIndex].ItemArray[4].ToString();
            txtunitprice.Text = lintmdata.Rows[e.NewEditIndex].ItemArray[5].ToString();
            txtlineamt.Text = lintmdata.Rows[e.NewEditIndex].ItemArray[6].ToString();
            ViewState["lineamtforupdating"] = lintmdata.Rows[e.NewEditIndex].ItemArray[6].ToString();
            Hashtable hs = (Hashtable)ViewState["HashtableChargeType"];
            string val = lintmdata.Rows[e.NewEditIndex].ItemArray[7].ToString();
            int qtyid = 0;
            foreach (DictionaryEntry entry in hs)
            {

                // entry.Key, entry.Value);
                if (entry.Value.ToString().ToLower().Trim().Equals(val.ToLower().Trim()))
                {
                    qtyid = Convert.ToInt32(entry.Key);
                    break;
                }
            }
            ddlQtyUnitofMeasure.SelectedValue = qtyid.ToString();
            txtpack.Text = lintmdata.Rows[e.NewEditIndex].ItemArray[8].ToString();
            txtbilloflading.Text = lintmdata.Rows[e.NewEditIndex].ItemArray[9].ToString();
            txtComm.Text = lintmdata.Rows[e.NewEditIndex].ItemArray[10].ToString();
           
            btnadd.Visible = false;
            Updatetnfoelne.Visible = true;
            Cancelbtnforlne.Visible = true;
        }

        protected void btnadd_Click(object sender, EventArgs e)
        {
            errmessage.Text = string.Empty;
            if (txtprtnumbr.Text != string.Empty && txtponumbr.Text != string.Empty && txtrelease.Text != string.Empty && txtqtyshipped.Text != string.Empty && txtunitprice.Text != string.Empty && ddlQtyUnitofMeasure.SelectedValue != "1")
            {
                DataTable dtCurrentTable = (DataTable)ViewState["PopulatedTableLineItemDetails"];
                Hashtable qom = (Hashtable)ViewState["HashtableListOfQtyUnitOfMeasure"];
                if (dtCurrentTable != null)
                {
                    string prtnum = txtprtnumbr.Text.ToString();
                    string pomun = txtponumbr.Text.ToString();
                    foreach (DataRow dr in dtCurrentTable.Rows)
                    {
                        if (dr[0].ToString().ToLower() == prtnum.ToLower() && dr[1].ToString().ToLower() == pomun.ToLower())
                        {
                            // ClientScript.RegisterClientScriptBlock(typeof(object), "Alert", "<script>alert('script Same PO number and Part number already exists ')</script>",true);
                            //string AlerteMsg = "Same PO number and Part number already exists";
                            //ClientScript.RegisterClientScriptBlock(typeof(object), "Alert", AlerteMsg, true);
                            //string display = "Same PO number and Part number are already added";
                            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);


                            //Response.Write("<script type='text/javascript'>alert('" + AlerteMsg + "');</script>");
                            errmessage.Text = "Same PO number and Part number already added";
                            return;
                        }
                    }
                }

                DataRow drCurrentRow = dtCurrentTable.NewRow();
                drCurrentRow["part_item_no"] = txtprtnumbr.Text.ToString();
                drCurrentRow["po_no"] = txtponumbr.Text.ToString();
                drCurrentRow["po_amendment_no"] = txtpoamend.Text.ToString();
                drCurrentRow["release_no"] = txtrelease.Text.ToString();


                //Negative value
                if (Convert.ToInt32(txtqtyshipped.Text) < 0)
                {
                    string negativeValue = txtqtyshipped.Text;
                    string[] results = negativeValue.Split(new[] { '-' });
                    drCurrentRow["qty_shipped"] = "(" + results[1] + ")";
                }
                else
                {
                    //Positive value
                    drCurrentRow["qty_shipped"] = txtqtyshipped.Text.ToString();
                }

                //Negative value
                if (Convert.ToInt64(txtunitprice.Text) < 0)
                {
                    string negativeValue = txtunitprice.Text;
                    string[] results = negativeValue.Split(new[] { '-' });
                    drCurrentRow["unit_price"] = "(" + results[1] + ")";
                }
                else
                {
                    //positive value added by sach
                   // drCurrentRow["unit_price"] = Convert.ToDecimal(txtunitprice.Text.ToString());
                    //drCurrentRow["unit_price"] = Double.Parse(txtunitprice.Text.ToString());
                    drCurrentRow["unit_price"] = txtunitprice.Text.ToString();      
                    
                }

                if (Convert.ToInt64(txtlineamt.Text) < 0)
                {
                    string negativeValue = txtlineamt.Text;
                    string[] results = negativeValue.Split(new[] { '-' });
                    drCurrentRow["amount"] = "(" + results[1] + ")";
                }
                else
                {
                    //positive value
                    drCurrentRow["amount"] = txtlineamt.Text.ToString(); //Convert.ToInt32(txtqtyshipped.Text.ToString()) * Convert.ToInt32(txtunitprice.Text.ToString());
                    //drCurrentRow["amount"] = String.Format("{0:0.00}", Double.Parse(txtqtyshipped.Text.ToString()));
                }


                drCurrentRow["qty_unitofmeasure_id"] = ddlQtyUnitofMeasure.SelectedItem.Text;
                drCurrentRow["packing_slip"] = txtpack.Text.ToString();
                drCurrentRow["bill_lading"] = txtbilloflading.Text.ToString();
                drCurrentRow["Comments"] = txtComm.Text.ToString();
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["PopulatedTableLineItemDetails"] = dtCurrentTable;
                gvLineItemDetails.DataSource = dtCurrentTable;
                gvLineItemDetails.DataBind();


                //Addition of line amount

                if (ViewState["totallineamount"] != null)
                {
                    if (ViewState["totallineamount"].ToString().Contains('('))
                    {
                        string[] totallineamt = ViewState["totallineamount"].ToString().Split(new[] { '(', ')' });
                        totalLineAmount = -Convert.ToInt64(Convert.ToDecimal(totallineamt[1]));
                    }
                    else
                    {
                        totalLineAmount = Convert.ToInt64(Convert.ToDecimal(ViewState["totallineamount"]));
                    }
                }

                long AmountFromAdditional = 0;
                if (ViewState["totalAdditionalAmount"] != null)
                {
                    AmountFromAdditional = Convert.ToInt64(Convert.ToDecimal(ViewState["totalAdditionalAmount"]));
                }


                totalLineAmount = Convert.ToInt64(Convert.ToDecimal(txtlineamt.Text)) + totalLineAmount;
                ViewState["totallineamount"] = totalLineAmount;




                if (totalLineAmount < 0)
                {
                    string[] res = totalLineAmount.ToString().Split(new[] { '-' });
                    txtTotalLineAmount.Text = "(" + res[1] + ")";
                }
                else
                {
                    txtTotalLineAmount.Text = totalLineAmount.ToString();
                    //txtTotalLineAmount.Text = String.Format("{0:0.00}", Double.Parse(txtTotalLineAmount.Text.ToString()));

                }

                if ((totalLineAmount + AmountFromAdditional) < 0)
                {
                    string[] res = (totalLineAmount + AmountFromAdditional).ToString().Split(new[] { '-' });
                    txtTotalInvoiceAmount.Text = "(" + res[1] + ")";
                }
                else
                {
                    txtTotalInvoiceAmount.Text = (totalLineAmount + AmountFromAdditional).ToString();
                }
                rowcounthidden.Value = gvLineItemDetails.Rows.Count.ToString();
                clearlineitemtextboxes();
            }

        }

        protected ArrayList getLineItemDtls()
        {
            ArrayList getitm = new ArrayList();
            //System.Collections.ArrayList getitm = new ArrayList();
            DataTable lineitm = (DataTable)ViewState["PopulatedTableLineItemDetails"];
            double totallineamt = 0;
            double totalamt = 0;
            //}

            //Code to indicate Submit or save draft button has been clicked
            int gvRowCount = 0;
            if (ViewState["draftnofromlistForlineitemGrid"] != null)
            {
                gvRowCount = gvLineItemDetails.Rows.Count;
            }
            else if (ViewState["PopulatedTableLineItemDetails"] != null)
            {
                gvRowCount = gvLineItemDetails.Rows.Count;
            }
            else
            {
                gvRowCount = gvLineItemDetails.Rows.Count - 1;
            }
            lineitemsBO objline;
            if (gvRowCount > 0)
            {
                //for (int i = 0; i < gvRowCount; i++)
                foreach (DataRow dr in lineitm.Rows)
                {

                    // ViewState["Totallineamt"] = totallineamt;
                    //ViewState["TotalAmt"] = totalamt;
                    // drCurrentRow = dtCurrentTable.NewRow();
                    // ArrayList getitm = new ArrayList();
                    objline = new lineitemsBO();

                    // drCurrentRow = dtCurrentTable.NewRow();
                    //drCurrentRow["Inv_No"] = a + 1;
                    //int multi = Convert.ToInt32(TextBoxQtyShipped.Text) * Convert.ToInt32(TextBoxUnitPrice.Text);
                    //TextBoxAmount.Text = Convert.ToString(multi);
                    Hashtable qom = (Hashtable)ViewState["HashtableListOfQtyUnitOfMeasure"];




                    string flgFrmSvBtn = Convert.ToString(ViewState["FlagFrmSaveBtn"]);
                    string flgFrmSbmtBtn = Convert.ToString(ViewState["FlagFrmSubmitBtn"]);

                    if (flgFrmSvBtn == "yes")
                    {
                        objline.InvNo = Convert.ToInt32(ViewState["InvNoForAll3GrdFromSaveBtn"]);
                        objline.InvCode = Convert.ToString(ViewState["InvCodeForAll3GrdFromSaveBtn"]);
                    }
                    else if (flgFrmSbmtBtn == "yes")
                    {

                        objline.InvNo = Convert.ToInt32(ViewState["InvNoForAll3GrdFromSubmitBtn"]);
                        objline.InvCode = Convert.ToString(ViewState["InvCodeForAll3GrdFromSubmitBtn"]);
                    }





                    objline.Partitemnumber = dr["part_item_no"].ToString();
                    objline.Ponumber = dr["po_no"].ToString();
                    objline.Poamendmentnumber = dr["po_amendment_no"].ToString();
                    objline.Releasenumber = dr["release_no"].ToString();
                    //objline.Qtyshipped = dr["qty_shipped"].ToString();
                    if (dr["qty_shipped"].ToString().Contains('('))
                    {
                        string[] qty_shipped = dr["qty_shipped"].ToString().Split(new[] { '(', ')' });
                        objline.Qtyshipped = qty_shipped[1];
                        objline.QunatityShipInd = "Cr";

                    }
                    else
                    {
                        objline.Qtyshipped = dr["qty_shipped"].ToString();
                        objline.QunatityShipInd = "Dr";
                    }

                    // Amol Code
                    Hashtable hs = (Hashtable)ViewState["HashtableChargeType"];
                    string val = dr["qty_unitofmeasure_id"].ToString();
                    int qtyid = 0;
                    foreach (DictionaryEntry entry in hs)
                    {

                        // entry.Key, entry.Value);
                        if (entry.Value.ToString().ToLower().Trim().Equals(val.ToLower().Trim()))
                        {
                            qtyid = Convert.ToInt32(entry.Key);
                            break;
                        }
                    }
                    objline.Qtyuom = qtyid;
                    // Amol code end
                    //string strvalue = dr["qty_unitofmeasure_id"].ToString();
                    //int strcode = qtyuom.Items.ValueOf(ddlQtyUnitofMeasure.Items.FindByText(strvalue));
                    //int strcode = qtyuom.SelectedValue.
                    //objline.Qtyuom = Convert.ToInt32(dr["qty_unitofmeasure_id"].ToString());

                    //objline.Amount = dr["amount"].ToString();
                    if (dr["amount"].ToString().Contains('('))
                    {
                        string[] amount = dr["amount"].ToString().Split(new[] { '(', ')' });
                        objline.Amount = amount[1];
                        objline.AmountInd = "Cr";
                    }
                    else
                    {
                        objline.Amount = dr["amount"].ToString();
                        objline.AmountInd = "Dr";
                    }
                    objline.Packingslip = dr["packing_slip"].ToString();
                    objline.Billlading = dr["bill_lading"].ToString();
                    objline.Comments = dr["Comments"].ToString();

                    //objline.Unitprice = dr["unit_price"].ToString();
                    if (dr["unit_price"].ToString().Contains('('))
                    {
                        string[] unit_price = dr["unit_price"].ToString().Split(new[] { '(', ')' });
                        objline.Unitprice = unit_price[1];
                        objline.UnitPriceInd = "Cr";
                    }
                    else
                    {
                        objline.Unitprice = dr["unit_price"].ToString();
                        objline.UnitPriceInd = "Dr";
                    }

                    //getitm.Add(objline);

                    getitm.Add(objline);
                }
            }

            return getitm;

        }

        protected void clearlineitemtextboxes()
        {
            txtprtnumbr.Text = string.Empty;
            txtpoamend.Text = string.Empty;
            txtponumbr.Text = string.Empty;
            txtrelease.Text = string.Empty;
            txtqtyshipped.Text = string.Empty;
            txtunitprice.Text = string.Empty;
            txtlineamt.Text = string.Empty;
            ddlQtyUnitofMeasure.SelectedValue = "1";
            txtpack.Text = string.Empty;
            txtbilloflading.Text = string.Empty;
            txtComm.Text = string.Empty;

        }

        #region Additional Charges
        private void SetNewRowAdditionalCharge()
        {

            DataTable dt = new DataTable();
            //DataRow dr = null;
            //dt.Columns.Add(new DataColumn("Inv_no", typeof(string)));
            dt.Columns.Add(new DataColumn("charge_no", typeof(string)));
            dt.Columns.Add(new DataColumn("Charge_id", typeof(string)));
            dt.Columns.Add(new DataColumn("charge", typeof(string)));
            dt.Columns.Add(new DataColumn("amount", typeof(string)));
            dt.Columns.Add(new DataColumn("description", typeof(string)));
            dt.Columns.Add(new DataColumn("Gst_no", typeof(string)));
            //dr = dt.NewRow();
            //dt.Rows.Add(dr);
            //dr = dt.NewRow();
            //ViewState["TempTableAdditionalCharge"] = dt;
            ViewState["PopulateTableAdditionalCharge"] = dt;
            gvAdditionalChargeDetails.DataSource = dt;
            gvAdditionalChargeDetails.DataBind();


        }

        protected void gvAdditionalChargeDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = (DataTable)ViewState["PopulateTableAdditionalCharge"];


            #region Calculation of Additional Charge amounts and Total Invoice amount after deletion

            int rowIndex = 0;
            int totalAdditionalChargesAmount2 = 0;
            int totalAdditionalChargesAmountAfterDelete = 0;

            //int LineAmount = Convert.ToInt32(e.RowIndex..Cells[4].Text);

            if (ViewState["totallineamount"] != null)
            {
                if (ViewState["totallineamount"].ToString().Contains('('))
                {
                    string[] totallineamt = ViewState["totallineamount"].ToString().Split(new[] { '(', ')' });
                    totalLineAmount = -Convert.ToInt64(Convert.ToDecimal(totallineamt[1]));
                }
                else
                {
                    totalLineAmount = Convert.ToInt64(Convert.ToDecimal(ViewState["totallineamount"]));
                }
            }

            long AmountFromAdditional = 0;
            if (ViewState["totalAdditionalAmount"] != null)
            {
                AmountFromAdditional = Convert.ToInt64(Convert.ToDecimal(ViewState["totalAdditionalAmount"]));
            }
            int rowind = Convert.ToInt32(e.RowIndex.ToString());
            long Addcharge = Convert.ToInt64(Convert.ToDouble(dt.Rows[rowind].ItemArray[4].ToString()));
            long totaladdcharge = 0;
            if (ViewState["totalAdditionalAmount"] != null)
            {
                totaladdcharge = Convert.ToInt64(Convert.ToDecimal(ViewState["totalAdditionalAmount"]));
            }
            long totaladdchargeafterdelete = totaladdcharge - Addcharge;
            ViewState["totalAdditionalAmount"] = totaladdchargeafterdelete;
            txtTotalAdditionalCharges.Text = totaladdchargeafterdelete.ToString();
            if (txtTotalInvoiceAmount.Text.Contains('('))
            {
                string[] totalInvoiceAmount = txtTotalInvoiceAmount.Text.Split(new[] { '(', ')' });
                txtTotalInvoiceAmount.Text = totalInvoiceAmount[1];
                txtTotalInvoiceAmount.Text = (-Convert.ToInt64(Convert.ToDouble(txtTotalInvoiceAmount.Text)) - Addcharge).ToString();
            }
            else
            {



                txtTotalInvoiceAmount.Text = (Convert.ToInt64(Convert.ToDouble(txtTotalInvoiceAmount.Text)) - Addcharge).ToString();
            }

            if (Convert.ToInt64(txtTotalInvoiceAmount.Text) < 0)
            {
                string[] res = txtTotalInvoiceAmount.Text.Split(new[] { '-' });
                txtTotalInvoiceAmount.Text = "(" + res[1] + ")";

            }
            #endregion
            dt.Rows.RemoveAt(e.RowIndex);
            ViewState["PopulateTableAdditionalCharge"] = dt;
            gvAdditionalChargeDetails.DataSource = dt;
            gvAdditionalChargeDetails.DataBind();
        }

        protected void btnaddchrge_Click(object sender, EventArgs e)
        {
            if (txtchrge.Text != string.Empty && txtaddamt.Text != string.Empty && txtdescp.Text != string.Empty && txtgst.Text != string.Empty && ddlchtgetype.SelectedValue != "1" && txtchrgenmber.Text != string.Empty)
            {

                DataTable dtCurrentTable = (DataTable)ViewState["PopulateTableAdditionalCharge"];
                DataRow drCurrentRow = dtCurrentTable.NewRow();
                drCurrentRow["charge_no"] = txtchrgenmber.Text.ToString(); ;
                drCurrentRow["Charge_id"] = ddlchtgetype.SelectedItem.Text;
                drCurrentRow["charge"] = Convert.ToInt64(txtchrge.Text);
                drCurrentRow["amount"] =txtaddamt.Text.ToString();
                drCurrentRow["description"] = txtdescp.Text.ToString();
                drCurrentRow["Gst_no"] = txtgst.Text.ToString();
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["PopulateTableAdditionalCharge"] = dtCurrentTable;
                gvAdditionalChargeDetails.DataSource = dtCurrentTable;
                gvAdditionalChargeDetails.DataBind();

                //
                //Addition of amount of additional charge
                if (ViewState["totalAdditionalAmount"] != null)
                {

                    totalAdditionalChargesAmount = Convert.ToInt64(Convert.ToDecimal(ViewState["totalAdditionalAmount"]));
                }
                long AmountFromLine = 0;
                if (ViewState["totallineamount"] != null)
                {
                    if (ViewState["totallineamount"].ToString().Contains('('))
                    {
                        string[] totallineamt = ViewState["totallineamount"].ToString().Split(new[] { '(', ')' });
                        AmountFromLine = -Convert.ToInt64(Convert.ToDecimal(totallineamt[1]));
                    }
                    else
                    {
                        AmountFromLine = Convert.ToInt64(Convert.ToDecimal(ViewState["totallineamount"]));
                    }
                }

                totalAdditionalChargesAmount = Convert.ToInt64(txtaddamt.Text) + totalAdditionalChargesAmount;
                ViewState["totalAdditionalAmount"] = totalAdditionalChargesAmount;

                txtTotalAdditionalCharges.Text = totalAdditionalChargesAmount.ToString();

                if ((totalAdditionalChargesAmount + AmountFromLine) >= 0)
                {
                    txtTotalInvoiceAmount.Text = (totalAdditionalChargesAmount + AmountFromLine).ToString();
                }
                else
                {
                    string[] res = (totalAdditionalChargesAmount + AmountFromLine).ToString().Split(new[] { '-' });
                    txtTotalInvoiceAmount.Text = "(" + res[1] + ")";
                }

                clearaddchargetextboxes();
            }

        }

        protected ArrayList getAddItemDtls()
        {
            ArrayList getAdditionalCharge = new ArrayList();

            DataTable addchrgeitems = (DataTable)ViewState["PopulateTableAdditionalCharge"];
            int gvRowCount = 0;
            if (ViewState["draftnofromlistForAdditemGrid"] != null)
            {
                gvRowCount = gvAdditionalChargeDetails.Rows.Count;
            }
            else if (ViewState["PopulateTableAdditionalCharge"] != null)
            {
                gvRowCount = gvAdditionalChargeDetails.Rows.Count;
            }
            else
            {
                gvRowCount = gvAdditionalChargeDetails.Rows.Count - 1;
            }

            addchrgeBO objadd;
            if (gvRowCount > 0)
            {
                foreach (DataRow dr in addchrgeitems.Rows)
                {
                    objadd = new addchrgeBO();


                    string flgFrmSvBtn = Convert.ToString(ViewState["FlagFrmSaveBtn"]);
                    string flgFrmSbmtBtn = Convert.ToString(ViewState["FlagFrmSubmitBtn"]);

                    if (flgFrmSvBtn == "yes")
                    {
                        objadd.InvNo = Convert.ToInt32(ViewState["InvNoForAll3GrdFromSaveBtn"]);
                        objadd.InvCode = Convert.ToString(ViewState["InvCodeForAll3GrdFromSaveBtn"]);
                    }
                    else if (flgFrmSbmtBtn == "yes")
                    {

                        objadd.InvNo = Convert.ToInt32(ViewState["InvNoForAll3GrdFromSubmitBtn"]);
                        objadd.InvCode = Convert.ToString(ViewState["InvCodeForAll3GrdFromSubmitBtn"]);

                    }

                    objadd.Chargenumber = dr["charge_no"].ToString();
                    Hashtable hs = (Hashtable)ViewState["HashtableCharge"];
                    string val = dr["Charge_id"].ToString();
                    int chrgeid = 0;
                    foreach (DictionaryEntry entry in hs)
                    {

                        // entry.Key, entry.Value);
                        if (entry.Value.ToString().ToLower().Trim().Equals(val.ToLower().Trim()))
                        {
                            chrgeid = Convert.ToInt32(entry.Key);
                            break;
                        }
                    }
                    objadd.Chargetype = chrgeid;

                    objadd.Charge = dr["charge"].ToString();
                    objadd.Chrgeamount = dr["amount"].ToString();
                    objadd.Chrgedescp = dr["description"].ToString();
                    objadd.Gst = dr["Gst_no"].ToString();

                    getAdditionalCharge.Add(objadd);

                }
            }

            //DataTable Adchrgeitm = (DataTable)ViewState["TempTableAddCharge"];
            //foreach (DataRow dr in Adchrgeitm.Rows)
            //{
            //    objadd.Chargenumber = dr["charge_no"].ToString();
            //    objadd.Chargetype = Convert.ToInt32(dr["ddlfootChargeType"].ToString());
            //    objadd.Charge = dr["charge"].ToString();
            //    objadd.Chrgeamount = dr["amount"].ToString();
            //    objadd.Chrgedescp = dr["description"].ToString();
            //    objadd.Gst = dr["Gst_no"].ToString();
            //    getitm.Add(objadd);

            //}
            return getAdditionalCharge;

        }

        protected void clearaddchargetextboxes()
        {
            txtchrgenmber.Text = string.Empty;
            ddlchtgetype.SelectedValue = "1";
            txtchrge.Text = string.Empty;
            txtaddamt.Text = string.Empty;
            txtdescp.Text = string.Empty;
            txtgst.Text = string.Empty;

        }

        #endregion Additional Charges

        #region Account Distribution
        private void SetInitialRowAccDist()
        {

            DataTable dt = new DataTable();
            // DataRow dr = null;
           // dt.Columns.Add(new DataColumn("Inv_no", typeof(string)));
            dt.Columns.Add(new DataColumn("Debit_Credit", typeof(string)));
            dt.Columns.Add(new DataColumn("general_ledger_account", typeof(string)));
            dt.Columns.Add(new DataColumn("costcenter_1", typeof(string)));
            dt.Columns.Add(new DataColumn("costcenter_2", typeof(string)));
            dt.Columns.Add(new DataColumn("WBS_no", typeof(string)));
            dt.Columns.Add(new DataColumn("amount", typeof(string)));
            // dr = dt.NewRow();
            //Store the DataTable in ViewState
            //ViewState["TempTableAccDist"] = dt;
            // ViewState["PopulateAccountDistributionDetails"] = dt;
            ViewState["PopulateAccountDistDetails"] = dt;
            gvAccountDistributionDetails.DataSource = dt;

            gvAccountDistributionDetails.DataBind();

        }

        protected void gvAccountDistributionDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (ViewState["PopulateAccountDistDetails"] != null)
            {
                DataTable dt = (DataTable)ViewState["PopulateAccountDistDetails"];
                dt.Rows.RemoveAt(e.RowIndex);
                gvAccountDistributionDetails.DataSource = dt;
                gvAccountDistributionDetails.DataBind();
            }
        }



        protected void btnaddacc_Click(object sender, EventArgs e)
        {
            if (txtgen.Text != string.Empty && txtcs1.Text != string.Empty && txtamt.Text != string.Empty)
            {

                DataTable dtCurrentTable = (DataTable)ViewState["PopulateAccountDistDetails"];

                DataRow drCurrentRow = dtCurrentTable.NewRow();
                drCurrentRow["Debit_Credit"] = txtbdcr.Text.ToString();
                drCurrentRow["general_ledger_account"] = txtgen.Text.ToString();
                drCurrentRow["costcenter_1"] = txtcs1.Text.ToString();
                drCurrentRow["costcenter_2"] = tctcs2.Text.ToString();
                drCurrentRow["WBS_no"] = txtwbs.Text.ToString();
                drCurrentRow["amount"] = txtamt.Text.ToString();
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["PopulateAccountDistDetails"] = dtCurrentTable;
                gvAccountDistributionDetails.DataSource = dtCurrentTable;

                gvAccountDistributionDetails.DataBind();
                clearaccountdistributiontextboxes();
            }
        }

        protected ArrayList getAccDistrDtls()
        {
            ArrayList getAccountDistList = new ArrayList();
            DataTable Accdistritems = (DataTable)ViewState["PopulateAccountDistDetails"];

            int gvRowCount = 0;
            if (ViewState["draftnofromlistForAccitemGrid"] != null)
            {
                gvRowCount = gvAccountDistributionDetails.Rows.Count;
            }
            else if (ViewState["PopulateAccountDistDetails"] != null)
            {
                gvRowCount = gvAccountDistributionDetails.Rows.Count;
            }
            else
            {
                gvRowCount = gvAccountDistributionDetails.Rows.Count - 1;
            }
            accdistrBO objacc;
            if (gvRowCount > 0)
            {
                foreach (DataRow dr in Accdistritems.Rows)
                {
                    objacc = new accdistrBO();


                    string flgFrmSvBtn = Convert.ToString(ViewState["FlagFrmSaveBtn"]);
                    string flgFrmSbmtBtn = Convert.ToString(ViewState["FlagFrmSubmitBtn"]);

                    if (flgFrmSvBtn == "yes")
                    {
                        objacc.InvNo = Convert.ToInt32(ViewState["InvNoForAll3GrdFromSaveBtn"]);
                        objacc.InvCode = Convert.ToString(ViewState["InvCodeForAll3GrdFromSaveBtn"]);
                    }
                    else if (flgFrmSbmtBtn == "yes")
                    {

                        objacc.InvNo = Convert.ToInt32(ViewState["InvNoForAll3GrdFromSubmitBtn"]);
                        objacc.InvCode = Convert.ToString(ViewState["InvCodeForAll3GrdFromSubmitBtn"]);

                    }
                    objacc.Debitcredit = dr["Debit_Credit"].ToString();
                    objacc.Costcenter1 = dr["costcenter_1"].ToString();
                    objacc.Costcenter2 = dr["costcenter_2"].ToString();
                    objacc.Generalledger = dr["general_ledger_account"].ToString();
                    objacc.Amount = dr["amount"].ToString();
                    objacc.Wbsnumber = dr["WBS_no"].ToString();

                    getAccountDistList.Add(objacc);

                }
            }

            //DataTable Accdistr = (DataTable)ViewState["TempTableAccDist"];
            //foreach (DataRow dr in Accdistr.Rows)
            //{
            //    objacc.Debitcredit = dr["Debit_Credit"].ToString();
            //    objacc.Costcenter1 = dr["costcenter_1"].ToString();
            //    objacc.Costcenter2 = dr["costcenter_2"].ToString();
            //    objacc.Generalledger = dr["general_ledger_account"].ToString();
            //    objacc.Amount = dr["amount"].ToString();
            //    objacc.Wbsnumber = dr["WBS_no"].ToString();
            //    getitm.Add(objacc);
            //}
            return getAccountDistList;
        }

        protected void clearaccountdistributiontextboxes()
        {
            txtbdcr.Text = string.Empty;
            txtgen.Text = string.Empty;
            txtcs1.Text = string.Empty;
            tctcs2.Text = string.Empty;
            txtwbs.Text = string.Empty;
            txtamt.Text = string.Empty;
        }

        #endregion Account Distribution

        #region DatabasePOGRData
        //private ArrayList getpodatabase()
        //{
        //    DataSet podtlsdtset = new DataSet();
        //    System.Collections.ArrayList getitm = new ArrayList();
        //    System.Collections.ArrayList getpoitm = new ArrayList();
        //    try
        //    {
        //        getitm = getLineItemDtls();

        //        POBO objpodtls;
        //        lineitemsBO[] lineitemlist = (lineitemsBO[])getitm.ToArray(typeof(lineitemsBO));
        //        foreach (lineitemsBO dr in lineitemlist)
        //        {
        //            string ponumber = dr.Ponumber;
        //            string partitmnmber = dr.Partitemnumber;
        //            if (ponumber != null && partitmnmber != null)
        //            {
        //                //podtlsdtset = new BLL.POGRdetailsBLL().GetPOdetails(ponumber);
        //                podtlsdtset = new BLL.POGRdetailsBLL().GetPOdetails(ponumber,scode);
        //                //, partitmnmber
        //                if (podtlsdtset != null)
        //                {
        //                    objpodtls = new POBO();
        //                    objpodtls.Ponumber = podtlsdtset.Tables[0].Rows[0][0].ToString();
        //                    objpodtls.Scode = podtlsdtset.Tables[0].Rows[0][1].ToString();
        //                    objpodtls.Sname = podtlsdtset.Tables[0].Rows[0][2].ToString();
        //                    objpodtls.Orderdate = podtlsdtset.Tables[0].Rows[0][3].ToString();
        //                    objpodtls.Partitemnumber = podtlsdtset.Tables[0].Rows[0][4].ToString();
        //                    objpodtls.Poamendmentnumber = podtlsdtset.Tables[0].Rows[0][5].ToString();
        //                    objpodtls.Releasenumber = podtlsdtset.Tables[0].Rows[0][6].ToString();
        //                    objpodtls.Qtyshipped = podtlsdtset.Tables[0].Rows[0][7].ToString();
        //                    objpodtls.Qtyuom = podtlsdtset.Tables[0].Rows[0][8].ToString();
        //                    objpodtls.Unitprice = podtlsdtset.Tables[0].Rows[0][9].ToString();
        //                    objpodtls.Amount = podtlsdtset.Tables[0].Rows[0][10].ToString();
        //                    objpodtls.Comments = podtlsdtset.Tables[0].Rows[0][11].ToString();
        //                    objpodtls.Packingslip = podtlsdtset.Tables[0].Rows[0][12].ToString();
        //                    objpodtls.Billlading = podtlsdtset.Tables[0].Rows[0][13].ToString();
        //                    getpoitm.Add(objpodtls);
        //                }
        //                else
        //                {
        //                    errmessage.Text = "The PO Number" + dr.Ponumber + "does not belong the the current supplier" + Environment.NewLine;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errmessage.Text = ex.Message;


        //    }
        //    return getpoitm;
        //}

        private ArrayList getgrdatabse()
        {
            DataSet dtagrdtls = new DataSet();
            System.Collections.ArrayList getitm = new ArrayList();
            System.Collections.ArrayList getpoitm = new ArrayList();
            try
            {

                getitm = getLineItemDtls();
                lineitemsBO[] lineitemlist = (lineitemsBO[])getitm.ToArray(typeof(lineitemsBO));
                GRBO objgrdtls;
                foreach (lineitemsBO dr in lineitemlist)
                {
                    string ponumber = dr.Ponumber;
                    string supcode = txtSupplierCode.Text;
                    string prt = dr.Partitemnumber;
                    if (supcode != null && ponumber != null && prt != null)
                    {
                        dtagrdtls = new BLL.POGRdetailsBLL().GetGRdetails(supcode, ponumber);
                        if (dtagrdtls != null)
                        {
                            objgrdtls = new GRBO();
                            objgrdtls.Scode = dtagrdtls.Tables[0].Rows[0][0].ToString();
                            objgrdtls.Goodsrecievedid = dtagrdtls.Tables[0].Rows[0][1].ToString();
                            objgrdtls.Ponumber = dtagrdtls.Tables[0].Rows[0][2].ToString();
                            objgrdtls.Status = dtagrdtls.Tables[0].Rows[0][3].ToString();
                            objgrdtls.Location = dtagrdtls.Tables[0].Rows[0][4].ToString();
                            objgrdtls.Partitemnumber = dtagrdtls.Tables[0].Rows[0][5].ToString();
                            objgrdtls.Batch = dtagrdtls.Tables[0].Rows[0][6].ToString();
                            objgrdtls.Expirydate = dtagrdtls.Tables[0].Rows[0][7].ToString();
                            objgrdtls.Packsize = dtagrdtls.Tables[0].Rows[0][8].ToString();
                            objgrdtls.Quantity = dtagrdtls.Tables[0].Rows[0][9].ToString();
                            getpoitm.Add(objgrdtls);
                        }
                        else
                        {
                            errmessage.Text = "The PO Number" + dr.Ponumber + "does not belong to the current supplier" + Environment.NewLine;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                errmessage.Text = ex.Message;


            }
            return getpoitm;


        }
        #endregion DatabasePOGRData

        #region "Screen Data Collection"
        protected void CollectscreenData()
        {
            objsub.Suppliercode = txtSupplierCode.Text.Trim();
            objsub.Invoicenumber = txtInvoiceNumber.Text.Trim();
            objsub.Shippedto = Convert.ToInt32(ddlShippedTo.SelectedItem.Value);
            objsub.Invoiceto = txtInvoiceTo.Text.Trim();
            objsub.Payableto = Convert.ToInt32(ddlPayableTo.SelectedItem.Value);
            objsub.Supplieraddress = txtSupplierAddress.Text.Trim();
            objsub.Emailaddress = txtEmailAddress.Text.Trim();
            objsub.Finaldestination = txtFinalDestination.Text.Trim();
            objsub.Currency = txtCurrency.Text.Trim();
            objsub.Comments = txtComments.Text.Trim();
            objsub.Invoicedate = txtInvoiceDate.Text.Trim();
            objsub.Shippeddate = txtShippedDate.Text.Trim();
            objsub.Shippedvia = Convert.ToInt32(ddlShippedVia.SelectedItem.Value);
            objsub.ShippedViaOther = txtOther.Text.Trim();
            if (ddlShippedVia.SelectedItem.Text.Trim() == "Other")
            {
               // objsub.ShippedViaMeans = txtOther.Text;
                objsub.ShippedViaMeans = txtOther.Text;
            }
            else if (ddlShippedVia.SelectedItem.Text.Trim() == "Select")
            {
                objsub.ShippedViaMeans = null;
            }
            else
            {
                objsub.ShippedViaMeans = ddlShippedVia.SelectedItem.Text;
            }
            objsub.Attchments = //attc.Text.Trim();
            objsub.CreatedBy = Session["Userid"].ToString();
            objsub.ModifiedBy = Session["Userid"].ToString();

            //if total line amount is a negative value
            if (txtTotalLineAmount.Text.Contains('('))
            {
                string[] totalLineAmount = txtTotalLineAmount.Text.Split(new[] { '(', ')' });
                objsub.totalline = totalLineAmount[1];
                objsub.TotalLineInd = "Cr";

            }
            else
            {
                objsub.totalline = txtTotalLineAmount.Text.Trim();
                objsub.TotalLineInd = "Dr";
            }

            //if total additional charge amount is a negative value
            if (txtTotalAdditionalCharges.Text.Contains('('))
            {
                string[] totalAdditionalCharges = txtTotalAdditionalCharges.Text.Split(new[] { '(', ')' });
                objsub.Totalladdcharge = totalAdditionalCharges[1];
                objsub.TotalAddChargeInd = "Cr";

            }
            else
            {
                objsub.Totalladdcharge = txtTotalAdditionalCharges.Text.Trim();
                objsub.TotalAddChargeInd = "Dr";
            }
            objsub.Totalladdcharge = txtTotalAdditionalCharges.Text.Trim();

            if (txtTotalInvoiceAmount.Text.Contains('('))
            {
                string[] totalInvoiceAmount = txtTotalInvoiceAmount.Text.Split(new[] { '(', ')' });
                //objsub.totalline = totalInvoiceAmount[1];
                objsub.Totalamt = totalInvoiceAmount[1];
                objsub.TotalAmountInd = "Cr";

            }
            else
            {
                objsub.Totalamt = txtTotalInvoiceAmount.Text.Trim();
                objsub.TotalAmountInd = "Dr";
            }
            //objsub.Totalamt = txtTotalInvoiceAmount.Text.Trim();

            if (ViewState["draftnofromlist"] != null)
            {
                objsub.Draftno = (string)ViewState["draftnofromlist"].ToString();
            }

        }





        #endregion "Screen Data Collection"

        #region Compare Data
        protected bool compareData(string errmsg, bool validated)
        {
            //if (txtprtnumbr.Text != string.Empty && txtponumbr.Text != string.Empty && txtrelease.Text != string.Empty && txtqtyshipped.Text != string.Empty && txtunitprice.Text != string.Empty && ddlQtyUnitofMeasure.SelectedValue != "1")
            if (gvLineItemDetails.Rows.Count > 0)
            {
                System.Collections.ArrayList getitm = new ArrayList();
                getitm = getLineItemDtls();

                lineitemsBO[] lineitemlist = (lineitemsBO[])getitm.ToArray(typeof(lineitemsBO));

                errmsg = string.Empty;
                int indof = 0;
                foreach (lineitemsBO dr in lineitemlist)
                {
                    //DataTable lineitempart = new DataTable();
                    
                    
                    /////////////////////////////////////////////////////////////////////////////////Get PO Details From Database
                    string ponumber = dr.Ponumber;
                    string partitmnmber = dr.Partitemnumber;
                    string supcode = txtSupplierCode.Text;
                    if (ponumber != null && supcode != null)
                    {
                        DataSet podtlsdtset = new DataSet();
                        //podtlsdtset = new BLL.POGRdetailsBLL().GetPOdetails(ponumber);
                        podtlsdtset = new BLL.POGRdetailsBLL().GetPOdetails(ponumber, supcode);
                        //, partitmnmber
                        // if (podtlsdtset != null)
                        if (podtlsdtset.Tables[0].Rows.Count > 0)
                        {
                            objpodtls = new POBO();
                            objpodtls.Ponumber = podtlsdtset.Tables[0].Rows[0][0].ToString();
                            objpodtls.Scode = podtlsdtset.Tables[0].Rows[0][1].ToString();
                            objpodtls.Sname = podtlsdtset.Tables[0].Rows[0][2].ToString();
                            objpodtls.Orderdate = podtlsdtset.Tables[0].Rows[0][3].ToString();
                            objpodtls.Partitemnumber = podtlsdtset.Tables[0].Rows[0][4].ToString();
                            objpodtls.Poamendmentnumber = podtlsdtset.Tables[0].Rows[0][5].ToString();
                            objpodtls.Releasenumber = podtlsdtset.Tables[0].Rows[0][6].ToString();
                            objpodtls.Qtyshipped = podtlsdtset.Tables[0].Rows[0][7].ToString();
                            objpodtls.Qtyuom = podtlsdtset.Tables[0].Rows[0][8].ToString();
                            objpodtls.Unitprice = podtlsdtset.Tables[0].Rows[0][9].ToString();
                            objpodtls.Amount = podtlsdtset.Tables[0].Rows[0][10].ToString();
                            objpodtls.Comments = podtlsdtset.Tables[0].Rows[0][11].ToString();
                            objpodtls.Packingslip = podtlsdtset.Tables[0].Rows[0][12].ToString();
                            objpodtls.Billlading = podtlsdtset.Tables[0].Rows[0][13].ToString();
                            objpodtls.CreditOrDebit = podtlsdtset.Tables[0].Rows[0][14].ToString();

                            //getpoitm.Add(objpodtls);
                        }
                        else
                        {
                            errormessg = "The PO Number " + dr.Ponumber + " does not belong to the current supplier" + Environment.NewLine;
                            gvLineItemDetails.Rows[indof].Cells[2].BackColor = System.Drawing.Color.Red;
                            //errmessage.Text = "The PO Number" + dr.Ponumber + "does not belong the the current supplier" + Environment.NewLine;
                            return validated;
                        }
                    }

                    //////////////////////////////////////////////////////////////////////////Get GR Details From Database
                    // string ponumber = dr.Ponumber;
                    DataSet dtagrdtls = new DataSet();
                    // string supcode = txtSupplierCode.Text;
                    string prt = dr.Partitemnumber;
                    if (supcode != null && ponumber != null && prt != null)
                    {
                        dtagrdtls = new BLL.POGRdetailsBLL().GetGRdetails(supcode, ponumber);
                        if (dtagrdtls.Tables[0].Rows.Count > 0)
                        {
                            objgrdtls = new GRBO();
                            objgrdtls.Scode = dtagrdtls.Tables[0].Rows[0][0].ToString();
                            objgrdtls.Goodsrecievedid = dtagrdtls.Tables[0].Rows[0][1].ToString();
                            objgrdtls.Ponumber = dtagrdtls.Tables[0].Rows[0][2].ToString();
                            objgrdtls.Status = dtagrdtls.Tables[0].Rows[0][3].ToString();
                            objgrdtls.Location = dtagrdtls.Tables[0].Rows[0][4].ToString();
                            objgrdtls.Partitemnumber = dtagrdtls.Tables[0].Rows[0][5].ToString();
                            objgrdtls.Batch = dtagrdtls.Tables[0].Rows[0][6].ToString();
                            objgrdtls.Expirydate = dtagrdtls.Tables[0].Rows[0][7].ToString();
                            objgrdtls.Packsize = dtagrdtls.Tables[0].Rows[0][8].ToString();
                            objgrdtls.Quantity = dtagrdtls.Tables[0].Rows[0][9].ToString();
                            objgrdtls.CreditOrDebit = dtagrdtls.Tables[0].Rows[0][10].ToString();
                            //getpoitm.Add(objgrdtls);
                        }
                        else
                        {
                            errormessg = "The PO Number " + dr.Ponumber + " does not belong to the current supplier" + Environment.NewLine;
                            gvLineItemDetails.Rows[indof].Cells[2].BackColor = System.Drawing.Color.Red;
                            //errmessage.Text = "The PO Number" + dr.Ponumber + "does not belong to the current supplier " + Environment.NewLine;
                            return validated;
                        }
                        ////////////////////////////////////////////////////////////////////////////////////////

                        if (dr.Ponumber.Trim().ToLower() == objpodtls.Ponumber.ToLower() && dr.Ponumber.Trim().ToLower() == objgrdtls.Ponumber.ToLower())
                        {
                            if (txtSupplierCode.Text.Trim().ToLower() == objpodtls.Scode.ToLower())
                            {
                                if (dr.Ponumber.Trim().ToLower() == objpodtls.Ponumber.ToLower())
                                {
                                    if (dr.Partitemnumber.Trim().ToLower() == objpodtls.Partitemnumber.ToLower() && dr.Partitemnumber.Trim().ToLower() == objgrdtls.Partitemnumber.ToLower())
                                    {
                                        //return true;    
                                    }
                                    else
                                    {
                                        // errmsg += "The Part/ItemNumber " + dr.Partitemnumber + " for the PO number" + dr.Ponumber + " is incorrect. The correct value is " + objpodtls.Partitemnumber.ToLower() + "</br>" + Environment.NewLine;//get message for incorrect partitemnumber
                                        errmsg += "The Part/ItemNumber " + dr.Partitemnumber + " for the PO number" + dr.Ponumber + " is incorrect. " + "</br>" + Environment.NewLine;//get message for incorrect partitemnumber
                                        gvLineItemDetails.Rows[indof].Cells[1].BackColor = System.Drawing.Color.Red;
                                    }

                                    if (dr.Poamendmentnumber.Trim() == string.Empty)
                                    {
                                        //if poamendment number is not entered, it is not to be validated
                                    }
                                    else
                                    {
                                        if (dr.Poamendmentnumber.Trim().ToLower() == objpodtls.Poamendmentnumber.ToLower())
                                        {

                                        }
                                        else
                                        {
                                            if (objpodtls.Poamendmentnumber == string.Empty)
                                            {
                                                errmsg += "There is no PO Amendment Number for the PO " + dr.Ponumber + ". </br>" + Environment.NewLine;//get message
                                                gvLineItemDetails.Rows[indof].Cells[3].BackColor = System.Drawing.Color.Red;
                                            }
                                            else
                                            {
                                                // errmsg += "The PO Amendment Number " + dr.Poamendmentnumber + " for the PO number " + dr.Ponumber + " is incorrect. The correct value is" + objpodtls.Poamendmentnumber + ". </br>" + Environment.NewLine;//get message 
                                                errmsg += "The PO Amendment Number " + dr.Poamendmentnumber + " for the PO number " + dr.Ponumber + " is incorrect. " + ". </br>" + Environment.NewLine;//get message 
                                                gvLineItemDetails.Rows[indof].Cells[3].BackColor = System.Drawing.Color.Red;
                                            }
                                        }
                                    }

                                    if (dr.Releasenumber.Trim().ToLower() == objpodtls.Releasenumber.ToLower())
                                    {

                                    }
                                    else
                                    {
                                        //errmsg += "The Release Number " + dr.Releasenumber + " for the PO number " + dr.Ponumber + " is incorrect. The correct value is " + objpodtls.Releasenumber + ". </br>" + Environment.NewLine; //get message 
                                        errmsg += "The Release Number " + dr.Releasenumber + " for the PO number " + dr.Ponumber + " is incorrect" + ". </br>" + Environment.NewLine; //get message
                                        gvLineItemDetails.Rows[indof].Cells[4].BackColor = System.Drawing.Color.Red;
                                    }
                                    int qty = new BLL.CreateInvoiceBLL().getExactQty(dr, txtSupplierCode.Text.Trim());
                                    if (dr.QunatityShipInd.ToLower().Contains("cr"))
                                    {
                                        dr.Qtyshipped = "-" + dr.Qtyshipped;
                                    }
                                    //if (dr.Qtyshipped.Contains('('))
                                    //{
                                    //    //string negativeValue = txtlineamt.Text;
                                    //    //string[] results = negativeValue.Split(new[] { '-' });
                                    //    //drCurrentRow["amount"] = "(" + results[1] + ")";
                                    //    string gotValue = dr.Qtyshipped;
                                    //    string[] results = gotValue.Split(new[] { '(', ')' });
                                    //    dr.Qtyshipped = "-" + results[1];
                                    //}

                                    if (objpodtls.CreditOrDebit.ToLower().Contains("dr")) // incase of DR
                                    {
                                        if (Convert.ToInt32(dr.Qtyshipped.Trim()) > 0)
                                        {
                                            if (qty >= 0)
                                            {
                                                if (Convert.ToInt32(dr.Qtyshipped.Trim()) <= (Convert.ToInt32(objpodtls.Qtyshipped)))
                                                {



                                                    if (Convert.ToInt32(dr.Qtyshipped.Trim()) <= qty)
                                                    {
                                                        if (Convert.ToInt32(dr.Qtyshipped.Trim()) <= (Convert.ToInt32(objgrdtls.Quantity)))
                                                        {
                                                        }
                                                        else
                                                        {
                                                            //errmsg += "The Quantity entered does not match with the Goods Reciept Quantity or doesn't exist. The correct value is " + objgrdtls.Quantity.ToString() + ".</br>" + Environment.NewLine;
                                                            errmsg += "The Quantity entered does not match with the Goods Reciept Quantity or doesn't exist" + ".</br>" + Environment.NewLine;
                                                            if (!string.IsNullOrEmpty(objgrdtls.Quantity))
                                                            {
                                                                errmsg += "The Goods Reciept Quantity is " + objgrdtls.Quantity.ToString() + ". </br>" + Environment.NewLine;
                                                            }
                                                            gvLineItemDetails.Rows[indof].Cells[5].BackColor = System.Drawing.Color.Red;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //errmsg += "The Quantity entered does not match the total PO qunatity. The Actual Quantity should be less than or equal to" + qty + ". </br>" + Environment.NewLine;
                                                        errmsg += "The Quantity entered does not match the total PO qunatity." + "</br>" + Environment.NewLine;
                                                        gvLineItemDetails.Rows[indof].Cells[5].BackColor = System.Drawing.Color.Red;

                                                    }

                                                }

                                                else
                                                {
                                                    // errmsg += "The Quantity " + dr.Qtyshipped.Trim() + " for the PO number " + dr.Ponumber + " is incorrect. The Actual Quantity should be less than or equal to " + qty + ". </br>" + Environment.NewLine;
                                                    errmsg += "The Quantity " + dr.Qtyshipped.Trim() + " for the PO number " + dr.Ponumber + " is incorrect" + "</br>" + Environment.NewLine;
                                                    gvLineItemDetails.Rows[indof].Cells[5].BackColor = System.Drawing.Color.Red;
                                                }
                                            }
                                            else
                                            {
                                                errmsg += "The Quantity " + dr.Qtyshipped.Trim() + " for the PO number " + dr.Ponumber + " is incorrect" + "</br>" + Environment.NewLine;
                                                gvLineItemDetails.Rows[indof].Cells[5].BackColor = System.Drawing.Color.Red;
                                            }
                                        }
                                        else
                                        {
                                            errmsg += "The Quantity " + dr.Qtyshipped.Trim() + " for the PO number " + dr.Ponumber + " is incorrect" + "</br>" + Environment.NewLine;
                                            gvLineItemDetails.Rows[indof].Cells[5].BackColor = System.Drawing.Color.Red;
                                        }
                                    }
                                    else // incase of CR
                                    {
                                        if (Convert.ToInt32(dr.Qtyshipped.Trim()) < 0)
                                        {
                                            if (qty >= 0)
                                            {
                                                string poqtyship = "-";
                                                objpodtls.Qtyshipped = poqtyship + objpodtls.Qtyshipped;

                                                string grqtyship = "-";
                                                objgrdtls.Quantity = grqtyship + objgrdtls.Quantity;


                                                if (Convert.ToInt32(dr.Qtyshipped.Trim()) < 0)
                                                {


                                                    if (Convert.ToInt32(dr.Qtyshipped.Trim()) >= (Convert.ToInt32(objpodtls.Qtyshipped)))
                                                    {



                                                        //if (Convert.ToInt32(dr.Qtyshipped.Trim()) <= qty)
                                                        //{
                                                        if (Convert.ToInt32(dr.Qtyshipped.Trim()) >= (Convert.ToInt32(objgrdtls.Quantity)))
                                                        {
                                                        }
                                                        else
                                                        {
                                                            // errmsg += "The Quantity entered does not match with the Goods Reciept Quantity or doesn't exist. The correct value is -" + objgrdtls.Quantity.ToString() + ".</br>" + Environment.NewLine;
                                                            errmsg += "The Quantity entered does not match with the Goods Reciept Quantity or doesn't exist." + "</br>" + Environment.NewLine;
                                                            if (!string.IsNullOrEmpty(objgrdtls.Quantity))
                                                            {
                                                                errmsg += "The Goods Reciept Quantity is -" + objgrdtls.Quantity.ToString() + ". </br>" + Environment.NewLine;
                                                            }
                                                            gvLineItemDetails.Rows[indof].Cells[5].BackColor = System.Drawing.Color.Red;
                                                        }
                                                        //}
                                                        //else
                                                        //{
                                                        //    errmsg += "The Quantity entered does not match the total PO qunatity. The Actual Quantity should be less than or equal to" + qty + ". </br>" + Environment.NewLine;
                                                        //}

                                                    }

                                                    else
                                                    {
                                                        // errmsg += "The Quantity " + dr.Qtyshipped.Trim() + " for the PO number " + dr.Ponumber + " is incorrect. The Actual Quantity should be less than or equal to -" + qty + ". </br>" + Environment.NewLine;
                                                        errmsg += "The Quantity " + dr.Qtyshipped.Trim() + " for the PO number " + dr.Ponumber + " is incorrect." + " </br>" + Environment.NewLine;
                                                        gvLineItemDetails.Rows[indof].Cells[5].BackColor = System.Drawing.Color.Red;
                                                    }
                                                }
                                                else if (Convert.ToInt32(dr.Qtyshipped.Trim()) > 0)
                                                {
                                                    if (Convert.ToInt32(dr.Qtyshipped.Trim()) <= (Convert.ToInt32(objpodtls.Qtyshipped)))
                                                    {



                                                        //if (Convert.ToInt32(dr.Qtyshipped.Trim()) <= qty)
                                                        //{
                                                        if (Convert.ToInt32(dr.Qtyshipped.Trim()) <= (Convert.ToInt32(objgrdtls.Quantity)))
                                                        {
                                                        }
                                                        else
                                                        {
                                                            // errmsg += "The Quantity entered does not match with the Goods Reciept Quantity or doesn't exist. The correct value is -" + objgrdtls.Quantity.ToString() + ".</br>" + Environment.NewLine;
                                                            errmsg += "The Quantity entered does not match with the Goods Reciept Quantity or doesn't exist." + "</br>" + Environment.NewLine;
                                                            if (!string.IsNullOrEmpty(objgrdtls.Quantity))
                                                            {
                                                                errmsg += "The Goods Reciept Quantity is -" + objgrdtls.Quantity.ToString() + ". </br>" + Environment.NewLine;
                                                               
                                                            }
                                                            gvLineItemDetails.Rows[indof].Cells[5].BackColor = System.Drawing.Color.Red;
                                                        }
                                                        //}
                                                        //else
                                                        //{
                                                        //    errmsg += "The Quantity entered does not match the total PO qunatity. The Actual Quantity should be less than or equal to" + qty + ". </br>" + Environment.NewLine;
                                                        //}

                                                    }

                                                    else
                                                    {
                                                        // errmsg += "The Quantity " + dr.Qtyshipped.Trim() + " for the PO number " + dr.Ponumber + " is incorrect. The Actual Quantity should be less than or equal to -" + qty + ". </br>" + Environment.NewLine;
                                                        errmsg += "The Quantity " + dr.Qtyshipped.Trim() + " for the PO number " + dr.Ponumber + " is incorrect." + " </br>" + Environment.NewLine;
                                                        gvLineItemDetails.Rows[indof].Cells[5].BackColor = System.Drawing.Color.Red;
                                                    }
                                                }


                                            }
                                            else
                                            {
                                                errmsg += "The Quantity " + dr.Qtyshipped.Trim() + " for the PO number " + dr.Ponumber + " is incorrect" + "</br>" + Environment.NewLine;
                                                gvLineItemDetails.Rows[indof].Cells[5].BackColor = System.Drawing.Color.Red;
                                            }
                                        }
                                        else
                                        {
                                            errmsg += "The Quantity " + dr.Qtyshipped.Trim() + " for the PO number " + dr.Ponumber + " is incorrect" + "</br>" + Environment.NewLine;
                                            gvLineItemDetails.Rows[indof].Cells[5].BackColor = System.Drawing.Color.Red;
                                        }
                                    }


                                    if (dr.Qtyuom == (Convert.ToInt32(objpodtls.Qtyuom)))
                                    {

                                    }
                                    else
                                    {
                                        int ind = Convert.ToInt32(objpodtls.Qtyuom);
                                        string val = ddlQtyUnitofMeasure.Items[dr.Qtyuom - 1].Text;// qtyunm.Items[dr.Qtyuom-1].Text;//qtyunm.Items[dr.Qtyuom].Value;
                                        //=li.Text;
                                        string actval = ddlQtyUnitofMeasure.Items[ind - 1].Text; //qtyunm.Items[ind-1].Text;
                                        //errmsg += "The Quantity Unit of Measure " + val + " for the PO numbe " + dr.Ponumber + " is incorrect. The correct value is " + actval + ". </br>" + Environment.NewLine;
                                        errmsg += "The Quantity Unit of Measure " + val + " for the PO numbe " + dr.Ponumber + " is incorrect" + ". </br>" + Environment.NewLine;
                                        gvLineItemDetails.Rows[indof].Cells[8].BackColor = System.Drawing.Color.Red;
                                    }

                                    //if (objline.Packingslip == objpodtls.Packingslip)
                                    //{ }
                                    //else
                                    //{
                                    //    errmsg += "The Packingslip Number " + objline.Packingslip + " is incorrect. </br>" + Environment.NewLine;
                                    //}

                                    //if (objline.Billlading == objpodtls.Billlading)
                                    //{

                                    //}
                                    //else
                                    //{
                                    //    errmsg += "The BillLadding Number " + objline.Billlading + " is incorrect. </br>" + Environment.NewLine;
                                    //}
                                }
                                else
                                {
                                    //get message for no matching po number
                                    errmsg += "The PO Number " + dr.Ponumber + " is incorrect." + "</br>" + Environment.NewLine;
                                    gvLineItemDetails.Rows[indof].Cells[2].BackColor = System.Drawing.Color.Red;
                                }
                            }
                            else
                            {
                                //get message for incorrect po number for this supplier
                                errmsg += "The Supplier does not have the PO numbered  " + dr.Ponumber + ". </br>" + Environment.NewLine;
                                gvLineItemDetails.Rows[indof].Cells[2].BackColor = System.Drawing.Color.Red;

                            }
                        }
                        else
                        {
                            errmsg += "The PO Number " + dr.Ponumber + " is incorrect. </br>" + Environment.NewLine;
                            gvLineItemDetails.Rows[indof].Cells[2].BackColor = System.Drawing.Color.Red;
                        }


                        //if (polist.Length>0 && grlist.Length>0 )
                        //{
                        //    foreach (POBO db in polist)
                        //    {
                        //        foreach (GRBO dg in grlist)
                        //        {
                        //            //getpodatabase();
                        //            //getgrdatabse();

                        //        }
                        //    }
                        //}

                        //else 
                        //{
                        //    errmsg += "The PO Number " + dr.Ponumber + " is incorrect. </br>" + Environment.NewLine;
                        //    errormessg = errmsg;
                        //}
                    }
                    indof++;
                }

                if (errmsg == string.Empty)
                {
                    validated = true;
                    done = validated;
                }
                else
                {
                    errormessg = errmsg;
                    validated = false;
                    done = validated;
                }

            }
            return validated;
        }
        #endregion Compare Data

        #region "Buttons Click"
        protected void txtsubmit_Click(object sender, EventArgs e)
        {
            //Response.Redirect("Listdr.aspx");
            errmessage.Text = string.Empty;
            txtstatusTimestamp.Text = string.Empty;
            bool sucess = true;
            CollectscreenData();
            string strInvCode = new BLL.CreateInvoiceBLL().createInvoice(objsub);

            ViewState["InvCodeForAll3GrdFromSubmitBtn"] = strInvCode;
            ViewState["InvNoForAll3GrdFromSubmitBtn"] = Convert.ToInt32(strInvCode.Substring(6));
            ViewState["FlagFrmSubmitBtn"] = "yes";


            if (string.IsNullOrEmpty(strInvCode))
            {
                sucess = false;
            }

            else
            {
                /////Save line Items to Database.............
                //objline.InvCode = strInvCode;
                //objline.InvNo = Convert.ToInt32(strInvCode.Substring(6));
                ArrayList LineItemList = getLineItemDtls();


                lineitemsBO[] list = (lineitemsBO[])LineItemList.ToArray((typeof(lineitemsBO)));

                for (int i = 0; i < list.Length; i++)
                {
                    lineitemsBO li = list[i];

                    BLL.CreateInvoiceBLL createInvoice = new CreateInvoiceBLL();
                    createInvoice.inslineitemdetils(li);

                }
                ////Save Add charges...........................
                //objadd.InvCode = strInvCode;
                //objadd.InvNo = Convert.ToInt32(strInvCode.Substring(6));

                ArrayList AdditionChargeList = getAddItemDtls();
                addchrgeBO[] list1 = (addchrgeBO[])AdditionChargeList.ToArray((typeof(addchrgeBO)));

                for (int i = 0; i < list1.Length; i++)
                {
                    addchrgeBO ac = list1[i];

                    BLL.CreateInvoiceBLL createInvoice = new CreateInvoiceBLL();
                    createInvoice.insaddchrgedtls(ac);
                    ////createInvoice.insaddchrgedtls(ac);

                }

                //Save Account Distribution Details

                //objacc.InvCode = strInvCode;
                //objacc.InvNo = Convert.ToInt32(strInvCode.Substring(6));
                ArrayList AccountDistributionList = getAccDistrDtls();
                accdistrBO[] list2 = (accdistrBO[])AccountDistributionList.ToArray((typeof(accdistrBO)));
                for (int i = 0; i < list2.Length; i++)
                {
                    accdistrBO ad = list2[i];

                    BLL.CreateInvoiceBLL createInvoice = new CreateInvoiceBLL();
                    createInvoice.insAccDistredtls(ad);

                }



            }

            // Code for Moving Files from attachemnt to Invocie Folder

            if (ViewState["draftnofromlist"] != null) // check if submit is happening from draft
            {

                string draftno = ViewState["draftnofromlist"].ToString();
                AttachmentBO attachFileDraftBo = new AttachmentBO();
                attachFileDraftBo.Inv_Code = draftno;
                DataSet attachmentDataset = new AttachmentBLLcs().GetAllAttachmentFileDetailsByDraftID(attachFileDraftBo);
                new CreateInvoiceDraftBLL().UpdateForDraftToInvoice(draftno);
                //VendorPortal.CreateInvoice createobj = new VendorPortal.CreateInvoice();

                string userId = Session["Userid"].ToString();
                new AttachmentBLLcs().UpdateAttachments(strInvCode, draftno, userId, attachmentDataset);

                string SourcePathDraft = Server.MapPath("~") + @"\Attachments\" + draftno;
                string SourcePathDraftFromAttachment = Server.MapPath("~") + @"\Attachments\" + Session["Userid"].ToString() + @"\"; ;
                string DestinationPathInvoice = Server.MapPath("~") + @"\Attachments\" +  strInvCode.ToString();

                if (attachmentDataset.Tables[0] != null)
                    if (!Directory.Exists(DestinationPathInvoice))
                    {
                        Directory.CreateDirectory(DestinationPathInvoice);
                    }

                linitemtxtboxes.Visible = false;
                addchargetxtboxes.Visible = false;
                accdistrtxtboxes.Visible = false;

                ArrayList fileList = new ArrayList();

                for (int i = 0; i < attachmentDataset.Tables[0].Rows.Count; i++)
                {
                    fileList.Add(attachmentDataset.Tables[0].Rows[i][0].ToString());
                }

                foreach (string file in fileList)
                {
                    string filename = file;
                    string sourcefile = SourcePathDraft + @"\" + file;
                    string Destinationfile = DestinationPathInvoice + @"\" + file;

                    if (File.Exists(sourcefile))
                    {
                        File.Move(sourcefile, Destinationfile);
                    }
                    else
                    {

                        string sourcefilefromattach;
                        sourcefilefromattach = SourcePathDraftFromAttachment + @"\" + file;
                        if (File.Exists(sourcefilefromattach))
                        {
                            File.Move(sourcefilefromattach, Destinationfile);
                        }

                    }
                }

            }


            else // execute when when submit is not from draft
            {
                AttachmentBO attachFileBo = new AttachmentBO();
                attachFileBo.Userid = Session["Userid"].ToString(); ;
                attachFileBo.Inv_Code = strInvCode;
                DataSet attachmentDataset = new AttachmentBLLcs().GetAllAttachmentFileDetailsByInvoiceID(attachFileBo);

                if (attachmentDataset.Tables[0] != null)
                {
                    string SourcePath = Server.MapPath("~") + @"\Attachments\" + Session["Userid"].ToString() + @"\";
                    string DestinationPath = Server.MapPath("~") + @"\Attachments\" + strInvCode.ToString();

                    if (!Directory.Exists(DestinationPath))
                    {
                        Directory.CreateDirectory(DestinationPath);
                    }

                    ArrayList fileList = new ArrayList();
                    for (int i = 0; i < attachmentDataset.Tables[0].Rows.Count; i++)
                    {
                        fileList.Add(attachmentDataset.Tables[0].Rows[i][0].ToString());
                    }

                    foreach (string file in fileList)
                    {
                        string filename = file;
                        string sourcefile = SourcePath + @"\" + file;
                        string Destinationfile = DestinationPath + @"\" + file;

                        if (File.Exists(sourcefile))
                        {
                            File.Move(sourcefile, Destinationfile);
                        }
                    }
                }

            }
            // Code for Moving Files from attachemnt to Invocie Folder

            // Code for Email notification
            if (sucess)
            {

                txtstatusTimestamp.Text = "Invoice has been created  successfully! and invoice no is " + strInvCode.ToString();
                if (gvAdditionalChargeDetails.Rows.Count == 0)
                {
                    DisplayAdditionalChargeGridviewWhenEmpty();
                }

                if (gvAccountDistributionDetails.Rows.Count == 0)
                {
                    DisplayAccountChargeGridviewWhenEmpty();
                }
                ViewState["invoiceCodeForExport"] = strInvCode;
                txtInvoiceNumber.Text = strInvCode.ToString();
                this.ExportWord.Visible = true;

                //Hide all buttons
                txtsubmit.Visible = false;
                //txtsave.Visible = false;
                btnValidateInvoice.Visible = false;
                txtclr.Visible = false;
                this.txtcancel.Text = "Back";

                DisableControlsAfterCreateInvoice();
                //Code for sending email notification 
                //Commenting the code for sending email. Reason- SMTP server is down and successfull msg is not displaying

                /* UserEmailBO userobj = new UserEmailBO();

                 userobj.Approver = "Approver";
                 userobj.Supplier = txtSupplierCode.Text.Trim();
                 userobj.Invoicecode = strInvCode;

                 Utitlity.EmailHelper emailobj = new Utitlity.EmailHelper();
                 emailobj.SendEmail(Utitlity.EmailType.InvoiceCreated, userobj);*/
            }

        }


        protected void txtsave_Click1(object sender, EventArgs e)
        {
            errmessage.Text = string.Empty;
            txtstatusTimestamp.Text = string.Empty;
            IsSaveasDraft = true;
            bool success = true;
            //======================Saving in 'tbl_Invdetails_bkp' table ==================================
            CollectscreenData();
            string strInvCode = new BLL.CreateInvoiceDraftBLL().createInvoiceDraft(objsub);

            ViewState["InvCodeForAll3GrdFromSaveBtn"] = strInvCode;
            ViewState["InvNoForAll3GrdFromSaveBtn"] = Convert.ToInt32(strInvCode.Substring(6));
            ViewState["FlagFrmSaveBtn"] = "yes";


            //========================== Delete records from tbl_Invlineitems_details_bkp, tbl_Invadditionalcharge_details_bkp, and =========
            // ========================== tbl_InvAccountDistribution_details_bkp tables for this draft number before inserting   =============

            BLL.CreateInvoiceDraftBLL createInvoiceDraftBllobj = new CreateInvoiceDraftBLL();
            createInvoiceDraftBllobj.DeleteLineItemChargeAccountDetailsBkp(strInvCode);

            //======================Saving in 'tbl_Invlineitems_details_bkp' table ==================================


            if (string.IsNullOrEmpty(strInvCode))
            {
                success = false;

            }
            else
            {
                //objline.InvCode = strInvCode;
                // if (strInvCode.Length > 0)
                // {
                //     //objline.InvNo = Convert.ToInt32(strInvCode.Substring(6));
                // Session["InvNoForLineItem"] = Convert.ToInt32(strInvCode.Substring(6)); //************NKKKKKKKKKK


                //}

                ArrayList LineItemList = getLineItemDtls();


                lineitemsBO[] list = (lineitemsBO[])LineItemList.ToArray((typeof(lineitemsBO)));
                if (list.Length > 0)
                {

                    for (int i = 0; i < list.Length; i++)
                    {
                        lineitemsBO li = list[i];

                        BLL.CreateInvoiceDraftBLL createInvoiceDraft = new CreateInvoiceDraftBLL();
                        createInvoiceDraft.inslineitemdetils(li);

                    }
                }

                else
                {
                    int invno = Convert.ToInt32(ViewState["InvNoForAll3GrdFromSaveBtn"]);
                    string InvCode = Convert.ToString(ViewState["InvCodeForAll3GrdFromSaveBtn"]);

                    lineitemsBO li = new lineitemsBO();
                    li.InvNo = invno;
                    li.InvCode = InvCode;
                    //li.Partitemnumber = string.Empty;

                    BLL.CreateInvoiceDraftBLL createInvoiceDraft = new CreateInvoiceDraftBLL();
                    createInvoiceDraft.inslineitemdetils(li);


                }

                //======================Saving in 'tbl_Invadditionalcharge_details_bkp' table ===================================

                //objadd.InvCode = strInvCode; ***********
                //Session["InvCodeForAdditionalCharge"] = Convert.ToInt32(strInvCode.Substring(6));
                //if (strInvCode.Length > 0)
                //{
                //    //objadd.InvNo = Convert.ToInt32(strInvCode.Substring(6));
                //    Session["InvNoForAdditionalCharge"] = Convert.ToInt32(strInvCode.Substring(6));
                //}
                ArrayList AdditionChargeList = getAddItemDtls();
                addchrgeBO[] list1 = (addchrgeBO[])AdditionChargeList.ToArray((typeof(addchrgeBO)));

                if (list1.Length > 0)
                {

                    for (int i = 0; i < list1.Length; i++)
                    {
                        addchrgeBO ac = list1[i];

                        BLL.CreateInvoiceDraftBLL createInvoiceDraft = new CreateInvoiceDraftBLL();
                        createInvoiceDraft.insaddchrgedtls(ac);

                    }
                }

                else
                {
                    int invno = Convert.ToInt32(ViewState["InvNoForAll3GrdFromSaveBtn"]);
                    string InvCode = Convert.ToString(ViewState["InvCodeForAll3GrdFromSaveBtn"]);
                    addchrgeBO ac = new addchrgeBO();

                    ac.InvNo = invno;
                    ac.InvCode = InvCode;
                    //  ac.Chargenumber = string.Empty;

                    BLL.CreateInvoiceDraftBLL createInvoiceDraft = new CreateInvoiceDraftBLL();
                    createInvoiceDraft.insaddchrgedtls(ac);

                }

                //======================Saving in 'tbl_InvAccountDistribution_details_bkp' table ===================================

                //objacc.InvCode = strInvCode;
                //if (strInvCode.Length > 0)
                //{
                //    objacc.InvNo = Convert.ToInt32(strInvCode.Substring(6));
                //}

                ArrayList AccountDistributionList = getAccDistrDtls();
                accdistrBO[] list2 = (accdistrBO[])AccountDistributionList.ToArray((typeof(accdistrBO)));
                if (list2.Length > 0)
                {
                    for (int i = 0; i < list2.Length; i++)
                    {
                        accdistrBO ad = list2[i];

                        BLL.CreateInvoiceDraftBLL createInvoiceDraft = new CreateInvoiceDraftBLL();
                        createInvoiceDraft.insAccDistredtls(ad);

                    }
                }
                else
                {

                    int invno = Convert.ToInt32(ViewState["InvNoForAll3GrdFromSaveBtn"]);
                    string InvCode = Convert.ToString(ViewState["InvCodeForAll3GrdFromSaveBtn"]);
                    accdistrBO ad = new accdistrBO();
                    ad.InvNo = invno;
                    ad.InvCode = InvCode;
                    //ad.Debitcredit = string.Empty;
                    BLL.CreateInvoiceDraftBLL createInvoiceDraft = new CreateInvoiceDraftBLL();
                    createInvoiceDraft.insAccDistredtls(ad);

                }


                // Code for Moving Files from attachemnt to Draft Folder

                AttachmentBO attachFileBo = new AttachmentBO();
                attachFileBo.Userid = Session["Userid"].ToString();
                attachFileBo.Inv_Code = strInvCode;



                string SourcePath = Server.MapPath("~") + @"\Attachments\";
                string DestinationPath = SourcePath + strInvCode.ToString();

                if (!Directory.Exists(DestinationPath))
                {
                    Directory.CreateDirectory(DestinationPath);
                }

                ArrayList fileList = new ArrayList();
                DataSet attachmentDataset = new AttachmentBLLcs().GetAllAttachmentFileDetailsByDraftID(attachFileBo);
                for (int i = 0; i < attachmentDataset.Tables[0].Rows.Count; i++)
                {
                    fileList.Add(attachmentDataset.Tables[0].Rows[i][0].ToString());
                }

                foreach (string file in fileList)
                {
                    string filename = file;
                    string sourcefile = SourcePath + Session["Userid"].ToString() +  @"\" + file;
                    string Destinationfile = DestinationPath + @"\" + file;

                    if (File.Exists(sourcefile))
                    {
                        File.Move(sourcefile, Destinationfile);
                    }
                }


                // Code for Moving Files from attachemnt to Draft Folder

                if (success)
                {
                    txtstatusTimestamp.Text = "Invoice created in Draft mode successfully! and invoice no is " + strInvCode.ToString();
                    txtInvoiceNumber.Text = strInvCode.ToString();
                    //=======================================================================================
                    this.ExportWord.Visible = true;
                    //Hide all buttons
                    //txtsave.Visible = false;
                    btnValidateInvoice.Visible = true;
                    txtclr.Visible = true;
                    txtsubmit.Visible = false;
                    this.txtcancel.Text = "Back";

                }
            }
        }

        protected void btnValidateInvoice_Click(object sender, EventArgs e)
        {
            errmessage.Text = string.Empty;
            txtstatusTimestamp.Text = string.Empty;
            if (ddlPayableTo.SelectedIndex == 0 || ddlShippedTo.SelectedIndex == 0 || ddlShippedVia.SelectedIndex == 0)
            {
                return;
            }
            //Inserted by devendra.. If Final Destination is entered Comments are compulsary
            if (txtFinalDestination.Text != string.Empty)
            {
                if (txtComments.Text == string.Empty)
                {
                    return;
                }
            }
            bool flag = false;
            string msg = string.Empty;
            // getgrdatabse();
            //compareData(); 
            compareData(msg, flag);
            if (done == true)
            {
                txtstatusTimestamp.Text = "Sucessfully Validated";
                txtsubmit.Visible = true;

                btnValidateInvoice.Visible = false;
                linitemtxtboxes.Visible = false;
                //addchargetxtboxes.Visible = false;
                //accdistrtxtboxes.Visible = false;
                this.gvLineItemDetails.Enabled = false;               
               
            }
            else
            {
                //errormessg = errormessg.Replace("@", Environment.NewLine);
                errmessage.Text = errormessg;
                errmessage.Focus();
                // errmessage.Focus =
                //show all the incorrect text
            }
            if (txtFinalDestination.MaxLength > 0)
            {
                //RequiredFieldValidatorComments.Visible = true;
            }



        }

        protected void lbnSupplierCodeLookup_Click(object sender, EventArgs e)
        {
            MPE.Show();
        }


        protected void txtcancel_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/Home.aspx");
           
        }
        #endregion "Buttons Click"

        #region "Validations"
        //Validation will fail if user has entered in final destination but forgot to enter in comment textbox
        protected void CustValForComment(object sender, ServerValidateEventArgs arg)
        {
            if (txtFinalDestination.Text.Length > 0 && txtComments.Text.Length == 0)
            {

                arg.IsValid = false;
            }
            else
            {
                arg.IsValid = true;
            }
        }
        //Validation will fail if user enter futute date for Shipped Date
        //protected void CustValForShippedDate(object sender, ServerValidateEventArgs arg)
        //{
        //    DateTime todayDate = DateTime.Today; 
        //    DateTime ShippedDate = Convert.ToDateTime(txtShippedDate.Text);

        //    if (ShippedDate > todayDate)
        //    {

        //        arg.IsValid = false;
        //    }
        //    else
        //    {
        //        arg.IsValid = true;
        //    }
        //}

        //Validation will fail if user enters past date for Invoice date 
        protected void CustValForInvoiceDate(object sender, ServerValidateEventArgs arg)
        {
            DateTime todayDate = DateTime.Today;
            DateTime InvoiceDate = Convert.ToDateTime(txtInvoiceDate.Text.Trim());
            DateTime Shippeddate = Convert.ToDateTime(txtShippedDate.Text.Trim());
            //|| InvoiceDate < Shippeddate
            if (InvoiceDate < todayDate)
            {

                arg.IsValid = false;

            }
            else
            {
                arg.IsValid = true;
            }
        }

        //================================================
        //Validation for Shipped to
        protected void CustValForDdlShippedTo(object sender, ServerValidateEventArgs arg)
        {

            string StrShippetTo = Convert.ToString(ddlShippedTo.SelectedItem);
            if (StrShippetTo == "Select")
            {

                arg.IsValid = false;

            }
            else
            {
                arg.IsValid = true;
            }
        }

        //================================================
        //Validation for Payable to
        protected void CustValForDdlPayableTo(object sender, ServerValidateEventArgs arg)
        {

            string StrPayableTo = Convert.ToString(ddlPayableTo.SelectedItem);
            if (StrPayableTo == "Select")
            {

                arg.IsValid = false;

            }
            else
            {
                arg.IsValid = true;
            }
        }

        //================================================
        //Validation for Shipped Via
        protected void CustValForDdlShippedVia(object sender, ServerValidateEventArgs arg)
        {

            string StrShippedVia = Convert.ToString(ddlShippedVia.SelectedItem);
            if (StrShippedVia == "Select")
            {

                arg.IsValid = false;

            }
            else
            {
                arg.IsValid = true;
            }
            if (StrShippedVia == "Other")
            {

                txtOther.Visible = true;

            }
            else
            {
               
            }
        }



        #endregion "Validations"

        protected void ExportWord_Click(object sender, EventArgs e)
        {
            string filename = string.Empty;
            if (ViewState["invoiceCodeForExport"] != null)
            {
                filename = ViewState["invoiceCodeForExport"].ToString();
            }
            else
            {
                filename = "Invoice";
            }
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename=" + filename + ".xls"));
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            string tab = string.Empty;

            StringWriter strWriter = new StringWriter();
            HtmlTextWriter htWriter = new HtmlTextWriter(strWriter);

            //frmtbl.RenderControl(htWriter);
            //Response.Output.Write(strWriter.ToString());
            //Response.Flush();
            //Response.End();
            ///////////////////////////////////////////////////////

            Table tblforexprot = new Table();
            tblforexprot.BorderWidth = 1;
            tblforexprot.GridLines = GridLines.Both;
            tblforexprot.BorderStyle = BorderStyle.Solid;
            tblforexprot.BorderColor = System.Drawing.Color.Black;

            TableRow header = new TableRow();
            TableCell headercontent = new TableCell();
            headercontent.ColumnSpan = 7;
            headercontent.Attributes.Add("align", "center");
            header.Font.Bold = true;
            header.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
            header.ForeColor = System.Drawing.Color.White;
            header.HorizontalAlign = HorizontalAlign.Center;
            header.BorderStyle = BorderStyle.Solid;
            header.BorderColor = System.Drawing.Color.Black;



            //headercontent.Text = "Coder's Worklist";
            headercontent.Text = txtInvoiceNumber.Text.Trim();
            header.Cells.Add(headercontent);
            tblforexprot.Rows.Add(header);

            TableRow HeaderRow = new TableRow();

            HeaderRow.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
            HeaderRow.ForeColor = System.Drawing.Color.White;
            HeaderRow.HorizontalAlign = HorizontalAlign.Center;
            HeaderRow.Font.Bold = true;
            HeaderRow.BorderStyle = BorderStyle.Solid;
            HeaderRow.BorderColor = System.Drawing.Color.Black;


            TableCell HeaderApproverName = new TableCell();
            TableCell HeaderAllocationDate = new TableCell();
            TableCell HeaderMappingType = new TableCell();
            TableCell HeaderReason = new TableCell();


            HeaderReason.Text = txtSupplierCode.Text;
            HeaderMappingType.Text = txtInvoiceNumber.Text;
            HeaderAllocationDate.Text = txtInvoiceTo.Text;
            HeaderApproverName.Text = txtEmailAddress.Text;


            HeaderRow.Cells.Add(HeaderApproverName);
            HeaderRow.Cells.Add(HeaderAllocationDate);
            HeaderRow.Cells.Add(HeaderMappingType);
            HeaderRow.Cells.Add(HeaderReason);

            tblforexprot.Rows.Add(HeaderRow);

            tab = "";

            Response.Write("\t" + tab);
            Response.Write("\t" + tab);
            Response.Write("\t" + tab);
            Response.Write("\t" + tab);

            Response.Write("\t" + "Invoice Details");
            Response.Write("\n");

            Response.Write("\n");

            Response.Write("Supplier Code");
            Response.Write("\t" + txtSupplierCode.Text);
            Response.Write("\n");

            Response.Write("Invoice Number");
            Response.Write("\t" + txtInvoiceNumber.Text);
            Response.Write("\n");

            Response.Write("Supplier To");
            Response.Write("\t" + ddlShippedTo.SelectedItem.Text);
            Response.Write("\n");

            Response.Write("Payable To");
            Response.Write("\t" + ddlPayableTo.SelectedItem.Text);
            Response.Write("\n");

            Response.Write("EmailAddress");
            Response.Write("\t" + txtEmailAddress.Text);
            Response.Write("\n");

            Response.Write("Currency");
            Response.Write("\t" + txtCurrency.Text);
            Response.Write("\n");

            Response.Write("Invoice To");
            Response.Write("\t" + txtInvoiceTo.Text);
            Response.Write("\n");

            Response.Write("Supplier Address");
            Response.Write("\t" + txtSupplierAddress.Text);
            Response.Write("\n");

            Response.Write("Final Destination");
            Response.Write("\t" + txtFinalDestination.Text);
            Response.Write("\n");

            Response.Write("Comments");
            Response.Write("\t" + txtComments.Text);
            Response.Write("\n");

            Response.Write("Invoice Date");
            Response.Write("\t" + txtInvoiceDate.Text);
            Response.Write("\n");

            Response.Write("Shipped Date");
            Response.Write("\t" + txtShippedDate.Text);
            Response.Write("\n");

            Response.Write("Shipped Via");
            Response.Write("\t" + ddlShippedVia.SelectedItem.Text);
            Response.Write("\n");

            Response.Write("Total Line Amount");
            Response.Write("\t" + txtTotalLineAmount.Text);
            Response.Write("\n");

            Response.Write("Total Additional Charges");
            Response.Write("\t" + txtTotalAdditionalCharges.Text);
            Response.Write("\n");

            Response.Write("Total Invoice Amount");
            Response.Write("\t" + txtTotalInvoiceAmount.Text);
            Response.Write("\n");


            tab = "\t";

            Response.Write("\n");

            Response.Write("\n");

            TableRow DataItemRow;
            //foreach (WorkListDTO obj in WorkListDTOobjlist)
            //{
            DataItemRow = new TableRow();
            HeaderApproverName = new TableCell();
            HeaderAllocationDate = new TableCell();
            HeaderMappingType = new TableCell();
            HeaderReason = new TableCell();


            DataItemRow.Cells.Add(HeaderApproverName);
            DataItemRow.Cells.Add(HeaderAllocationDate);
            DataItemRow.Cells.Add(HeaderMappingType);
            DataItemRow.Cells.Add(HeaderReason);



            DataTable TableLineItem = new DataTable();
            DataTable TableAdditionalCharge = new DataTable();
            DataTable TableAccountDistribution = new DataTable();
            DataTable TableAttachment = new DataTable();

            TableLineItem = (DataTable)ViewState["PopulatedTableLineItemDetails"];
            TableAdditionalCharge = (DataTable)ViewState["PopulatedTableAdditionalCharge"];
            TableAccountDistribution = (DataTable)ViewState["TempTableAccDist"];
            TableAttachment = (DataTable)ViewState["TempTableFileInfo"];

            if (TableLineItem.Rows.Count > 1)
            {
                Response.Write("Line Item Details");
                foreach (DataColumn dtcol in TableLineItem.Columns)
                {
                    if (dtcol.ColumnName != ("Inv_No"))
                    {
                        Response.Write(tab + dtcol.ColumnName);
                        tab = "\t";
                    }
                }
                Response.Write("\n");


                // code to be added by amol

                // "qty_unitofmeasure_id"
                //   DropDownlistQtyUnitofMeasure
                Hashtable hs = new Hashtable();
                DataSet dsLoadQtyUnitofMeasure = new BLL.LoadValuesinDropDownlistBLL().LoadValuesinDropDownList("tbl_qty_unitmeasure");
                if (dsLoadQtyUnitofMeasure.Tables[0].Rows.Count != 0)
                {

                    //ddl.DataTextField = "description";
                    //ddl.DataValueField = "qty_unitofmeasure_id";

                    foreach (DataRow dritem in dsLoadQtyUnitofMeasure.Tables[0].Rows)
                    {
                        hs.Add(dritem["qty_unitofmeasure_id"], dritem["description"]);
                    }

                }
                //code to be added by amol


                foreach (DataRow dr in TableLineItem.Rows)
                {
                    tab = "\t";

                    for (int j = 1; j < TableLineItem.Columns.Count; j++)
                    {
                        //if(dr.col
                        if (j == 8)
                        {
                            string col = dr[j].ToString();
                            if (col.Length > 0)
                            {
                                int index = Convert.ToInt32(col);
                                string val = hs[index].ToString();
                                Response.Write(tab + val);
                            }
                        }
                        else
                        {
                            Response.Write(tab + Convert.ToString(dr[j]));
                        }
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
            }
            if (TableAdditionalCharge != null)
            {
                Response.Write("Additional Charge Details");
                foreach (DataColumn dtcol in TableAdditionalCharge.Columns)
                {
                    if (dtcol.ColumnName != ("Inv_no"))
                    {
                        Response.Write(tab + dtcol.ColumnName);
                        tab = "\t";
                    }
                }
                Response.Write("\n");


                foreach (DataRow dr in TableAdditionalCharge.Rows)
                {
                    tab = "\t";
                    for (int j = 1; j < TableAdditionalCharge.Columns.Count; j++)
                    {
                        Response.Write(tab + Convert.ToString(dr[j]));
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
            }
            if (TableAccountDistribution.Rows.Count > 1)
            {
                Response.Write("Account Distribution Details");
                foreach (DataColumn dtcol in TableAccountDistribution.Columns)
                {
                    if (dtcol.ColumnName != ("Inv_no"))
                    {
                        Response.Write(tab + dtcol.ColumnName);
                        tab = "\t";
                    }
                }
                Response.Write("\n");


                foreach (DataRow dr in TableAccountDistribution.Rows)
                {
                    tab = "\t";
                    for (int j = 1; j < TableAccountDistribution.Columns.Count; j++)
                    {
                        Response.Write(tab + Convert.ToString(dr[j]));
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
            }
            if (TableAttachment.Rows.Count > 0)
            {
                Response.Write("Attachment Details");
                foreach (DataColumn dtcol in TableAttachment.Columns)
                {
                    if (dtcol.ColumnName != ("Attachment_id"))
                    {
                        Response.Write(tab + dtcol.ColumnName);
                        tab = "\t";
                    }
                }
                Response.Write("\n");

                foreach (DataRow dr in TableAttachment.Rows)
                {
                    tab = "\t";
                    for (int j = 1; j < TableAttachment.Columns.Count; j++)
                    {
                        Response.Write(tab + Convert.ToString(dr[j]));
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
            }
            Response.End();




        }

        private void DisableAJAXExtenders(Control parent)
        {
            for (int i = 0; i < parent.Controls.Count; i++)
            {
                if (parent.Controls[i].GetType().ToString().IndexOf("Extender") != -1 && parent.Controls[i].ID != null)
                {
                    parent.Controls.RemoveAt(i);
                    parent.Controls[i].Dispose();
                    //Control c;

                }
                if (parent.Controls[i].Controls.Count > 0)
                {
                    DisableAJAXExtenders(parent.Controls[i]);
                }
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //errmessage.Text = errormessg;
        }

        protected void txtclr_Click(object sender, EventArgs e)
        {
            txtInvoiceNumber.Text = "";
            txtInvoiceTo.Text = "";
            txtFinalDestination.Text = "";
            txtComments.Text = "";
            PopulateDropDownlist();
            PopulateSupplier();
            SetInitialRow();
            SetNewRowAdditionalCharge();
            SetInitialRowAccDist();
            BindAttachmentGridviewData();
            txtTotalInvoiceAmount.Text = "";
            txtTotalLineAmount.Text = "";
            txtTotalAdditionalCharges.Text = "";
            clearlineitemtextboxes();
            clearaddchargetextboxes();
            clearaccountdistributiontextboxes();
            gvLineItemDetails.DataBind();
            gvAccountDistributionDetails.DataBind();
            gvAdditionalChargeDetails.DataBind();

            ViewState["totallineamount"] = null;//string.Empty;
            ViewState["totalAdditionalAmount"] = null; //string.Empty;
            ViewState["totallineamount"] = null;//string.Empty;
        }

        protected void ddlPayableTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPayableTo.SelectedItem.Text != "Select")
            {
                txtInvoiceTo.Text = Convert.ToString(ddlPayableTo.SelectedItem);
            }
            else
            {
                txtInvoiceTo.Text = string.Empty;
            }
        }

        protected void addTrigger_PreRender(object sender, EventArgs e)
        {
            if (sender is LinkButton)
            {
                LinkButton lnk1 = (LinkButton)sender;
                ScriptManager NewScriptManager = (ScriptManager)this.Master.FindControl("ScriptManager1");//this.FindControl("ScriptManager1");//this.FindControl("ScriptManager1");
                //ToolkitScriptManager1.RegisterPostBackControl(lnk1);
                NewScriptManager.RegisterPostBackControl(lnk1);
                //this.ToolkitScriptManager1.RegisterPostBackControl(lnk1);
                
            }

        }

        private void DisableControlsAfterCreateInvoice()
        {


            txtSupplierCode.Enabled = false;
            ddlShippedTo.Enabled = false;
            // ddlPayableTo.SelectedItem.Enabled = false;
            txtEmailAddress.Enabled = false;
            txtCurrency.Enabled = false;
            txtInvoiceNumber.Enabled = false;
            txtInvoiceTo.Enabled = false;
            txtSupplierAddress.Enabled = false;
            txtFinalDestination.Enabled = false;
            txtComments.Enabled = false;
            txtInvoiceDate.Enabled = false;
            txtShippedDate.Enabled = false;
            ddlShippedVia.Enabled = false;
            txtOther.Enabled = false;
            txtTotalLineAmount.Enabled = false;
            linitemtxtboxes.Visible = false;
            addchargetxtboxes.Visible = false;
            accdistrtxtboxes.Visible = false;
            gvLineItemDetails.Enabled = false;
            gvAdditionalChargeDetails.Enabled = false;
            gvAccountDistributionDetails.Enabled = false;

            ddlPayableTo.Enabled = false;
            //Hide all buttons
            txtsubmit.Visible = false;
            txtsave.Visible = false;
            btnValidateInvoice.Visible = false;
            txtclr.Visible = false;
            AttachButton.Enabled = false;
            FileUpload1.Enabled = false;
            gvDetails.Enabled = false;
            lbnSupplierCodeLookup.Enabled = false;
            txtOther.Enabled = false;
        }

        //Gridview empty
        private void DisplayAdditionalChargeGridviewWhenEmpty()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Charge# *");
            dt.Columns.Add("Charge Type *");
            dt.Columns.Add("Charge *");
            dt.Columns.Add("Amount ($) *");
            dt.Columns.Add("Description *");
            dt.Columns.Add("GST (Goods & Service Tax) *");
            gvAdditionalChargeDetails.Visible = true;
            gvAdditionalChargeDetails.ShowHeaderWhenEmpty = true;
            gvAdditionalChargeDetails.DataSource = dt;
            gvAdditionalChargeDetails.DataBind();
        }

        //Gridview empty
        private void DisplayAccountChargeGridviewWhenEmpty()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Debit/Credit");
            dt.Columns.Add("General Ledger *");
            dt.Columns.Add("Cost centre1 *");
            dt.Columns.Add("Cost centre2");
            dt.Columns.Add("WBS No.");
            dt.Columns.Add("Amount ($) *");
            gvAccountDistributionDetails.Visible = true;
            gvAccountDistributionDetails.ShowHeaderWhenEmpty = true;
            gvAccountDistributionDetails.DataSource = dt;
            gvAccountDistributionDetails.DataBind();
        }

        protected void ExportExcel_Click(object sender, EventArgs e)
        {

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename=" + txtInvoiceNumber.Text.Trim() + ".xls"));
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            string tab = string.Empty;

            StringWriter strWriter = new StringWriter();
            HtmlTextWriter htWriter = new HtmlTextWriter(strWriter);

            //frmtbl.RenderControl(htWriter);
            //Response.Output.Write(strWriter.ToString());
            //Response.Flush();
            //Response.End();
            ///////////////////////////////////////////////////////

            Table tblforexprot = new Table();
            tblforexprot.BorderWidth = 1;
            tblforexprot.GridLines = GridLines.Both;
            tblforexprot.BorderStyle = BorderStyle.Solid;
            tblforexprot.BorderColor = System.Drawing.Color.Black;

            TableRow header = new TableRow();
            TableCell headercontent = new TableCell();
            headercontent.ColumnSpan = 7;
            headercontent.Attributes.Add("align", "center");
            header.Font.Bold = true;
            header.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
            header.ForeColor = System.Drawing.Color.White;
            header.HorizontalAlign = HorizontalAlign.Center;
            header.BorderStyle = BorderStyle.Solid;
            header.BorderColor = System.Drawing.Color.Black;



            //headercontent.Text = "Coder's Worklist";
            headercontent.Text = txtInvoiceNumber.Text.Trim();
            header.Cells.Add(headercontent);
            tblforexprot.Rows.Add(header);

            TableRow HeaderRow = new TableRow();

            HeaderRow.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
            HeaderRow.ForeColor = System.Drawing.Color.White;
            HeaderRow.HorizontalAlign = HorizontalAlign.Center;
            HeaderRow.Font.Bold = true;
            HeaderRow.BorderStyle = BorderStyle.Solid;
            HeaderRow.BorderColor = System.Drawing.Color.Black;


            TableCell HeaderApproverName = new TableCell();
            TableCell HeaderAllocationDate = new TableCell();
            TableCell HeaderMappingType = new TableCell();
            TableCell HeaderReason = new TableCell();


            HeaderReason.Text = txtSupplierCode.Text;
            HeaderMappingType.Text = txtInvoiceNumber.Text;
            HeaderAllocationDate.Text = txtInvoiceTo.Text;
            HeaderApproverName.Text = txtEmailAddress.Text;


            HeaderRow.Cells.Add(HeaderApproverName);
            HeaderRow.Cells.Add(HeaderAllocationDate);
            HeaderRow.Cells.Add(HeaderMappingType);
            HeaderRow.Cells.Add(HeaderReason);

            tblforexprot.Rows.Add(HeaderRow);

            tab = "";

            Response.Write("\t" + tab);
            Response.Write("\t" + tab);
            Response.Write("\t" + tab);
            Response.Write("\t" + tab);

            Response.Write("\t" + "Invoice Details");
            Response.Write("\n");

            Response.Write("\n");

            Response.Write("Supplier Code");
            Response.Write("\t" + txtSupplierCode.Text);
            Response.Write("\n");

            Response.Write("Invoice Number");
            Response.Write("\t" + txtInvoiceNumber.Text);
            Response.Write("\n");

            Response.Write("Supplier To");
            Response.Write("\t" + ddlShippedTo.SelectedItem.Text);
            Response.Write("\n");

            Response.Write("Payable To");
            Response.Write("\t" + ddlPayableTo.SelectedItem.Text);
            Response.Write("\n");

            Response.Write("EmailAddress");
            Response.Write("\t" + txtEmailAddress.Text);
            Response.Write("\n");

            Response.Write("Currency");
            Response.Write("\t" + txtCurrency.Text);
            Response.Write("\n");

            Response.Write("Invoice To");
            Response.Write("\t" + txtInvoiceTo.Text);
            Response.Write("\n");

            Response.Write("Supplier Address");
            Response.Write("\t" + txtSupplierAddress.Text);
            Response.Write("\n");

            Response.Write("Final Destination");
            Response.Write("\t" + txtFinalDestination.Text);
            Response.Write("\n");

            Response.Write("Comments");
            Response.Write("\t" + txtComments.Text);
            Response.Write("\n");

            Response.Write("Invoice Date");
            Response.Write("\t" + txtInvoiceDate.Text);
            Response.Write("\n");

            Response.Write("Shipped Date");
            Response.Write("\t" + txtShippedDate.Text);
            Response.Write("\n");

            if (ddlShippedVia.SelectedValue == "5")
            {
                Response.Write("Shipped Via");
                Response.Write("\t" + txtOther.Text);
                Response.Write("\n");
            }
            else
            {
                Response.Write("Shipped Via");
                Response.Write("\t" + ddlShippedVia.SelectedItem.Text);
                Response.Write("\n");
            }
            Response.Write("Total Line Amount");
            Response.Write("\t" + txtTotalLineAmount.Text);
            Response.Write("\n");

            Response.Write("Total Additional Charges");
            Response.Write("\t" + txtTotalAdditionalCharges.Text);
            Response.Write("\n");

            Response.Write("Total Invoice Amount");
            Response.Write("\t" + txtTotalInvoiceAmount.Text);
            Response.Write("\n");


            tab = "\t";

            Response.Write("\n");

            Response.Write("\n");

            TableRow DataItemRow;
            //foreach (WorkListDTO obj in WorkListDTOobjlist)
            //{
            DataItemRow = new TableRow();
            HeaderApproverName = new TableCell();
            HeaderAllocationDate = new TableCell();
            HeaderMappingType = new TableCell();
            HeaderReason = new TableCell();


            DataItemRow.Cells.Add(HeaderApproverName);
            DataItemRow.Cells.Add(HeaderAllocationDate);
            DataItemRow.Cells.Add(HeaderMappingType);
            DataItemRow.Cells.Add(HeaderReason);



            DataTable TableLineItem = new DataTable();
            DataTable TableAdditionalCharge = new DataTable();
            DataTable TableAccountDistribution = new DataTable();
            DataTable TableAttachment = new DataTable();

            TableLineItem = (DataTable)ViewState["PopulatedTableLineItemDetails"];
            TableAdditionalCharge = (DataTable)ViewState["PopulateTableAdditionalCharge"];
            TableAccountDistribution = (DataTable)ViewState["PopulateAccountDistDetails"];
            TableAttachment = (DataTable)ViewState["TempTableFileInfo"];


            Response.Write("\n");
            Response.Write("Line Item Details");
            foreach (DataColumn dtcol in TableLineItem.Columns)
            {
                if (dtcol.ColumnName != ("Inv_No"))
                {
                    Response.Write(tab + dtcol.ColumnName);
                    tab = "\t";
                }
            }
            if (TableLineItem != null)
            {
                // Response.Write("Line Item Details");

                if (TableLineItem.Rows.Count >= 1)
                {
                    // Response.Write("Line Item Details");
                    //foreach (DataColumn dtcol in TableLineItem.Columns)
                    //{
                    //    if (dtcol.ColumnName != ("Inv_No"))
                    //    {
                    //        Response.Write(tab + dtcol.ColumnName);
                    //        tab = "\t";
                    //    }
                    //}
                    Response.Write("\n");


                    foreach (DataRow dr in TableLineItem.Rows)
                    {
                        tab = "\t";
                        
                            tab = "\t";
                            for (int j = 0; j < TableLineItem.Columns.Count; j++)
                            {

                                // code to be added by amol

                                // "qty_unitofmeasure_id"
                                //   DropDownlistQtyUnitofMeasure

                                //code to be added by amol

                                Response.Write(tab + Convert.ToString(dr[j]));
                                tab = "\t";
                            }
                            Response.Write("\n");
                        }
                    }

                }
                Response.Write("\n");
                Response.Write("Additional Charge Details");
                foreach (DataColumn dtcol in TableAdditionalCharge.Columns)
                {
                    if (dtcol.ColumnName != ("Inv_no"))
                    {
                        Response.Write(tab + dtcol.ColumnName);
                        tab = "\t";
                    }
                }
                if (TableAdditionalCharge != null)
                {

                    Response.Write("\n");


                    foreach (DataRow dr in TableAdditionalCharge.Rows)
                    {
                        tab = "\t";
                        for (int j = 0; j < TableAdditionalCharge.Columns.Count; j++)
                        {
                            Response.Write(tab + Convert.ToString(dr[j]));
                            tab = "\t";
                        }
                        Response.Write("\n");
                    }

                }
                Response.Write("\n");
                Response.Write("Account Distribution Details");
                foreach (DataColumn dtcol in TableAccountDistribution.Columns)
                {
                    if (dtcol.ColumnName != ("Inv_No"))
                    {
                        Response.Write(tab + dtcol.ColumnName);
                        tab = "\t";
                    }
                }
                if (TableAccountDistribution != null)
                {

                    if (TableAccountDistribution.Rows.Count >= 1)
                    {
                        //Response.Write("Account Distribution Details");
                        //foreach (DataColumn dtcol in TableAccountDistribution.Columns)
                        //{
                        //    if (dtcol.ColumnName != ("Inv_no"))
                        //    {
                        //        Response.Write(tab + dtcol.ColumnName);
                        //        tab = "\t";
                        //    }
                        //}
                        Response.Write("\n");


                        foreach (DataRow dr in TableAccountDistribution.Rows)
                        {
                            tab = "\t";
                            for (int j = 1; j < TableAccountDistribution.Columns.Count; j++)
                            {
                                Response.Write(tab + Convert.ToString(dr[j]));
                                tab = "\t";
                            }
                            Response.Write("\n");
                        }
                    }

                }
                Response.Write("\n");
                Response.Write("Attachment Details");
                foreach (DataColumn dtcol in TableAttachment.Columns)
                {
                    if (dtcol.ColumnName != ("Attachment_id"))
                    {
                        Response.Write(tab + dtcol.ColumnName);
                        tab = "\t";
                    }
                }
                if (TableAttachment != null)
                {

                    if (TableAttachment.Rows.Count >= 0)
                    {
                        //Response.Write("Attachment Details");
                        //foreach (DataColumn dtcol in TableAttachment.Columns)
                        //{
                        //    if (dtcol.ColumnName != ("Attachment_id"))
                        //    {
                        //        Response.Write(tab + dtcol.ColumnName);
                        //        tab = "\t";
                        //    }
                        //}
                        Response.Write("\n");

                        foreach (DataRow dr in TableAttachment.Rows)
                        {
                            tab = "\t";
                            for (int j = 0; j < TableAttachment.Columns.Count; j++)
                            {
                                Response.Write(tab + Convert.ToString(dr[j]));
                                tab = "\t";
                            }
                            Response.Write("\n");
                        }
                    }
                    Response.Write("\n");
                }
                Response.End();
            }

        protected void Updatetnfoelne_Click(object sender, EventArgs e)
        {
            int rowindex = (int)ViewState["lineitemrowindex"];
            DataTable lintmdata = (DataTable)ViewState["PopulatedTableLineItemDetails"];

            lintmdata.Rows[rowindex][0] = txtprtnumbr.Text.ToString();
            lintmdata.Rows[rowindex][1] = txtponumbr.Text.ToString();
            lintmdata.Rows[rowindex][2] = txtpoamend.Text.ToString();
            lintmdata.Rows[rowindex][3] = txtrelease.Text.ToString();
            lintmdata.Rows[rowindex][4] = txtqtyshipped.Text.ToString();
            lintmdata.Rows[rowindex][5] = txtunitprice.Text.ToString();
            long amount = Convert.ToInt64(txtqtyshipped.Text.ToString()) * Convert.ToInt64(txtunitprice.Text.ToString());
            //long amount = Convert.ToInt64(txtqtyshipped.Text.ToString()) * String.Format("{0:0.00}", Double.Parse(txtunitprice.Text.ToString()));
            lintmdata.Rows[rowindex][6] = amount.ToString();
            lintmdata.Rows[rowindex][7] = ddlQtyUnitofMeasure.SelectedItem.Text;
            lintmdata.Rows[rowindex][8] = txtpack.Text.ToString();
            lintmdata.Rows[rowindex][9] = txtbilloflading.Text.ToString();
            lintmdata.Rows[rowindex][10] = txtComm.Text.ToString();
            ViewState["PopulatedTableLineItemDetails"] = lintmdata;
            gvLineItemDetails.DataSource = lintmdata;
            gvLineItemDetails.DataBind();
            string strlneamtforupdating = (string)ViewState["lineamtforupdating"];
            int lneamtforupdating = Convert.ToInt32(Convert.ToDecimal(strlneamtforupdating));
            
            if (ViewState["totallineamount"] != null)
            {
                if (ViewState["totallineamount"].ToString().Contains('('))
                {
                    string[] totallineamt = ViewState["totallineamount"].ToString().Split(new[] { '(', ')' });
                    totalLineAmount = -Convert.ToInt64(Convert.ToDecimal(totallineamt[1]));
                }
                else
                {
                    totalLineAmount = Convert.ToInt64(Convert.ToDecimal(ViewState["totallineamount"]));
                }
            }

            long AmountFromAdditional = 0;
            if (ViewState["totalAdditionalAmount"] != null)
            {
                AmountFromAdditional = Convert.ToInt64(Convert.ToDecimal(ViewState["totalAdditionalAmount"]));
            }


            totalLineAmount = Convert.ToInt64(Convert.ToDecimal(txtlineamt.Text)) + totalLineAmount - Convert.ToInt64(lneamtforupdating);
            ViewState["totallineamount"] = totalLineAmount;




            if (totalLineAmount < 0)
            {
                string[] res = totalLineAmount.ToString().Split(new[] { '-' });
                txtTotalLineAmount.Text = "(" + res[1] + ")";
            }
            else
            {

                 //totalLineAmount.ToString()=String.Format("{0:0.00}", Double.Parse(txtTotalLineAmount.Text.ToString()))
                txtTotalLineAmount.Text = totalLineAmount.ToString();
                //txtTotalLineAmount.Text = String.Format("{0:0.00}", Double.Parse(txtTotalLineAmount.Text.ToString()));
            }

            if ((totalLineAmount + AmountFromAdditional) < 0)
            {
                string[] res = (totalLineAmount + AmountFromAdditional).ToString().Split(new[] { '-' });
                txtTotalInvoiceAmount.Text = "(" + res[1] + ")";
            }
            else
            {
                txtTotalInvoiceAmount.Text = (totalLineAmount + AmountFromAdditional).ToString();
            }
            rowcounthidden.Value = gvLineItemDetails.Rows.Count.ToString();
            clearlineitemtextboxes();
            Updatetnfoelne.Visible = false;
            Cancelbtnforlne.Visible = false;
            btnadd.Visible = true;
        }

          protected void Cancelbtnforlne_Click(object sender, EventArgs e)
        {
            clearlineitemtextboxes();
            Updatetnfoelne.Visible = false;
            Cancelbtnforlne.Visible = false;
            btnadd.Visible = true;
        }

          protected void ddlShippedVia_SelectedIndexChanged(object sender, EventArgs e)
          {
              if (ddlShippedVia.SelectedValue == "5")
              {
                  txtOther.Visible = true;
                  txtOther.Enabled = true;
              }
              else
              {
                  txtOther.Visible = false;
              }
          }

          [System.Web.Script.Services.ScriptMethod()]

          [System.Web.Services.WebMethod]

          public static List<string> GetCountries(string prefixText)
          {
              ArrayList lstParam = new System.Collections.ArrayList();
              SqlParameter param;

              param = new SqlParameter();
              param.ParameterName = "@Currency";
              param.DbType = DbType.String;
              param.Value = prefixText;
              lstParam.Add(param);

              DataSet dsintell = new DAL.SqlHelper().SelectDataSet("Select Currency FROM tblCurrency WHERE Currency like @Currency+'%'", lstParam, true);
              DataTable dt = dsintell.Tables[0];

              List<string> CountryNames = new List<string>();

              for (int i = 0; i < dt.Rows.Count; i++)
              {

                  CountryNames.Add(dt.Rows[i][0].ToString());

              }

              return CountryNames;

          }

          protected void ddlchtgetype_SelectedIndexChanged(object sender, EventArgs e)
          {
              if (ddlchtgetype.SelectedValue == "5")
              {
                  txtOtherChargeType.Visible = true;
              }
              else
              {
                  txtOtherChargeType.Visible = false;
              }
          }
        }
        
    }








// #endregion
