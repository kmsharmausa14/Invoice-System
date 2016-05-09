<%@ Page Title="Log In" Language="C#" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="VendorPortal.Account.Login" %>

    
<html>
<<body>
<form runat="server">
    <table>
        <tr>
            <td class="style5">
                <img src="../Images/logo.jpg" style="width: 394px" />
            </td>
            <td class="style7"  >
                <table>
                    <tr align="left" valign="top">
                    <asp:label runat="server" Text="Welcome to Vendor Portal" class="login"
                                ></asp:label>
                    </tr>
                    <tr>
                        <td>
                            The Vendor Portal is a wesite where a vendor can login
                             <br /> 
                            and create invoices for a particular puchase order.
                            <br />
                            liashcbfsdlahblfjbhljhbjb kjhknjbb jhbcljhbvluhbcj,
                            <br />
                            hbluytddjhgi cg7i6c ,mvci7 c,nhhvy6cfj, ,jchgubc.
                            <br />
                            kh76tgcljn ,hgci7gkjbvykdfcuinilufgd79bvviih
                         </td>
                         </tr>
                         </table>
                         </td>
                         <td class="style8">
                         </td>
                          <td align="right" style="font-family: 'Bauhaus 93'; font-size:large; font-weight: bold; color: #0066CC; background-color: #00CCFF;" 
                class="style3">To Know more Register here </td>
                        
                    </tr>
                </table>
    <div style="width: 741px">
    </div>
    <table>
        <tr>
            <td class="style4" style="background-color: #0066CC; line-height: normal;"  >
           
           </td>
             <td rowspan="2" class="style2" >
                            <table align="right" style="width: 358px">
                                <tr align="left">
                                   
                                    <td align="left" class="style6"  colspan="2">
                                        <asp:Label ID="Label1" runat="server"   class="login" Text="Vendor Login"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="font-family: 'Bauhaus 93'; font-size: small; font-weight: bold; color: #0066CC">
                                    User Name
                                    </td>
                                    <td class="style6">
                                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="font-family: 'Bauhaus 93'; font-size: small; font-weight: bold; color: #0066CC">
                                    Password:
                                    </td>
                                    <td class="style6">
                                    <asp:TextBox ID="TextBox2" runat="server" TextMode="Password"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                   <td align="right"><asp:CheckBox runat="server"/></td>
                                   <td colspan="0.5" style="font-family: 'Bauhaus 93'; font-size: small; font-weight: bold; color: #0066CC" 
                                        class="style6">Remember Me&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="Button1" runat="server" Text="Login" BackColor="#0066CC" Font-Bold="True" 
                                           Font-Names="Bauhaus 93" ForeColor="White" /></td>
                                        
                                   
                                </tr>

                            </table>
                         </td>
        </tr>
    </table>
   </form>
   </body>
</html>
   
