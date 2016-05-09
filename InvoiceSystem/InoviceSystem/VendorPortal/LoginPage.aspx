<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="VendorPortal.LoginPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>Vendor Login Page</title>
    <link href="Styles/invsys.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 54px;
        }
        
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }
         .modalPopup 
{ 
background-color: #FFFFFF; 
border-width: 3px; 
border-style: solid; 
border-color: Gray; 
padding-top: 10px; 
padding-left: 10px; 
font-family:Verdana;
width: 400px; 
height: 300px; 
}
        .style2
        {
            font-family: Verdana;
            font-size: 12px;
            width: 72px;
        }
    </style>
</head>
<body>
    <form id="Form1" runat="server">
    <ajax:ToolkitScriptManager ID="ScriptManager" runat="server" />
   <%-- <table id="mainTable" style="width: 1250px; height: 800px" cellspacing="0" cellpadding="0">--%>
    <table id="mainTable" width="1200px" style="height: 802px" cellspacing="0" cellpadding="0" border="0"
        align="center">
        <tr style="height: 15%; width: 100%">
            <td>
                <img src="Images/wrapper_to_bg.png" style="width: 100%" />
            </td>
        </tr>
        <tr style="width: 100%; height:84%; background-color: #C0C0C0;">
            <td style="width: 100%; height:84%">
                <table style="width: 100%" align="right" cellpadding="0" cellspacing="0" border="0">
                    <tr style="width: 100%">
                        <td align="left" style="width: 40%">
                            <img src="Images/loginimage.png" style="height: 339px; width: 540px" />
                        </td>
                        <td style="border: 5; width: 60%" align="right" valign="middle">
                            <table width="60%">
                                <tr>
                                    <td colspan="3" height="10px" bgcolor="#666666" align="left">
                                        <span class="normalheader">Login Details</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="20px" colspan="3" align="left">
                                        <asp:Label ID="lblmsg" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <%--<td height="20px"></td>--%></tr>
                                <tr>
                                    <td class="loginlabletext" align="left" width="20%">
                                       <asp:Label ID="lblUsername" runat="server" Text="User name: " Width="85px"></asp:Label>
                                    </td>
                                    <td align="left" width="45%">
                                        <asp:TextBox runat="server" ID="txtUserID" type="text" Width="225px" 
                                            MaxLength="15"></asp:TextBox>
                                        
                                    </td>
                                    <td width="35%" align="left">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="left" width="100%" id="Logintd" runat="server">
                                        <%--<asp:Panel ID="panelLogin" runat="server" >--%>
                                        <table width="100%">
                                            <tr>
                                                <td class="loginlabletext" align="left" width="20%">
                                                    <asp:Label ID="lblPassword" runat="server" Text="Password: " Width="83px"></asp:Label>
                                                </td>
                                                <td align="left" width="45%">
                                                    <%-- <input id="txtPassword" type="password"" />--%>
                                                    <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" type="text" 
                                                        Width="225px" MaxLength="15"></asp:TextBox>
                                                </td>
                                                <td width="35%" align="left">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" align="center">
                                                    <asp:Label ID="lblErrorMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                                  
                                                </td>
                                            </tr>
                                             <tr>
                                                <td height="10px">
                                                    </td>
                                                    <td colspan="2" height="3px" align="left">
                                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Enter the user name"
                                            ControlToValidate="txtUserID" Display="Dynamic" SetFocusOnError="True" ValidationGroup="login"
                                            CssClass="errormsg"></asp:RequiredFieldValidator>
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td height="10px">
                                                    </td>
                                                    <td colspan="2" height="3px" align="left">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Enter the password"
                                                        ControlToValidate="txtPassword" Display="Dynamic" SetFocusOnError="True" ValidationGroup="login"
                                                        CssClass="errormsg"></asp:RequiredFieldValidator>
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td width="300px" align="left" colspan="2">
                                                    <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="button" ValidationGroup="login"
                                                        Width="120px" BackColor="#5D5D5D" Font-Bold="True" ForeColor="White" OnClick="btnLogin_Click1" />
                                                </td>
                                            </tr>
                                        </table>
                                        <%--  </asp:Panel>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td colspan="3">
                                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                        <tr>
                                                            <td colspan="3" bgcolor="#B4B4B4" height="3px" align="left">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" height="3px" align="center">
                                                                <asp:Panel ID="panelChangePassword" runat="server" Visible="false">
                                                                    <table>
                                                                        <tr>
                                                                            <td class="loginlabletext" align="left" width="30%">
                                                                                Old Password:
                                                                            </td>
                                                                            <td align="left" width="30%">
                                                                                <asp:TextBox ID="txtCurrentPassword" runat="server" TextMode="Password"></asp:TextBox>
                                                                            </td>
                                                                            <td width="40%">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="loginlabletext" align="left" width="30%">
                                                                                New Password:
                                                                            </td>
                                                                            <td align="left" width="30%">
                                                                                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password"></asp:TextBox>
                                                                            </td>
                                                                            <td width="40%">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="loginlabletext" align="left" width="30%">
                                                                                Confirm Password:
                                                                            </td>
                                                                            <td align="left" width="30%">
                                                                                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                                                                            </td>
                                                                            <td width="40%">
                                                                                <asp:CompareValidator ID="CompareValidatorConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword"
                                                                                    ControlToCompare="txtNewPassword" ErrorMessage="Confirm password is not same as New password"
                                                                                    Operator="Equal" ForeColor="Red"></asp:CompareValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="loginlabletext" align="center" width="70%" colspan="2">
                                                                                <asp:Button ID="btnChangePword" runat="server" Text="Change Password" Width="120px"
                                                                                    BackColor="#5D5D5D" Font-Bold="True" ForeColor="White" OnClick="btnChangePword_Click" />
                                                                                &nbsp;&nbsp;
                                                                                <asp:Button ID="btnCanel" runat="server" Text="Cancel" Width="100px" BackColor="#5D5D5D"
                                                                                    Font-Bold="True" ForeColor="White" OnClick="btnCanel_Click" CausesValidation="false" />
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <%--<td colspan="3" height="3px">--%>
                                                            <td align="left">
                                                                <asp:LinkButton ID="LnkBtnForgortPwd" runat="server" 
                                                                    onclick="LnkBtnForgortPwd_Click">Forgot Password?</asp:LinkButton>
                                                                <ajax:ModalPopupExtender ID="ModalPopupExtender1" runat="server" CancelControlID="btn_forgotcancel"
                                                                    BackgroundCssClass="modalBackground" TargetControlID="LnkBtnForgortPwd" PopupControlID="pnlpopup1">
                                                                </ajax:ModalPopupExtender>
                                                                <asp:Panel ID="pnlpopup1" runat="server" Height="80px" Width="350px" BorderColor="Black"
                                                                    BorderWidth="1px" CssClass="modalPopup">
                                                                    <table >
                                                                        <tr>
                                                                            <td  style="font-family: Verdana">
                                                                               User ID:
                                                                               <span style="color:Red">*</span>
                                                                            </td>
                                                                            <td><asp:TextBox runat="server" ID="txt_forgotuserid" Readonly="false"></asp:TextBox></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center" colspan="2" align="right">
                                                                                <asp:Button ID="btnsendemail" runat="server" Width="130px"  Font-Bold="True"
                                                                                     Text="Send Password"  BackColor="#5D5D5D"  ForeColor="White" 
                                                                                    onclick="btnsendemail_Click"/>
                                                                            </td>
                                                                            <td>
                                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                                                <asp:Button  ID="btn_forgotcancel" runat="server" Font-Bold="True"
                                                                                     Text="Cancel"  BackColor="#5D5D5D"  ForeColor="White"  />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <br />
                                                                </asp:Panel>
                                                            </td>
                                                            <%--</td>--%>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" bgcolor="#666666" height="3px">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" valign="middle" colspan="2">
                            <%--<ajax:ModalPopupExtender ID="ModalPopupExtender1" runat="server" CancelControlID="btnOK"
                                BackgroundCssClass="modalBackground" TargetControlID="LnkBtnForgortPwd" PopupControlID="pnlpopup1" />
                            <asp:Panel ID="pnlpopup1" runat="server" Height="200px" Width="400px" BorderColor="Black"
                                BorderWidth="1px">
                                <h4>
                                    Please contact Administration to reset your Password</h4>
                                <br />
                                <asp:Button ID="btnOK" runat="server" Text="Cancel" />
                            </asp:Panel>--%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="height: 1%; width: 100%" valign="bottom">
            <td align="center" valign="middle" class="mainfooter">
                ©1998-2014 Syntel, Inc. | all rights reserved
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
