using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using BO;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace VendorPortal
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
           
            if (Session["Userid"] == null)
            {
                Response.Redirect("~/LoginPage.aspx");
            }
            else
            {
                this.lblUserName.Text = Session["Userid"].ToString();
            }
            lbl_userid.Text = Session["Userid"].ToString();
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Session.Abandon();           
           
            Response.Redirect("LoginPage.aspx");
        }
         
        protected void btn_ChangePword_Click(object sender, EventArgs e)
        {
            if (txt_CurrentPassword.Text != string.Empty && txt_NewPassword.Text != string.Empty && txt_ConfirmPassword.Text != string.Empty)
            {
                string entered = txt_CurrentPassword.Text.Trim();
                    ArrayList lstParam = new System.Collections.ArrayList();
                    SqlParameter param;
                    param = new SqlParameter();
                    param.ParameterName = "@userid";
                    param.DbType = DbType.String;
                    param.Value = Convert.ToString(Session["Userid"]);
                    lstParam.Add(param);
                    string oldpwd = (string)new DAL.SqlHelper().SelectValue("SELECT password FROM tbl_user where user_id = @userid", lstParam,true);
                    if (entered == oldpwd)
                    {
                        LoginDetailsBLL logDtlObj = new LoginDetailsBLL();
                        LoginBO objLogin = new LoginBO();
                        objLogin.UserId = (string)Session["Userid"];
                        objLogin.Password = txt_ConfirmPassword.Text;
                        logDtlObj.ChangeResetedPassword(objLogin);
                        string display = "Password Changed Successfully";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
                    }
                    else 
                    {
                        string display = "Old  Password is incorrect";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + display + "');", true);
                    }
                
            }
            //btn_ChangePword.Attributes.Add("", "javascript:alert('Password Changed successfully')");
            
           //(this.GetType(), "myalert", "alert('" + display + "');", true);
        }

        //protected void LinkButtonContactUs_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("#");
        //}

        //protected void LinkButtonHelp_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("#");
        //}
    }
}
