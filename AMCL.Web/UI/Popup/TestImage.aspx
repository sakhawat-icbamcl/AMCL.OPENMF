﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestImage.aspx.cs" Inherits="UI_TestImage" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Unit Holder Signature And Photo</title>
    <script language="javascript" type="text/javascript"> 
  
 
  
   
        
    function PopupYes()
    {
    window.close();
       this.parent.PopupYes();
        
     
        
    }
</script>
    <link rel="Stylesheet" type="text/css" href="../../CSS/amcl.css" />
    <style type="text/css">
        .style2
        {
            width: 77px;
        }
        .style3
        {
            width: 392px;
        }
        .style4
        {
            width: 99px;
        }
        .style6
        {
            font-size: medium;
            font-weight: bold;
            color: #009933;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <table align="center" width="800">
      <tr>
             <td align="left" class="style2" rowspan="2">Name:</td> 
             <td align="left" id="tdHolderName" runat="server" class="style3"></td>
              <td align="left" class="style4" >Registration No:</td> 
             <td align="left" id="tdReg" runat="server"></td>
      </tr>
         <tr>
             
             <td align="left" id="tdFund" runat="server" class="style3"></td>
             <td align="left" class="style4">Branch:</td> 
             <td align="left" id="tdBranch" runat="server"></td>
        </tr> 
        </table>
        <br />
         <br />
          <br />
           <br />
        <table align="center" width="600" >
         <tr>
             <td align="center" ><span class="style6"  style="border:1px solid">Signature</span>
             </td>           
             <td align="center" ><span class="style6"  style="border:1px solid">Photo</span>
               
             </td>
        </tr> 
         <%--<tr>
             <td align="left" >&nbsp;
             </td>           
             <td align="left" >&nbsp;
               
             </td>
        </tr> --%>
        <tr>
             <td align="left"  style="border:1px solid">
             </td>           
             <td align="left"  style="border:1px solid">
                 &nbsp;</td>
        </tr> 
        <tr>
             <td colspan="2">&nbsp;</td>
        </tr> 
         <tr>
             <td colspan="2" align="center" style="border:1px solid" >               
                 <asp:Button ID="regCloseButton0" runat="server" Text="Close" 
                CssClass="buttoncommon" OnClientClick="return PopupYes();" AccessKey="c" />
                 <asp:Button ID="regCloseButton" runat="server" Text="Close" 
                CssClass="buttoncommon" OnClientClick="window.close();" AccessKey="c" />
        </td>
        </tr> 
      </table>
    
    
    </div>
    
    </form>
</body>
</html>
