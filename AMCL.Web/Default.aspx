<%@Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" Title="ICB Asset Management Compnay Limited Login Page (Design and Developed by Sakhawat)" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login Page</title>
    <script language="javascript" type="text/javascript"> 
    function fnValidation()
    {
         if(document.getElementById("<%=loginIDTextBox.ClientID%>").value =="")
        {
            document.getElementById("<%=loginIDTextBox.ClientID%>").focus();
            alert("Please Enter LoginID");
            return false;
            
        }
        if(document.getElementById("<%=loginPasswardTextBox.ClientID%>").value =="")
        {
            document.getElementById("<%=loginPasswardTextBox.ClientID%>").focus();
            alert("Please Enter Login Password");
            return false;
            
        }
    }
    
  
  </script>
<link rel="Stylesheet" type="text/css" href="CSS/amcl.css"/>
    <style type="text/css">
        .style3
        {
            font-size: small;
            font-family: "Courier New";
            font-weight: 700;
        }
        .style4
        {
            font-family: "Courier New", Courier, monospace;
            font-weight: bold;
        }
        .style5
        {
            font-family: "Courier New";
            font-size: small;
            color:Red;
        }
        
        .style6
        {
            text-align: center;
            color: #3399FF;
            font-weight: bold;
            font-family: "Times New Roman";
            font-size: x-large;
        }
        
        .style7
        {
            width: 161px;
        }
        
        .style8
        {
            font-size: x-small;
            color: #CC33FF;
        }
        
        .auto-style1 {
            text-align: center;
            height: 42px;
        }
        .auto-style2 {
            height: 42px;
        }
        
    </style>
</head>
<body onkeydown="if (event.keyCode==13) {event.keyCode=9; return event.keyCode }">
    <form id="form1" runat="server" method="post" >
    <div>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
   
    <table width="100%"  align="center" cellpadding="0" cellspacing="0">
    <tr>
        <td  class="style6" 
            colspan="2">
                       IAMCL Trade&nbsp; Units</td>
    </tr>
   </table>
    <br />
    <br />
    <br />
    <br />
    <table align="center" cellpadding="0" cellspacing="0" style="width: 233px">    
        <tr>
            <td align="right" class="style3">
             Login ID:
            </td>
            <td align="left" class="style3" colspan="2">
            <asp:TextBox ID="loginIDTextBox" runat="server" CssClass="textInputStyle" TabIndex="1" ></asp:TextBox>
            </td>
        </tr>
        
        <tr>
            <td class="style4" align="right">
                <span class="style3">Password:</span>
            </td>
            <td colspan="2" align="left">
            <asp:TextBox ID="loginPasswardTextBox" runat="server" TextMode="Password" CssClass="textInputStyle"  TabIndex="2"></asp:TextBox>
            </td>
        </tr>
        <tr>
           <%-- <td align="left">
             <cc1:CaptchaControl ID="ccJoin" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="3" CaptchaHeight="50" CaptchaWidth="200" CaptchaLineNoise="None" CaptchaMinTimeout="10" CaptchaMaxTimeout="240" />
            </td>--%>
        </tr>
         <tr>
         <td class="auto-style1">
         &nbsp;
         </td>
         <td align="center" class="auto-style2">
                <asp:Button ID="loginButton" runat="server" Text="Login" 
                    CssClass="buttoncommon" OnClientClick="return fnValidation();" 
                   TabIndex="3" Height="27px" Width="116px"/>
         </td>
         <td class="auto-style2">
         &nbsp;
         </td>

         </tr>
         <tr>
            <td align="center" colspan="3">
            <asp:Label runat="server" ID="loginErrorLabel" Visible="false" Text="" class="style5"></asp:Label>
            </td>
          </tr>
        </table>
    </div>
    </form>
</body>
</html>
