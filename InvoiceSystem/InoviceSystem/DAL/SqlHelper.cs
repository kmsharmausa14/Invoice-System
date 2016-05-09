using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
using System.Configuration;

namespace DAL
{
    public class SqlHelper
    {
        //Return Connection String 
        string _connStr = string.Empty;
        private string connString()
        {


            try
            {


                //byte[] byt = Convert.FromBase64String(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DAL.Properties.Settings.InvoiceSystemConnection"].ConnectionString);

                //string connString = System.Text.ASCIIEncoding.ASCII.GetString(byt);
                _connStr = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DAL.Properties.Settings.InvoiceSystemConnection"].ConnectionString;
            }
            catch { }
            return _connStr;
        }

        //Return Data
        public DataSet SelectDataSet(string select, ArrayList lstParam, bool isQuery)
        {

            DataSet tb = new DataSet();
            string conStr = connString();
            if (conStr != string.Empty)
            {
                SqlCommand cmd = new SqlCommand();
                if (isQuery)
                    cmd.CommandType = CommandType.Text;
                else
                    cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandText = select;

                cmd.Connection = new SqlConnection(conStr);

                if (lstParam != null)
                    for (int i = 0; i < lstParam.Count; i++)
                        cmd.Parameters.Add(lstParam[i]);
                cmd.Connection.Open();
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);

                try
                {
                    Adap.Fill(tb);
                    //===============================

                    //SqlDataReader dr = cmd.ExecuteReader();
                    //DataTable dt = new DataTable();
                    //dt.Load(dr);

                    //tb.Tables.Add(dt);
                    ////return tb;


                    //===================

                }
                catch (Exception err)
                {
                    //System.Windows.Forms.MessageBox.Show(err.Message, "SqlServer Error");
                }
                finally
                {
                    Adap.Dispose();
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                }
            }
            if (tb == null)
            {

            }
            if (tb.Tables.Count > 0)
            {
            }

            //

            return tb;
        }

