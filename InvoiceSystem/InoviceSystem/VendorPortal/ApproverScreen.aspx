<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ApproverScreen.aspx.cs" Inherits="VendorPortal.ApproverScreen" %>

<%--<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%--<link href="Styles/invsys.css" rel="stylesheet" type="text/css" />--%>
    <link href="Styles/invsys.css" rel="Stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.4.1.js"></script>
    <script src="Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript">
        jQuery.fn.ForAlphanumericNumericCommaFullstop =
            function () {
                //debugger;
                // debugger;
                return this.each(function () {
                    $(this).keypress(function (e) {
                        var regex = new RegExp("^[a-zA-Z0-9., ]+$");

                        // var unregex = new RegExp("^[/.,<>!@#$%^&*()_+={}[]\|?]");
                        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
                        if (regex.test(str) || e.keyCode == 8 || e.keyCode == 9) {
                            return true;
                        }
                        else if (e.keyCode == 188 || e.keyCode == 190 || e.keyCode == 46 || e.keyCode == 44 || e.keyCode == 190 || e.keyCode == 32) {
                            return true;
                        }
                        else {
                            e.preventDefault();
                            return false;
                        }
                    });
                });

            };

        function ValidateLengthAfterTextChange(txtfinal) {
            //debugger;
            var final = document.getElementById(txtfinal);
            if (final.value.length > 250) {
                alert("Maximum 250 characters allowed!");
                var substr = final.value.substring(0, 250);
                final.value = "";
                final.value = substr;

            }

        }
        function ValidateLength(txtfinal) {
            //debugger;
            var final = document.getElementById(txtfinal);
            if (final.value.length > 250) {

                event.returnValue = false;

            }


        }
    </script>
    <style type="text/css">
        .style2
        {
            width: 182px;
        }
        .style3
        {
            width: 165px;
        }
        .style4
        {
            width: 255px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server" ClientIDMode="Static">
    <%--<ajax:ToolkitScriptManager ID="ToolkitScriptManager" runat="server">
    </ajax:ToolkitScriptManager>--%>
    <table style="width: 1150px; height: 760px" cellpadding="0" cellspacing="0" border="0"
        align="left" backcolor="#F7F7F7">
        <%-- <tr>--%>
        <tr valign="top" align="center" style="font-family: Dotum; font-size: x-large; font-weight: bold;
            font-style: normal; color: #0033CC">
            <td align="center">
                <table align="center" width="1150px" style="font-family: Verdana; color: #006666;
                    font-size: 12px;">
                    <tr>
                        <td align="center" style="font-family: Verdana; font-weight: bold; font-size: 20PX;
                            color: #006666;">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            Approver Screen
                        </td>
                        <%--<td align="right" style="font-size: 15px">
                            <a id="A1" href="Approver_WrkQue.aspx" runat="server"><b>Back</b></a>
                        </td>--%>
                    </tr>
                    <tr>
                        <td  style="width:100%">
                            <div style="background-color: #F7F7F7; width:100%">
                                <table style="width:100%">
                                    <tr id="inv_dtls" runat="server" style="width:100%">
                                        <td style="width:100%"> 
                                            <table style="width:100%">
                                                <tr style="width:100%; background-color: #AEC9FF">
                                                    <td  align="left" style="width:100% ; font-family: Verdana; font-size: medium; font-style: normal;
                                                        color: #FFFFFF; background-color: #5D5D5D;" colspan="9" width="1150px">
                                                        Invoice Details
                                                    </td>
                                                </tr>
                                            </table>
                                            <table style="width:100%">
                                                <tr style="width:100%">
                                                    <td colspan="6" align="left" style="width:100%">
                                                        <asp:ValidationSummary ID="ValidationSummaryApproverScreen" runat="server" ForeColor="Red" />
                                                    </td>
                                                </tr>
                                                <tr style="width:100%">
                                                    <td colspan="6" align="left" style="width:100%">
                                                        <asp:Label runat="server" ID="errmessage" Font-Bold="true" ForeColor="Red"></asp:Label>
                                                        <asp:Label runat="server" ID="succesmsg" Font-Bold="true" ForeColor="Green"></asp:Label>
                                                    </td>
                                                </tr>
                                                </table>
                                                <table style="width:100%" cellpadding="10">
                                                <tr >
                                              
                                                
                                                    <td align="left" colspan="1" >
                                                        <b>
                                                            <asp:Label ID="lblSupplierCode" runat="server" Text= "Supplier Code:" ></asp:Label></b>
                                                    </td>
                                                    <td align="left" valign="top"  colspan="2">
                                                        <asp:TextBox ID="txtSupplierCode" runat="server" ToolTip="Supplier Code" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                   
                                                    <td align="left" valign="top"  colspan="1">
                                                        <b>
                                                            <asp:Label ID="lblInvoiceNumber" runat="server" Text="Invoice Number:"></asp:Label></b>
                                                    </td>
                                                    <td align="left" valign="top" colspan="2">
                                                        <asp:TextBox ID="txtInvoiceNumber" runat="server" ToolTip="Invoice Number" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" colspan="1">
                                                        <b>
                                                            <asp:Label ID="lblShippedTo" runat="server" Text="Shipped To:"></asp:Label></b>
                                                    </td>
                                                    <td align="left" valign="top" colspan="2">
                                                        <asp:TextBox ID="txtShippedTo" runat="server" ToolTip="Shipped To" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                   
                                                    <td align="left" colspan="1">
                                                        <b>
                                                            <asp:Label ID="lblInvoiceTo" runat="server" Text="Invoice To:"></asp:Label></b>
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:TextBox ID="txtInvoiceTo" runat="server" ToolTip="Invoice To" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="1">
                                                        <b>
                                                            <asp:Label ID="lblPayableTo" runat="server" Text="Payable To:"></asp:Label></b>
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:TextBox ID="txtPayableTo" runat="server" ToolTip="Payable To" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                   
                                                    <td valign="top" align="left" colspan="1">
                                                        <b>
                                                            <asp:Label ID="lblSupplierAddress" runat="server" Text="Supplier Address:"></asp:Label>
                                                        </b>
                                                    </td>
                                                    <td align="left" valign="top" colspan="2">
                                                        <asp:TextBox ID="txtSupplierAddress" runat="server" Width="223px" Height="70px" TextMode="MultiLine"
                                                            ToolTip="Supplier Address" TabIndex="6" ReadOnly="true" Font-Names="Verdana"
                                                            Font-Size="Small"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" colspan="1">
                                                        <b>
                                                            <asp:Label ID="lblEmailAddress" runat="server" Text="E-mail Address:"></asp:Label></b>
                                                    </td>
                                                    <td align="left" valign="top" colspan="2">
                                                        <asp:TextBox ID="txtEmailAddress" runat="server" ToolTip="Email Address" TabIndex="7"
                                                            Width="217px" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    
                                                    <td align="left" valign="top" colspan="1">
                                                        <b>
                                                            <asp:Label ID="lblFinalDestination" runat="server" Text="Final Destination:"></asp:Label></b>
                                                    </td>
                                                    <td align="left" valign="top" colspan="2">
                                                        <asp:TextBox ID="txtFinalDestination" runat="server" Width="231px" Height="70px"
                                                            TextMode="MultiLine" MaxLength="250" ToolTip="Final Destination" TabIndex="8"
                                                            Font-Names="Verdana" Font-Size="Small"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top" colspan="1">
                                                        <b>
                                                            <asp:Label ID="lblCurrency" runat="server" Text="Currency:"></asp:Label></b>
                                                    </td>
                                                    <td align="left" valign="top" colspan="2">
                                                        <asp:TextBox ID="txtCurrency" runat="server" ToolTip="Supplier Currency" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                   
                                                    <td align="left" valign="top" colspan="1">
                                                        <b>
                                                            <asp:Label ID="lblComments" runat="server" Text="Comments:"></asp:Label></b>
                                                    </td>
                                                    <td align="left" valign="top" colspan="2">
                                                        <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Width="232px" Height="70px"
                                                            MaxLength="250" ToolTip="Comments" TabIndex="10" Font-Names="Verdana" Font-Size="Small"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="listi" runat="server">
                                        <td >
                                            <table style="width:100%">
                                                <tr>
                                                    <td align="left" class="InvallSubheaders" colspan="6" style="width:100%">
                                                        Order Details
                                                    </td>
                                                </tr>
                                            </table>
                                            <table style="width:100%" cellpadding="10">
                                                <tr valign="top">
                                                    <td  colspan="1" align="left" class="style3" >
                                                        <b>
                                                            <asp:Label ID="lblInvoiceDate" runat="server" Text="Invoice Date"></asp:Label></b>:&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td align="left" colspan="3" class="style2">
                                                        <asp:TextBox ID="txtInvoiceDate" runat="server" ToolTip="mm/dd/yyyy" ReadOnly="true"></asp:TextBox>
                                                        <%--<ajax:CalendarExtender ID="CalendarExtenderInvoiceDate" TargetControlID="txtInvoiceDate"
                                                            runat="server" />--%>
                                                    </td>
                                                   
                                                    
                                                    <td align="center" colspan="1" class="style4" >
                                                        <b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:Label ID="lblShippedDate" runat="server" Text="Shipped Date:"></asp:Label></b>&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:TextBox runat="server" ID="txtShippedDate" ToolTip="mm/dd/yyyy" Text="mm/dd/yyyy"
                                                            class="water" ReadOnly="true" style="margin-left: 36px"></asp:TextBox>
                                                        <%--<ajax:CalendarExtender ID="CalendarExtenderShippedDate" TargetControlID="txtShippedDate"
                                                            runat="server" />--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="1" class="style3">
                                                        <b>
                                                            <asp:Label ID="lblShippedVia" runat="server" Text="Shipped Via:"></asp:Label></b>
                                                    </td>
                                                    <td align="left" colspan="3" class="style2">
                                                        <asp:TextBox ID="txtShippedVia" runat="server" ToolTip="Shipped Via" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                    
                                                    <td align="center" colspan="1" class="style4">
                                                        <b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Attachments:</b>
                                                    </td>
                                                    <td valign="top" align="left" colspan="2">
                                                        <asp:TextBox ID="txtfile" runat="server" TextMode="SingleLine" 
                                                            style="margin-left: 34px"></asp:TextBox>
                                                        <asp:Button ID="Button4" runat="server" Text="Browse" Width="120px" BackColor="#5D5D5D"
                                                            Font-Bold="True" ForeColor="White" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                   
                                                    <td  align="right" colspan="6">
                                                        <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="false" DataKeyNames="File_name"
                                                            OnRowCommand="gvDetails_RowCommand" OnRowCreated="gvDetails_RowCreated">
                                                            <%--onselectedindexchanged="gvDetails_SelectedIndexChanged">--%>
                                                            <%--onselectedindexchanged="gvDetails_SelectedIndexChanged"--%>
                                                            <%-- onrowcreated="gvDetails_RowCreated" --%>
                                                            <HeaderStyle BackColor="#5D5D5D" Font-Bold="true" ForeColor="White" />
                                                            <Columns>
                                                                <asp:BoundField DataField="Attachment_id" HeaderText="Id" Visible="false" />
                                                                <asp:TemplateField HeaderText="File_name">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="link1" CommandName="Click" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>'
                                                                            runat="server" CausesValidation="false"><%# Eval("File_name")%></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <%--<tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td align="left">
                                                        &nbsp;
                                                    </td>
                                                </tr>--%>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="line_items" runat="server">
                                        <td style="width:100%">
                                            <table style="width:100%">
                                                <tr style="background-color: #003399; width:100%">
                                                    <td align="left" class="InvallSubheaders" colspan="9" style="width:100%">
                                                        Line Item Details
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <br />
                                                    </td>
                                                </tr>
                                            </table>
                                            <div style="height: auto; width: 1120px; overflow: auto; border: 2; border-color: Black;">
                                                <asp:GridView ID="gvLineItemDetails" runat="server" AutoGenerateColumns="false" Width="100%"
                                                    ShowHeader="true" ShowFooter="true" ReadOnly="true">
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <%#Eval("Inv_No") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Part/Item Number">
                                                            <ItemTemplate>
                                                                <%#Eval("part_item_no") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="PO Number">
                                                            <ItemTemplate>
                                                                <%#Eval("po_no") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="PO Amendment Number">
                                                            <ItemTemplate>
                                                                <%#Eval("po_amendment_no") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Release/Requistion Number">
                                                            <ItemTemplate>
                                                                <%#Eval("release_no") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Quantity Shipped">
                                                            <ItemTemplate>
                                                                <%#Eval("qty_shipped") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Unit Price">
                                                            <ItemTemplate>
                                                                <%#Eval("unit_price") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Amount">
                                                            <ItemTemplate>
                                                                <%#Eval("amount") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qty Unit of Measure">
                                                            <ItemTemplate>
                                                                <%#Eval("description")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Packing Slip">
                                                            <ItemTemplate>
                                                                <%#Eval("packing_slip") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Bill of Lading">
                                                            <ItemTemplate>
                                                                <%#Eval("bill_lading") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Comments">
                                                            <ItemTemplate>
                                                                <%#Eval("Comments") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="ad_crgs" runat="server">
                                        <td>
                                            <table style="width:100%">
                                                <tr style="width:100%; background-color: #003399">
                                                    <td align="left" class="InvallSubheaders" colspan="8" style="width:100%">
                                                        Additional Charge Details
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="height: auto; width: 1120px; overflow: auto; border: 2; border-color: Black;">
                                                            <asp:GridView ID="gvAdditionalChargeDetails" runat="server" AutoGenerateColumns="false"
                                                                Width="100%" ShowHeader="true" DataKeyNames="Inv_No">
                                                                <Columns>
                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <%#Eval("Inv_No") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Charge# *">
                                                                        <ItemTemplate>
                                                                            <%#Eval("charge_no") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Charge Type">
                                                                        <ItemTemplate>
                                                                            <%#Eval("Charge_type")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Charge">
                                                                        <ItemTemplate>
                                                                            <%#Eval("charge") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Amount">
                                                                        <ItemTemplate>
                                                                            <%#Eval("amount","{0:F}") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Description">
                                                                        <ItemTemplate>
                                                                            <%#Eval("description") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="GST (Goods & Service Tax)">
                                                                        <ItemTemplate>
                                                                            <%#Eval("Gst_no") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="acc_distr" runat="server">
                                        <td>
                                            <table style="width:100%">
                                                <tr style="width:100% ; background-color: #003399">
                                                    <td align="left" class="InvallSubheaders" colspan="9" style="width:100%">
                                                        Account Distribution Details
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <br />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table style="width: 100%">
                                                <tr style="width: 100%">
                                                    <td style="width: 100%">
                                                        <div style="height: auto; width: 1120px; overflow: auto; border: 2; border-color: Black;">
                                                            <asp:GridView ID="gvAccountDistributionDetails" runat="server" AutoGenerateColumns="false"
                                                                Width="100%" ShowHeader="true" DataKeys="Inv_No">
                                                                <Columns>
                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <%#Eval("Inv_No") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Debit/Credit">
                                                                        <ItemTemplate>
                                                                            <%#Eval("Debit_Credit") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="General Ledger">
                                                                        <ItemTemplate>
                                                                            <%#Eval("general_ledger_account") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Cost centre1">
                                                                        <ItemTemplate>
                                                                            <%#Eval("costcenter_1") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Cost centre2">
                                                                        <ItemTemplate>
                                                                            <%#Eval("costcenter_2") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="WBS No.">
                                                                        <ItemTemplate>
                                                                            <%#Eval("WBS_no") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Amount">
                                                                        <ItemTemplate>
                                                                            <%#Eval("amount","{0:F}") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="endfrm" runat="server" width="100%">
                                        <td>
                                            <table width="100%">
                                                <tr>
                                                    <td width="20%" valign="top" align="right">
                                                        <asp:Label ID="lblReason" Text="Reason for Rejection" runat="server"></asp:Label>
                                                    </td>
                                                    <td width="20%">
                                                        <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Width="200px" Height="70px"
                                                            ToolTip="Reason" MaxLength="250"></asp:TextBox>
                                                        <script type="text/javascript">
                                                            $(document).ready(function () {
                                                                $('#txtReason').ForAlphanumericNumericCommaFullstop();
                                                            });
                                                           
                                                        </script>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorReason" runat="server" ControlToValidate="txtReason"
                                                            ErrorMessage="Please enter Reason" Display="Dynamic" Text="*" ForeColor="Red">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td align="right">
                                                        <table align="right">
                                                            <tr align="right">
                                                                <td align="left" style="font-size: 12px; font-weight: bold">
                                                                    <b>
                                                                        <asp:Label ID="lblTotalLineAmount" runat="server" Text="Total Line Amount:"></asp:Label></b>
                                                                </td>
                                                                <td align="right" style="font-size: 17px; font-weight: bold">
                                                                    
                                                                    <asp:TextBox ID="txtTotalLineAmount" runat="server" ToolTip="Total Line Amount" ReadOnly="true"
                                                                        Style="text-align: right"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr align="right">
                                                                <td align="left" style="font-size: 12px; font-weight: bold">
                                                                    <b>
                                                                        <asp:Label ID="lblTotalAdditionalCharges" runat="server" Text="Total Additonal Charges:"></asp:Label></b>
                                                                </td>
                                                                <td align="right" style="font-size: 17px; font-weight: bold">
                                                                    
                                                                    <asp:TextBox ID="txtTotalAdditionalCharges" runat="server" ToolTip="Total Additonal Charges"
                                                                        ReadOnly="true" Style="text-align: right"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr align="right" style="font-size: 12px; font-weight: bold">
                                                                <td align="left">
                                                                    <b>
                                                                        <asp:Label ID="lblTotalInvoiceAmount" runat="server" Text="Total Invoice Amount:"></asp:Label></b>
                                                                </td>
                                                                <td align="right" style="font-size: 17px; font-weight: bold">
                                                                    
                                                                    <asp:TextBox ID="txtTotalInvoiceAmount" runat="server" ToolTip="Total Invoice Amount:"
                                                                        ReadOnly="true" Style="text-align: right"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnApprove" runat="server" Text="Approve" Width="120px" BackColor="#5D5D5D"
                                                            Font-Bold="True" ForeColor="White" OnClick="btnApprove_Click" CausesValidation="False" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnReject" runat="server" Text="Reject" Width="120px" BackColor="#5D5D5D"
                                                            Font-Bold="True" ForeColor="White" OnClick="btnReject_Click" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnCancel" runat="server" Text="Back" Width="120px" BackColor="#5D5D5D"
                                                            Font-Bold="True" ForeColor="White" OnClick="btnCancel_Click" CausesValidation="False" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
