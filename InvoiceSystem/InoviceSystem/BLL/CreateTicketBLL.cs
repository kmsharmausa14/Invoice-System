using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
using System.Configuration;
using DAL;
using BO;

namespace BLL
{
    public class CreateTicketBLL
    {

        //Create Ticket        
        //public int CreateTicket(string ticketDescription, string title, string createdBy, string affectedUser, int priority, int status, DateTime loginDate, DateTime lastUpdatedTs, bool isActive,
        //    int projectProcessID, int notifyById, int locationID, int siteBuildingID, int floorwingID, int seatID, int supportDomainID, int categoryID, int areaID, int SubAreaID)
        public int CreateTicket()
        {
            ArrayList lstParam = new System.Collections.ArrayList();

            SqlParameter param;

           

            //====================

            param = new SqlParameter();
            param.ParameterName = "@AttachmentPath";
            param.DbType = DbType.String;
            param.Value = "";
            lstParam.Add(param);

            //==============NKK==================

            /*param = new SqlParameter();
            param.ParameterName = "@OutTicketID";
            param.DbType = DbType.Int32;
            param.Direction = ParameterDirection.ReturnValue;
            lstParam.Add(param);*/


            int x;
           // x = new DAL.SqlHelper().ReturnValue("sp_Employee_Insert", lstParam);
            x = new DAL.SqlHelper().ReturnValue("[dbo].[GSD_CreateTicket]", lstParam);
            /*if (x == 0)
                return Convert.ToInt32(x);
            else
                return -1;*/
            return x;
        }
    }
}