        //Return Data Row
        public DataTable SelectData(string select, ArrayList lstParam, bool isQuery)
        {
            DataTable tb = new DataTable();
            string conStr = connString();
            if (conStr != string.Empty)
            {
                SqlCommand cmd = new SqlCommand();
                if (isQuery)
                    cmd.CommandType = CommandType.Text;
                else
                    cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandText = select;

                cmd.Connection = new SqlConnection(conStr);

                if (lstParam != null)
                    for (int i = 0; i < lstParam.Count; i++)
                        cmd.Parameters.Add(lstParam[i]);

                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(tb);
                }
                catch (Exception err)
                {
                    //System.Windows.Forms.MessageBox.Show(err.Message, "SqlServer Error");
                }
                finally
                {
                    Adap.Dispose();
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                }
            }
            return tb;
        }

        //Return Data Row
        public DataRow SelectRow(string select, ArrayList lstParam, bool isQuery)
        {

            DataTable tb = new DataTable();
            string conStr = connString();
            if (conStr != string.Empty)
            {
                SqlCommand cmd = new SqlCommand();
                if (isQuery)
                    cmd.CommandType = CommandType.Text;
                else
                    cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = select;
                if (lstParam != null)
                    for (int i = 0; i < lstParam.Count; i++)
                        cmd.Parameters.Add(lstParam[i]);

                cmd.Connection = new SqlConnection(conStr);
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                try
                {
                    Adap.Fill(tb);
                }
                catch (Exception err)
                {
                    //  System.Windows.Forms.MessageBox.Show(err.Message, "SqlServer Error");
                }
                finally
                {
                    Adap.Dispose();
                    cmd.Parameters.Clear();
                    cmd.Dispose();

                }
            }
            if (tb.Rows.Count == 0)
                return null;
            else
                return tb.Rows[0];

        }

        //Update Data
        public bool UpdateData(string select, ArrayList lstParam, bool isQuery)
        {
            int recd = 0;
            string conStr = connString();
            if (conStr != string.Empty)
            {
                SqlCommand cmd = new SqlCommand();
                SqlConnection con = new SqlConnection(conStr);
                if (isQuery)
                    cmd.CommandType = CommandType.Text;
                else
                    cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandText = select;
                if (lstParam != null)
                    for (int i = 0; i < lstParam.Count; i++)
                        cmd.Parameters.Add(lstParam[i]);

                cmd.Connection = con;

                try
                {
                    con.Open();
                    recd = cmd.ExecuteNonQuery();
                }
                catch (Exception err)
                {
                    //Console.WriteLine(err.Message);  
                    //System.WindowsForms.MessageBox.Show(err.Message, "SqlServer Error");
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                }
            }
            if (recd == 0)
                return false;
            else
                return true;
        }



        //Update Transaction
        public bool UpdateTransData(ArrayList select, ArrayList lstParam, bool isQuery)
        {

            bool rt = true;
            string conStr = connString();
            if (conStr != string.Empty)
            {
                SqlTransaction trans;
                SqlCommand cmd = null;
                SqlConnection con = new SqlConnection(conStr);
                con.Open();
                trans = con.BeginTransaction();
                try
                {

                    for (int j = 0; j < select.Count; j++)
                    {
                        cmd = new SqlCommand();
                        cmd.Connection = con;
                        if (isQuery)
                            cmd.CommandType = CommandType.Text;
                        else
                            cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = select[j].ToString();
                        if (lstParam[j] != null)
                        {
                            ArrayList lst = (ArrayList)lstParam[j];
                            for (int i = 0; i < lst.Count; i++)
                                cmd.Parameters.Add(lst[i]);
                        }
                        cmd.Transaction = trans;

                        cmd.ExecuteNonQuery();
                    }
                    trans.Commit();

                }
                catch (Exception err)
                {
                    rt = false;
                    trans.Rollback();
                    // System.Windows.Forms.MessageBox.Show(err.Message, "SqlServer Error");
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                }
            }
            return rt;

        }

        //Return Single Value
        public object SelectValue(string select, ArrayList lstParam, bool isQuery)
        {
            object retVal = null;
            string conStr = connString();
            if (conStr != string.Empty)
            {

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand();
                    if (isQuery)
                        cmd.CommandType = CommandType.Text;
                    else
                        cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = select;
                    if (lstParam != null)
                        for (int i = 0; i < lstParam.Count; i++)
                            cmd.Parameters.Add(lstParam[i]);
                    cmd.Connection = con;
                    try
                    {
                        con.Open();
                        retVal = cmd.ExecuteScalar();
                    }
                    catch (Exception err)
                    {
                        //  System.Windows.Forms.MessageBox.Show(err.Message, "SqlServer Error");
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Parameters.Clear();
                        cmd.Dispose();
                    }
                }
            }
            return retVal;
        }

        // Return Stored Procedure return 
        public int ReturnValue(string select, ArrayList lstParam) 
        {
            int rval = -101;
            string conStr = connString();
            if (conStr != string.Empty)
            {

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = select;
                    if (lstParam != null)
                        for (int i = 0; i < lstParam.Count; i++)
                            cmd.Parameters.Add(lstParam[i]);

                    /*SqlParameter param = new SqlParameter();
                    param.ParameterName = "@return_value";
                    param.DbType = DbType.Int32;
                    param.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(param);*/
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@success";
                    param.DbType = DbType.Int32;
                    //param.Direction = ParameterDirection.ReturnValue;

                    param.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(param);

                    cmd.Connection = con;

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        //rval = (int)cmd.Parameters["@OutTicketID"].Value;
                        rval = (int)param.Value;
                    }
                    catch (Exception err)
                    {
                        rval = -1;// System.Windows.Forms.MessageBox.Show(err.Message, "SqlServer Error");
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Parameters.Clear();
                        cmd.Dispose();
                    }
                }
            }
            return rval;
        }


        // Return Stored Procedure return 
        public int ReturnAttachmentIdAfterInsert(string select, ArrayList lstParam)
        {
            int rval = -101;
            string conStr = connString();
            if (conStr != string.Empty)
            {

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = select;
                    if (lstParam != null)
                        for (int i = 0; i < lstParam.Count; i++)
                            cmd.Parameters.Add(lstParam[i]);

                   
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@OutAttachmentID";
                    param.DbType = DbType.Int32;
                    //param.Direction = ParameterDirection.ReturnValue;

                    param.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(param);

                    cmd.Connection = con;

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();                        
                        rval = (int)param.Value;
                    }
                    catch (Exception err)
                    {
                        
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Parameters.Clear();
                        cmd.Dispose();
                    }
                }
            }
            return rval;
        }

        
        // Return Stored Procedure return 
        public int ReturnValuefromLogin(string select, ArrayList lstParam)
        {
            int rval = -101;
            string conStr = connString();
            if (conStr != string.Empty)
            {

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = select;
                    if (lstParam != null)
                        for (int i = 0; i < lstParam.Count; i++)
                            cmd.Parameters.Add(lstParam[i]);

                    /*SqlParameter param = new SqlParameter();
                    param.ParameterName = "@return_value";
                    param.DbType = DbType.Int32;
                    param.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(param);*/
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@OutUserCount";
                    param.DbType = DbType.Int32;
                    //param.Direction = ParameterDirection.ReturnValue;

                    param.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(param);

                    cmd.Connection = con;

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        //rval = (int)cmd.Parameters["@OutTicketID"].Value;
                        rval = (int)param.Value;
                    }
                    catch (Exception err)
                    {

                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Parameters.Clear();
                        cmd.Dispose();
                    }
                }
            }
            return rval;
        }

        
        public int ReturnRoleIdByUserId(string select, ArrayList lstParam)
        {
            int rval = -101;
            string conStr = connString();
            if (conStr != string.Empty)
            {

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = select;
                    if (lstParam != null)
                        for (int i = 0; i < lstParam.Count; i++)
                            cmd.Parameters.Add(lstParam[i]);
                   
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@OutRoleId";
                    param.DbType = DbType.Int32;                    

                    param.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(param);

                    cmd.Connection = con;

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();                        
                        rval = (int)param.Value;
                    }
                    catch (Exception err)
                    {

                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Parameters.Clear();
                        cmd.Dispose();
                    }
                }
            }
            return rval;
        }


        public string GetSupplierCode(string select, ArrayList lstParam)
        {            
            string Scodeval = string.Empty;
            string conStr = connString();
            if (conStr != string.Empty)
            {

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = select;
                    if (lstParam != null)
                        for (int i = 0; i < lstParam.Count; i++)
                            cmd.Parameters.Add(lstParam[i]);
                   
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@OutSupplierCode";
                    param.DbType = DbType.String;
                    param.Size = 50;
                    //param.Direction = ParameterDirection.ReturnValue;

                    param.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(param);
                    cmd.Connection = con;

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();                        
                        Scodeval = Convert.ToString(param.Value);
                    }
                    catch (Exception err)
                    {

                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Parameters.Clear();
                        cmd.Dispose();
                    }
                }
            }
            return Scodeval;

        }

        public string GetQtyUnitOfMeasureDescp(string select, ArrayList lstParam)
        {
            string strQtyUnitOfMeasureDescp = string.Empty;
            string conStr = connString();
            if (conStr != string.Empty)
            {

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = select;
                    if (lstParam != null)
                        for (int i = 0; i < lstParam.Count; i++)
                            cmd.Parameters.Add(lstParam[i]);

                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@OutQtyUnitOfMeasure_descp";
                    param.DbType = DbType.String;
                    param.Size = 50;
                    //param.Direction = ParameterDirection.ReturnValue;

                    param.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(param);
                    cmd.Connection = con;

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        strQtyUnitOfMeasureDescp = Convert.ToString(param.Value);
                    }
                    catch (Exception err)
                    {

                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Parameters.Clear();
                        cmd.Dispose();
                    }
                }
            }
            return strQtyUnitOfMeasureDescp;

        }

        public string GetInvoiveCodeAfterInsert(string select, ArrayList lstParam)
        {

            string Icode = string.Empty;
            string conStr = connString();
            if (conStr != string.Empty)
            {

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = select;
                    if (lstParam != null)
                        for (int i = 0; i < lstParam.Count; i++)
                            cmd.Parameters.Add(lstParam[i]);

                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@outinvoice_code";
                    param.DbType = DbType.String;
                    param.Size = 50;
                    //param.Direction = ParameterDirection.ReturnValue;

                    param.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(param);
                    cmd.Connection = con;

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        Icode = Convert.ToString(param.Value);
                    }
                    catch (Exception err)
                    {

                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Parameters.Clear();
                        cmd.Dispose();
                    }
                }
            }
            return Icode;

        }

        public void Insert(string select, ArrayList lstParam)
        {

            //string Icode = string.Empty;
            string conStr = connString();
            if (conStr != string.Empty)
            {

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = select;
                    if (lstParam != null)
                        for (int i = 0; i < lstParam.Count; i++)
                            cmd.Parameters.Add(lstParam[i]);

                    /*SqlParameter param = new SqlParameter();
                    param.ParameterName = "@outinvoice_code";
                    param.DbType = DbType.String;
                    param.Size = 50;
                    //param.Direction = ParameterDirection.ReturnValue;

                    param.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(param);*/
                    cmd.Connection = con;

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                       // Icode = Convert.ToString(param.Value);
                    }
                    catch (Exception err)
                    {

                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Parameters.Clear();
                        cmd.Dispose();
                    }
                }
            }
            //return Icode;

        }




        //-------------------------------4-9-2013----------------
        // Return Stored Procedure return 
        public int IsManager(string select, ArrayList lstParam)
        {
            int rval = -101;
            string conStr = connString();
            if (conStr != string.Empty)
            {

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = select;
                    if (lstParam != null)
                        for (int i = 0; i < lstParam.Count; i++)
                            cmd.Parameters.Add(lstParam[i]);

                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@OutManagerCount";
                    param.DbType = DbType.Int32;
                    //param.Direction = ParameterDirection.ReturnValue;

                    param.Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(param);

                    cmd.Connection = con;

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        //rval = (int)cmd.Parameters["@OutTicketID"].Value;
                        rval = (int)param.Value;
                    }
                    catch (Exception err)
                    {

                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Parameters.Clear();
                        cmd.Dispose();
                    }
                }
            }
            return rval;
        }



        //============================NKK==============================




        //Deletes Record from a table

        public bool DeleteData(string select, ArrayList lstParam, bool isQuery)
        {
            int recd = 0;
            string conStr = connString();
            if (conStr != string.Empty)
            {
                SqlCommand cmd = new SqlCommand();
                SqlConnection con = new SqlConnection(conStr);
                if (isQuery)
                    cmd.CommandType = CommandType.Text;
                else
                    cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandText = select;
                if (lstParam != null)
                    for (int i = 0; i < lstParam.Count; i++)
                        cmd.Parameters.Add(lstParam[i]);

                cmd.Connection = con;

                try
                {
                    con.Open();
                    recd = cmd.ExecuteNonQuery();
                }
                catch (Exception err)
                {

                    //System.WindowsForms.MessageBox.Show(err.Message, "SqlServer Error");
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                }
            }
            if (recd == 0)
                return false;
            else
                return true;
        }


    }
}
