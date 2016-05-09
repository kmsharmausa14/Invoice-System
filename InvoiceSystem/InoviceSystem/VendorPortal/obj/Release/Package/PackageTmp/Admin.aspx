<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="VendorPortal.Admin" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css "/> 
<link href="Styles/Site.css" rel="stylesheet" type="text/css" /> 
<script src="http://code.jquery.com/jquery-1.9.1.js"></script> 
<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script> 


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
width: 500px; 
height: 300px; 
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
 border-bottom-left-radius:1em;
 border-top-left-radius:1em;
 border-top-right-radius:1em;
 border-bottom-right-radius:1em;
 } 
 
    .fieldset
 {
 border:1px solid;
 border-bottom-left-radius:0.7em;
 border-top-left-radius:0.7em;
 border-top-right-radius:0.7em;
 border-bottom-right-radius:0.7em;
 } 
 .grid
 {
     font-family: Verdana; 
font-size: medium; 
font-style: normal;
color: #FFFFFF; 
background-color: #5D5D5D;  
   border-bottom-left-radius:0.8em;
 border-top-left-radius:0.8em;
 border-top-right-radius:0.8em;
 border-bottom-right-radius:0.8em;  
     }
     
     .bindgrid
 {
     font-family: Verdana; 
font-size: medium; 
font-style: normal;
color: #000000; 
background-color:transparent;  
   border-bottom-left-radius:0.8em;
 border-top-left-radius:0.8em;
 border-top-right-radius:0.8em;
 border-bottom-right-radius:0.8em;  
     }
