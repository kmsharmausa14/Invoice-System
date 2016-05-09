using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using BO;
using BLL;
using System.Data;
using System.Data.SqlClient;

namespace VendorPortal
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup

            string attachmentpath = Server.MapPath("~") + @"\Attachments";
            if (!System.IO.Directory.Exists(attachmentpath))
            {
                System.IO.Directory.CreateDirectory(attachmentpath);
            }

        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

            if (Session["Userid"] != null)
            {
                BLL.AttachmentBLLcs obj1 = new AttachmentBLLcs();
                AttachmentBO attachbo = new AttachmentBO();
                string userId = Session["Userid"].ToString();
                attachbo.Userid = userId;
                DataSet dt = obj1.GetAttachmentInfo(attachbo);

                AttachmentBO attachbo1 = new AttachmentBO();
                attachbo1.Userid = userId;
                string filename = string.Empty;
                string attachid = string.Empty;

                if (dt.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                    {
                        attachid = dt.Tables[0].Rows[i][0].ToString();
                        filename = dt.Tables[0].Rows[i][1].ToString();

                        attachbo1.AttachmentId = Convert.ToInt32(attachid);
                        obj1.SendDeletedAttachmentInfo(attachbo1);

                        if (Session["serverpath"] != null)
                        {
                            string filepath = Session["serverpath"].ToString() + @"/Attachments/" + filename;

                            if (System.IO.File.Exists(filepath))
                            {
                                System.IO.File.Delete(filepath);
                            }
                        }
                    }

                }
            }

        }

    }
}
