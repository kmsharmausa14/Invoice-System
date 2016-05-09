using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VendorPortal
{
	public partial class SupplierCodeLookUp : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (Session["Userid"] == null)
            {
                Response.Redirect("LoginPage.aspx");
            }
            if (!IsPostBack)
            {

                ShowGvColumn();
                DataSet ds = new BLL.SupplierDetailsBLL().GetAllSupplier();
                if (ds.Tables[0].Rows.Count != 0)
                {
                    grd_ViewSupplierCodeLookUp.DataSource = ds;
                    grd_ViewSupplierCodeLookUp.DataBind();
                }   

            }

		}

        //Initially columns will be hide but we will change the status to Visible depends on source page
        private void ShowGvColumn()
        {
            
            string flageFromCreateInv = Convert.ToString(Request.QueryString["QS_FlagfromCreateInvoicePage"]);

            string flageFromLstOfDraft = Convert.ToString(Request.QueryString["QS_FlagfromListOfDraftPage"]);

            string flageFromApproverWrkQue = Convert.ToString(Request.QueryString["QS_FlagfromApproverWrkQuePage"]);

            string flageFromInvStatus = Convert.ToString(Request.QueryString["QS_FlagfromInvStatusPage"]);


            
            if (flageFromCreateInv == "Yes")
            {
                grd_ViewSupplierCodeLookUp.Columns[0].Visible = true;
            }
            else if (flageFromLstOfDraft == "Yes")
            {
                grd_ViewSupplierCodeLookUp.Columns[1].Visible = true;
            }
            else if (flageFromApproverWrkQue == "Yes")
            {
                grd_ViewSupplierCodeLookUp.Columns[2].Visible = true;
            }
            else if (flageFromInvStatus == "Yes")
            {
                grd_ViewSupplierCodeLookUp.Columns[3].Visible = true;
            }
           
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            //Response.Redirect("CreateInvoice.aspx");
            //Response.Redirect("~/Home.aspx");

            string flageFrmCreateInv = Convert.ToString(Request.QueryString["QS_FlagfromCreateInvoicePage"]);
            string flageFrmLstOfDraft = Convert.ToString(Request.QueryString["QS_FlagfromListOfDraftPage"]);
            string flageFrmApproverWrkQue = Convert.ToString(Request.QueryString["QS_FlagfromApproverWrkQuePage"]);
            string flageFrmInvStatus = Convert.ToString(Request.QueryString["QS_FlagfromInvStatusPage"]);
            if (flageFrmCreateInv=="Yes")
            {
                Response.Redirect("~/CreateInvoice.aspx");
            }
            else if (flageFrmLstOfDraft == "Yes")
            {
                Response.Redirect("~/Listdr.aspx?QS_Scode1=abc");
            }
            else if (flageFrmApproverWrkQue == "Yes")
            {
                Response.Redirect("~/Approver_WrkQue.aspx?QS_Scode2=abc"); 
            }
            else if (flageFrmInvStatus == "Yes")
            {
                Response.Redirect("~/InvStatus.aspx?QS_Scode3=abc");

            }
        }
	}
}