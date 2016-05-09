<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Reports.aspx.cs" Inherits="VendorPortal.Reports" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<style type="text/css">
     .modalPopup 
{ 
background-color: #FFFFFF; 
border-width: 3px; 
border-style: solid; 
border-color: Gray; 
padding-top: 10px; 
padding-left: 10px; 
font-family:Verdana;
width: auto; 
height: auto; 
border:1px solid;
 border-bottom-left-radius:1em;
 border-top-left-radius:1em;
 border-top-right-radius:1em;
 border-bottom-right-radius:1em;
}
.reportgrid
{
align:center;
text-align:center;    
}


.modalBackground 
{ 
background-color: ThreeDShadow; 
filter: alpha(opacity=80); 
opacity: 0.8; 
z-index: 10000; 
}
      .div
 {
 border:1px solid;
 border-bottom-left-radius:0.40em;
 border-top-left-radius:0.40em;
 border-top-right-radius:0.40em;
 border-bottom-right-radius:0.40em;
 } 
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--.aplha
{
}
    <h2>
        DayWise Report</h2>
    <div>
        <asp:GridView ID="gvwDayWiseRep" runat="server">
        </asp:GridView>
    </div>
    <h2>
        TAT Report</h2>
    <div>
        <asp:GridView ID="gvwTAT" runat="server">
        </asp:GridView>
    </div>
    <h2>
        Supplier Report</h2>
    <div>
        <asp:GridView ID="gvwSupRep" runat="server">
        </asp:GridView>
    </div>--%>
    <table align="center">
        <tr>
            <td>
                <h1 style="font-family:Verdana" >Reports</h1>
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td style="font-size:large">
                <asp:LinkButton runat="server" Text="DayWise Report" ID="lbtnDay"></asp:LinkButton>
            </td>
        </tr>
        <tr>
        <td>
        <br />
        </td>
        </tr>
        <tr>
            <td style="font-size:large">
                <asp:LinkButton runat="server" Text="TAT Report" ID="lbtnTAT"></asp:LinkButton>
            </td>
        </tr>
        <tr>
        <td>
        <br />
        </td>
        </tr>
        <tr>
            <td style="font-size:large">
               <%-- <asp:LinkButton runat="server" Text="SupplierWise Report" ID="lbtnSupplier"></asp:LinkButton>--%>
            </td>
        </tr>
        <tr>
        <td>
        <br />
        </td>
        </tr>
        <tr>
            <td style="font-size:large" align="center">
              
              <asp:Button ID="btnCancel" runat="server" Text="Back" Width="120px" BackColor="#5D5D5D"
                                        Font-Bold="True" ForeColor="White" 
                    onclick="btnCancel_Click" />
            </td>
        </tr>
        <tr>
        <td>
        <br />
        </td>
        </tr>
        
    </table>
    <asp:Panel runat="server" ID="pnlDay" CssClass="modalPopup">
        <h2 >DayWise Report</h2>
        <div style="align:center">
            <asp:GridView ID="gvwDayWiseRep" runat="server" CssClass="reportgrid">
                <HeaderStyle  BackColor="DimGray" ForeColor="White"/>
                 <AlternatingRowStyle BackColor="LightGray"/>
                 <RowStyle  Wrap="true"/>
            </asp:GridView>
            <br />
            <table width="100%">
                <tr>
                    <td align="center">
                        <%--<asp:Button ID="btnDayExport" CssClass="div" Text="Export" runat="server" />--%>
                        <asp:Button ID="btnCancelDay" CssClass="div" Text="OK" runat="server" />
                    </td>
                </tr>
            </table>
             
        </div>
    </asp:Panel>
    <ajax:ModalPopupExtender ID="mpeDay" runat="server" PopupControlID="pnlDay" TargetControlID="lbtnDay"
        CancelControlID="btnCancelDay" BackgroundCssClass="modalBackground">
    </ajax:ModalPopupExtender>

    <asp:Panel runat="server" ID="pnlTAT" CssClass="modalPopup">
        <h2>TAT Report</h2>
        <div>
            <asp:GridView ID="gvwTAT" runat="server" CssClass="reportgrid">
                <HeaderStyle  BackColor="DimGray" ForeColor="White"/>
                 <AlternatingRowStyle BackColor="LightGray"/>
                 <RowStyle  Wrap="true"/>
            </asp:GridView>
            <br />
            <table width="100%">
                <tr>
                    <td align="center">
                        <%--<asp:Button ID="btnTATExport" CssClass="div" Text="Export" runat="server" />--%>
                        <asp:Button ID="btnCancelTAT" CssClass="div"  Text="OK" runat="server" />
                    </td>
                </tr>
            </table>
            
        </div>
    </asp:Panel>
    <ajax:ModalPopupExtender ID="mpeTAT" runat="server" PopupControlID="pnlTAT" TargetControlID="lbtnTAT"
        CancelControlID="btnCancelTAT" BackgroundCssClass="modalBackground">
    </ajax:ModalPopupExtender>





   <%-- <asp:Panel runat="server" ID="pnlSupplier" CssClass="modalPopup">
        <h2>SupplierWise Report</h2>
        <div>
            <asp:GridView ID="gvwSupplier" runat="server" CssClass="reportgrid">
                <HeaderStyle  BackColor="DimGray" ForeColor="White" HorizontalAlign="Center"/>
                <AlternatingRowStyle BackColor="LightGray"/>
                <RowStyle  Wrap="true" HorizontalAlign="Center"/>
                
            </asp:GridView>
            <br />
            <table width="100%">
                <tr>
                    <td align="center">
                        <asp:Button ID="btnSupplierExport" CssClass="div" Text="Export" runat="server" />
                        <asp:Button ID="btnCancelSupplier" CssClass="div" Text="OK" runat="server" />
                    </td>
                </tr>
            </table>
            
        </div>
    </asp:Panel>
    <ajax:ModalPopupExtender ID="mpeSupplier" runat="server" PopupControlID="pnlSupplier"
        TargetControlID="lbtnSupplier" CancelControlID="btnCancelSupplier" BackgroundCssClass="modalBackground">
    </ajax:ModalPopupExtender>--%>
</asp:Content>
