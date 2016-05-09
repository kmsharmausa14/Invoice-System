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

namespace VendorPortal
{
    public partial class ApproverScreen : System.Web.UI.Page
    {
        protected string flageFromInvStatus = string.Empty;
        protected string flageFromApprWorkQue = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Userid"] == null)
            {
                Response.Redirect("LoginPage.aspx");
            }
            if (!IsPostBack)
            {
                PopulateApproverScreenFromApproverWorkQueueOrInvoiceStatus();
                HideButtons();
                if (gvAdditionalChargeDetails.Rows.Count == 0)
                {
                    SetNewRowAdditionalCharge();
                }

                if (gvAccountDistributionDetails.Rows.Count == 0)
                {
                    SetInitialRowAccDist();
                }
            }
            this.txtReason.Attributes.Add("onkeypress", "return ValidateLength('" + this.txtReason.ClientID + "')");
            this.txtReason.Attributes.Add(" onChange", "return ValidateLengthAfterTextChange('" + this.txtReason.ClientID + "')");//
            DisableControlsAfterCreateInvoice();

        }

        //Hide the 'Approve' and 'Reject' buttons if user is coming from Invoice Status page.
        private void HideButtons()
        {
           flageFromInvStatus = Convert.ToString(Request.QueryString["FlageFromInvStatus"]);
           
            if (flageFromInvStatus == "yes")
            {
                btnApprove.Visible = false;
                btnReject.Visible = false;
            }
            else
            {
                btnApprove.Visible = true;
                btnReject.Visible = true;
            }
            
        }

        private void SetNewRowAdditionalCharge()
        {
            DataRow dr = null;
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Inv_no", typeof(string)));
            dt.Columns.Add(new DataColumn("charge_no", typeof(string)));
            dt.Columns.Add(new DataColumn("Charge_type", typeof(string)));
            dt.Columns.Add(new DataColumn("charge", typeof(string)));
            dt.Columns.Add(new DataColumn("amount", typeof(string)));
            dt.Columns.Add(new DataColumn("description", typeof(string)));
            dt.Columns.Add(new DataColumn("Gst_no", typeof(string)));
            dr = dt.NewRow();
            dr["charge_no"] = " - ";//string.Empty;
            dr["Charge_type"] = string.Empty;
            dr["charge"] = string.Empty;
            dr["amount"] = string.Empty;
            dr["description"] = string.Empty;
            dr["Gst_no"] = string.Empty;
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            ViewState["TempTableAdditionalCharge"] = dt;
            ViewState["PopulateTableAdditionalCharge"] = dt;
            gvAdditionalChargeDetails.DataSource = dt;
            gvAdditionalChargeDetails.DataBind();


        }

        private void SetInitialRowAccDist()
        {

            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("Inv_no", typeof(string)));
            dt.Columns.Add(new DataColumn("Debit_Credit", typeof(string)));
            dt.Columns.Add(new DataColumn("general_ledger_account", typeof(string)));
            dt.Columns.Add(new DataColumn("costcenter_1", typeof(string)));
            dt.Columns.Add(new DataColumn("costcenter_2", typeof(string)));
            dt.Columns.Add(new DataColumn("WBS_no", typeof(string)));
            dt.Columns.Add(new DataColumn("amount", typeof(string)));
            dr = dt.NewRow();
            dr["Debit_Credit"] = " - ";//string.Empty;
            dr["general_ledger_account"] = string.Empty;
            dr["costcenter_1"] = string.Empty;
            dr["costcenter_2"] = string.Empty;
            dr["WBS_no"] = string.Empty;
            dr["amount"] = string.Empty;
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            //Store the DataTable in ViewState

            ViewState["TempTableAccDist"] = dt;
            ViewState["PopulateAccountDistributionDetails"] = dt;
            ViewState["PopulateAccountDistDetails"] = dt;
            gvAccountDistributionDetails.DataSource = dt;

