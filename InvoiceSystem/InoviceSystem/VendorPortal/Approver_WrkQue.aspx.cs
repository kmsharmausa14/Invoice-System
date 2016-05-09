using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;
using System.Data;

namespace VendorPortal
{
    public partial class Approver_WrkQue : System.Web.UI.Page
    {
         
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Userid"] == null)
            {
                Response.Redirect("LoginPage.aspx");
            }
            AsgnToSupplierCode();
            if (!IsPostBack)
            {
                AsgnToSupplierCode();

                //Calender will not allow future dates
                CalendarExtenderFromDate.EndDate = DateTime.Now.Date;
                CalendarExtenderToDate.EndDate = DateTime.Now.Date;
                //Date validations
                txtToDate.Attributes.Add("onchange", "ValidateFromDate()");
                txtFromDate.Attributes.Add("onchange", "ValidateTodateGreaterThanFromDate()");
                if (Convert.ToString(Request.QueryString["QS_Scode2"]) == null)
                {
                    PopulateApproverGridview();
                }
                
               
            }
            //Added by sachin
            ShowGvColumn();
            DataSet ds = new BLL.SupplierDetailsBLL().GetAllSupplier();
            if (ds.Tables[0].Rows.Count != 0)
            {
                grd_ViewSupplierCodeLookUp.DataSource = ds;
                grd_ViewSupplierCodeLookUp.DataBind();
            }  
            btnGo.Attributes.Add("onclick", "return  ValidatePageValue(" + txtGoToPage.ClientID + "," + this.Hidtotalpage.ClientID + ")");
           
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

            Response.Redirect("~/Approver_WrkQue.aspx");
           
        }
        //End supplier code
        private void AsgnToSupplierCode()
        {
            string SC = Convert.ToString(Request.QueryString["QS_Scode"]);
            if (SC != null)
            {
                txtSupplierId.Text = SC;
            }
            else
            {
                txtSupplierId.Text = "";
            }

        }

        private void TotalPage()
        {
            int totalPage = 0;
            DataTable dt = (DataTable)ViewState["ApproverTable"];
            if (dt != null)
            {
                int totalRecords = dt.Rows.Count;
                int pageSize = gvApproverWorkqueue.PageSize;
                if ((totalRecords % pageSize) == 0)
                    totalPage = (totalRecords) / (pageSize);
                else
                    totalPage = ((totalRecords) / (pageSize)) + 1;
                lblTotalPage.Text = totalPage.ToString();
                Hidtotalpage.Value = lblTotalPage.Text;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PopulateApproverGridview();
           // ClearAllTextBoxes();
        }

        protected void gvApproverWorkqueue_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvApproverWorkqueue.PageIndex = e.NewPageIndex;
            PopulateApproverGridview();
        }

        private void PopulateApproverGridview()
        {
            int recordcount = 0;
            lblNoRecords.Visible = false;
            lblNoRecords.Text = string.Empty;
            pagingPanel.Visible = false;
            ListOfDraftInvBO listOfDraftInvBO = new BO.ListOfDraftInvBO();
            listOfDraftInvBO.IvoiceNumber = txtInvoiceNumber.Text.Trim();
            listOfDraftInvBO.PoNumber = txtPONo.Text.Trim();
            listOfDraftInvBO.SupplierId = txtSupplierId.Text.Trim();
            listOfDraftInvBO.ToDate = txtToDate.Text.Trim();
            listOfDraftInvBO.FromDate = txtFromDate.Text.Trim();
            listOfDraftInvBO.UserId = Session["Userid"].ToString();

            DataSet dspopulateGridview = new BLL.ApproverWorkqueueBLL().ApproverWorkqueueGridview(listOfDraftInvBO);
            if (dspopulateGridview.Tables.Count > 0)
            {
                if (dspopulateGridview.Tables[0].Rows.Count > 0)
                {
                    gvApproverWorkqueue.DataSource = dspopulateGridview;
                    ViewState["ApproverTable"] = dspopulateGridview.Tables[0];
                    gvApproverWorkqueue.DataBind();
                    TotalPage();
                    txtPageSize.Text = gvApproverWorkqueue.PageSize.ToString();
                    pagingPanel.Visible = true;
                    recordcount = dspopulateGridview.Tables[0].Rows.Count;
                    hidPageSize.Value = recordcount.ToString();
                   
                }
                else
                {
                    gvApproverWorkqueue.DataSource = null;
                    gvApproverWorkqueue.DataBind();
                    lblNoRecords.Visible = true;
                    lblNoRecords.Text = "No records found";
                }
            }
            else
            {
                gvApproverWorkqueue.DataSource = null;
                gvApproverWorkqueue.DataBind();
                lblNoRecords.Visible = true;
                lblNoRecords.Text = "No records found";
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            if (txtGoToPage.Text != string.Empty && txtGoToPage.Text != "0")
            {
                gvApproverWorkqueue.PageIndex = Convert.ToInt32(txtGoToPage.Text) - 1;
                txtGoToPage.Text = "";
                PopulateApproverGridview();
            }
        }

       
        protected void txtPageSize_TextChanged(object sender, EventArgs e)
        {
            if (txtPageSize.Text != string.Empty && txtPageSize.Text != "0")
            {
                if (Convert.ToInt32(txtPageSize.Text) <= Convert.ToInt32(hidPageSize.Value))
                {

                    int pageSize = Convert.ToInt32(txtPageSize.Text);
                    gvApproverWorkqueue.PageSize = pageSize;
                    PopulateApproverGridview();
                    TotalPage();
                }
                else
                {
                    string display = "Enter Page size less than or equal to " + hidPageSize.Value;
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
                    txtPageSize.Text = string.Empty;


                }
            }
            
        }

        protected void LinkBtnSupplierCodeLookup1_Click(object sender, EventArgs e)
        {
            //Commented by sachin 
            //Response.Redirect("SupplierCodeLookUp.aspx?QS_FlagfromApproverWrkQuePage=Yes");
            //AddedControl by sachin
            MPE.Show();
        }

        //================================================
        //Validation for Dates
        protected void CustValForBothDates1(object sender, ServerValidateEventArgs arg)
        {

            if ((txtFromDate.Text == "" && txtToDate.Text != "") || (txtFromDate.Text != "" && txtToDate.Text == ""))
            {

                arg.IsValid = false;

            }
            else
            {
                arg.IsValid = true;
            }
        }

        protected void ClearAllTextBoxes()
        {
            txtSupplierId.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtPONo.Text = "";
            txtInvoiceNumber.Text = "";

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearAllTextBoxes();
            //lblNoRecords.Visible = false;
            //gvApproverWorkqueue.DataSource = null;
            //pagingPanel.Visible = false;
            //gvApproverWorkqueue.DataBind();
        }
    }
}