<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SupplierCodeLookUp.aspx.cs" Inherits="VendorPortal.SupplierCodeLookUp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Styles/invsys.css" rel="stylesheet" type="text/css" />
     <script src="Scripts/jquery-1.4.1.js"></script>
    <script src="Scripts/jquery-1.4.1.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td align="center">
                <table  width="95%" cellpadding="0" cellspacing="0" border="0">
                 <tr>
                        <td height="30px">
                            
                        </td>
                    </tr>
                     <tr>
                        <td align="left"class="Invallheader" >
                          &nbsp;Please select supplier records
                       </td>
                      </tr>
                      <tr>
                        <td height="10px">
                            
                        </td>
                    </tr>
                    <tr>
                            <td height="30px" align="left">
                                <asp:GridView ID="grd_ViewSupplierCodeLookUp" runat="server" AutoGenerateColumns="False" 
            DataKeyNames="s_code" EmptyDataText="Cannot find related information in contacts" AllowPaging="True" Width="100%" CssClass="GridText" >
            <Columns>                
                    <asp:HyperLinkField DataTextField="s_code" HeaderText="Supplier Code" DataNavigateUrlFields="s_code"
                    DataNavigateUrlFormatString="~/CreateInvoice.aspx?QS_Scode={0}"  ItemStyle-ForeColor="Black" ControlStyle-ForeColor="Black"
                    AccessibleHeaderText="s_code"  Visible="False" HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="White" />
                                     
                    <asp:HyperLinkField DataTextField="s_code" HeaderText="Supplier Code" DataNavigateUrlFields="s_code"
                    DataNavigateUrlFormatString="~/Listdr.aspx?QS_Scode1={0}"   ItemStyle-ForeColor="Black" ControlStyle-ForeColor="Black"
                    AccessibleHeaderText="s_code"  Visible="False"  HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="White"/>

                    <asp:HyperLinkField DataTextField="s_code" HeaderText="Supplier Code" DataNavigateUrlFields="s_code"
                    DataNavigateUrlFormatString="~/Approver_WrkQue.aspx?QS_Scode2={0}"  ItemStyle-ForeColor="Black" ControlStyle-ForeColor="Black"
                    AccessibleHeaderText="s_code"  Visible="False"  HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="White"/>

                    <asp:HyperLinkField DataTextField="s_code" HeaderText="Supplier Code" DataNavigateUrlFields="s_code"
                    DataNavigateUrlFormatString="~/InvStatus.aspx?QS_Scode3={0}"   ItemStyle-ForeColor="Black" ControlStyle-ForeColor="Black"
                    AccessibleHeaderText="s_code"  Visible="False"  HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="White"/>

                <asp:BoundField  DataField="s_name"   HeaderText="Supplier Name"  HeaderStyle-HorizontalAlign="Left"  ItemStyle-ForeColor="Black" ItemStyle-BackColor="White"/>
                
            </Columns>
                                    <EmptyDataRowStyle BackColor="#7A99B8" BorderStyle="None" Height="15px" />
        </asp:GridView>

                            </td>
                    </tr>   
                </table>

            </td>
            </tr>
            <tr>
            <td>
            <span></span>
            </td>
            </tr>
            <tr>
            <td>
            <span></span>
            </td>
            </tr>
</table>

 <table width="100%" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td width="100%" align="center">
            
            <asp:Button ID="btn_Cancel" CssClass="button" runat="server" Text="Cancel" Width="120px" BackColor="#5D5D5D"
                                                                        Font-Bold="True" ForeColor="White"
                onclick="btn_Cancel_Click" />
        </td>
    </tr>
 </table>
</asp:Content>