</style>
<%--.aplha
{
}--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <div id="">
            
            <div id="div_newuser" class="div">
            <fieldset class="fieldset" style=" color:white; font-weight:bolder;font-family:Verdana;font-size:larger; background-color: #5D5D5D"><span >Add User</span></fieldset>
           <table>
            <tr>
                <td></td>
            </tr>
            <tr>
                <td></td>
            </tr>
            <tr>
                <td>
                    <span style="color:Green; font-size:smaller; font-family:Verdana">Note:- If Role is <b>Supplier</b> only Supplier 
                    Code and UserID is required to enter.<br />
                           &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;If Role selected as <b>Admin or Approver</b>, please fill all fields except supplier code</span>
                </td>
            </tr>
            <tr>
                <td></td>
            </tr>
            <tr>
                <td></td>
            </tr>
        </table>
        <table width="100%" style="font-family:Verdana; font-size:small">

            <tr>
            <td rowspan="5" width="5%"></td>
                <td >
                    <asp:Label ID="lblrole" runat="server" Text="Role:"></asp:Label><span style="color:Red">*</span>  
                </td>
                <td>
                   <asp:DropDownList runat="server" ID="ddl_addrole" Width="170px" 
                        AutoPostBack="true" onselectedindexchanged="ddl_addrole_SelectedIndexChanged">
                        <asp:ListItem Value="0" Text="-Select Role-"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Approver"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Supplier"></asp:ListItem>
                        <asp:ListItem Value="3" Text="Admin"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td >
                    <asp:Label ID="lbluserid" runat="server" Text="UserID:"></asp:Label><span style="color:Red">*</span>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtuser"  Width="170px"></asp:TextBox>  <%--CssClass="aplha"--%>
                </td>
                <td rowspan="5" width="15%"></td>
            </tr>
            <tr>
                <td>
                   
                </td>
            </tr>
            <tr>
                <td>
                   
                </td>
            </tr>
            <tr>
                <td>
                <asp:Label ID="lblname" runat="server" Text="Name:"></asp:Label>&nbsp;<asp:Label ID="lblnamestar" runat="server" ForeColor="red" Text="*"></asp:Label></td>
                <td>
                    
                     <asp:TextBox runat="server" ID="txtname"  Width="170px"></asp:TextBox>
                </td>
                 <td>
                    <asp:Label runat="server" ID="lbl_supp" Text="Supplier Code:"></asp:Label>
                <asp:Label ID="lbl_suppstar" runat="server" ForeColor="red" Text="*"></asp:Label></td>
                <td>
                    <%--<asp:TextBox runat="server" ID="txt_supplier"></asp:TextBox>--%>
                    <asp:DropDownList runat="server" ID="ddl_supplier" Width="170px" 
                        AutoPostBack="True" onselectedindexchanged="ddl_supplier_SelectedIndexChanged">
                        <asp:ListItem Value="0"  Text="--Select Supplier Code--"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                
            </tr>
            <tr>
                <td>
                   
                </td>
            </tr>
            <tr>
                <td>
                   
                </td>
            </tr>
            <tr runat="server">
            <td></td>
               <td>
                    <asp:Label ID="lblemail" runat="server" Text ="EmailId:"></asp:Label>&nbsp;<asp:Label ID="lblemailstar" runat="server" ForeColor="red" Text="*"></asp:Label></td>
                <td>
                    <asp:TextBox runat="server" ID="txt_email" Width="180px"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" 
                        ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                        ControlToValidate="txt_email" ErrorMessage="Invalid Email Format" 
                        ForeColor="Red" Font-Size="Smaller" ></asp:RegularExpressionValidator>
                </td>
                 <td>
                    <asp:Label ID="lblcontact" runat="server" Text ="Contact:"></asp:Label>
                <asp:Label ID="lblcontactstar" runat="server" ForeColor="red" Text="*"></asp:Label></td>
                <td>
                    <asp:TextBox runat="server" ID="txtcontact" Width="170px"></asp:TextBox>
                </td>  
            </tr>
            <tr>
                <td>
                    <span>
                        <br />
                    </span>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="6">
                    <asp:Button runat="server" id="btn_create" Text="Add" 
                        onclick="btn_create_Click" Width="80px" Height="40px" BackColor="Gray"  
                        ForeColor="White" CssClass="grid"/>
                    <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                    <asp:Button runat="server" ID="btn_cancel" Text="Clear" Height="40px" Width="80px"
                        BackColor="Gray" ForeColor="White" CssClass="grid"  
                        onclick="btn_cancel_Click" CausesValidation="False"/>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
        </table>
    </div>            
            <div id="div2" class="div">
            <fieldset class="fieldset" style=" color:white; font-weight:bolder;font-family:Verdana;font-size:larger; background-color: #5D5D5D"><span >Delete User</span></fieldset>
                <div id="div_edituser" style="font-family:Verdana">
                <table>
                    <tr>
                        <td>
                            <br />
                            <br />
                        </td>
                    </tr>
                </table>
                <table style="font-family:Verdana; font-size:small">
        <tr>
        <td>
        <asp:Label runat="server" ID="lbl_searchuser" Text="Search By :" Font-Bold="true"></asp:Label>
        </td>
        <td>
            &nbsp;&nbsp;&nbsp;
        </td>
        <td></td>
        <td></td>

                <td>
                    <asp:RadioButton  runat="server" ID="rad_userid" ClientIDMode="Static" Text="UserID:" 
                        GroupName="criteria" AutoPostBack="True" 
                        oncheckedchanged="rad_userid_CheckedChanged1"/>
                </td>

                <%--CssClass="aplha"--%>
                <td><asp:TextBox runat="server" ClientIDMode="Static" ID="txt_userid"></asp:TextBox><span style="color:Red" id="alertid" runat="server">*</span></td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorUserID" runat="server" ControlToValidate="txt_userid" 
                                                                        ErrorMessage="Please enter UserID" Display="Dynamic" ForeColor="white" Text="*">
                                                                    </asp:RequiredFieldValidator>
                <td><asp:Button runat="server" ID="btn_usersearch" Text="Go" BackColor="Gray" ForeColor="White"
                        onclick="btn_usersearch_Click"/></td>
          </tr>
          <tr>
            <td>
               
            </td>
          </tr>
          <tr>
            <td>
               
            </td>
          </tr>
           <tr>  
           <td></td>
           <td></td>        
            <td></td>
            <td></td>
                <td>
                    <asp:RadioButton  runat="server" ID="rad_usertype" ClientIDMode="Static" Text="User Type:" 
                        GroupName="criteria" AutoPostBack="True" 
                        oncheckedchanged="rad_usertype_CheckedChanged1" />
                </td>
                <td>
                    <asp:DropDownList runat="server" ClientIDMode="Static" ID="ddl_usertype" 
                        AutoPostBack="True" onselectedindexchanged="ddl_usertype_SelectedIndexChanged" Height="28" Width="165px">
                        <asp:ListItem Value="0" Text=""></asp:ListItem>
                        <asp:ListItem Value="1" Text="Approver"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Supplier"></asp:ListItem>
                        <asp:ListItem Value="3" Text="Admin"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                </tr>
                <tr>
            <td>
                
            </td>
          </tr>
          <tr>
            <td>
               
            </td>
          </tr>
                <tr>
                <td></td>
                <td></td>
            <td></td>
            <td></td>
                <td>
                    <asp:RadioButton  runat="server" ID="rad_showall" Text="Show All Users:" 
                        GroupName="criteria" AutoPostBack="True" 
                        oncheckedchanged="rad_showall_CheckedChanged" />
                </td>
            </tr>
            <tr>
                <td colspan="18" align="center">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button runat="server" ID="btnClearDeleteSection" Text="Clear" BackColor="Gray" Autopostback="true" CausesValidation="False" Width="80px" Height="40px"
                        ForeColor="White" onclick="btnClearDeleteSection_Click"  CssClass="grid"/>
                </td>
            </tr>
        </table>
               
    </div>
                    
            </div>
            <div>
        <table style="width:100%" align="center">
            <tr>
                <td width="100%" align="center" >
                   <%-- <asp:Label runat="server" ID="lbl_griderror" ForeColor="Red" Font-Size="Larger"></asp:Label>--%>
                    <asp:GridView ID="gv_userdtls" runat="server" 
                        AutoGenerateColumns="true" EmptyDataText="No Records Found"
                        CssClass="bindgrid" AllowPaging="true" PageSize="5" OnPageIndexChanging="gvuserdtls_PageIndexChanging"
                        OnRowDeleting="gv_userdtls_deleting" Width="70%">
                        <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="DeleteButton" runat="server" CausesValidation="False"
                                    CommandName="Delete" Text="Delete"
                                    OnClientClick="return deleteconfm()"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>                           
                        </Columns>
                        <AlternatingRowStyle BackColor="#eeeeee" /> 
                        <HeaderStyle BackColor="Gray" ForeColor="White"/>
                        <RowStyle HorizontalAlign ="Left" Font-Size="Small"/>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    
                </td>
            </tr>
        </table>
    </div>
            
        </div>

   
    
</asp:Content>