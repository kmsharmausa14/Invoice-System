<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Home.aspx.cs" Inherits="VendorPortal._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script src="Scripts/jquery-1.4.1.js"></script>
    <script src="Scripts/jquery-1.4.1.min.js"></script>
    <style type="text/css">
        
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table style="width: 100%; height: 650px">
        <tr style="width: 1100px; height: 650px;">
            <td style="width: 60%; height: 100%;" valign="middle">
                <table style="width: 100%; height: 100%;" align="center" backcolor="#F7F7F7">
                    <tr style="width: 100%; height: 5%;">
                        <td style="width: 100%; height: 5%;" colspan="2">
                            <br />
                        </td>
                    </tr>
                    <tr style="width: 100%; height: 15%;">
                        <td style="width: 30%; height: 15%;" align="right" valign="middle">
                            <a href="CreateInvoice.aspx?QS_FlageFromHomePage=yes">
                                <img src="Images/create.jpg" style="height: 69px; width: 86px;"/></a>
                            <br />
                        </td>
                        <td style="width: 70%; height: 15%;" align="left">
                            <h3 style="font-family: Verdana; font-weight: bold; color: #070B19">
                                <a href="CreateInvoice.aspx?QS_FlageFromHomePage=yes">Create Invoice</a></h3>
                            &nbsp;
                        </td>
                    </tr>
                    <tr style="width: 100%; height: 5%;">
                        <td style="width: 100%; height: 5%;" colspan="2">
                            <br />
                        </td>
                    </tr>
                    <tr style="width: 100%; height: 15%;" runat="server" id="trLstOfDraft">
                        <td style="width: 30%; height: 15%;" align="right" runat="server" valign="middle">
                            <a href="Listdr.aspx">
                                <img src="Images/draft.png" style="height: 69px; width: 86px;" /></a>
                            <br />
                        </td>
                        <td style="width: 70%; height: 15%;" align="left">
                            <h3 style="font-family: Verdana; font-weight: bold; color: #003399">
                                <a href="Listdr.aspx">List Of Draft Invoices</a></h3>
                        </td>
                    </tr>
                    <tr style="width: 100%; height: 15%;" runat="server" id="trApproverWorkQueue">
                        <td style="width: 30%; height: 15%;" align="right" valign="middle">
                            <a href="Approver_WrkQue.aspx">
                                <img src="Images/approve.jpg" style="height: 69px; width: 86px;" /></a>
                            <br />
                        </td>
                        <td style="width: 70%; height: 15%;" align="left">
                            <h3 style="font-family: Verdana; font-weight: bold; color: #003399">
                                <a href="Approver_WrkQue.aspx">Approver Workqueue</a></h3>
                            &nbsp;
                        </td>
                    </tr>
                    <tr style="width: 100%; height: 5%;">
                        <td style="width: 100%; height: 5%;" colspan="2">
                            <br />
                        </td>
                    </tr>
                    <tr style="width: 100%; height: 15%;">
                        <td style="width: 30%; height: 15%;" align="right" valign="middle">
                            <a href="InvStatus.aspx">
                                <img src="Images/status.jpg" style="height: 69px; width: 86px;" /></a>
                            <br />
                        </td>
                        <td style="width: 70%; height: 15%;" align="left">
                            <h3 style="font-family: Verdana; font-weight: bold; color: #003399">
                                <a href="InvStatus.aspx">Invoice Status</a></h3>
                            &nbsp;
                        </td>
                    </tr>
                    <tr style="width: 100%; height: 5%;">
                        <td style="width: 100%; height: 5%;" colspan="2">
                            <br />
                        </td>
                    </tr>
                    <tr style="width: 100%; height: 15%;" runat="server" id="trReport">
                        <td style="width: 30%; height: 15%;" align="right" valign="middle">
                            <a href="#">
                                <img src="Images/dynamic_reports.jpg" style="height: 69px; width: 86px;" enabled="false" /></a>
                        </td>
                        <td style="width: 70%; height: 15%;" align="left">
                            <h3 style="font-family: Verdana; font-weight: bold; color: #003399">
                                <a href="Reports.aspx">Reports</a></h3> <%--Reports.aspx--%>
                            &nbsp;
                        </td>
                    </tr>
                    <tr style="width: 100%; height: 5%;">
                        <td style="width: 100%; height: 5%;" colspan="2">
                            <br />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width: 40%; height: 100%;">
                <table style="width: 100%; height: 100%;">
                    <tr style="width: 100%; height: 50%;">
                        <td style="width: 100%; height: 50%;">
                            <img id="Img1" src="~/Images/invoice1.jpg" runat="server" style="height: 100%; width: 100%" />
                        </td>
                    </tr>
                    <tr style="width: 100%; height: 50%;">
                        <td style="width: 100%; height: 50%;">
                            <img id="Img2" src="Images/invoice2.jpg" runat="server" style="height: 100%; width: 100%" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
