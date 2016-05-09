<%@ Page Title="Approver Workqueue" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Approver_WrkQue.aspx.cs" Inherits="VendorPortal.Approver_WrkQue" %>

<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="ContentHeadApproverWorkQueue" ContentPlaceHolderID="HeadContent"
    runat="server">
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

        //Validations for To Date and From Date
        function ValidateFromDate() {
            //debugger;
            var ToDate = document.getElementById("txtToDate").value;
            var FromDate = document.getElementById("txtFromDate").value;


            if (FromDate.length == 0 && ToDate.length > 0) {
                alert("Please select From Date");
                document.getElementById("txtToDate").value = '';
            }
            else {
                ValidateTodateGreaterThanFromDate();
            }
        }

        function ValidateTodateGreaterThanFromDate() {
            //debugger;
            var date = new Date();
            var textDate = (date.getMonth() + 1) + "/" + date.getDate() + "/" + date.getFullYear().toString().substr(0, 4);

            var ToDatelen = document.getElementById("txtToDate").value;
            var FromDatelen = document.getElementById("txtFromDate").value;

            if (ToDatelen.length > 0) {
                var value = isValidDateFormat(ToDatelen);
                if (value == false) {
                    document.getElementById("txtToDate").value = textDate;
                    return false;
                }
            }
            if (FromDatelen.length > 0) {
                var value = isValidDateFormat(FromDatelen);
                if (value == false) {
                    document.getElementById("txtFromDate").value = textDate;
                    return false;
                }
            }

            var ToDate = new Date(document.getElementById("txtToDate").value);
            var FromDate = new Date(document.getElementById("txtFromDate").value);


            if (FromDatelen.length > 0 && ToDatelen.length > 0) {
                if (FromDate > ToDate) {
                    alert("To Date should be greater than or equal to From Date");
                    document.getElementById("txtToDate").value = textDate;
                }
            }
        }


        function isValidDateFormat(dateString) {
            // debugger;
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
            //return false;

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
    </script>
</asp:Content>
<asp:Content ID="ContentMainApproverWorkqueue" ContentPlaceHolderID="MainContent" ClientIDMode="Static"
    runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtSupplierId").ForAlphanumericNumericOnly();
            $('#txtPONo').ForAlphanumericNumericOnly();
            $('#txtInvoiceNumber').ForAlphanumericNumericOnly();
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {
            $("#txtSupplierId").ForAlphanumericNumericOnly();
            $('#txtPONo').ForAlphanumericNumericOnly();
            $('#txtInvoiceNumber').ForAlphanumericNumericOnly();
        });
    </script>
    <script type="text/javascript">
        function onlyNumeric() {

            if ((event.keyCode > 47 && event.keyCode < 58)) {
                event.returnValue = true;
            }
            else {
                event.returnValue = false;
            }
        }
        function ValidatePageValue(txtpage, hidtotal) {

            if (txtpage.value.length > 0) {
                if (txtpage.value > hidtotal.value) {
                    alert('Please enter page number less than or equal to ' + hidtotal.value);
                    txtpage.value = '';
                    return false;
                }
            }
            else {
                alert('Please enter page number');
                return false;
            }
        }

    </script>
    <%#Eval("po_no") %>
    <table style="width: 1150px; height: 760px" cellpadding="0" cellspacing="0" border="0"
        align="left" backcolor="#F7F7F7">
        <tr valign="top" align="center" style="font-family: Dotum; font-size: x-large; font-weight: bold;
            font-style: normal; color: #0033CC">
            <td>
                <table align="center" width="1000px" style="font-family: Verdana; color: #006666;
                    font-size: 12px;">
                    <tr>
                        <td align="center" style="font-family: Verdana; font-weight: bold; font-size: 20PX;
                            color: #006666;">
                            Approver Workqueue
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <%#Eval("s_currency") %>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td width="100%">
                            <table width="100%">
                                <tr>
                                    <td style="font-family: Verdana; font-size: 12px">
                                        <asp:Label ID="lblSupplierId" runat="server" Text="Supplier Code :"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtSupplierId" runat="server" ToolTip="Supplier Code" MaxLength="50"
                                            TabIndex="1"></asp:TextBox>
                                        <script type="text/javascript">                                            $("#txtSupplierId").ForAlphanumericNumericOnly();</script>
                                        <asp:LinkButton ID="LinkBtnSupplierCodeLookup1" runat="server" OnClick="LinkBtnSupplierCodeLookup1_Click"
                                            CausesValidation="False"><img src="Images/Lookup.png" /></asp:LinkButton>
                                    </td>
                                    <td style="font-family: Verdana; font-size: 12px">
                                        <asp:Label ID="lblFromDate" runat="server" Text="From Date :"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtFromDate" runat="server" ToolTip="From Date" TabIndex="2"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalendarExtenderFromDate" TargetControlID="txtFromDate"
                                            runat="server" />
                                    </td>
                                    <td style="font-family: Verdana; font-size: 12px">
                                        <asp:Label ID="lblToDate" runat="server" Text="To Date :"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtToDate" runat="server" ToolTip="To Date" TabIndex="3"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalendarExtenderToDate" TargetControlID="txtToDate" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-family: Verdana; font-size: 12px">
                                        <asp:Label ID="lblPONo" runat="server" Text="PO # :"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtPONo" runat="server" ToolTip="PO #" TabIndex="4" MaxLength="15"></asp:TextBox>
                                        <script type="text/javascript">                                            $('#txtPONo').ForAlphanumericNumericOnly();</script>
                                    </td>
                                    <td style="font-family: Verdana; font-size: 12px">
                                        <asp:Label ID="lblInvoiceNumber" runat="server" Text="Invoice # :"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtInvoiceNumber" runat="server" ToolTip="Invoice #" TabIndex="5"
                                            MaxLength="15"></asp:TextBox>
                                        <script type="text/javascript">                                            $('#txtInvoiceNumber').ForAlphanumericNumericOnly();</script>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="4" width="50%">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" Width="120px" BackColor="#5D5D5D"
                                            Font-Bold="True" ForeColor="White" TabIndex="6" OnClick="btnSearch_Click" />
                                    </td>
                                    <td width="40px">
                                        <span></span>
                                    </td>
                                    <td align="left" colspan="4">
                                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="120px" BackColor="#5D5D5D"
                                            Font-Bold="True" ForeColor="White" TabIndex="6" OnClick="btnClear_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr style="width: 90%">
                                    <td style="font-family: Verdana; font-size: 12px; width: 90%" colspan="8" align="center">
                                        <asp:Label ID="lblNoRecords" runat="server" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8" align="center">
                                        <asp:GridView ID="gvApproverWorkqueue" runat="server" AutoGenerateColumns="false"
                                            Width="70%" ShowHeader="true" TabIndex="7" AllowPaging="true" PageSize="5" OnPageIndexChanging="gvApproverWorkqueue_PageIndexChanging">
                                            <HeaderStyle Font-Size="14px" BorderColor="Black" Font-Bold="true" Font-Underline="true"
                                                BackColor="#999999" BorderStyle="Double" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <Columns>
                                                <asp:HyperLinkField AccessibleHeaderText="Invoice Number" HeaderText="Invoice #"
                                                    DataNavigateUrlFields="Inv_Code" DataNavigateUrlFormatString="~/ApproverScreen.aspx?QS_Inv_Code={0}&FlageFromApproverWorkQue=yes"
                                                    DataTextField="Inv_Code" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left"></asp:HyperLinkField>
                                                <asp:TemplateField HeaderText="Supplier Code">
                                                    <ItemTemplate>
                                                        <%#Eval("s_code") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PO #">
                                                    <ItemTemplate>
                                                        <%#Eval("po_no") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Currency">
                                                    <ItemTemplate>
                                                        <%#Eval("s_currency") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Supplier Name">
                                                    <ItemTemplate>
                                                        <%#Eval("s_name") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status">
                                                    <ItemTemplate>
                                                        <%#Eval("status_descp")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="8">
                                        <asp:Panel ID="pagingPanel" Visible="false" runat="server">
                                            <table width="60%">
                                                <tr>
                                                    <td style="width: 30%">
                                                        <asp:Label Text="Page Size" runat="server" ID="lblPageSize"></asp:Label>
                                                        <asp:TextBox ID="txtPageSize" runat="server" Width="47px" AutoPostBack="True" OnTextChanged="txtPageSize_TextChanged"></asp:TextBox>
                                                        <asp:HiddenField ID="hidPageSize" runat="server" />
                                                        <script type="text/javascript">                                                            $('#txtPageSize').ForNumericOnly();</script>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblGoToPage" runat="server" Text="GoTo:"></asp:Label>
                                                        <asp:TextBox ID="txtGoToPage" runat="server" Width="47px"></asp:TextBox>
                                                        <script type="text/javascript">                                                            $('#txtGoToPage').ForNumericOnly();</script>
                                                        of
                                                        <asp:Label ID="lblTotalPage" runat="server"></asp:Label>
                                                        <asp:Button ID="btnGo" runat="server" Text="Go" Width="20%" OnClick="btnGo_Click"
                                                            CausesValidation="False" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:HiddenField ID="Hidtotalpage" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr align="center" style="width: 100%">
                        <td align="center" width="100%">
                            <asp:Button ID="btnCancel" runat="server" Text="Back" Width="120px" BackColor="#5D5D5D"
                                Font-Bold="True" ForeColor="White" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
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
                                            DataNavigateUrlFormatString="~/Approver_WrkQue.aspx?QS_Scode={0}" ItemStyle-ForeColor="Black"
                                            ControlStyle-ForeColor="Black" AccessibleHeaderText="s_code" Visible="False"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="White" />
                                        <asp:HyperLinkField DataTextField="s_code" HeaderText="Supplier Code" DataNavigateUrlFields="s_code"
                                            DataNavigateUrlFormatString="~/Approver_WrkQue.aspx?QS_Scode1={0}" ItemStyle-ForeColor="Black"
                                            ControlStyle-ForeColor="Black" AccessibleHeaderText="s_code" Visible="False"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="White" />
                                        <asp:HyperLinkField DataTextField="s_code" HeaderText="Supplier Code" DataNavigateUrlFields="s_code"
                                            DataNavigateUrlFormatString="~/Approver_WrkQue.aspx?QS_Scode2={0}" ItemStyle-ForeColor="Black"
                                            ControlStyle-ForeColor="Black" AccessibleHeaderText="s_code" Visible="False"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="White" />
                                        <asp:HyperLinkField DataTextField="s_code" HeaderText="Supplier Code" DataNavigateUrlFields="s_code"
                                            DataNavigateUrlFormatString="~/Approver_WrkQue.aspx?QS_Scode3={0}" ItemStyle-ForeColor="Black"
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
    <ajax:ModalPopupExtender ID="MPE" runat="server" TargetControlID="LinkBtnSupplierCodeLookup1"
        PopupControlID="SupplierLookupPanel" BackgroundCssClass="modalBackground" CancelControlID="btn_Cancel">
    </ajax:ModalPopupExtender>
</asp:Content>
