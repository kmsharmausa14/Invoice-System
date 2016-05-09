using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VendorPortal
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //DataTable dt = new DataTable();
            //DataSet ds=new 
            if (!IsPostBack)
            {
                DataSet ds = new BLL.UserProfileBLL().Report1();
                DataSet ds1 = new BLL.UserProfileBLL().Report2();
                DataSet ds2 = new BLL.UserProfileBLL().Report3();
               
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                
                DataTable dtTAT = new DataTable();
                dtTAT = ds1.Tables[0];

                DataTable dtSupplier = new DataTable();
                dtSupplier = ds2.Tables[0];

                gvwDayWiseRep.DataSource = dt;
                gvwDayWiseRep.DataBind();

                gvwTAT.DataSource = dtTAT;
                gvwTAT.DataBind();

                //gvwSupplier.DataSource = dtSupplier;
                //gvwSupplier.DataBind();

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }



    }
}