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
    public partial class Listdr : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Userid"] == null)
            {
                Response.Redirect("LoginPage.aspx");
            }
            AssignToSupplierCode();
            if (!IsPostBack)
            {
                AssignToSupplierCode();

                //Calender will not display future dates
                CalendarExtenderFromDate.EndDate = DateTime.Now.Date;
                CalendarExtenderToDate.EndDate = DateTime.Now.Date;
                //Date validations
                txtToDate.Attributes.Add("onchange", "ValidateFromDate()");
                txtFromDate.Attributes.Add("onchange", "ValidateTodateGreaterThanFromDate()");
                if (Convert.ToString(Request.QueryString["QS_Scode1"]) == null)
                {
                    PopulateListofDraftGridview();
                }
               

            }
           

            ShowGvColumn();
            DataSet ds = new BLL.SupplierDetailsBLL().GetAllSupplier();
            if (ds.Tables[0].Rows.Count != 0)
            {
                grd_ListDraftLookUp.DataSource = ds;
                grd_ListDraftLookUp.DataBind();
            }  
            btnGo.Attributes.Add("onclick", "return  ValidatePageValue(" + txtGoToPage.ClientID + "," + this.hidtotalpage.ClientID + ")");
            //txtPageSize.Attributes.Add("onchange", "return ValidatePageSize(" + txtPageSize.ClientID + "," + this.hidPageSize.ClientID + ")");
           
        }

        //Added supplier code changes
        private void ShowGvColumn()
        {
            DataSet ds = new BLL.SupplierDetailsBLL().GetAllSupplier();
            if (ds.Tables[0].Rows.Count != 0)
            {
                grd_ListDraftLookUp.DataSource = ds;
                grd_ListDraftLookUp.DataBind();
            }
            grd_ListDraftLookUp.Columns[0].Visible = true;
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
           
                Response.Redirect("~/Listdr.aspx");
            
        }
        //End supplier code

        private void AssignToSupplierCode()
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
            DataTable dt = (DataTable)ViewState["ListofDraftTable"];
            if (dt != null)
            {
                int totalRecords = dt.Rows.Count;
                int pageSize = gvListofDraft.PageSize;
                if ((totalRecords % pageSize) == 0)
                    totalPage = (totalRecords) / (pageSize);
                else
                    totalPage = ((totalRecords) / (pageSize)) + 1;
                lblTotalPage.Text = totalPage.ToString();
                hidtotalpage.Value = lblTotalPage.Text;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                PopulateListofDraftGridview();
                //ClearAllTextBoxes();
            }
            else
            {
                lblNoRecords.Visible = false;
                gvListofDraft.DataSource = null;
                pagingPanel.Visible = false;
                gvListofDraft.DataBind();
            }

        }

        private void PopulateListofDraftGridview()
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
            listOfDraftInvBO.UserId = Session["Userid"].ToString();

            DataSet dspopulateGridview = new BLL.ListOfDraftInvoiceBLL().ListOfDraftInv(listOfDraftInvBO);
            
            if (dspopulateGridview.Tables.Count > 0)
            {
                if (dspopulateGridview.Tables[0].Rows.Count > 0)
                {

                    gvListofDraft.DataSource = dspopulateGridview;
                    ViewState["ListofDraftTable"] = dspopulateGridview.Tables[0];
                    gvListofDraft.DataBind();
                    TotalPage();
                    txtPageSize.Text = gvListofDraft.PageSize.ToString();
                    pagingPanel.Visible = true;
                     recordcount = dspopulateGridview.Tables[0].Rows.Count;
                     hidPageSize.Value = recordcount.ToString();
                   
                   
                }
                else
                {
                    gvListofDraft.DataSource = null;
                    gvListofDraft.DataBind();
                    lblNoRecords.Visible = true;
                    lblNoRecords.Text = "No records found";
                }
            }
            else
            {
                gvListofDraft.DataSource = null;
                gvListofDraft.DataBind();
                lblNoRecords.Visible = true;
                lblNoRecords.Text = "No records found";
            }
            
           
            
        }

        protected void gvListofDraft_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvListofDraft.PageIndex = e.NewPageIndex;
            PopulateListofDraftGridview();
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            if (txtGoToPage.Text != string.Empty && txtGoToPage.Text != "0")
            {
                gvListofDraft.PageIndex = Convert.ToInt32(txtGoToPage.Text) - 1;
                txtGoToPage.Text = "";
                PopulateListofDraftGridview();
            }
        }

        protected void txtPageSize_TextChanged(object sender, EventArgs e)
        {
            if (txtPageSize.Text != string.Empty && txtPageSize.Text!="0")
            {
                if (Convert.ToInt32(txtPageSize.Text) <= Convert.ToInt32(hidPageSize.Value))
                {
                    int pageSize = Convert.ToInt32(txtPageSize.Text);
                    gvListofDraft.PageSize = pageSize;
                    PopulateListofDraftGridview();
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

        protected void LinkBtnSupplierCodeLookup_Click(object sender, EventArgs e)
        {
            MPEListDr.Show();
        }

        //================================================
        //Validation for Dates
        //protected void CustValForFromDate(object sender, ServerValidateEventArgs arg)
        //{
        //    if (txtFromDate.Text == string.Empty && txtToDate.Text != string.Empty)
        //    {
        //        arg.IsValid = false;
               
        //    }
        //    else
        //    {
        //        arg.IsValid = true;
        //    }
           
        //}
        

      
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
            //gvListofDraft.DataSource = null;
            //pagingPanel.Visible = false;
            //gvListofDraft.DataBind();
        }


    }
}
