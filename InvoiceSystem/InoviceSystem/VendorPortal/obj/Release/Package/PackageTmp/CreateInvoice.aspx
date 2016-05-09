<%@ Page Title="CreateInvoice" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CreateInvoice.aspx.cs" EnableEventValidation="false" Inherits="VendorPortal.CreateInvoice" %>

<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .gridView
        {
            table-layout: fixed;
        }
        .col
        {
            word-wrap: break-word;
        }
    </style>
    <link href="Styles/invsys.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.4.1.js"></script>
    <script src="Scripts/jquery-1.4.1.min.js"></script>
    <style type="text/css">
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=50);
            opacity: 0.5;
        }
    </style>
    <script type="text/javascript">


        function popup(mylink, windowname) {
            if (!window.focus) return true;
            var href;
            if (typeof (mylink) == 'string')
                href = mylink.href;
            window.open(href, windowname, 'width=700,height=400,scrollbars=yes');
            return false;
        }

        function displayMessage(printContent) {

            var inf = printContent;
            win = window.open("print.htm", 'popup', 'toolbar = no, status = yes, scrollbars=1 width=1200 height=1000');

            win.document.writeln(inf);
            win.document.close(); // new line 
        }
        function PrintMessage() {
            //var inf = printContent;

            window.print();
            //window.PrintMessage(printContent);
        }
        function calAmtLineDetails() {
            var txtqty = document.getElementById("txtqtyshipped");
            var txtu = document.getElementById("txtunitprice");
            var txtamt = document.getElementById("txtlineamt");

            if (txtqty.value != null && txtu.value != null) {
                txtamt.value = txtqty.value * txtu.value;


            }
        }





        function ValidateTextboxOfLineItemGrid() {
            //            debugger;
            var txtqty = document.getElementById("txtqtyshipped");
            var txtu = document.getElementById("txtunitprice");
            var txtamt = document.getElementById("txtlineamt");
            var txtPartNumber = document.getElementById("txtprtnumbr");
            var txtPONumber = document.getElementById("txtponumbr");
            var txtReleaseNbr = document.getElementById("txtrelease");
            var QuantityUnitMeasure = document.getElementById("ddlQtyUnitofMeasure");


            if (txtReleaseNbr.disabled == true) {
                alert('Cannot update data in line item details');
            }
            else {
                if (txtqty.value.length > 0 && txtu.value.length > 0 && txtamt.value.length > 0 && txtPartNumber.value.length > 0 && txtPONumber.value.length > 0 && txtReleaseNbr.value.length > 0 && QuantityUnitMeasure.value > 1) {

                    //                    if (txtqty.value <= 150) {
                    //                        return true;
                    //                    }
                    //                    else {
                    //                        alert('Quantity Shipped should be less or equal to 150');
                    //                        txtqty.value = '';
                    return false;
                    //                    }

                }
                else {
                    alert('Please enter mandatory fields of Line Items Details Section');
                    return false;
                }

            }

        }

        function ValidateTextbox(rowcounthidden) {
            // debugger;
            var txtqty = document.getElementById("txtqtyshipped");
            var txtu = document.getElementById("txtunitprice");
            var txtamt = document.getElementById("txtlineamt");
            var txtPartNumber = document.getElementById("txtprtnumbr");
            var txtPONumber = document.getElementById("txtponumbr");
            var txtReleaseNbr = document.getElementById("txtrelease");
            var QuantityUnitMeasure = document.getElementById("ddlQtyUnitofMeasure");
            //var gvlinedetcount = document.getElementById('rowcounthidden');

            if (rowcounthidden.value > 0) {
                return true;

            }
            else {
                alert('Please enter mandatory fields in Line Item Details Section ');
                return false;


            }

        }


        function ValidateTextboxOfAdditionalChargeGrid() {
            //            debugger;
            var txtChargenumber = document.getElementById("txtchrge");
            var textAmount = document.getElementById("txtaddamt");
            var textDescription = document.getElementById("txtdescp");
            var textGST = document.getElementById("txtgst");
            var ChargeType = document.getElementById("ddlchtgetype");
            var charge = document.getElementById("txtchrge");
            if (txtChargenumber.disabled == true) {
                alert('Cannot update data in line item details');
            }
            else {
//               // if (txtChargenumber.value.length > 0 && textAmount.value.length > 0 && textDescription.value.length > 0 && textGST.value.length > 0 && ChargeType.value > 1 && charge.value.length > 0) {
//                 //   return true;
//               // }
//               // else {
//               //     alert('Please enter mandatory fields of Additional Charge Section');
//               //     return false;
//               // }
            }
        }


        function ValidateTextboxOfAccountDistributionGrid() {

            var textGeneralLedger = document.getElementById("txtgen");
            var textCostCentre1 = document.getElementById("txtcs1");

            var textamount = document.getElementById("txtamt");
            //var DebitandCredit = document.getElementById("txtbdcr");
            // var WBSNumber = document.getElementById("txtwbs");
            //&& DebitandCredit.value.length > 0 && WBSNumber.value.length > 0
            if (textGeneralLedger.disabled == true) {
                alert('Cannot update data in line item details');
            }
            else {
                if (textGeneralLedger.value.length > 0 && textCostCentre1.value.length > 0 && textamount.value.length > 0) {
                    return true;
                }
                else {
                    alert('Please enter mandatory fields of Account Distribution Section');
                    return false;
                }
            }
        }



        //jquery function for allowing only mumbers
        jQuery.fn.ForNumericOnly =
            function () {
                return this.each(function () {
                    $(this).keydown(function (event) {
                        var keycode = (event.keyCode ? event.keyCode : event.which);
                        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9) {
                            return true; // let it happen, don't do anything
                        }
                        else {
                            // Ensure that it is a number and stop the keypress
                            if ((event.keyCode < 48 || event.keyCode > 57 || event.shiftKey) && (event.keyCode < 96 || event.keyCode > 105)) {
                                event.preventDefault();
                            }
                        }

                    });

                });

            };

        //Unit price and Quantity shipped accepting negative values
        jQuery.fn.ForNumericMinusOnly =
            function () {
                //debugger;
                return this.each(function () {
                    $(this).keydown(function (event) {
                        var keycode = (event.keyCode ? event.keyCode : event.which);
                        //alert(keycode);
                        var str = String.fromCharCode(!event.charCode ? event.which : event.charCode);

                        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 173 || event.keyCode == 109 || event.keyCode == 189) {

                            return true; // let it happen, don't do anything
                        }
                        else {
                            // Ensure that it is a number and stop the keypress
                            if ((event.keyCode < 48 || event.keyCode > 57 || event.shiftKey) && (event.keyCode < 96 || event.keyCode > 105)) {
                                event.preventDefault();
                            }
                        }

                    });

                });

            };

        jQuery.fn.ForAlphanumericNumericOnly =
            function () {
                //                debugger;
                return this.each(function () {
                    $(this).keypress(function (e) {

                        var regex = new RegExp("^[a-zA-Z0-9]+$");

                        // var unregex = new RegExp("^[/.,<>!@#$%^&*()_+={}[]\|?]");
                        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);

                        if (regex.test(str) || e.keyCode == 46 || e.keyCode == 8 || e.keyCode == 9) {
                            return true;
                        }
                        else {
                            e.preventDefault();
                            return false;
                        }
                    });
                });

            };



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

        function ValidateShippedDateGreaterThanInvoiceDate() {
            // debugger;

            var date = new Date();
            var textDate = (date.getMonth() + 1) + "/" + date.getDate() + "/" + date.getFullYear().toString().substr(0, 4);

            var InvoiceDatelen = document.getElementById("txtInvoiceDate").value;
            var ShippedDatelen = document.getElementById("txtShippedDate").value;
            var InvoiceDate = new Date(document.getElementById('txtInvoiceDate').value);
            var ShippedDate = new Date(document.getElementById('txtShippedDate').value);
            //InvoiceDate = Date.parse(document.getElementById('txtInvoiceDate').value);
            //ShippedDate = Date.parse(document.getElementById('txtShippedDate').value);


            if (ShippedDatelen.length > 0) {
                var value = isValidDateFormat(ShippedDatelen);
                if (value == false) {
                    document.getElementById("txtShippedDate").value = textDate;
                    return false;
                }

            }

            if (InvoiceDatelen.length > 0) {
                var value = isValidDateFormat(InvoiceDatelen);
                if (value == false) {
                    document.getElementById("txtInvoiceDate").value = textDate;
                    return false;
                }

            }


            var hidinvoicedateval = document.getElementById("hidinvoicedate").value;
            var hidshippeddateval = document.getElementById("hidshippeddate").value; //hidcount
            var hidcount = document.getElementById("hidcount").value;



            if (document.getElementById("hidIsFromDraft").value == "draft" || document.getElementById("hidIsFromDraft").value == "inv") {


                InvoiceDate = new Date(hidinvoicedateval);
                ShippedDate = new Date(hidshippeddateval);

                document.getElementById('txtInvoiceDate').value = hidinvoicedateval;
                document.getElementById("txtShippedDate").value = hidshippeddateval;

                if (document.getElementById("hidcount").value == "true") {
                    if (document.getElementById("hidIsFromDraft").value == "inv") {
                        var txtinv = document.getElementById('txtInvoiceDate');
                        txtinv.disabled = true;
                        var txtship = document.getElementById("txtShippedDate");
                        txtship.disabled = true;
                    }
                    document.getElementById("hidIsFromDraft").value = "";
                }
                if (document.getElementById("hidcount").value == "false") {
                    document.getElementById("hidcount").value = "true";
                }
                // document.getElementById("hidIsFromDraft").value = "false";
            }
            if (InvoiceDatelen.length > 0 && ShippedDatelen.length > 0) {

                if (ShippedDate > InvoiceDate) {
                    alert("Invoice date should be between Shipped date and Current Date");
                    document.getElementById("txtInvoiceDate").value = textDate;
                }
            }
        }

        //validation for date format
        function isValidDateFormat(dateString) {
            //debugger;
            // First check for the pattern  if (!/^\d{2}\/\d{2}\/\d{4}$/.test(dateString))
            var parts = dateString.split("/");
            var day = parseInt(parts[1], 10);
            var month = parseInt(parts[0], 10);
            var year = parseInt(parts[2], 10);
            var dateformat;
            if (day < 10 && month < 10) {
                var dateformat = "0" + month + "/0" + day + "/" + year;
            }
            else {
                if (day < 10) {

                    var dateformat = month + "/0" + day + "/" + year;
                }
                if (month < 10) {
                    var dateformat = "0" + month + "/" + day + "/" + year;
                }
            }

            if (day < 10 || month < 10) {
                if (!/^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/.test(dateformat)) {

                    alert("Invalid date format");
                    return false;
                }
            }

            else {
                if (!/^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/.test(dateString)) {

                    alert("Invalid date format");
                    return false;
                }
            }



            // Check the ranges of month and year
            if (year < 1000 || year > 3000 || month == 0 || month > 12) {
                alert("Invalid date format");
                return false;
            }

            var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

            // Adjust for leap years
            if (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0))
                monthLength[1] = 29;

            // Check the range of the day
            if (day > monthLength[month - 1]) {
                alert("Invalid Date format");
                return false;
            }
            else if (day > 0 && day <= monthLength[month - 1])
                return true;
            //return day > 0 && day <= monthLength[month - 1];


        };

        function checkDate(sender, args) {
            //debugger;
            document.getElementById("hidIsFromDraft").value = "false";
            if (sender._selectedDate > new Date()) {
                alert("No future Date allowed for Invoice Date!");
            }
            else {
                var InvoiceDate = document.getElementById('<%=txtInvoiceDate.ClientID%>');
                var ShippedDate = document.getElementById('<%=txtShippedDate.ClientID%>');
                if (new Date(ShippedDate.value) > new Date(InvoiceDate.value)) {
                    alert("Invoice date should be between shipped date and Current Date");
                }

            }
        }

        function checkDate1(sender, args) {
            // debugger;
            document.getElementById("hidIsFromDraft").value = "false";

            if (sender._selectedDate > new Date()) {
                alert("No future Date allowed for Shipped Date!");
            }


            else {
                var InvoiceDate = document.getElementById('<%=txtInvoiceDate.ClientID%>');
                var ShippedDate = document.getElementById('<%=txtShippedDate.ClientID%>');
                if (new Date(ShippedDate.value) > new Date(InvoiceDate.value)) {
                    alert("Invoice date should be between shipped date and Current Date");
                }

            }

        }
        //            
     
 

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" ClientIDMode="Static"
    runat="server">
    <asp:UpdatePanel ID="Updatepanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <script type="text/javascript">
                $(document).ready(function () {

                    $('#txtpoamend').ForAlphanumericNumericOnly();
                    $('#txtrelease').ForAlphanumericNumericOnly();
                    $('#txtqtyshipped').ForNumericMinusOnly();
                    $('#txtunitprice').ForNumericMinusOnly();
                    $('#txtlineamt').ForNumericOnly();
                    $('#txtpack').ForAlphanumericNumericOnly();
                    $('#txtbilloflading').ForAlphanumericNumericOnly();
                    $('#txtprtnumbr').ForAlphanumericNumericOnly();
                    $('#txtponumbr').ForAlphanumericNumericOnly();
                    $('#txtgst').ForAlphanumericNumericOnly();
                    $('#txtdescp').ForAlphanumericNumericCommaFullstop();
                    $('#txtaddamt').ForNumericOnly();
                    $('#txtchrge').ForNumericOnly();
                    $('#txtchrgenmber').ForNumericOnly();
                    $('#txtamt').ForNumericOnly();
                    $('#txtwbs').ForNumericOnly();
                    $('#tctcs2').ForAlphanumericNumericOnly();
                    $('#txtcs1').ForAlphanumericNumericOnly();
                    $('#txtgen').ForNumericOnly();
                    $('#txtbdcr').ForAlphanumericNumericOnly();
                    $('#txtComm').ForAlphanumericNumericCommaFullstop();
                    $('#txtFinalDestination').ForAlphanumericNumericCommaFullstop();

                });
                var prm = Sys.WebForms.PageRequestManager.getInstance();

                prm.add_endRequest(function () {
                    $('#txtpoamend').ForAlphanumericNumericOnly();
                    $('#txtrelease').ForAlphanumericNumericOnly();
                    $('#txtqtyshipped').ForNumericMinusOnly();
                    $('#txtunitprice').ForNumericMinusOnly();
                    $('#txtlineamt').ForNumericOnly();
                    $('#txtpack').ForAlphanumericNumericOnly();
                    $('#txtbilloflading').ForAlphanumericNumericOnly();
                    $('#txtprtnumbr').ForAlphanumericNumericOnly();
                    $('#txtponumbr').ForAlphanumericNumericOnly();
                    $('#txtgst').ForAlphanumericNumericOnly();
                    $('#txtdescp').ForAlphanumericNumericCommaFullstop();
                    $('#txtaddamt').ForNumericOnly();
                    $('#txtchrge').ForNumericOnly();
                    $('#txtchrgenmber').ForNumericOnly();
                    $('#txtamt').ForNumericOnly();
                    $('#txtwbs').ForNumericOnly();
                    $('#tctcs2').ForAlphanumericNumericOnly();
                    $('#txtcs1').ForAlphanumericNumericOnly();
                    $('#txtgen').ForNumericOnly();
                    $('#txtbdcr').ForAlphanumericNumericOnly();
                    $('#txtComm').ForAlphanumericNumericCommaFullstop();
                    $('#txtFinalDestination').ForAlphanumericNumericCommaFullstop();

                });</script>
            <div id="divbody" runat="server">
                <table style="height: 500px; width: 1000px; backcolor: #F7F7F7" cellpadding="0" cellspacing="0"
                    border="0">
                    <tr valign="top" align="center" style="font-family: Dotum; font-size: x-large; font-weight: bold;
                        font-style: normal; color: #0033CC">
                        <td>
                            <table align="center" width="900px" style="font-family: Verdana; color: #006666;
                                font-size: 12px;">
                                <tr>
                                    <td align="center" class="Invallheader">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        Create Invoice
                                    </td>
                                    <%-- <td width="100px" align="right">
                            <asp:LinkButton ID="PrintPriview" Text="Print Preview"  runat="server"
                                OnClientClick="displayMessage(body.innerHTML)" CausesValidation="false"></asp:LinkButton>
                                    </td>--%>
                                    <td width="100px" align="right" id="exp" runat="server">
                                        <asp:LinkButton ID="ExportWord" runat="server" Text="Export To" CausesValidation="false"
                                            OnClick="ExportExcel_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" >
                                        <div style="background-color: #F7F7F7; width: 1150px">
                                            <table w>
                                                <tr id="inv_dtls" runat="server" style="height: 80%; width: 90%">
                                                    <td style="width:90%">
                                                        <table style="width:100%">
                                                            <tr style="width: 100%">
                                                                <td align="left" class="InvallSubheaders" colspan="6" width="1120px">
                                                                    Invoice Details
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table cellpadding="3" align="left">
                                                            <tr>
                                                                <td colspan="6" align="left">
                                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="true"
                                                                        ForeColor="Red" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="6" align="left" width="60%">
                                                                    <asp:Label runat="server" ID="errmessage" Font-Bold="true" ForeColor="Red"></asp:Label>
                                                                    <br />
                                                                    <asp:Label ID="txtstatusTimestamp" runat="server" ForeColor="Green"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" valign="top" width="120px" colspan="1">
                                                                    <b>
                                                                        <asp:Label ID="Label8" runat="server" Text="Supplier Code:" Width="100px"></asp:Label></b>
                                                                    <asp:Label ID="Label9" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top" colspan="2">
                                                                    <asp:TextBox ID="txtSupplierCode" runat="server" ToolTip="Supplier Code" TabIndex="1"
                                                                        ReadOnly="true" Enabled="false"></asp:TextBox>
                                                                    <asp:LinkButton ID="lbnSupplierCodeLookup" runat="server" OnClick="lbnSupplierCodeLookup_Click"
                                                                        CausesValidation="False"><img src="Images/Lookup.png"/></asp:LinkButton>
                                                                    <%--<asp:LinkButton ID="lbnSupplierCodeLookup" runat="server" OnClick="return popup(this,example)"></asp:LinkButton>--%>
                                                                </td>
                                                                <td align="left" valign="top" width="130px" colspan="1">
                                                                    <asp:HiddenField ID="hidinvoicedate" runat="server" />
                                                                    <b>
                                                                        <asp:Label ID="lblInvoiceNumber" runat="server" Text="Invoice Number:"></asp:Label></b>
                                                                    <asp:Label ID="Label4" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                                    <asp:HiddenField ID="hidshippeddate" runat="server" />
                                                                </td>
                                                                <td align="left" valign="top" colspan="2">
                                                                    <asp:TextBox ID="txtInvoiceNumber" runat="server" ToolTip="Invoice Number" TabIndex="2"
                                                                        ReadOnly="true" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" valign="top" colspan="1">
                                                                    <b>
                                                                        <asp:Label ID="lblShippedTo" runat="server" Text="Shipped To:"></asp:Label></b>
                                                                    <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top" width="230px" colspan="2">
                                                                    <asp:DropDownList ID="ddlShippedTo" Width="155px" Height="22px" runat="server" ToolTip="Shipped To"
                                                                        TabIndex="3">
                                                                    </asp:DropDownList>
                                                                    <asp:CustomValidator ID="CustValDdlShippedTo" runat="server" ErrorMessage="Please select Shipped To"
                                                                        ForeColor="white" Text="*" OnServerValidate="CustValForDdlShippedTo"></asp:CustomValidator>
                                                                </td>
                                                                <td align="left" colspan="1">
                                                                    <b>
                                                                        <asp:Label ID="lblInvoiceTo" runat="server" Text="Invoice To:"></asp:Label></b>
                                                                    <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                                </td>
                                                                <td align="left" colspan="2">
                                                                    <asp:TextBox ID="txtInvoiceTo" runat="server" ToolTip="Invoice To" TabIndex="4" ReadOnly="True"
                                                                        Enabled="false"></asp:TextBox>
                                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatorInvoiceTo" runat="server" ControlToValidate="txtInvoiceTo"
                                                                        ErrorMessage="Please enter Invoice To" Display="Dynamic" ForeColor="white" Text="*">
                                                                    </asp:RequiredFieldValidator>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" valign="top" colspan="1">
                                                                    <b>
                                                                        <asp:Label ID="lblPayableTo" runat="server" Text="Payable To:"></asp:Label></b>
                                                                    <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top" colspan="2">
                                                                    <asp:DropDownList ID="ddlPayableTo" Width="155px" Height="22px" runat="server" ToolTip="Payable To"
                                                                        TabIndex="5" AutoPostBack="True" OnSelectedIndexChanged="ddlPayableTo_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <asp:CustomValidator ID="CustValDdlPayableTo" runat="server" ErrorMessage="Please select Payable To"
                                                                        OnServerValidate="CustValForDdlPayableTo" ForeColor="white" Text="*"></asp:CustomValidator>
                                                                </td>
                                                                <td align="left" valign="top" colspan="1">
                                                                    <b>
                                                                        <asp:Label ID="lblCurrency" runat="server" Text="Currency:"></asp:Label></b>
                                                                </td>
                                                                <td align="left" valign="top" colspan="2">
                                                                    <asp:TextBox ID="txtCurrency" runat="server" ToolTip="Supplier Currency" TabIndex="6"
                                                                        ReadOnly="false" ></asp:TextBox>
                                                                      <ajax:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtCurrency"
                                                                            MinimumPrefixLength="1" EnableCaching="true" CompletionSetCount="1" CompletionInterval="1000" ServiceMethod="GetCountries"
                                                                            DelimiterCharacters=";, :"
                                                                            ShowOnlyCurrentWordInCompletionListItem="true" >
                                                                      </ajax:AutoCompleteExtender>   

                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" valign="top" colspan="1">
                                                                    <b>
                                                                        <asp:Label ID="lblEmailAddress" runat="server" Text="E-mail Address:"></asp:Label></b>
                                                                    <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top" colspan="2">
                                                                    <asp:TextBox ID="txtEmailAddress" runat="server" ToolTip="Email Address" TabIndex="7"
                                                                        Width="217px" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                                    <asp:HiddenField ID="hidcount" runat="server" />
                                                                </td>
                                                                <td valign="top" align="left" colspan="1">
                                                                    <b>
                                                                        <asp:Label ID="lblSupplierAddress" runat="server" Text="Supplier Address:"></asp:Label></b>
                                                                </td>
                                                                <td align="left" valign="top" colspan="2">
                                                                    <asp:HiddenField ID="hidIsFromDraft" runat="server" />
                                                                    <asp:TextBox ID="txtSupplierAddress" runat="server" Width="223px" Height="70px" TextMode="MultiLine"
                                                                        ToolTip="Supplier Address" TabIndex="8" ReadOnly="true" Font-Names="Verdana"
                                                                        Font-Size="Small"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" valign="top" colspan="1">
                                                                    <b>
                                                                        <asp:Label ID="lblFinalDestination" runat="server" Text="Final Destination:"></asp:Label></b>
                                                                </td>
                                                                <td align="left" valign="top" colspan="2">
                                                                    <asp:TextBox ID="txtFinalDestination" runat="server" Width="231px" Height="70px"
                                                                        TextMode="MultiLine" MaxLength="250" ToolTip="Final Destination" TabIndex="9"
                                                                        Font-Names="Verdana" Font-Size="Small"></asp:TextBox>
                                                                    <script type="text/javascript">
                                                                           
                                                                    </script>
                                                                </td>
                                                                <td align="left" valign="top" colspan="1">
                                                                    <b>
                                                                        <asp:Label ID="lblComments" runat="server" Text="Comments:"></asp:Label></b>
                                                                </td>
                                                                <td align="left" valign="top" colspan="2">
                                                                    <%-- <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Width="232px" Height="70px"
                                                                        MaxLength="250" ToolTip="Comments" TabIndex="10" Font-Names="Verdana" Font-Size="Small"></asp:TextBox>--%>
                                                                    <asp:TextBox ID="txtComments" runat="server" Width="231px" Height="70px" TextMode="MultiLine"
                                                                        MaxLength="250" ToolTip="Comments" TabIndex="10" Font-Names="Verdana" Font-Size="Small"></asp:TextBox>
                                                                    <script type="text/javascript">
                                                                         
                                                                    </script>
                                                                    <asp:CustomValidator ID="CusValComment" runat="server" Text="*" ForeColor="Red" ErrorMessage="Please enter comments. "
                                                                        Display="Dynamic" OnServerValidate="CustValForComment"></asp:CustomValidator>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="listi" runat="server">
                                                    <td>
                                                        <table style="width:100%">
                                                            <tr style="width:100%">
                                                                <td align="left" class="InvallSubheaders" colspan="6" style="width:100%">
                                                                    Order Details
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table cellpadding="3" align="left">
                                                            <tr valign="top">
                                                                <td align="left" width="150px" colspan="1">
                                                                    <b>
                                                                        <asp:Label ID="lblShippedDate" runat="server" Text="Shipped Date:"></asp:Label></b>
                                                                    <asp:Label ID="Label6" runat="server" Text="*" ForeColor="Red"></asp:Label>&nbsp;&nbsp;&nbsp;
                                                                </td>
                                                                <td align="left" width="280px" colspan="2">
                                                                    <asp:TextBox runat="server" ID="txtShippedDate" ToolTip="mm/dd/yyyy" class="water"
                                                                        TabIndex="11" ViewStateMode="Enabled"></asp:TextBox>
                                                                    <ajax:CalendarExtender ID="CalendarExtenderShippedDate" TargetControlID="txtShippedDate"
                                                                        runat="server" OnClientDateSelectionChanged="checkDate1" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorShippedDate" runat="server"
                                                                        ControlToValidate="txtShippedDate" ErrorMessage="Please select Shipped Date"
                                                                        Display="Dynamic" ForeColor="white" Text="*">
                                                                    </asp:RequiredFieldValidator>
                                                                    <%--  <asp:CustomValidator ID="CusValShippedDate" runat="server" ErrorMessage="Shipped date cannot be future date" Display="Dynamic" OnServerValidate="CustValForShippedDate">*</asp:CustomValidator>--%>
                                                                </td>
                                                                <td align="left" width="150px" colspan="1">
                                                                    <b>
                                                                        <asp:Label ID="lblInvoiceDate" runat="server" Text="Invoice Date:"></asp:Label></b>
                                                                    <asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                                </td>
                                                                <td align="left" width="280px" colspan="2">
                                                                    <asp:TextBox ID="txtInvoiceDate" runat="server" ToolTip="mm/dd/yyyy" TabIndex="12"></asp:TextBox>
                                                                    <ajax:CalendarExtender ID="CalendarExtenderInvoiceDate" TargetControlID="txtInvoiceDate"
                                                                        runat="server" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorInvoiceDate" runat="server"
                                                                        ControlToValidate="txtInvoiceDate" ErrorMessage="Please select Invoice Date"
                                                                        Display="Dynamic" ForeColor="white" Text="*">
                                                                    </asp:RequiredFieldValidator>
                                                                    <%-- <asp:CustomValidator ID="CusValInvoiceDate" ClientValidationFunction="ValidateDate" runat="server" ErrorMessage="Invoice date should not be less than current date" OnServerValidate="CustValForInvoiceDate">*</asp:CustomValidator>--%>
                                                                </td>
                                                                <%--<td width="220px"></td>--%>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" colspan="1">
                                                                    <b>
                                                                        <asp:Label ID="lblShippedVia" runat="server" Text="Shipped Via:"></asp:Label></b>
                                                                    <asp:Label ID="Label10" runat="server" Text="*" ForeColor="Red"></asp:Label>&nbsp;&nbsp;&nbsp;
                                                                </td>
                                                                <td align="left" colspan="2">
                                                                    <asp:DropDownList ID="ddlShippedVia" runat="server" Width="155px" Height="22px" ToolTip="Shipped Via"
                                                                        TabIndex="13" OnSelectedIndexChanged="ddlShippedVia_SelectedIndexChanged" AutoPostBack="true">
                                                                    </asp:DropDownList>
                                                                    <asp:TextBox ID="txtOther" runat="server" Visible="false"></asp:TextBox>
                                                                    <asp:CustomValidator ID="CustValDdlShippedVia" runat="server" ErrorMessage="Please select Shipped Via"
                                                                        OnServerValidate="CustValForDdlShippedVia" ForeColor="white" Text="*"></asp:CustomValidator>
                                                                </td>
                                                                <td align="left" colspan="1">
                                                                    <b>Attachments:</b>
                                                                </td>
                                                                <td valign="top" align="left" colspan="2">
                                                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
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
                                                                    <script type="text/javascript">
                                                                        function SetData(str) {
                                                                            // debugger;
                                                                            //var hid = document.getElementById("HiddenField1").value;
                                                                            // var lnk = document.getElementById("link1").value;
                                                                            //               var features = "menubar=no,location=no,resizable,scrollbars,status=no,width=800,height=600,top=10,left=10";
                                                                            //               var newWindow = window.open('WebForm1.aspx',hid,features);
                                                                            //var newWindow = window.open(hid);
                                                                            //newWindow.focus();
                                                                            //alert(hid);

                                                                        }
                                                                        function onlyNumeric() {
                                                                            //debugger;
                                                                            if ((event.keyCode > 47 && event.keyCode < 58) || event.keyCode == 45) {
                                                                                event.returnValue = true;
                                                                            }
                                                                            else {
                                                                                event.returnValue = false;
                                                                            }
                                                                        }


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
                                                                        function ValidateLengthComments(txtfinal) {
                                                                            //debugger;
                                                                            var final = document.getElementById(txtfinal);
                                                                            if (final.value.length > 250) {

                                                                                event.returnValue = false;

                                                                            }
                                                                        }

                                                                    </script>
                                                                    <asp:Label ID="lblmessage" runat="server" />
                                                                    <asp:GridView ID="gvDetails" runat="server" AutoGenerateColumns="false" DataKeyNames="File_name"
                                                                        OnRowCommand="gvDetails_RowCommand" OnRowCreated="gvDetails_RowCreated" OnRowDeleting="gvDetails_RowDeleting">
                                                                        <HeaderStyle BackColor="#cccccc" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Attachment_id" HeaderText="Id" Visible="false" />
                                                                            <asp:TemplateField HeaderText="File_name">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="link1" CommandName="Click" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>'
                                                                                        runat="server" OnClientClick="SetData()" CausesValidation="false" OnPreRender="addTrigger_PreRender"><%# Eval("File_name")%></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkFileDelete" Text="Delete" runat="server" CommandName="Delete"
                                                                                        CausesValidation="false" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>'></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                    <asp:Label ID="UploadStatusLabel" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
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
                                                                    <br />
                                                                    <asp:Button ID="AttachButton" runat="server" Text="Attach File" Width="120px" BackColor="#5D5D5D"
                                                                        Font-Bold="True" ForeColor="White" ToolTip="Attach selected file" OnClick="btnsave_Click"
                                                                        CausesValidation="False" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="line_items" runat="server">
                                                    <td>
                                                        <%--changed from width 99 to 100% by sachin--%>
                                                        <table width="100%" align="center" style="backcolor: #F7F7F7">
                                                            <tr>
                                                                <td align="left" class="InvallSubheaders" colspan="8" width="100%">
                                                                    <b>Line Item Details</b>
                                                                </td>
                                                            </tr>
                                                            <tr id="linitemtxtboxes" runat="server">
                                                                <td>
                                                                    <table width="94%" cellpadding="5" style="font-family: Verdana; color: #006666; font-size: 12px;">
                                                                        <tr>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lblprtnumber" runat="server" Text="Part/Item Number:"></asp:Label><asp:Label
                                                                                        ID="Label11" runat="server" Text="*" ForeColor="Red"></asp:Label></b>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtprtnumbr" runat="server" TabIndex="14" MaxLength="15"></asp:TextBox>
                                                                                <%-- Onkeypress=" Nospecialchar()"--%>
                                                                                <script type="text/javascript">                                                                                    $('#txtprtnumbr').ForAlphanumericNumericOnly();</script>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lblponumber" runat="server" Text="PO Number:"></asp:Label><asp:Label
                                                                                        ID="Label17" runat="server" Text="*" ForeColor="Red"></asp:Label></b>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtponumbr" runat="server" TabIndex="15" MaxLength="15"></asp:TextBox>
                                                                                <script type="text/javascript">                                                                                    $('#txtponumbr').ForAlphanumericNumericOnly();</script>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lblpoamend" runat="server" Text="PO Amendment Number:"></asp:Label></b>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtpoamend" runat="server" TabIndex="16" MaxLength="15"></asp:TextBox>
                                                                                <script type="text/javascript">                                                                                    $('#txtpoamend').ForAlphanumericNumericOnly();</script>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lblRel" runat="server" Text="Release/Requisition Number:"></asp:Label><asp:Label
                                                                                        ID="Label14" runat="server" Text="*" ForeColor="Red"></asp:Label></b>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtrelease" runat="server" TabIndex="17" MaxLength="15"></asp:TextBox>
                                                                                <script type="text/javascript">                                                                                    $('#txtrelease').ForAlphanumericNumericOnly();</script>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lblqtyshipped" runat="server" Text="Quantity Shipped:"></asp:Label><asp:Label
                                                                                        ID="Label13" runat="server" Text="*" ForeColor="Red"></asp:Label></b>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtqtyshipped" runat="server" TabIndex="18" MaxLength="7"></asp:TextBox>
                                                                                <script type="text/javascript">                                                                                    $('#txtqtyshipped').ForNumericMinusOnly();</script>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lblunit" runat="server" Text="Unit Price:"></asp:Label></b><asp:Label
                                                                                        ID="Label16" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtunitprice" runat="server" TabIndex="19" MaxLength="7" Style="text-align: right"></asp:TextBox>
                                                                                <script type="text/javascript">                                                                                    $('#txtunitprice').ForNumericMinusOnly();</script>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lblamt" runat="server" Text="Amount:"></asp:Label><asp:Label ID="Label12"
                                                                                        runat="server" Text="*" ForeColor="Red"></asp:Label></b>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtlineamt" runat="server" Enabled="False" TabIndex="20" Style="text-align: right"></asp:TextBox>
                                                                                <script type="text/javascript">                                                                                    $('#txtlineamt').ForNumericOnly();</script>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lblQtyunitMeasure" runat="server" Text="Quantity Unit Of Measure:"></asp:Label><asp:Label
                                                                                        ID="Label15" runat="server" Text="*" ForeColor="Red"></asp:Label></b>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="ddlQtyUnitofMeasure" runat="server" ToolTip="Qty Unit of Measure"
                                                                                    Width="150px" TabIndex="21">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lblpackingslip" runat="server" Text="Packing Slip:"></asp:Label></b>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtpack" runat="server" TabIndex="22" MaxLength="15"></asp:TextBox>
                                                                                <script type="text/javascript">                                                                                    $('#txtpack').ForAlphanumericNumericOnly();</script>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top" align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lblbill" runat="server" Text="Bill Of Lading:"></asp:Label></b>
                                                                            </td>
                                                                            <td valign="top" align="left">
                                                                                <asp:TextBox ID="txtbilloflading" runat="server" TabIndex="23" MaxLength="15"></asp:TextBox>
                                                                                <script type="text/javascript">                                                                                    $('#txtbilloflading').ForAlphanumericNumericOnly();</script>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td valign="top" align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lblComm" runat="server" Text="Comments:"></asp:Label></b>
                                                                            </td>
                                                                            <td colspan="3" valign="top" align="left">
                                                                                <asp:TextBox ID="txtComm" TextMode="MultiLine" runat="server" Width="190px" Height="70px"
                                                                                    TabIndex="24" MaxLength="256"></asp:TextBox>
                                                                                <script type="text/javascript">                       </script>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
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
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td valign="middle" align="right" colspan="2">
                                                                                <asp:Button ID="btnadd" runat="server" Text="Add Line Item" Width="150px" BackColor="#5D5D5D"
                                                                                    TabIndex="25" Font-Bold="True" ForeColor="White" ToolTip="Add Line Item" CausesValidation="False"
                                                                                    OnClick="btnadd_Click" />
                                                                                <input type="hidden" id="rowcounthidden" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
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
                                                                            <td valign="middle" align="center" colspan="2">
                                                                                <asp:Button ID="Updatetnfoelne" runat="server" Text="Update" Width="150px" BackColor="#5D5D5D"
                                                                                    Font-Bold="True" ForeColor="White" ToolTip="Update" CausesValidation="False"
                                                                                    OnClick="Updatetnfoelne_Click" />
                                                                                <input type="hidden" id="Hidden2" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td valign="middle" align="center" colspan="2">
                                                                                <asp:Button ID="Cancelbtnforlne" runat="server" Text="Cancel" Width="150px" BackColor="#5D5D5D"
                                                                                    Font-Bold="True" ForeColor="White" ToolTip="Cancel" OnClick="Cancelbtnforlne_Click"
                                                                                    CausesValidation="False" />
                                                                                <input type="hidden" id="Hidden1" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <div style="overflow: auto; width: 1100px; height:100px"> 

                                                            <asp:GridView ID="gvLineItemDetails" runat="server" AutoGenerateColumns="false" Width="1190"
                                                                ShowHeader="true" ShowFooter="true" OnRowEditing="gvLineItemDetails_RowEditing"
                                                                OnRowDeleting="gvLineItemDetails_RowDeleting" TabIndex="18" CssClass="gridView" VerticalAlign="Bottom">
                                                                <HeaderStyle BackColor="#cccccc" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Sr.No.">
                                                                        <ItemTemplate>
                                                                            <%# Container.DataItemIndex + 1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Part/Item Number *" ItemStyle-CssClass="col">
                                                                        <ItemTemplate>
                                                                            <%#Eval("part_item_no") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PO Number *" ItemStyle-CssClass="col">
                                                                        <ItemTemplate>
                                                                            <%#Eval("po_no") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PO Amendment Number" ItemStyle-CssClass="col">
                                                                        <ItemTemplate>
                                                                            <%#Eval("po_amendment_no") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Release /Requistion Number *" ItemStyle-CssClass="col">
                                                                        <ItemTemplate>
                                                                            <%#Eval("release_no") %>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Wrap="true" Width="13%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Quantity Shipped *" ItemStyle-CssClass="col">
                                                                        <ItemTemplate>
                                                                            <%#Eval("qty_shipped") %>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Wrap="true" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Unit Price*" ItemStyle-CssClass="col">
                                                                        <ItemTemplate>
                                                                            <%#Eval("unit_price", "{0:F}")%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Amount*" ItemStyle-CssClass="col">
                                                                        <ItemTemplate>
                                                                            <%#Eval("amount","{0:F}") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Qty Unit of Measure *" ItemStyle-CssClass="col">
                                                                        <ItemTemplate>
                                                                            <%#Eval("qty_unitofmeasure_id")%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="15%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Packing Slip" ItemStyle-CssClass="col">
                                                                        <ItemTemplate>
                                                                            <%#Eval("packing_slip") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Bill of Lading" ItemStyle-CssClass="col">
                                                                        <ItemTemplate>
                                                                            <%#Eval("bill_lading") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Comments" ItemStyle-Wrap="true" ItemStyle-CssClass="col">
                                                                        <ItemTemplate>
                                                                            <%#Eval("Comments") %>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Wrap="true" Width="100px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkEdit" Text="Edit" ToolTip="Edit" runat="server" CausesValidation="false"
                                                                                CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkDelete" Text="Delete" ToolTip="Delete" runat="server" CausesValidation="false"
                                                                                CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>'></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr id="ad_crgs" runat="server">
                                                    <td>
                                                        <%--changed from width 99 to 100% by sachin--%>
                                                        <table width="100%" align="center" style="backcolor: #F7F7F7">
                                                            <tr>
                                                                <td align="left" class="InvallSubheaders" width="100%">
                                                                    <b>Additional Charge Details</b>
                                                                </td>
                                                            </tr>
                                                            <tr id="addchargetxtboxes" runat="server">
                                                                <td>
                                                                    <table width="94%" cellpadding="5" style="font-family: Verdana; color: #006666; font-size: 12px;">
                                                                        <tr>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lblchrgenumber" runat="server" Text="Charge #:"></asp:Label></b>
                                                                            </td>
                                                                            <%--<asp:Label ID="Label18" runat="server" ForeColor="Red" Text="*"></asp:Label>--%>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtchrgenmber" runat="server" TabIndex="26" MaxLength="15"></asp:TextBox>
                                                                                <script type="text/javascript">                                                                                    $('#txtchrgenmber').ForNumericOnly();</script>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lblchrgetype" runat="server" Text="Charge Type:"></asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="ddlchtgetype" Height="22px" Width="160px" runat="server" ToolTip="Shipped To"
                                                                                    TabIndex="27" onselectedindexchanged="ddlchtgetype_SelectedIndexChanged" AutoPostBack="true">
                                                                                </asp:DropDownList>
                                                                                
                                                                            </td>
                                                                            <td>
                                                                            <asp:TextBox ID="txtOtherChargeType" runat="server" Visible="false"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lblchrge" runat="server" Text="Charge:"></asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtchrge" runat="server" TabIndex="28" MaxLength="15"></asp:TextBox>
                                                                                <script type="text/javascript">                                                                                    $('#txtchrge').ForNumericOnly();</script>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lbladdamt" runat="server" Text="Amount:"></asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtaddamt" runat="server" TabIndex="29" Style="text-align: right"
                                                                                    MaxLength="15"></asp:TextBox>
                                                                                <script type="text/javascript">                                                                                    $('#txtaddamt').ForNumericOnly();</script>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lbldescp" runat="server" Text="Description:"></asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtdescp" runat="server" TabIndex="30" Height="30px" TextMode="MultiLine"
                                                                                    Width="177px" MaxLength="256"></asp:TextBox>
                                                                                <script type="text/javascript">                                                                                    $('#txtdescp').ForAlphanumericNumericCommaFullstop();</script>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lblgst" runat="server" Text="GST(Goods & Service Tax):"></asp:Label>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtgst" runat="server" TabIndex="31" MaxLength="15"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
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
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td colspan="2" align="right">
                                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                <asp:Button ID="btnaddchrge" runat="server" Text="Add Charge" Width="150px" BackColor="#5D5D5D"
                                                                                    Font-Bold="True" ForeColor="White" ToolTip="Add Charge" TabIndex="32" CausesValidation="False"
                                                                                    OnClick="btnaddchrge_Click" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                <div style="overflow: auto; width: 1100px; height:100px">
                                                                    <asp:GridView ID="gvAdditionalChargeDetails" runat="server" AutoGenerateColumns="false"
                                                                        Width="1100px" ShowHeader="true" ShowFooter="false" OnRowDeleting="gvAdditionalChargeDetails_RowDeleting"
                                                                        TabIndex="18" CssClass="gridView" Height="100px">
                                                                        <HeaderStyle BackColor="#cccccc" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Sr.No.">
                                                                                <ItemTemplate>
                                                                                    <%# Container.DataItemIndex + 1 %>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Charge#">
                                                                                <ItemTemplate>
                                                                                    <%#Eval("charge_no") %>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Charge Type">
                                                                                <ItemTemplate>
                                                                                    <%#Eval("Charge_id")%>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Charge">
                                                                                <ItemTemplate>
                                                                                    <%#Eval("charge", "{0:F}")%>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Amount">
                                                                                <ItemTemplate>
                                                                                    <%#Eval("amount", "{0:F}")%>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Description " ItemStyle-Wrap="true" ItemStyle-CssClass="col">
                                                                                <ItemTemplate>
                                                                                    <%#Eval("description") %>
                                                                                </ItemTemplate>
                                                                                <ItemStyle CssClass="col" Wrap="True" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="GST (Goods & Service Tax)">
                                                                                <ItemTemplate>
                                                                                    <%#Eval("Gst_no", "{0:F}") %>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkAdditionalChargeDelete" Text="Delete" ToolTip="Delete" runat="server"
                                                                                        CommandName="Delete" CausesValidation="false" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>'></asp:LinkButton>
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
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr id="acc_distr" runat="server">
                                                    <td>
                                                        <%--changed from width 99 to 100% by sachin--%>
                                                        <table width="100%" align="center" style="backcolor: #F7F7F7">
                                                            <tr>
                                                                <td align="left" class="InvallSubheaders" colspan="8" width="100%">
                                                                    <b>Account Distribution Details</b>
                                                                </td>
                                                            </tr>
                                                            <tr id="accdistrtxtboxes" runat="server">
                                                                <td>
                                                                    <table width="94%" cellpadding="5" style="font-family: Verdana; color: #006666; font-size: 12px;">
                                                                        <tr>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lbldb" runat="server" Text="Debit/Credit:"></asp:Label></b>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtbdcr" runat="server" TabIndex="33" MaxLength="15"></asp:TextBox>
                                                                                <script type="text/javascript">                                                                                    $('#txtbdcr').ForAlphanumericNumericOnly();</script>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lblgen" runat="server" Text="General Ledger:"></asp:Label>
                                                                                    <%--<asp:Label ID="Label24" runat="server" ForeColor="Red" Text="*"></asp:Label>--%></b>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtgen" runat="server" TabIndex="34" MaxLength="15"></asp:TextBox>
                                                                                <script type="text/javascript">                                                                                    $('#txtgen').ForNumericOnly();</script>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lblcs1" runat="server" Text="Cost Center 1:"></asp:Label>
                                                                                   <%-- <asp:Label ID="Label25" runat="server" ForeColor="Red" Text="*"></asp:Label>--%></b>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtcs1" runat="server" TabIndex="35" MaxLength="15"></asp:TextBox>
                                                                                <script type="text/javascript">                                                                                    $('#txtcs1').ForAlphanumericNumericOnly();</script>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lblcs2" runat="server" Text="Cost Center 2:"></asp:Label></b>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="tctcs2" runat="server" TabIndex="36" MaxLength="15"></asp:TextBox>
                                                                                <script type="text/javascript">                                                                                    $('#tctcs2').ForAlphanumericNumericOnly();</script>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lblwbs" runat="server" Text="WBS Number:"></asp:Label></b>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtwbs" runat="server" TabIndex="37" MaxLength="15"></asp:TextBox>
                                                                                <script type="text/javascript">                                                                                    $(document).ready(function () { $('#txtwbs').ForNumericOnly(); });</script>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lblamtu" runat="server" Text="Amount:"></asp:Label>
                                                                                    <%--<asp:Label ID="Label26" runat="server" ForeColor="Red" Text="*"></asp:Label>--%></b>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtamt" runat="server" TabIndex="38" MaxLength="15" Style="text-align: right"></asp:TextBox>
                                                                                <script type="text/javascript">
                                                                                    $(document).ready(function () {

                                                                                        $('#txtpoamend').ForAlphanumericNumericOnly();
                                                                                        $('#txtrelease').ForAlphanumericNumericOnly();
                                                                                        $('#txtqtyshipped').ForNumericMinusOnly();
                                                                                        $('#txtunitprice').ForNumericMinusOnly();
                                                                                        $('#txtlineamt').ForNumericOnly();
                                                                                        $('#txtpack').ForAlphanumericNumericOnly();
                                                                                        $('#txtbilloflading').ForAlphanumericNumericOnly();
                                                                                        $('#txtprtnumbr').ForAlphanumericNumericOnly();
                                                                                        $('#txtponumbr').ForAlphanumericNumericOnly();
                                                                                        $('#txtgst').ForAlphanumericNumericOnly();
                                                                                        $('#txtdescp').ForAlphanumericNumericCommaFullstop();
                                                                                        $('#txtaddamt').ForNumericOnly();
                                                                                        $('#txtchrge').ForNumericOnly();
                                                                                        $('#txtchrgenmber').ForNumericOnly();
                                                                                        $('#txtamt').ForNumericOnly();
                                                                                        $('#txtwbs').ForNumericOnly();
                                                                                        $('#tctcs2').ForAlphanumericNumericOnly();
                                                                                        $('#txtcs1').ForAlphanumericNumericOnly();
                                                                                        $('#txtgen').ForNumericOnly();
                                                                                        $('#txtbdcr').ForAlphanumericNumericOnly();
                                                                                        $('#txtComm').ForAlphanumericNumericCommaFullstop();
                                                                                        $('#txtFinalDestination').ForAlphanumericNumericCommaFullstop();
                                                                                        $('#txtComments').ForAlphanumericNumericCommaFullstop();
                                                                                    });
                                                                                    var prm = Sys.WebForms.PageRequestManager.getInstance();

                                                                                    prm.add_endRequest(function () {
                                                                                        $('#txtpoamend').ForAlphanumericNumericOnly();
                                                                                        $('#txtrelease').ForAlphanumericNumericOnly();
                                                                                        $('#txtqtyshipped').ForNumericMinusOnly();
                                                                                        $('#txtunitprice').ForNumericMinusOnly();
                                                                                        $('#txtlineamt').ForNumericOnly();
                                                                                        $('#txtpack').ForAlphanumericNumericOnly();
                                                                                        $('#txtbilloflading').ForAlphanumericNumericOnly();
                                                                                        $('#txtprtnumbr').ForAlphanumericNumericOnly();
                                                                                        $('#txtponumbr').ForAlphanumericNumericOnly();
                                                                                        $('#txtgst').ForAlphanumericNumericOnly();
                                                                                        $('#txtdescp').ForAlphanumericNumericCommaFullstop();
                                                                                        $('#txtaddamt').ForNumericOnly();
                                                                                        $('#txtchrge').ForNumericOnly();
                                                                                        $('#txtchrgenmber').ForNumericOnly();
                                                                                        $('#txtamt').ForNumericOnly();
                                                                                        $('#txtwbs').ForNumericOnly();
                                                                                        $('#tctcs2').ForAlphanumericNumericOnly();
                                                                                        $('#txtcs1').ForAlphanumericNumericOnly();
                                                                                        $('#txtgen').ForNumericOnly();
                                                                                        $('#txtbdcr').ForAlphanumericNumericOnly();
                                                                                        $('#txtComm').ForAlphanumericNumericCommaFullstop();
                                                                                        $('#txtFinalDestination').ForAlphanumericNumericCommaFullstop();
                                                                                        $('#txtComments').ForAlphanumericNumericCommaFullstop();

                                                                                    });</script>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
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
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td align="right" colspan="2">
                                                                                &nbsp;<asp:Button ID="btnaddacc" runat="server" Text="Add Account" Width="150px"
                                                                                    BackColor="#5D5D5D" Font-Bold="True" ForeColor="White" ToolTip="Add Account"
                                                                                    TabIndex="39" CausesValidation="False" OnClick="btnaddacc_Click" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                <div style="overflow: auto; width: 1100px; height:100px">
                                                                    <asp:GridView ID="gvAccountDistributionDetails" runat="server" AutoGenerateColumns="false"
                                                                        Width="1100px" ShowHeader="true" ShowFooter="false" AutoGenerateEditButton="false"
                                                                        OnRowDeleting="gvAccountDistributionDetails_RowDeleting" TabIndex="20" CssClass="gridView">
                                                                        <HeaderStyle BackColor="#cccccc" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Sr.No.">
                                                                                <ItemTemplate>
                                                                                    <%# Container.DataItemIndex + 1 %>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Debit/Credit">
                                                                                <ItemTemplate>
                                                                                    <%#Eval("Debit_Credit") %>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="General Ledger *">
                                                                                <ItemTemplate>
                                                                                    <%#Eval("general_ledger_account") %>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Cost centre1 *">
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
                                                                            <asp:TemplateField HeaderText="Amount *">
                                                                                <ItemTemplate>
                                                                                    <%#Eval("amount", "{0:F}")%>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkdelAccountDist" Text="Delete" ToolTip="Delete" runat="server"
                                                                                        CommandName="Delete" CausesValidation="false" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>'></asp:LinkButton>
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
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="InvallSubheaders" width="100%">
                                                        <b>Total Amount</b>
                                                    </td>
                                                </tr>
                                                <tr id="endfrm" runat="server" width="100%">
                                                    <td>
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="width: 60%">
                                                                </td>
                                                                <td align="left">
                                                                    <table>
                                                                        <tr>
                                                                            <td align="left" style="font-size: 12px; font-weight: bold">
                                                                                <b>
                                                                                    <asp:Label ID="lblTotalLineAmount" runat="server" Text="Total Line Amount:"></asp:Label></b>
                                                                            </td>
                                                                            <td align="right" style="font-size: 17px; font-weight: bold">
                                                                                
                                                                                <asp:TextBox ID="txtTotalLineAmount" runat="server" ToolTip="Total Line Amount" TabIndex="40"
                                                                                    ReadOnly="true" Enabled="true" Style="text-align: right"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="right">
                                                                            <td align="left" style="font-size: 12px; font-weight: bold">
                                                                                <b>
                                                                                    <asp:Label ID="lblTotalAdditionalCharges" runat="server" Text="Total Additonal Charges:"></asp:Label></b>
                                                                            </td>
                                                                            <td align="right" style="font-size: 17px; font-weight: bold">
                                                                                
                                                                                <asp:TextBox ID="txtTotalAdditionalCharges" runat="server" ToolTip="Total Additonal Charges"
                                                                                    TabIndex="41" ReadOnly="true" Enabled="true" Style="text-align: right"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="right" style="font-size: 12px; font-weight: bold">
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:Label ID="lblTotalInvoiceAmount" runat="server" Text="Total Invoice Amount:"></asp:Label></b>
                                                                            </td>
                                                                            <td align="right" style="font-size: 17px; font-weight: bold">
                                                                                
                                                                                <asp:TextBox ID="txtTotalInvoiceAmount" runat="server" ToolTip="Total Invoice Amount:"
                                                                                    TabIndex="44" ReadOnly="true" Enabled="true" Style="text-align: right"></asp:TextBox>
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
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="lblCommentsforstatus" runat="server" Text="Comments: " Visible="false"></asp:Label>
                                                                    <asp:Label ID="txtCommentsforstatus" runat="server" Visible="false"></asp:Label>
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
                                                                                <asp:Button ID="txtclr" runat="server" Text="Clear" Width="120px" BackColor="#5D5D5D"
                                                                                    CausesValidation="false" Font-Bold="True" ForeColor="White" ToolTip="Clear" TabIndex="45"
                                                                                    OnClick="txtclr_Click" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnValidateInvoice" runat="server" Text="Validate Invoice" OnClick="btnValidateInvoice_Click"
                                                                                    Width="120px" BackColor="#5D5D5D" Font-Bold="True" ForeColor="White" ToolTip="Validate Invoice"
                                                                                    TabIndex="46" CausesValidation="true" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="txtcancel" runat="server" Text="Cancel" Width="120px" BackColor="#5D5D5D"
                                                                                    CausesValidation="false" Font-Bold="True" ForeColor="White" ToolTip="Cancel"
                                                                                    TabIndex="47" OnClick="txtcancel_Click" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="txtsave" runat="server" CausesValidation="false" Text="Save As Draft"
                                                                                    Width="120px" BackColor="#5D5D5D" Font-Bold="True" ForeColor="White" OnClick="txtsave_Click1"
                                                                                    ToolTip="Save As Draft" TabIndex="48" />
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Button ID="txtsubmit" runat="server" Text="Submit Invoice" Width="120px" BackColor="#5D5D5D"
                                                                                    Font-Bold="True" ForeColor="White" OnClick="txtsubmit_Click" ToolTip="Submit Invoice"
                                                                                    TabIndex="49" CausesValidation="true" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
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
                <%-- added by sachin 12 feb 2014--%>
                <asp:Panel ID="SupplierLookupPanel" runat="server" BorderColor="Black" BackColor="White"
                    Height="300px" Width="800px" HorizontalAlign="Center">
                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td align="center">
                                <table width="95%" cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td height="30px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="Invallheader">
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
                                                DataKeyNames="s_code" EmptyDataText="Cannot find related information in contacts"
                                                AllowPaging="True" Width="100%" CssClass="GridText">
                                                <Columns>
                                                    <asp:HyperLinkField DataTextField="s_code" HeaderText="Supplier Code" DataNavigateUrlFields="s_code"
                                                        DataNavigateUrlFormatString="~/CreateInvoice.aspx?QS_Scode={0}" ItemStyle-ForeColor="Black"
                                                        ControlStyle-ForeColor="Black" AccessibleHeaderText="s_code" Visible="False"
                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="White" />
                                                    <asp:HyperLinkField DataTextField="s_code" HeaderText="Supplier Code" DataNavigateUrlFields="s_code"
                                                        DataNavigateUrlFormatString="~/Listdr.aspx?QS_Scode1={0}" ItemStyle-ForeColor="Black"
                                                        ControlStyle-ForeColor="Black" AccessibleHeaderText="s_code" Visible="False"
                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="White" />
                                                    <asp:HyperLinkField DataTextField="s_code" HeaderText="Supplier Code" DataNavigateUrlFields="s_code"
                                                        DataNavigateUrlFormatString="~/Approver_WrkQue.aspx?QS_Scode2={0}" ItemStyle-ForeColor="Black"
                                                        ControlStyle-ForeColor="Black" AccessibleHeaderText="s_code" Visible="False"
                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="White" />
                                                    <asp:HyperLinkField DataTextField="s_code" HeaderText="Supplier Code" DataNavigateUrlFields="s_code"
                                                        DataNavigateUrlFormatString="~/InvStatus.aspx?QS_Scode3={0}" ItemStyle-ForeColor="Black"
                                                        ControlStyle-ForeColor="Black" AccessibleHeaderText="s_code" Visible="False"
                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="White" />
                                                    <asp:BoundField DataField="s_name" HeaderText="Supplier Name" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-ForeColor="Black" ItemStyle-BackColor="White" />
                                                </Columns>
                                                <EmptyDataRowStyle BackColor="#7A99B8" BorderStyle="None" Height="15px" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                </panel>
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
                                <asp:Button ID="btn_Cancel" CssClass="button" runat="server" Text="Cancel" Width="120px"
                                    BackColor="#5D5D5D" Font-Bold="True" ForeColor="White" OnClick="btn_Cancel_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <ajax:ModalPopupExtender ID="MPE" runat="server" TargetControlID="lbnSupplierCodeLookup"
                    PopupControlID="SupplierLookupPanel" BackgroundCssClass="modalBackground" CancelControlID="btn_Cancel">
                </ajax:ModalPopupExtender>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="AttachButton" />
            <asp:PostBackTrigger ControlID="txtcancel" />
            <asp:PostBackTrigger ControlID="ExportWord" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
