using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BO;

namespace VendorPortal
{
    public partial class InvStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Userid"] == null)
            {
                Response.Redirect("LoginPage.aspx");
            }
            AgnToSupCode();
            if (!IsPostBack)
            {
                //Calender will not allow future dates
                CalendarExtenderFromDate.EndDate = DateTime.Now.Date;
                CalendarExtenderToDate.EndDate = DateTime.Now.Date;
                //Date validations
                txtToDate.Attributes.Add("onchange", "ValidateFromDate()");
                txtFromDate.Attributes.Add("onchange", "ValidateTodateGreaterThanFromDate()");
                if (Convert.ToString(Request.QueryString["QS_Scode3"]) == null)
                {
                    PopulateApproverGridview();
                }                
                PopulateDropDownlist();
                AgnToSupCode();

                GetRoleid();
               

            }
            ShowGvColumn();
            DataSet ds = new BLL.SupplierDetailsBLL().GetAllSupplier();
            if (ds.Tables[0].Rows.Count != 0)
            {
                grd_ViewSupplierCodeLookUp.DataSource = ds;
                grd_ViewSupplierCodeLookUp.DataBind();
            } 
            btnGo.Attributes.Add("onclick", "return  ValidatePageValue(" + txtGoToPage.ClientID + "," + this.hidtotalpage.ClientID + ")");
           
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
            string flageFrmCreateInv = Convert.ToString(Request.QueryString["QS_FlagfromCreateInvoicePage"]);
            if (flageFrmCreateInv == "Yes")
            {
                Response.Redirect("~/CreateInvoice.aspx");
            }
        }
        //End supplier code


        private void GetRoleid()
        {
            //int getRoleId = Convert.ToInt32(Session["RoleId"]);
            
            ////if user is approver
            //if (getRoleId == 1)
            //{
            //    //trLstOfDraft.Visible = false;
            //    gvInvoiceStatus.Columns[1].Visible = false;
            //}
            ////if user is supplier
            //else if (getRoleId == 2)
            //{
            //    //trApproverWorkQueue.Visible = false;
            //    gvInvoiceStatus.Columns[0].Visible = false;
            //}
        }
        private void AgnToSupCode()
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

        #region Populate status dropdownlist
        public void PopulateDropDownlist()
        {
            DataSet dsLoadStatus = new BLL.LoadValuesinDropDownlistBLL().LoadValuesinDropDownList("tbl_InvStatus");
            if (dsLoadStatus.Tables[0].Rows.Count != 0)
            {
                ddlStatus.DataSource = dsLoadStatus.Tables[0];
                ddlStatus.DataTextField = "status_description";
                ddlStatus.DataValueField = "status_id";
                ddlStatus.DataBind();
            }
        }
        #endregion

        #region Populate Gridview and Paging

        private void TotalPage()
        {
            int totalPage = 0;
            DataTable dt = (DataTable)ViewState["ApproverTable"];
            if (dt != null)
            {
                int totalRecords = dt.Rows.Count;
                int pageSize = gvInvoiceStatus.PageSize;
                if ((totalRecords % pageSize) == 0)
                    totalPage = (totalRecords) / (pageSize);
                else
                    totalPage = ((totalRecords) / (pageSize)) + 1;
                lblTotalPage.Text = totalPage.ToString();
                hidtotalpage.Value = lblTotalPage.Text;
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PopulateApproverGridview();
            //ClearAllTextBoxes();

        }

        private void PopulateApproverGridview()
        {
            int recordcount = 0;
            pagingPanel.Visible = false;
            lblNoRecords.Visible = false;
            ListOfDraftInvBO listOfDraftInvBO = new BO.ListOfDraftInvBO();
            listOfDraftInvBO.IvoiceNumber = txtInvoiceNumber.Text.Trim();
            listOfDraftInvBO.PoNumber = txtPONo.Text.Trim();
            listOfDraftInvBO.SupplierId = txtSupplierId.Text.Trim();
            listOfDraftInvBO.ToDate = txtToDate.Text.Trim();
            listOfDraftInvBO.FromDate = txtFromDate.Text.Trim();
            if (Convert.ToString(ddlStatus.SelectedItem) == "Select")
            {
                listOfDraftInvBO.Status = "Select"; //string.Empty;
            }
            else
            {
                //listOfDraftInvBO.Status = ddlStatus.SelectedValue;
                listOfDraftInvBO.Status = Convert.ToString(ddlStatus.SelectedItem);

            }
            listOfDraftInvBO.UserId = Session["Userid"].ToString();

            DataSet dspopulateGridview = new BLL.InvoiceStatusBLL().InvoiceStatusGridview(listOfDraftInvBO);
            if (dspopulateGridview.Tables.Count > 0)
            {
                if (dspopulateGridview.Tables[0].Rows.Count > 0)
                {
                    gvInvoiceStatus.DataSource = dspopulateGridview;
                    ViewState["ApproverTable"] = dspopulateGridview.Tables[0];
                    gvInvoiceStatus.DataBind();
                    TotalPage();
                    txtPageSize.Text = gvInvoiceStatus.PageSize.ToString();
                    pagingPanel.Visible = true;
                    recordcount = dspopulateGridview.Tables[0].Rows.Count;
                    hidPageSize.Value = recordcount.ToString();
                   
                }
                else
                {
                    gvInvoiceStatus.DataSource = null;
                    gvInvoiceStatus.DataBind();
                    lblNoRecords.Visible = true;
                    lblNoRecords.Text = "No records found";
                }
            }
            else
            {
                gvInvoiceStatus.DataSource = null;
                gvInvoiceStatus.DataBind();
                lblNoRecords.Visible = true;
                lblNoRecords.Text = "No records found";
            }
        }

        protected void gvInvoiceStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvInvoiceStatus.PageIndex = e.NewPageIndex;
            PopulateApproverGridview();
        }

        #endregion

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void txtPageSize_TextChanged(object sender, EventArgs e)
        {
            if (txtPageSize.Text != string.Empty && txtPageSize.Text != "0")
            {
                if (Convert.ToInt32(txtPageSize.Text) <= Convert.ToInt32(hidPageSize.Value))
                {
                    int pageSize = Convert.ToInt32(txtPageSize.Text);
                    gvInvoiceStatus.PageSize = pageSize;
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

        protected void btnGo_Click(object sender, EventArgs e)
        {
            if (txtGoToPage.Text != string.Empty && txtGoToPage.Text != "0")
            {
                gvInvoiceStatus.PageIndex = Convert.ToInt32(txtGoToPage.Text) - 1;
                txtGoToPage.Text = "";
                PopulateApproverGridview();
            }
        }

        protected void LinkBtnSupplierCodeLookup2_Click(object sender, EventArgs e)
        {
            //Response.Redirect("SupplierCodeLookUp.aspx?QS_FlagfromInvStatusPage=Yes");
            MPE.Show();
        }
        //================================================
        //Validation for Dates
        protected void CustValForBothDates2(object sender, ServerValidateEventArgs arg)
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

        private void ClearAllTextBoxes()
        {
            txtSupplierId.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtPONo.Text = "";
            txtInvoiceNumber.Text = "";
            ddlStatus.SelectedIndex = 0;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearAllTextBoxes();
            //lblNoRecords.Visible = false;
            //gvInvoiceStatus.DataSource = null;
            //pagingPanel.Visible = false;
            //gvInvoiceStatus.DataBind();
        }
    }
}
