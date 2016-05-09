using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VendorPortal
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {   
            if (!IsPostBack)
            {
                if (Session["Userid"] == null)
                {
                    Response.Redirect("LoginPage.aspx");
                }

                int getRoleId = Convert.ToInt32(Session["RoleId"]);
                //if user is approver
                if (getRoleId == 1)
                {
                    trLstOfDraft.Visible = false;
                    trReport.Visible = true;
                }
                //if user is supplier
                else if (getRoleId == 2)
                {
                    trApproverWorkQueue.Visible = false;
                    trReport.Visible = false;
                }
                //Adeded by sachin
                else if (getRoleId == 3)
                {
                    trReport.Visible = false;
                }
            }           
        }

        //protected void LinkButton1_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("CreateInvoice.aspx");
        //}

        //protected void LinkButton2_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("Listdr.aspx");
        //}

        //protected void LinkButton3_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("AP_WrkQue.aspx");
        //}

        //protected void LinkButton4_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("Approver_WrkQue.aspx");
        //}

        //protected void LinkButton5_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("InvStatus.aspx");
        //}



    }

}
