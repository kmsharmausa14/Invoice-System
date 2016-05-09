using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using BO;
using BLL;
using DAL;


namespace VendorPortal
{
    public partial class LoginPage : System.Web.UI.Page
    {
        LoginBO objLogin = new LoginBO();
        SupplierDetailsBO objSupDtl = new SupplierDetailsBO();

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnLogin_Click1(object sender, EventArgs e)
        {
            //lblErrorMessage.Text = string.Empty;
            CollectData();
            int CountUserFromResetPasswordTable = new BLL.LoginDetailsBLL().CheckUserFromResetPasswordTable(objLogin);

            if (CountUserFromResetPasswordTable >= 1)
            {
                lblmsg.Text = "Please change your password";
                txtUserID.ReadOnly = true;
                //Logintd.Visible = false;
                Logintd.Visible = false;
                panelChangePassword.Visible = true;
            }
            else
            {

                int CountUser = new BLL.LoginDetailsBLL().CheckLoginDetails(objLogin);
                if (CountUser >= 1)
                {
                    string userId = objLogin.UserId;
                    Session["Userid"] = userId;

                    Session["serverpath"] = Server.MapPath("~");
                    int roleid = new BLL.LoginDetailsBLL().GetRoleIdByUserId(userId);
                    Session["RoleId"] = roleid;
                    if (roleid == 3)
                    {
                        Response.Redirect("~/Admin.aspx");
                    }
                    else if (roleid == 1)
                    {
                        Response.Redirect("~/Home.aspx");
                    }
                    else
                    {
                        Session["SupplierCode"] = new BLL.LoginDetailsBLL().GetSupplierCode(objLogin);
                        Session["SupDetails"] = getSupplierDetails();
                        Response.Redirect("~/Home.aspx");
                    }



                    //Server.Transfer("~/Home.aspx");
                }
                else
                {
                    lblErrorMessage.Text = "Your username or password is incorrect";                
                }
            }

        }

        private SupplierDetailsBO getSupplierDetails()
        {
            DataSet dsSupplierDtl = new DataSet();
            string getSupplierCod = Session["SupplierCode"].ToString();
            if (getSupplierCod != null)
            {
                dsSupplierDtl = new BLL.SupplierDetailsBLL().GetSupplierDetailsById(getSupplierCod);
                objSupDtl.Code = dsSupplierDtl.Tables[0].Rows[0][0].ToString();
                objSupDtl.Name = dsSupplierDtl.Tables[0].Rows[0][1].ToString();
                objSupDtl.Address1 = dsSupplierDtl.Tables[0].Rows[0][2].ToString();
                objSupDtl.Address2 = dsSupplierDtl.Tables[0].Rows[0][3].ToString();
                objSupDtl.Address3 = dsSupplierDtl.Tables[0].Rows[0][4].ToString();
                objSupDtl.Street = dsSupplierDtl.Tables[0].Rows[0][5].ToString();
                objSupDtl.City = dsSupplierDtl.Tables[0].Rows[0][6].ToString();
                objSupDtl.State = dsSupplierDtl.Tables[0].Rows[0][7].ToString();
                objSupDtl.Country = dsSupplierDtl.Tables[0].Rows[0][8].ToString();
                objSupDtl.PostCode = dsSupplierDtl.Tables[0].Rows[0][9].ToString();
                objSupDtl.Tele = dsSupplierDtl.Tables[0].Rows[0][10].ToString();
                objSupDtl.Fax = dsSupplierDtl.Tables[0].Rows[0][11].ToString();
                objSupDtl.EmailId = dsSupplierDtl.Tables[0].Rows[0][12].ToString();
                objSupDtl.Currency = dsSupplierDtl.Tables[0].Rows[0][13].ToString();

            }

            return objSupDtl;
            //return abc;
        }

        private void CollectData()
        {
            objLogin.UserId = txtUserID.Text.Trim();
            objLogin.Password = txtPassword.Text.Trim();
        }

        //protected void LnkBtnForgortPwd_Click(object sender, EventArgs e)
        //{


        //    ///
        //}

        protected void btnChangePword_Click(object sender, EventArgs e)
        {
            //LoginDetailsBLL ldObj = new LoginDetailsBLL();
            LoginDetailsBLL logDtlObj = new LoginDetailsBLL();
            objLogin.UserId = txtUserID.Text;
            objLogin.Password = txtNewPassword.Text; ;


            if (txtUserID.Text != string.Empty && txtNewPassword.Text != string.Empty && txtCurrentPassword.Text != string.Empty && txtConfirmPassword.Text != string.Empty)
            {
                if (txtNewPassword.Text != txtCurrentPassword.Text)
                {
                    logDtlObj.ChangeResetedPassword(objLogin);
                    lblmsg.Text = "Password has been Changed Sucessfully";
                    Logintd.Visible = true;
                    panelChangePassword.Visible = false;
                    txtUserID.ReadOnly = false;
                }
                else
                {
                    lblmsg.Text = "New Password cannot be same as Old Password";
                }
            }
            else
            {
                lblmsg.Text = "Please enter Username or New Password or Current Password or Confirm Password";
            }


        }

        protected void btnCanel_Click(object sender, EventArgs e)
        {
            lblmsg.Text = string.Empty;
            Logintd.Visible = true;
            panelChangePassword.Visible = false;
        }

        protected void btnsendemail_Click(object sender, EventArgs e)
        {
            //Check wheter useris is valid.
            string userid = txt_forgotuserid.Text.Trim();
            int checkuser = new BLL.LoginDetailsBLL().checkuser(userid);
            if (checkuser == 1)
            {
                string emailid = new BLL.LoginDetailsBLL().getEmaild(userid);
                string password = new BLL.LoginDetailsBLL().getpassword(userid);
                if (emailid != string.Empty || emailid != null)
                {
                    LoginDetailsBLL logDtlObj = new LoginDetailsBLL();
                    LoginBO objLogin = new LoginBO();
                    objLogin.UserId = userid;
                    //objLogin.Password = password;
                    logDtlObj.Forgotpassword(objLogin);
                    string newpassword = new BLL.LoginDetailsBLL().getpassword(userid);
                    UserEmailBO userEmailBO = new UserEmailBO();
                    userEmailBO.UserName = userid;
                    userEmailBO.Password = newpassword;
                    userEmailBO.EmailTo = emailid;

                    new Utitlity.EmailHelper().SendEmail(Utitlity.EmailType.EmailForPasswordchange, userEmailBO);

                    string display = "PASSWORD Successfully Sent On Your Email";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
                }
                else
                {
                    string display = "User" + userid + " does not  Exists or Email Id is not registered";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
                }
                // update the DB by default password.
                //Get email id of userid.

                // Email Notificatio
            }
            else
            {
                string display = "User" + userid + " does not  Exists";
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
            }


        }

        protected void LnkBtnForgortPwd_Click(object sender, EventArgs e)
        {
            txt_forgotuserid.Text = txtUserID.Text.ToString();
        }
    }
}
