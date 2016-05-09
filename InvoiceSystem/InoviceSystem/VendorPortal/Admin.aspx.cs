using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using Utitlity;

namespace VendorPortal
{
    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txt_userid.Enabled = false;
                txtname.Enabled = false;
                ddl_supplier.Enabled = false;
                txt_email.Enabled = false;
                txtcontact.Enabled = false;
                alertid.Visible = false;
                ddl_usertype.Enabled = false;
                btn_usersearch.Enabled = false;
                lblnamestar.Visible = false;
                lblemailstar.Visible = false;
                lblcontactstar.Visible = false;
                lbl_suppstar.Visible = false;
                // btn_create.CausesValidation = true;
                DataSet scode = new BLL.AdminBLL().Gets_code();
                if (scode.Tables.Count > 0)
                {
                    if (scode.Tables[0].Rows.Count > 0)
                    {
                        ddl_supplier.DataSource = scode.Tables[0];
                        ddl_supplier.DataTextField = "s_code";
                        ddl_supplier.DataValueField = "s_code";
                        ddl_supplier.DataBind();
                    }
                }

            }
            if (ddl_addrole.SelectedValue == "2")
            {
                ddl_supplier.Enabled = true;
                txtname.Enabled = false;
                txt_email.Enabled = false;
                txtcontact.Enabled = false;
                lbl_suppstar.Visible = true;
                lblnamestar.Visible = false;
                lblemailstar.Visible = false;
                lblcontactstar.Visible = false;
                regexEmailValid.Enabled = false;
                RequiredFieldValidatorUserID.Enabled = false;
                //  btn_create.CausesValidation = false;
            }
            else if (ddl_addrole.SelectedValue == "1" || ddl_addrole.SelectedValue == "3")
            {
                txtname.Enabled = true;
                txt_email.Enabled = true;
                txtcontact.Enabled = true;
                ddl_supplier.Enabled = false;
                lblnamestar.Visible = true;
                lblemailstar.Visible = true;
                lblcontactstar.Visible = true;
                lbl_suppstar.Visible = false;
                regexEmailValid.Enabled = true;
                RequiredFieldValidatorUserID.Enabled = false;
                // btn_create.CausesValidation = true;
            }
            //td_supplier.Visible = false ;
            if (IsPostBack)
            {
               
                //if(rad_userid.Checked == true || rad_usertype.Checked == true || rad_showall.Checked == true)
                // {
                //onloadpopulatedata();
                // }
            }
            btn_usersearch.Attributes.Add("onclick", "return ValidateTextUserID()");
            //btn_create.Attributes.Add("onclick", "return ValidateContactUserIDAdmin()");
            //btn_create.Attributes.Add("onclick", "return ValidateUsernameTextboxAdmin()");
            //btn_create.Attributes.Add("onclick", "return ValidateUserIDTextbox(" + txtuser.ClientID + ")");
            btn_create.Attributes.Add("onclick", "return ValidateUserIDTextbox(" + txtuser.ClientID + "," + txtname.ClientID + " ," + txtcontact.ClientID + ")");
        }

        protected void onloadpopulatedata()
        {

            gv_userdtls.DataSource = null;
            gv_userdtls.DataBind();
            //lbl_griderror.Text = string.Empty;
            DataTable user_table = new DataTable();
            string parameter = string.Empty;
            int criteria = 0;
            if (rad_userid.Checked == true)
            {
                parameter = txt_userid.Text.ToString();
                criteria = 1;

            }
            else if (rad_usertype.Checked == true)
            {
                parameter = ddl_usertype.SelectedItem.Text.ToString();
                criteria = 2;
            }
            else if (rad_showall.Checked == true)
            {
                parameter = string.Empty;
                criteria = 0;
            }
            DataSet userdtls = new BLL.AdminBLL().GetUserDetails(parameter, criteria);//string.Empty to pe replaced by Parameter after SP is done with
            if (userdtls != null)
            {
                if (userdtls.Tables.Count > 0)
                {
                    if (userdtls.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i <= userdtls.Tables[0].Rows.Count - 1; i++)
                        {
                            user_table = userdtls.Tables[0];
                            if (user_table != null)
                            {
                                if (user_table.Rows.Count > 0)
                                {
                                    gv_userdtls.DataSource = user_table;
                                    ViewState["UserDetails"] = user_table;
                                    gv_userdtls.DataBind();
                                }
                                else
                                {
                                    //lbl_griderror.Text = "Sorry!!! No Records Found";
                                }
                            }
                            else
                            {
                                // lbl_griderror.Text = "Sorry!!! No Records Found";
                            }

                        }
                    }
                    else
                    {
                        //lbl_griderror.Text = "Sorry!!! No Records Found";
                    }
                }
                else
                {
                    // lbl_griderror.Text = "Sorry!!! No Records Found";
                }
            }
            else
            {
                // lbl_griderror.Text = "Sorry!!! No Records Found";
            }
        }

        protected void gv_userdtls_deleting(object sender, GridViewDeleteEventArgs e)
        {
            int pageindex = gv_userdtls.PageIndex;
            int rowindex = 0;
            if (pageindex == 0)
            {
                rowindex = e.RowIndex;
            }
            //if (pageindex == 1)
            //{
            //    rowindex = gv_userdtls.PageSize + (e.RowIndex + 1);
            //}
            if (pageindex >= 1)
            {
                rowindex = (gv_userdtls.PageSize * (pageindex)) + (e.RowIndex);
            }

            TableCell cell = gv_userdtls.Rows[e.RowIndex].Cells[0];
            string UserID = cell.Text.ToString();
            DataTable user_table = (DataTable)ViewState["UserDetails"];
            int index = rowindex;
            string userid = user_table.Rows[rowindex].ItemArray[0].ToString();
            int getvalue = new BLL.AdminBLL().IfExists(userid);
            // if (getvalue == 1)

            bool status = new BLL.AdminBLL().DeleteUser(userid);
            if (status == true)
            {
                onloadpopulatedata();
                string display = "User" + " " + userid + " is deleted";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
            }
            else
            {
                string display = "User" + " " + userid + " is not deleted due to some Error";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
            }
            //}
            // onloadpopulatedata();
        }

        protected void rad_showall_CheckedChanged(object sender, EventArgs e)
        {
            // lbl_griderror.Text = string.Empty;
            onloadpopulatedata();
            txt_userid.Enabled = false;
            btn_usersearch.Enabled = false;
            ddl_usertype.Enabled = false;
            ddl_usertype.SelectedValue = "0";
            txt_userid.Text = string.Empty;
        }

        protected void btn_create_Click(object sender, EventArgs e)
        {
            if (!(string.IsNullOrEmpty(txtuser.Text)) && ddl_addrole.SelectedValue != "0")
            {

                //modalPopupGraph.Hide();
                string userid = txtuser.Text.Trim();
                string username = txtname.Text.Trim();
                int roleid = Convert.ToInt32(ddl_addrole.SelectedValue.ToString());
                string s_code = ddl_supplier.SelectedItem.Text.ToString();//txt_supplier.Text.ToString();
                string contact = txtcontact.Text.Trim();
                string email = txt_email.Text.ToString();
                int success = 0;
                if (ddl_supplier.Enabled == false)
                {
                    success = new BLL.AdminBLL().AddUser(userid, roleid, email, username, contact);
                }
                if (ddl_supplier.Enabled == true)
                {
                    success = new BLL.AdminBLL().AddUserSupplier(userid, roleid, s_code);
                }

                if (success == 1)
                {
                    string display = "User" + " " + userid + " " + " added successfully.The Password has been sent to uour Email";

                    //email notification
                    string newpassword = new BLL.LoginDetailsBLL().getpassword(userid);
                    BO.UserEmailBO userbo = new BO.UserEmailBO();
                    userbo.UserName = userid;
                    userbo.Password = newpassword;
                    userbo.EmailTo = email;
                    string display2 = "User successfully added";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display2 + "');", true);
                    try
                    {
                        // new Utitlity.EmailHelper().SendEmail(EmailType.EmailForNewUser, userbo);
                    }
                    catch (Exception err)
                    {

                        // ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert(' Email not sent due some error as Email ID is Invalid');", true);
                    }

                    // ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
                    
                    clearall();
                }
                else if (success == -1)
                {

                    string display = "User" + " " + userid + " " + " already Exists";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
                    txtuser.Text = string.Empty;
                    ddl_addrole.SelectedValue = "0";
                    txt_email.Text = string.Empty;
                    //txt_supplier.Text = string.Empty;
                    //modalPopupGraph.Show();
                    clearall();
                }
            }
            else
            {
                string display = "Enter User Id and Role";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
                //modalPopupGraph.Show();
            }
        }

        protected void ddl_usertype_SelectedIndexChanged(object sender, EventArgs e)
        {
            // lbl_griderror.Text = string.Empty;
            onloadpopulatedata();
        }

        protected void rad_userid_CheckedChanged1(object sender, EventArgs e)
        {
            //lbl_griderror.Text = string.Empty;
            txt_userid.Enabled = true;
            alertid.Visible = true;
            txt_userid.Text = string.Empty;
            btn_usersearch.Enabled = true;
            ddl_usertype.Enabled = false;
            ddl_usertype.SelectedValue = "0";
            DataTable blank = new DataTable();
            gv_userdtls.DataSource = null;
            gv_userdtls.DataBind();
        }

        protected void rad_usertype_CheckedChanged1(object sender, EventArgs e)
        {
            ddl_usertype.Enabled = true;
            txt_userid.Enabled = false;
            btn_usersearch.Enabled = false;
            txt_userid.Text = string.Empty;
            DataTable blank = new DataTable();
            // gv_userdtls.DataSource = blank;
            //gv_userdtls.DataBind();
        }

        protected void btn_usersearch_Click(object sender, EventArgs e)
        {
            // lbl_griderror.Text = string.Empty;
            onloadpopulatedata();
            ddl_usertype.SelectedValue = "0";
        }

        protected void ddl_addrole_SelectedIndexChanged(object sender, EventArgs e)
        {
            //clearall();
            if (ddl_addrole.SelectedValue == "2")
            {
                ddl_supplier.Enabled = true;
                txtname.Enabled = false;
                txt_email.Enabled = false;
                txtcontact.Enabled = false;
                lbl_suppstar.Visible = true;
                regexEmailValid.Enabled = false;
                RequiredFieldValidatorUserID.Enabled = false;
                //btn_create.CausesValidation = false;
                //ddl_addrole.SelectedValue = "0";
                txtuser.Text = string.Empty;
                txtname.Text = string.Empty;
                ddl_supplier.ClearSelection();
                txt_email.Text = string.Empty;
                txtcontact.Text = string.Empty;
                lblnamestar.Visible = false;
                lblemailstar.Visible = false;
                lblcontactstar.Visible = false;
                lbl_suppstar.Visible = false;
            }
            else
            {
                txtname.Enabled = true;
                txt_email.Enabled = true;
                txtcontact.Enabled = true;
                ddl_supplier.Enabled = false;
                lblnamestar.Visible = true;
                lblemailstar.Visible = true;
                lblcontactstar.Visible = true;
                regexEmailValid.Enabled = true;
                RequiredFieldValidatorUserID.Enabled = false;
                // btn_create.CausesValidation = true;
                //ddl_addrole.SelectedValue = "0";
                txtuser.Text = string.Empty;
                txtname.Text = string.Empty;
                ddl_supplier.ClearSelection();
                txt_email.Text = string.Empty;
                txtcontact.Text = string.Empty;
                lblnamestar.Visible = false;
                lblemailstar.Visible = false;
                lblcontactstar.Visible = false;
                lbl_suppstar.Visible = false;
            }
            //modalPopupGraph.Show();
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            clearall();
        }

        protected void btnClearDeleteSection_Click(object sender, EventArgs e)
        {
            ClearControlDeleteSection();
        }

        public void ClearControlDeleteSection()
        {
            rad_userid.Controls.Clear();
            txt_userid.Text = "";
            ddl_usertype.SelectedValue = "0";
            rad_usertype.Controls.Clear();
            rad_showall.Controls.Clear();
            gv_userdtls.DataSource = null;
            gv_userdtls.DataBind();
            //gv_userdtls.Visible = false;
                        
        }

        public void clearall()
        {
            ddl_addrole.SelectedValue = "0";
            txtuser.Text = string.Empty;
            txtname.Text = string.Empty;
            ddl_supplier.ClearSelection();
            txt_email.Text = string.Empty;
            txtcontact.Text = string.Empty;
            lblnamestar.Visible = false;
            lblemailstar.Visible = false;
            lblcontactstar.Visible = false;
            lbl_suppstar.Visible = false;
        }


        protected void gvuserdtls_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_userdtls.PageIndex = e.NewPageIndex;
            onloadpopulatedata();
        }

        protected void ddl_supplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _strsuppliercode = ddl_supplier.SelectedItem.Text;
            DataSet dssupplierdetails = new BLL.SupplierDetailsBLL().GetSupplierDetailsById(_strsuppliercode);
            if (dssupplierdetails != null && dssupplierdetails.Tables.Count > 0)
            {
                if (dssupplierdetails.Tables[0].Rows.Count > 0)
                {
                    txtname.Text = dssupplierdetails.Tables[0].Rows[0].ItemArray[1].ToString();
                    txt_email.Text = dssupplierdetails.Tables[0].Rows[0].ItemArray[12].ToString();
                    txtcontact.Text = dssupplierdetails.Tables[0].Rows[0].ItemArray[10].ToString();
                }

            }
        }

    }
}