            gvAccountDistributionDetails.DataBind();



        }

        #region Populate all textbox from List of Draft Gridview
        private void BindAttachmentGridviewData(string invoiceno)
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
        private void PopulateApproverScreenFromApproverWorkQueueOrInvoiceStatus()
        {
            //string strInvCode = Request.QueryString["Inv_Code"]; //GetInvoiceCode()
            string strInvCode = GetInvoiceCode();

            //This code will executed if user is directing from List of Draft page to Create Invoice page
            if (string.IsNullOrEmpty(strInvCode) != true)
            {
                DataSet dsPopulateApproverScreen = new BLL.ApproverWorkqueueBLL().PopulateApproverScreenHeader(strInvCode);

                if (dsPopulateApproverScreen.Tables[0].Rows.Count > 0)
                {
                    txtSupplierCode.Text = dsPopulateApproverScreen.Tables[0].Rows[0][0].ToString();
                    txtShippedTo.Text = dsPopulateApproverScreen.Tables[0].Rows[0][1].ToString();
                    txtPayableTo.Text = dsPopulateApproverScreen.Tables[0].Rows[0][3].ToString();
                    txtEmailAddress.Text = dsPopulateApproverScreen.Tables[0].Rows[0][4].ToString();

                    txtCurrency.Text = dsPopulateApproverScreen.Tables[0].Rows[0][5].ToString();
                    txtInvoiceNumber.Text = dsPopulateApproverScreen.Tables[0].Rows[0][6].ToString();
                    txtInvoiceTo.Text = dsPopulateApproverScreen.Tables[0].Rows[0][7].ToString();
                    txtSupplierAddress.Text = dsPopulateApproverScreen.Tables[0].Rows[0][8].ToString();
                    txtFinalDestination.Text = dsPopulateApproverScreen.Tables[0].Rows[0][9].ToString();
                    txtComments.Text = dsPopulateApproverScreen.Tables[0].Rows[0][10].ToString();

                    if(!string.IsNullOrEmpty(dsPopulateApproverScreen.Tables[0].Rows[0][11].ToString()))
                    {
                    DateTime InvDate = Convert.ToDateTime(dsPopulateApproverScreen.Tables[0].Rows[0][11].ToString());
                    txtInvoiceDate.Text = InvDate.ToString("MM-dd-yyyy");
                    }

                     if(!string.IsNullOrEmpty(dsPopulateApproverScreen.Tables[0].Rows[0][12].ToString()))
                    {
                    DateTime ShippedDate = Convert.ToDateTime(dsPopulateApproverScreen.Tables[0].Rows[0][12].ToString());
                    txtShippedDate.Text = ShippedDate.ToString("MM-dd-yyyy");
                     }

                    //txtInvoiceDate.Text = dsPopulateApproverScreen.Tables[0].Rows[0][11].ToString();
                    //txtShippedDate.Text = dsPopulateApproverScreen.Tables[0].Rows[0][12].ToString();

                    //CalendarExtenderInvoiceDate.SelectedDate = Convert.ToDateTime(txtInvoiceDate.Text); //**********
                    //CalendarExtenderShippedDate.SelectedDate = Convert.ToDateTime(txtShippedDate.Text);//**********

                    txtShippedVia.Text = dsPopulateApproverScreen.Tables[0].Rows[0][13].ToString();
                    // = dsPopulateCreateInvoice.Tables[0].Rows[0][13].ToString();
                    txtTotalLineAmount.Text = dsPopulateApproverScreen.Tables[0].Rows[0][14].ToString();
                    txtTotalAdditionalCharges.Text = dsPopulateApproverScreen.Tables[0].Rows[0][15].ToString();
                    txtTotalInvoiceAmount.Text = Convert.ToString(Convert.ToDecimal(dsPopulateApproverScreen.Tables[0].Rows[0][16].ToString()));
                    txtReason.Text = dsPopulateApproverScreen.Tables[0].Rows[0][17].ToString();
                }

                DataSet dsPopulateLineItemsGridview = new BLL.ApproverWorkqueueBLL().PopulateApproverScreenGridview("gvLineItemDetails", strInvCode);
                if (dsPopulateLineItemsGridview.Tables[0].Rows.Count != 0)
                {
                    gvLineItemDetails.DataSource = dsPopulateLineItemsGridview.Tables[0];
                    gvLineItemDetails.DataBind();

                }

                DataSet dsPopulateAdditionalChargeDetailsGridview = new BLL.ApproverWorkqueueBLL().PopulateApproverScreenGridview("gvAdditionalChargeDetails", strInvCode);
                if (dsPopulateAdditionalChargeDetailsGridview.Tables[0].Rows.Count != 0)
                {
                    gvAdditionalChargeDetails.DataSource = dsPopulateAdditionalChargeDetailsGridview.Tables[0];
                    gvAdditionalChargeDetails.DataBind();

                }

                DataSet dsPopulateAccountDistributionDetailsGridview = new BLL.ApproverWorkqueueBLL().PopulateApproverScreenGridview("gvAccountDistributionDetails", strInvCode);
                if (dsPopulateAccountDistributionDetailsGridview.Tables[0].Rows.Count != 0)
                {
                    gvAccountDistributionDetails.DataSource = dsPopulateAccountDistributionDetailsGridview.Tables[0];
                    gvAccountDistributionDetails.DataBind();

                }
            }
            BindAttachmentGridviewData(strInvCode);
        }

        /*private void PopulateAllDates()
        {
            string strInvCodeFromListOfDraft = GetInvoiceCode();

            //If user in not redirecting from 'List of Draft' page to Create Invoice page
            if (string.IsNullOrEmpty(strInvCodeFromListOfDraft) == true)
            {
                CalendarExtenderInvoiceDate.SelectedDate = DateTime.Now.Date;
                CalendarExtenderShippedDate.SelectedDate = DateTime.Now.Date;
            }
            //If user in redirecting from 'List of Draft' page to Create Invoice page
            else
            {
                CalendarExtenderInvoiceDate.SelectedDate = Convert.ToDateTime(txtInvoiceDate.Text);
                CalendarExtenderShippedDate.SelectedDate = Convert.ToDateTime(txtShippedDate.Text);

            }

        }*/

        #endregion

        ////private void BindAttachmentGridviewData(string invoiceno)
        ////{
        ////    string userId = Session["Userid"].ToString();
        ////    BLL.AttachmentBLLcs obj1 = new AttachmentBLLcs();
        ////    AttachmentBO attachbo = new AttachmentBO();
        ////    attachbo.Userid = userId;
        ////    attachbo.Inv_Code = invoiceno;
        ////    DataSet dsLoadPayableTo = obj1.GetAllAttachmentFileDetailsByInvoiceID(attachbo);

        ////    if (dsLoadPayableTo.Tables.Count > 0)
        ////    {
        ////        gvDetails.DataSource = dsLoadPayableTo;
        ////        gvDetails.DataBind();

        ////    }
        ////}
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Response.Redirect("Home.aspx");
            flageFromApprWorkQue = Convert.ToString(Request.QueryString["FlageFromApproverWorkQue"]);
            flageFromInvStatus = Convert.ToString(Request.QueryString["FlageFromInvStatus"]);

            if (flageFromApprWorkQue == "yes")
            {
                Response.Redirect("~/Approver_WrkQue.aspx");
            }
            else if (flageFromInvStatus == "yes")
            {
                Response.Redirect("~/InvStatus.aspx");
            }

            
        }

        //Getting InvoiceCode which is coming from Approver_WrkQue page
        private string GetInvoiceCode()
        {
            string InvoiceCode = Convert.ToString(Request.QueryString["QS_Inv_Code"]);
            ViewState["InvCode"] = InvoiceCode;
            return InvoiceCode;
        }

        //Approve Invoice
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            string InvCode = GetInvoiceCode();
            string userid = (string)Session["Userid"];
            ApproveRejectBO approveRejectBO = new ApproveRejectBO();
            approveRejectBO.InvoiceCode = InvCode;
            approveRejectBO.ApproveRejectComments = Convert.ToString(txtReason.Text.Trim());

            ApproveRejectInvoiceBLL ApprvRejInvBllObj = new ApproveRejectInvoiceBLL();

            ApprvRejInvBllObj.ApproveInvoice(approveRejectBO,userid);

            UserEmailBO userobj = new UserEmailBO();
            //userobj.EmailTo = "Supplier email id";
            //userobj.Approver = "Approver";
           // userobj.Supplier ="";
            userobj.Invoicecode = InvCode;

            //Commenting the code for sending email. Reason- SMTP server is down and successfull msg is not displaying
            /*Utitlity.EmailHelper emailobj = new Utitlity.EmailHelper();
            emailobj.SendEmail(Utitlity.EmailType.InvoiceApprovedBy, userobj);*/

            //Approval message
            string dateTime = string.Empty;
            DataSet ds = ApprvRejInvBllObj.GetInvoiceStatusDetails(InvCode);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                     dateTime = ds.Tables[0].Rows[0][3].ToString();
                }

            }
            succesmsg.Text = "Invoice Code #" + InvCode + " approved successfully on " + dateTime;
            btnApprove.Visible = false;
            btnReject.Visible = false;
            txtReason.Enabled = false;
        }

        //Reject Invoice
        protected void btnReject_Click(object sender, EventArgs e)
        {
            string InvCode = GetInvoiceCode();
            string userid = (string)Session["Userid"];
            ViewState["InvCode"] = InvCode;

            ApproveRejectBO approveRejectBO = new ApproveRejectBO();
            approveRejectBO.InvoiceCode = InvCode;
            approveRejectBO.ApproveRejectComments = Convert.ToString(txtReason.Text.Trim());

            ApproveRejectInvoiceBLL ApprvRejInvBllObj = new ApproveRejectInvoiceBLL();

            ApprvRejInvBllObj.RejectInvoice(approveRejectBO, userid);

            UserEmailBO userobj = new UserEmailBO();
           // userobj.EmailTo = " Supplier email id";
           // userobj.Approver = "Approver"; // n
           // userobj.Supplier = "Supplier";
            userobj.Invoicecode = InvCode;

            //Commenting the code for sending email. Reason- SMTP server is down and successfull msg is not displaying
           /* Utitlity.EmailHelper emailobj = new Utitlity.EmailHelper();
            emailobj.SendEmail(Utitlity.EmailType.InvoiceRejectedBy, userobj);*/

            //Rejected message with timestamp
            string dateTime = string.Empty;
            DataSet ds = ApprvRejInvBllObj.GetInvoiceStatusDetails(InvCode);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dateTime = ds.Tables[0].Rows[0][3].ToString();
                }

            }
            succesmsg.Text = "Invoice Code #" + InvCode + " rejected on " + dateTime;
            btnApprove.Visible = false;
            btnReject.Visible = false;
            txtReason.Enabled = false;

        }

        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Click")
            {

                int rowIndex = int.Parse(e.CommandArgument.ToString());
                string val = (string)this.gvDetails.DataKeys[rowIndex]["File_name"];

                // string fullpath = Server.MapPath("~") + @"\Attachments\" + ViewState["InvCode"].ToString() + @"\";
                string strURL = @"attachments\\" + ViewState["InvCode"].ToString() + @"\\" + val;
                string fullpath1 = Server.MapPath("~") + @"\attachments\" + ViewState["InvCode"].ToString() + @"\" + val;

                if (File.Exists(fullpath1))
                {
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
                    response.End();

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
            //if (dtset != null)
            // {
            if (e.Row.RowIndex > -1)
            {

                int rowIndex = Convert.ToInt32(e.Row.RowIndex.ToString());
                string val = (string)this.gvDetails.DataKeys[rowIndex]["File_name"];
                LinkButton lnk = (LinkButton)e.Row.FindControl("link1");
                lnk.Text = val;
                string fullpath = Server.MapPath("~") + @"\Attachments\" + ViewState["InvCode"].ToString() + @"\";
                string savePath = fullpath + val;
                //HiddenField1.Value = savePath;


                //int rowIndex = int.Parse(e.CommandArgument.ToString());
                //string val = (string)this.gvDetails.DataKeys[rowIndex]["Filename"];



                //lnk.ID = dtset.Tables[0].Rows[e.Row.RowIndex][0].ToString();
                // lnk.Attributes.Add("onClientClick", "SetData(" + lnk.Text + ")");
                //SetDatadtset
                //}
            }

        }


        private void DisableControlsAfterCreateInvoice()
        {


            txtSupplierCode.Enabled = false;
            txtShippedTo.Enabled = false;
            txtPayableTo.Enabled = false;
            txtEmailAddress.Enabled = false;
            txtCurrency.Enabled = false;
            txtInvoiceNumber.Enabled = false;
            txtInvoiceTo.Enabled = false;
            txtSupplierAddress.Enabled = false;
            txtFinalDestination.Enabled = false;
            txtComments.Enabled = false;
            txtInvoiceDate.Enabled = false;
            txtShippedDate.Enabled = false;
            txtShippedVia.Enabled = false;
            txtTotalLineAmount.Enabled = false;
            //linitemtxtboxes.Visible = false;
            //addchargetxtboxes.Visible = false;
            //accdistrtxtboxes.Visible = false;
            gvLineItemDetails.Enabled = false;
            gvAdditionalChargeDetails.Enabled = false;
            gvAccountDistributionDetails.Enabled = false;
            Button4.Enabled = false;
            txtfile.Enabled = false;
            

            //ddlPayableTo.Enabled = false;
            //Hide all buttons
            //txtsubmit.Visible = false;
            //txtsave.Visible = false;
            //btnValidateInvoice.Visible = false;
            //txtclr.Visible = false;
            //AttachButton.Enabled = false;
            //FileUpload1.Enabled = false;
            //gvDetails.Enabled = false;

            //if (gvDetails.Rows.Count > 0)
            //{
            //    for (int i = 0; i < gvDetails.Rows.Count; i++)
            //    {
            //        LinkButton Deletelnk = (LinkButton)gvDetails.Rows[i].FindControl("lnkFileDelete");
            //        LinkButton lnkFile = (LinkButton)gvDetails.Rows[i].FindControl("link1");
            //        this.gvDetails.Enabled = true;
            //        Deletelnk.Enabled = false;
            //        lnkFile.Enabled = true;
            //    }
            //}

        }
    }
}